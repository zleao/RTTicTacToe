using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CQRSlite.Caching;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Messages;
using CQRSlite.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RTTicTacToe.CQRS.Database;
using RTTicTacToe.CQRS.Hubs;
using RTTicTacToe.CQRS.ReadModel.Infrastructure;
using RTTicTacToe.CQRS.ReadModel.Queries;
using RTTicTacToe.CQRS.WriteModel.EventStore;
using RTTicTacToe.CQRS.WriteModel.Handlers;

namespace RTTicTacToe.WebApi
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RTTicTacToe.WebApi.Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddMemoryCache();

            //Add Cqrs services
            services.AddSingleton<Router>(new Router());
            services.AddSingleton<ICommandSender>(y => y.GetService<Router>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<Router>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>());
            services.AddScoped<IEventStore, SqliteEventStore>();
            services.AddSingleton<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            services.AddScoped<CQRSlite.Domain.ISession, Session>();

            services.AddDbContext<DatabaseContext>(options => options.UseSqlite("Data Source=CQRSGame.db"));

            services.AddTransient<IDatabaseService>(sp => new DatabaseService(sp.GetRequiredService<DatabaseContext>()));
            services.AddTransient<IGameQueries, GameQueries>();

            //Scan for commandhandlers and eventhandlers
            services.Scan(scan => scan
                .FromAssemblies(typeof(GameCommandHandlers).GetTypeInfo().Assembly)
                .AddClasses(classes => classes.Where(x =>
                {
                    var allInterfaces = x.GetInterfaces();
                    return
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IHandler<>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableHandler<>));
                }))
                .AsSelf()
                .WithTransientLifetime()
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RTTicTacToe - API",
                    Description = "ASP.NET Core Web API with the implementation of the RTTicTacToe game backend",
                    Contact = new OpenApiContact
                    {
                        Name = "José Pereira",
                        Email = "leaopereira@gmail.com",
                        Url = new Uri("https://twitter.com/z_leao")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlPath);
            });

            //Register routes
            var serviceProvider = services.BuildServiceProvider();
            var registrar = new RouteRegistrar(new Provider(serviceProvider));
            registrar.RegisterInAssemblyOf(typeof(GameCommandHandlers));

            //Add support for SignalR
            services.AddSignalR();

            // Configure controllers behaviour
            services.AddControllers();
        }

        /// <summary>
        /// Configure the specified app and env.
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">App.</param>
        /// <param name="env">Env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RTTicTacToe V1");
            });

            //app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapControllers();

                //Register SignalR endpoint
                routes.MapHub<GameHub>("/api/gamehub");
            });

            //Ensure the database is created, using migrations
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.Database.Migrate();
            }
        }
    }

    /// <summary>
    /// Provider that makes scoped services work inside router.
    /// </summary>
    public class Provider : IServiceProvider
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RTTicTacToe.WebApi.Provider"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public Provider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _contextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <returns>The service.</returns>
        /// <param name="serviceType">Service type.</param>
        public object GetService(Type serviceType)
        {
            return _contextAccessor?.HttpContext?.RequestServices.GetService(serviceType) ??
                   _serviceProvider.GetService(serviceType);
        }
    }
}

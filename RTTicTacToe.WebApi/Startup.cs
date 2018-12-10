using System;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag.AspNetCore;
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
                .AddClasses(classes => classes.Where(x => {
                    var allInterfaces = x.GetInterfaces();
                    return
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IHandler<>)) ||
                        allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICancellableHandler<>));
                }))
                .AsSelf()
                .WithTransientLifetime()
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwagger();

            //Register routes
            services.AddHttpContextAccessor(); // No longer registered by default in ASP.NET Core 2.1
            var serviceProvider = services.BuildServiceProvider();
            var registrar = new RouteRegistrar(new Provider(serviceProvider));
            registrar.RegisterInAssemblyOf(typeof(GameCommandHandlers));

            //Add support for SignalR
            services.AddSignalR();
        }

        /// <summary>
        /// Configure the specified app and env.
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">App.</param>
        /// <param name="env">Env.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();

            //Register SignalR endpoint
            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/api/gamehub");
            });

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwaggerUi3WithApiExplorer(settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.GeneratorSettings.Title = "RTTicTacToe - API";
            });

            app.UseMvc();
        }
    }

    //
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

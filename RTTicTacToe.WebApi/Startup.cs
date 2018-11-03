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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag.AspNetCore;
using RTTicTacToe.CQRS.ReadModel.Queries;
using RTTicTacToe.CQRS.WriteModel.EventStore;
using RTTicTacToe.CQRS.WriteModel.Handlers;

namespace RTTicTacToe.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            //Add Cqrs services
            services.AddSingleton<Router>(new Router());
            services.AddSingleton<ICommandSender>(y => y.GetService<Router>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<Router>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>());
            services.AddSingleton<IEventStore, InMemoryEventStore>();
            services.AddSingleton<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            services.AddScoped<CQRSlite.Domain.ISession, Session>();

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            
            app.UseStaticFiles();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwaggerUi3WithApiExplorer(settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
            });

            app.UseMvc();
        }
    }

    //This makes scoped services work inside router.
    public class Provider : IServiceProvider
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public Provider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _contextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
        }

        public object GetService(Type serviceType)
        {
            return _contextAccessor?.HttpContext?.RequestServices.GetService(serviceType) ??
                   _serviceProvider.GetService(serviceType);
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Autofac;
using CentralTrade.Logger;
using CentralTrade.Repositories;
using CentralTrade.Repositories.Interfaces;
using CentralTrade.Domain.Services;
using System.Collections.Generic;

namespace CentralTrade.API
{
    public class Startup
    {
        private const string SwaggerEndpoint = "/swagger/v1/swagger.json";
        private const string FileToBeLogged = @"C:\log.txt";//read from externalized configuration

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //.Net core supports inbuilt IOC but as asked to use third party IOC container so used Autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //all logger settings can be read from externaized configuration
            //you can setup any logger chain to handle different type of logging for different sevierities
            builder.RegisterType<ConsoleLogger>()
                .As<ILogger>()
                .WithParameter("logSeverities", LogSeverityExtensions.All())//set up with all severity
                .WithParameter("nextLogger", new FileLogger(new List<LogSeverity>() { LogSeverity.Error }, FileToBeLogged))//setup file logger only for errors
                .SingleInstance();
            builder.RegisterType<TradeRepository>().As<ITradeRepository>();            
            builder.RegisterType<TradeTxService>().As<ITradeTxService>();
            builder.RegisterType<TradeService>().As<ITradeService>();
            builder.RegisterType<TradeViewRepository>().As<ITradeViewRepository>();
            builder.RegisterType<TradeViewRepository>().As<ITradeViewRepository>();
            builder.RegisterType<TradeViewCacheRepository>().As<ITradeViewCacheRepository>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMemoryCache();
            services.AddMvcCore().AddApiExplorer().AddControllersAsServices();
            services.Configure<GzipCompressionProviderOptions>(opts => opts.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.Configure<BrotliCompressionProviderOptions>(opts => opts.Level = System.IO.Compression.CompressionLevel.Optimal);

            services.AddResponseCompression(opts =>
            {
                opts.Providers.Add<GzipCompressionProvider>();
                opts.Providers.Add<BrotliCompressionProvider>();
                opts.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json", 
                    // Custom
                    "image/svg+xml",
                };
            });

            services.AddSingleton(Configuration);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Central Trade Service",
                    Version = "v1",
                    Description = "Central Trade Service",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact() { 
                    Email = "dhananjaykushwaha@hotmail.com"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (app == null)
            {
                return;
            }

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features?.Get<IExceptionHandlerPathFeature>();

                    await context.Response.WriteAsync(exceptionHandlerPathFeature?.Error.Message);
                });
            });

            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerEndpoint, "Central Trade API");
                c.RoutePrefix = "swagger";
            });
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHttpsRedirection();
        }
    }
}

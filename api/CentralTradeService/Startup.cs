using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Hosting;
using CentralTrade.Repositories.Interfaces;
using CentralTrade.Repositories;
using CentralTrade.Domain.Services;
using System;
using CentralTrade.Models;
using Microsoft.AspNetCore.Http;
using CentralTrade.API.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace CentralTrade.API
{
    public class Startup
    {
        private const string SwaggerEndpoint = "/swagger/v1/swagger.json";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddMvcCore().AddApiExplorer().AddJsonFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
            services.AddScoped<ITradeRepository, TradeRepository>();
            services.AddScoped<ITradeTxService, TradeTxService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Central Trade Service",
                    Version = "v1",
                    Description = "Central Trade Service",
                    Contact = new Contact
                    {
                        Name = "Dhananjay Kushwaha",
                        Email = "dhananjaykushwaha@hotmail.com"
                    }
                });
                c.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (app == null || env == null)
            {
                return;
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features?.Get<IExceptionHandlerPathFeature>();

                        await context.Response.WriteAsync(GetError(exceptionHandlerPathFeature?.Error));
                    });
                });
            }

            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerEndpoint, "Central Trade API");
                c.RoutePrefix = "swagger";
            });

            app.UseMvc();
        }

        private string GetError(Exception exception)
        {
            BaseResponse response = new BaseResponse()
            {
                ValidationResult = new ValidationResult()
                {
                    ErrorCode = CentralTrade.Models.Enums.ErrorCode.Unspecified,
                    Message = GetExceptionDetails(exception),
                    Success = false
                } };

            return JsonConvert.SerializeObject(response);
        }

        private string GetExceptionDetails(Exception ex)
        {
            string errorMsg = "An unexpected fault happened.Try again later.";

            if (ex == null) { return errorMsg; }
            else
            {

                errorMsg += " Error:" + ex.Message;

                if (ex.StackTrace != null)
                {
                    errorMsg += ex.StackTrace;
                }
            }

            return errorMsg;
        }
    }
}

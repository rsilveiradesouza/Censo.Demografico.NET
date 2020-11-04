using Censo.NET.API.Hubs;
using Censo.NET.Application;
using Censo.NET.Application.Interfaces;
using Censo.NET.Domain.Interfaces.API;
using Censo.NET.Domain.Interfaces.Data;
using Censo.NET.Infrastructure.Data;
using Censo.NET.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Censo.NET.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment CurrentEnvironment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            CurrentEnvironment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (!CurrentEnvironment.IsEnvironment("Teste"))
            {
                services.AddDbContext<CensoContext>(options => options
                      .UseSqlServer(Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING"))
                      .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
                      .EnableSensitiveDataLogging());
            }
            else
            {
                services.AddDbContext<CensoContext>(options => options
                    .UseInMemoryDatabase(Configuration["databaseName"])
                    .ConfigureWarnings(config => config.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            }

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials();
            }));

            services.AddTransient<IPesquisaRepository, PesquisaRepository>();
            services.AddTransient<IPesquisaService, PesquisaService>();
            services.AddTransient<IBuscaService, BuscaService>();
            services.AddTransient<ICensoHub, DashboardHub>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rafael Souza - Censo 2020" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rafael Souza - Censo 2020");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DashboardHub>("/hub/dashboard");
            });

            if (Environment.GetCommandLineArgs().Contains("--migrate"))
            {
                using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = serviceScope.ServiceProvider.GetService<CensoContext>();
                context.Database.Migrate();
            }
        }
    }
}

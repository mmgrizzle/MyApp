using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NJsonSchema;
using NSwag.AspNetCore;
using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using MyApp.Application.Students.Models;
using MyApp.Application.Infrastructure;
using MyApp.Application.Students.Queries;
using MyApp.WebApi.Infrastructure;
using IdentityServer4.AccessTokenValidation;
using MyApp.Persistance;

namespace MyApp.WebApi
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
            // Add framework services.
            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddMediatR(typeof(GetStudentQueryHandler).GetTypeInfo().Assembly);



            // Add DbContext using SQL Server Provider
            services.AddDbContext<MyAppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MyAppDatabase")));



            // Add Open API support (will generate specification document)
            services.AddSwagger();



            // Add Logging + Seq
            services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(); });



            // Add IdentityServer
            services.AddAuthentication(
                IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44357/";  //port of IDP
                    options.ApiName = "myapi";
                });



            // Mvc + Custom Exception Filter
            services.AddMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, MyAppDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            app.UseSwaggerUi3(typeof(Startup).GetTypeInfo().Assembly, s =>
            {
                s.GeneratorSettings.Title = "My App API";
                s.GeneratorSettings.DefaultUrlTemplate = "{controller}/{action}/{id?}";
                s.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

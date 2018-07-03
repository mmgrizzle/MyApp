using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyApp.Mvc.Services;

namespace MyApp.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {         
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // register an IImageGalleryHttpClient
            services.AddScoped<IMyAppHttpClient, MyAppHttpClient>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies",
                (options) => 
                {
                    options.AccessDeniedPath = "/Authorization/AccessDenied";
                })
              .AddOpenIdConnect("oidc", options =>
              {
                  options.SignInScheme = "Cookies";
                  options.Authority = "https://localhost:44357";
                  options.ClientId = "myappclient";
                  options.ResponseType = "code id_token";
                  options.Scope.Add("openid");
                  options.Scope.Add("profile");
                  options.Scope.Add("address");
                  options.Scope.Add("roles");
                  options.Scope.Add("myapi");
                  options.SaveTokens = true;
                  options.ClientSecret = "secret";
                  options.GetClaimsFromUserInfoEndpoint = true;
                  options.ClaimActions.Remove("amr");
                  options.ClaimActions.DeleteClaim("sid");
                  options.ClaimActions.DeleteClaim("idp");
                  options.ClaimActions.MapUniqueJsonKey("role", "role");

                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      NameClaimType = JwtClaimTypes.GivenName,
                      RoleClaimType = JwtClaimTypes.Role,
                  };

              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

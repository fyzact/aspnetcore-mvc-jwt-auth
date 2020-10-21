using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using UseJwtTokenInMvcWebApp.Helper;

namespace UseJwtTokenInMvcWebApp
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
             string Token = "verversecretToken:)";
            var key = Encoding.ASCII.GetBytes(Token);
            services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor,
            HttpContextAccessor>();
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
         .AddJwtBearer(token =>
         {
             token.RequireHttpsMetadata = false;
             token.SaveToken = true;
             token.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(key),
                 ValidateIssuer = true,
                 ValidIssuer = "www.example.com",
                 ValidateAudience = true,
                 ValidAudience = "www.example.com",
                 RequireExpirationTime = true,
                 ValidateLifetime = true,
                 ClockSkew = TimeSpan.Zero
             };
         });

            services.AddSingleton<JwtHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();
            app.Use(async (context, next) =>
            {
                var jqtToken = context.Request.Cookies["jwt-token"];
                if (!string.IsNullOrEmpty(jqtToken))
                {
                    context.Request.Headers.Add($"Authorization", $"Bearer { jqtToken}");
                }
                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

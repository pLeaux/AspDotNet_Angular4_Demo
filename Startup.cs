using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VEGA.Models;
using AutoMapper;
using VEGA.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;
using VEGA.Mapping;

namespace VEGA
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
            /* "AddMvc()" created authomatically, ignoreAuthorize added to turn off [Authorize] attr. for testing purposes
            services.AddMvc();
            */
            if (Configuration["AuthSettings:ignoreAuthorize"] == "true")
            {
                services.AddMvc(opts =>
                {
                    opts.Filters.Add(new AllowAnonymousFilter());
                });
            } else
            {
                services.AddMvc();
            } 

            // LP++
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<VegaDbContext>();
            services.AddAutoMapper();
            // each time IVehicleRepository is added as input param to Controller constructor, EF autocreates implementation VehicleRepository and injects it (CDI) to Service
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IHttpUtils, HttpUtils>();

            // inits and makes PhotoSettings injectable
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));

            // Leo++ Add Auth0 Authentication Services (see for instructions: https://manage.auth0.com/dashboard/eu/leaux-vega-demo/apis/5ce07ad492fe760834f347fc/quickstart )
            // RS256 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://leaux-vega-demo.eu.auth0.com/";
                options.Audience = "http://localhost:49707/api";
            });

            // add roles policy 
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRole", policy => policy.RequireClaim(Configuration["AuthSettings:roles_namespace"], "admin")); // AuthSettings are defined in appSettings.json
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // (note LP: which modules="middleware" are involved in inspecting+processing of request; if module is in pipeline, it will always check request, but not always do something)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Note: env.EnvironmentName is set from a) launchSettings.json or, if empty, fro b) Win Environment variable "ASPNETCORE_ENVIRONMENT" 

            if (env.IsDevelopment())
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>> Startup.Configure() started !"); 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // LP: auto-reload of opened web page, upon source code change
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                }); 
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // LP: without that, no images or static html files could be served 
            app.UseStaticFiles();

            // 2. Enable authentication middleware (added for auth0)
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"); 

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
                    // defaults: new { controller = "Vehicle-list", action = "Index" });
            });

        }
    }
}

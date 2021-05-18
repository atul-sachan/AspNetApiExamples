using BusinessApplication.TourManagement.Api.Contexts;
using BusinessApplication.TourManagement.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace BusinessApplication.TourManagement.Api
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
            //services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
            //    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(options=> {
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(Constants.CorsPolicy, 
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            var connectionString = Configuration["ConnectionStrings:TourManagementDB"];
            services.AddDbContext<TourManagementContext>(options => options.UseSqlServer(connectionString));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // register the repository
            services.AddScoped<ITourManagementRepository, TourManagementRepository>();

            // register the user info service
            services.AddScoped<IUserInfoService, UserInfoService>();

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
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseHttpsRedirection();
            app.UseCors(Constants.CorsPolicy);
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

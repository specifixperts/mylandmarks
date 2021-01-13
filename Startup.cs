using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyLandmarks.Api.Controllers;
using MyLandmarks.Api.Services;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using MyLandmarks.Api.Models;

namespace MyLandmarks.Api
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
           


            services.AddControllers();

            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
               .UseLazyLoadingProxies().EnableSensitiveDataLogging(true));

            services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<AppDbContext>()
               .AddUserManager<AppUserManager>();


            services.AddScoped<IApiKeyService, ApiKeyService>();

            
            Action<HttpClient> geoCitiesApiClient = c =>
            {
                c.BaseAddress = new Uri("https://wft-geo-db.p.rapidapi.com/v1/geo/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("x-rapidapi-key", "9c46a3934amsh389b9df8e96bf3dp1bd330jsn37933e4c967d");
                c.DefaultRequestHeaders.Add("x-rapidapi-host", "wft-geo-db.p.rapidapi.com");
                c.DefaultRequestHeaders.Add("useQueryString", "true");
            };

            Action<HttpClient> flickrApiClient = c =>
            {
                c.BaseAddress = new Uri("https://www.flickr.com/services/rest/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            };

            Action<HttpClient> fourSquareApiClient = c =>
            {
                c.BaseAddress = new Uri("https://api.foursquare.com/v2/venues/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            };

            Action<HttpClient> restCountriesApiClient = c =>
            {
                c.BaseAddress = new Uri("https://restcountries.eu/rest/v2/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            };

            services.AddAutoMapper(typeof(Startup));
            services.AddHttpClient<GeoCitiesClientService>(geoCitiesApiClient);
            services.AddHttpClient<FlickrClientService>(flickrApiClient);
            services.AddHttpClient<FourSquareClientService>(fourSquareApiClient);
            services.AddHttpClient<RestCountriesClientService>(restCountriesApiClient);

            //services.AddHttpClient<LocationsController>(locationsApiClient);

            services.AddMvc();
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddCors(config => config.AddPolicy("AllowAll",
                                                       p => p.AllowAnyOrigin()
                                                             .AllowAnyMethod()
                                                             .AllowAnyHeader()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Repositories;
using Repositories.IRepositories;
using server.Helpers;
using server.Service;
using Stripe;

namespace server
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        private const string publishableKey = "PublishableKey";
        private const string secretKey = "SecretKey";
        private const string apiKey = "APIKey";
        private const string domain = "Domain";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<stripeContext>(options =>
                           options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]),
                           ServiceLifetime.Scoped);

            services.InjectJwtAuthService(Configuration);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IStripePaymentsRepository, StripePaymentsRepository>();
            services.AddTransient<IAuthorizeCapturePaymentGateway, AuthorizeCapturePaymentGateway>();

            StripeConfiguration.ApiKey = Configuration.GetValue<string>(apiKey);

            services.Configure<Entity.Configuration.StripeOptions>(options =>
            {
                options.PublishableKey = Configuration.GetValue<string>(publishableKey);
                options.SecretKey = Configuration.GetValue<string>(secretKey);

                options.Domain = Configuration.GetValue<string>(domain);
            });

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(),
                };
            });
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseFileServer();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

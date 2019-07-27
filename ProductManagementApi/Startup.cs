using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProductManagementApi.DataStore;
using ProductManagementApi.Helper;
using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementApi
{
    public class Startup
    {
        private readonly string AllowFrontendWebOrigins = "_allowFrontendWebOrigins";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowFrontendWebOrigins,
                builder =>
                {
                    builder.WithOrigins(Configuration["AllowOrigins"]).AllowAnyHeader().AllowAnyMethod();
                });
            });

            
            services.AddSingleton(new ProductsDataStore(StubInitialProducts()));
            services.AddSingleton(new UserDataStore(StubInitialUsers()));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ConfigureJwtAuthentication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(AllowFrontendWebOrigins);
            app.UseHttpsRedirection();

            
            app.UseAuthentication();
            app.UseMvc();
        }

        private void ConfigureJwtAuthentication(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<UserDataStore>();
                        var userName = context.Principal.Identity.Name;
                        var user = userService.GetUserByUsername(userName);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
        private List<ProductDto> StubInitialProducts()
        {
            return new List<ProductDto>()
            {
                new ProductDto()
                {
                    Id = "PR0001",
                    Description = "HP 15-DA0042TX 15.6 inch Laptop [i7]",
                    Model = "4PZ98PA#ABG",
                    Brand = "Hp"
                },
                new ProductDto()
                {
                    Id = "PR0002",
                    Description = "FFalcon 24F1 24 inch HD LED TV",
                    Model = "24F1",
                    Brand = "Falcon"
                },
                new ProductDto()
                {
                    Id = "PR0003",
                    Description = "Apple iPhone XR 64GB (White)",
                    Model = "3801000078",
                    Brand = "Apple"
                },
                new ProductDto()
                {
                    Id = "PR0004",
                    Description = "Panasonic DC-FT7 Tough Camera [4K Video] (Orange)",
                    Model = "DC-FT7GN-D",
                    Brand = "Panasonic"
                },
                new ProductDto()
                {
                    Id = "PR0005",
                    Description = "Cygnett Protectshield Screen Protector for Fitbit Charge 3",
                    Model = "CY2852CPPRO",
                    Brand = "Cygnett"
                }
            };
        }

        private List<UserAuthenticationDto> StubInitialUsers()
        {
            return new List<UserAuthenticationDto>
            {
                new UserAuthenticationDto()
                {
                    Username = "admin",
                    Password = "admin"
                }
            };
        }
    }
}

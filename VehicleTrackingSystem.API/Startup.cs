using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VehicleTrackingSystem.CustomObjects.MapperConfig;
using VehicleTrackingSystem.CustomObjects.Settings;
using VehicleTrackingSystem.DataAccess.Repository;
using VehicleTrackingSystem.Domain.Services;
using VehicleTrackingSystem.Security.Handlers;
using VehicleTrackingSystem.Utilities.ExternalService;
using VehicleTrackingSystem.Validation.Service;

namespace VehicleTrackingSystem.API
{
    public partial class Startup
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
            
            ExtendThreadPool();
            var settings = GetAppConfigurationSection();
            AddVehicleTrackingSystemDependencies(services, settings);
            ConfigureSwaggerService(services);
            ConfigureSingletonServices(services);
            SetupCrossOriginResourceSharing(services);
            ConfigureJwtAuthentication(services);
            AutoMapperConfigurations(services);
            ConfigureTransientServices(services);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Vehicle Tacking System");
            });
        }

        private void ConfigureSwaggerService(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vehicle Tacking System", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
                        });

        }

        private void ConfigureJwtAuthentication(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(GetAppConfigurationSection().Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
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
            IdentityModelEventSource.ShowPII = true;
        }

        private static void AutoMapperConfigurations(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private void ExtendThreadPool()
        {
            ThreadPool.GetMinThreads(out var test, out var minIoc);
            ThreadPool.SetMinThreads(Configuration.GetValue<int>("MinThreads"), minIoc);
        }

        private static void SetupCrossOriginResourceSharing(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAllHeaders", builder => builder
                                .AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod()));
            services.AddHealthChecks();
        }

        private AppSettings GetAppConfigurationSection() => Configuration.GetSection("AppSettings").Get<AppSettings>();


        private void ConfigureSingletonServices(IServiceCollection services)
        {
            services.AddSingleton(GetAppConfigurationSection());
        }

        private void ConfigureTransientServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IUserServices), typeof(UserServices));
            services.AddTransient(typeof(IGeneralService), typeof(GeneralService));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(ICryptographyHandler), typeof(CryptographyHandler));
            services.AddTransient(typeof(IJwtTokenHandler), typeof(JwtTokenHandler));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IErrorMapper), typeof(ErrorMappingConfig));
            services.AddTransient(typeof(IVehicleServices), typeof(VehicleServices));
            services.AddTransient(typeof(IDeviceServices), typeof(DeviceServices));
            services.AddTransient(typeof(IExternalService), typeof(ExternalService));
        }
    }
}

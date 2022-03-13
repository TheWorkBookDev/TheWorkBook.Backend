﻿using System.Reflection;
using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Data;
using TheWorkBook.Backend.Service;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Utils;
using TheWorkBook.Utils.Abstraction;
using TheWorkBook.Utils.Abstraction.ParameterStore;

namespace TheWorkBook.Backend.API
{
    public class Startup
    {
        private readonly IEnvVariableHelper _envVariableHelper;
        readonly bool traceEnabled = false;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _envVariableHelper = new TheWorkBook.Utils.EnvVariableHelper(null, Configuration);

            string loggingLevel = _envVariableHelper.GetVariable("Logging__LogLevel__Default");
            if (!string.IsNullOrWhiteSpace(loggingLevel))
                traceEnabled = loggingLevel.Equals("Trace", StringComparison.InvariantCultureIgnoreCase);

            LogTrace("Startup()");
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            LogTrace("Entered ConfigureServices");

            // Add automapper.
            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IEnvVariableHelper, EnvVariableHelper>();

            services.AddScoped<IApplicationUser, ApplicationUser>();

            // Register Services
            services.AddTransient<IListingService, ListingService>();
            services.AddTransient<IUserService, UserService>();

            ConfigureDatabaseContext(services);
            
            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            AddSwaggerGenServices(services);
        }

        public void ConfigureDatabaseContext(IServiceCollection services)
        {
            //DB Connection
            using IParameterStore parameterStore = GetParameterStore(Configuration);

            LogTrace("Got IParameterStore object");

            IParameter connectionString = parameterStore.GetParameter("/database/app-connection-string");
            services.AddDbContext<TheWorkBookContext>(options => options.UseSqlServer(connectionString.Value));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            bool useDeveloperExceptionPage = _envVariableHelper.GetVariable<bool>("UseDeveloperExceptionPage");

            if (env.IsDevelopment() || useDeveloperExceptionPage)
            {
                app.UseDeveloperExceptionPage();
            }

            AddSwaggerMiddleware(app, env);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Nothing to see here, please move along :-)");
                });
            });
        }

        public void AddSwaggerGenServices(IServiceCollection services)
        {
            if (!_envVariableHelper.GetVariable<bool>("EnableSwagger"))
            {
                return;
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "TheWorkBook API",
                        Version = "v1",
                        Contact = new OpenApiContact
                        {
                            Name = "Ronan Farrell",
                            Email = "ronanfarrell@live.ie"
                        },
                        Description = "This API provides the backend functionality needed for TheWorkBook app."
                    });

                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

                //** Set the comments path for the Swagger JSON and UI.**
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });
        }

        public void AddSwaggerMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!_envVariableHelper.GetVariable<bool>("EnableSwagger"))
            {
                return;
            }

            app.UseStaticFiles();

            app.UseSwagger(options =>
            {
                //Workaround to use the Swagger UI "Try Out" functionality when deployed behind a reverse proxy (APIM) with API prefix /sub context configured
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    List<OpenApiServer> servers = new();

                    if (env.IsDevelopment())
                    {
                        servers.Add(new OpenApiServer { Url = "https://localhost:62129" });
                        servers.Add(new OpenApiServer { Url = "https://api.theworkbook.ie" });
                    }
                    else
                    {
                        servers.Add(new OpenApiServer { Url = "https://api.theworkbook.ie" });
                    }

                    swagger.Servers = servers;
                });
            });

            Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions swaggerUIOptions = new Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions();
            swaggerUIOptions.SwaggerEndpoint("v1/swagger.json", "TheWorkBook API v1");
            swaggerUIOptions.DisplayRequestDuration();

            app.UseSwaggerUI(swaggerUIOptions);
        }

        private IParameterStore GetParameterStore(IConfiguration configuration)
        {
            LogTrace("Entered GetParameterStore()");

            bool useSpecifiedParamStoreCreds = 
                _envVariableHelper.GetVariable("UseSpecifiedParamStoreCreds") != null
                    && _envVariableHelper.GetVariable("UseSpecifiedParamStoreCreds")
                        .Equals("true", StringComparison.InvariantCultureIgnoreCase);

            LogTrace($"useSpecifiedParamStoreCreds: {useSpecifiedParamStoreCreds}");

            AmazonSimpleSystemsManagementClient client;

            if (useSpecifiedParamStoreCreds)
            {
                Amazon.RegionEndpoint regionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_envVariableHelper.GetVariable("AWSRegion", true));

                LogTrace($"instantiating AmazonSimpleSystemsManagementClient: {useSpecifiedParamStoreCreds}");

                client = new AmazonSimpleSystemsManagementClient(_envVariableHelper.GetVariable("ParamStoreConnectionKey", true),
                    _envVariableHelper.GetVariable("ParamStoreConnectionSecret", true), regionEndpoint);
            }
            else
            {
                LogTrace($"instantiating default AmazonSimpleSystemsManagementClient: {useSpecifiedParamStoreCreds}");
                client = new AmazonSimpleSystemsManagementClient();
            }

            LogTrace("'AmazonSimpleSystemsManagementClient' client instantiated");

            return new TheWorkBook.Utils.ParameterStore.ParameterStore(client);
        }

        private void LogTrace(string logMessage)
        {
            if (traceEnabled)
            {
                LambdaLogger.Log(logMessage);
            }
        }
    }
}
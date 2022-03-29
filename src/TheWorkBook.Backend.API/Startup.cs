using System.Reflection;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.API.Extensions;
using TheWorkBook.Backend.API.Helper;
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

        public static IConfiguration? Configuration { get; private set; }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            bool useDeveloperExceptionPage = _envVariableHelper.GetVariable<bool>("UseDeveloperExceptionPage");

            if (env.IsDevelopment() || useDeveloperExceptionPage)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerAuthorized();
            AddSwaggerMiddleware(app, env);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Nothing to see here, please move along :-)");
                });
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            LogTrace("Entered ConfigureServices");

            // Add automapper.
            services.AddAutoMapper(typeof(Startup));

            services.AddDataProtection()
                .SetApplicationName("TheWorkBook")
                .PersistKeysToAWSSystemsManager($"/DataProtection");

            services.AddTransient<IEnvVariableHelper, EnvVariableHelper>();

            services.AddScoped<IApplicationUser, ApplicationUser>();

            // Register Services
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IListingService, ListingService>();
            services.AddTransient<IUserService, UserService>();

            //DB Connection
            ParameterStoreHelper parameterStoreHelper = new ParameterStoreHelper(_envVariableHelper);
            using IParameterStore parameterStore = parameterStoreHelper.GetParameterStore();

            ConfigureDatabaseContext(services, parameterStore);
            ConfigureAuthentication(services, parameterStore);

            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            services.AddAuthorization(options =>
            {
                // External users
                options.AddPolicy("ext.user.api.policy", policyUser =>
                {
                    policyUser.RequireClaim("scope", "api");
                });

                // Internal users and backend services/operations.
                options.AddPolicy("int.api.policy", policyUser =>
                {
                    policyUser.RequireClaim("scope", "int.api");
                });
            });

            AddSwaggerGenServices(services);
        }

        private void AddSwaggerGenServices(IServiceCollection services)
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

        private void AddSwaggerMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
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

        private void ConfigureAuthentication(IServiceCollection services, IParameterStore parameterStore)
        {
            IParameterList parameterList = parameterStore.GetParameterListByPath("/auth/");

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    string identityServerUrl = parameterList.GetParameterValue("IdentityServerUrl");
                    LogTrace("AddJwtBearer: Identity Server Url:" + identityServerUrl);
                    options.Authority = identityServerUrl;

                    // We have multiple identity token issuers per environment
                    List<string> validIssuers = new();
                    string identityValidIssuers = parameterList.GetParameterValue("IdentityValidIssuers");
                    string[] issuers = identityValidIssuers.Split(';');
                    validIssuers.AddRange(issuers);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidIssuers = validIssuers
                    };
                });
        }

        private void ConfigureDatabaseContext(IServiceCollection services, IParameterStore parameterStore)
        {
            LogTrace("Got IParameterStore object");

            IParameter connectionStringParam = parameterStore.GetParameter("/database/app-connection-string");

            LogTrace("Got connectionStringParam object");

            services.AddDbContext<TheWorkBookContext>(options => options.UseSqlServer(connectionStringParam.Value));
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

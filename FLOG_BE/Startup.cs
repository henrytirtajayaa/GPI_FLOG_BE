using AutoMapper;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;
using Infrastructure.Log;
using FLOG_BE.Model.Central;
using Infrastructure.Log.Interface;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.Filters;
using Infrastructure.Utils;
using Infrastructure.Attributes.CommaSeparated;
using FLOG_BE.Model.Companies;
using FLOG_BE.Model.Fetcher;

namespace FLOG_BE
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
            services.AddDbContext<FlogContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), builder => builder.UseRowNumberForPaging()));

            services.AddDbContext<CompanyContext>();

            
            services.AddMvcCore(o =>
            {
                o.Conventions.Add(new CommaSeparatedQueryStringConvention());
            })
            .AddCors()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddApiExplorer()
            .AddAuthorization(options =>
            {
                options.AddPolicy("internalapi", builder => builder
                    .RequireClaim("client_isInternalApi"));
            })
            .AddJsonFormatters().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver
                    = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK"; //utc iso format
            });

            services.AddSingleton(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Finance Accounting System GPI",
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.CustomSchemaIds(type => type.ToString());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = JwtSecurityKey.Create(Configuration["Jwt:Key"]),
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAutoMapper(x => x.AddGlobalIgnore("Initiator"));

            ConfigureInjection(services);
            ConfigureHandlers(services);
            ConfigureDecorators(services);
            ConfigureValidators(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                  builder.WithOrigins("*")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin()
                  );

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FAS");
            });
            app.UseStaticFiles();
            app.UseAuthentication();
        }

        private static void ConfigureInjection(IServiceCollection services)
        {
            //Infrastructure Need
            services.AddScoped<IMediator, Mediator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILogin, Login>();
            services.AddSingleton<IUserLoginProperties, UserLoginProperties>();
            services.AddSingleton<ILogWriter, LogWriter>();
            services.AddScoped<HATEOASLinkCollection, HATEOASLinkCollection>();
            services.AddScoped<ICompanyFetcher, CompanyFetcher>();
        }

        private static void ConfigureDecorators(IServiceCollection services)
        {
            services.AddScoped(typeof(IRequestHandlerDecorator<,>), typeof(IdentityDecorator<,>));
            services.AddScoped(typeof(IRequestHandlerDecorator<,>), typeof(ValidationDecorator<,>));
        }

        private static void ConfigureHandlers(IServiceCollection services)
        {
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IAsyncRequestHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IAsyncRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
        }

        private static void ConfigureValidators(IServiceCollection services)
        {
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
        }
    }
}

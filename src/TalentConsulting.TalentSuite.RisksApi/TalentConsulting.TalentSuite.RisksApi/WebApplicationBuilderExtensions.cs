using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TalentConsulting.TalentSuite.RisksApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;
using TalentConsulting.TalentSuite.RisksApi.Common.Validators;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PostRiskEndpoint;
using FluentValidation;
using System.Security;

namespace TalentConsulting.TalentSuite.RisksApi;

[ExcludeFromCodeCoverage]
internal static partial class WebApplicationBuilderExtensions
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Reports API", Version = "v1" });
            options.DocumentFilter<HealthChecksFilter>();
            options.CustomSchemaIds(OpenApiSchemaUtils.RenameDtoStrategy);
        });
        builder.ConfigureSerilog();
        builder.Services.AddApplicationInsightsTelemetry();
        builder.ConfigureAuth();
        builder.ConfigureCors();
        builder.ConfigureEntityFramework();
        builder.ConfigureApplicationDependencies();
        builder.ConfigureHealthChecks(); // must come after db init

        builder.Services.AddAutoMapper([typeof(WebApplicationBuilderExtensions).Assembly]);
    }

    private static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, services, loggerConfiguration) =>
        {
            var logLevelString = builder.Configuration["LogLevel"];

            var parsed = Enum.TryParse<LogEventLevel>(logLevelString, out var logLevel);

            loggerConfiguration.WriteTo.ApplicationInsights(
                services.GetRequiredService<TelemetryConfiguration>(),
                TelemetryConverter.Traces,
                parsed ? logLevel : LogEventLevel.Warning);

            loggerConfiguration.WriteTo.Console(
                parsed ? logLevel : LogEventLevel.Warning);
        });
    }

    private static void ConfigureAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            if (string.IsNullOrEmpty(builder.Configuration["JWT:Secret"]))
            {
                throw new SecurityException("The JWT:Secret configuration value must be set");
            }

            // Adding Jwt Bearer
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            if (builder.Environment.IsDevelopment())
            {
                options.AddPolicy("TalentConsultingUser", policy => policy.RequireAssertion(_ => true));
            }
            else
            {
                options.AddPolicy("TalentConsultingUser", policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("TalentConsultingReader") ||
                        context.User.IsInRole("TalentConsultingWriter")));
            }
        });
    }

    private static void ConfigureCors(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactAppForLocalDev", policy => {
                    policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
                });
            });
        }
    }

    private static void ConfigureEntityFramework(this WebApplicationBuilder builder)
    {
        var useDbType = builder.Configuration.GetValue<string>("UseDbType");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
            switch (useDbType)
            {
                //case "UseSqlServerDatabase" : options.UseSqlServer(connectionString); break;
                //case "UseSqlLite"           : options.UseSqlite(connectionString); break;
                case "UsePostgresDatabase"  : options.UseNpgsql(connectionString).ReplaceService<IHistoryRepository, SnakeCaseHistoryContext>(); break;
                default                     : options.UseInMemoryDatabase("TalentDb"); break;
            }
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }

    private static void ConfigureApplicationDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRisksProvider, RisksProvider>();

        // Validators
        builder.Services.AddScoped<IValidator<CreateRiskRequest>, CreateRiskRequestValidator>();
        builder.Services.AddScoped<IValidator<RiskDto>, RiskValidator>();
    }

    private static void ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddHealthChecks()
            .AddCheck<DefaultHealthCheck>("default");
    }
}
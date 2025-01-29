using System.Globalization;
using System.Text;
using Buisness.DependencyResolver;
using Buisness.Infrastructure.Factories;
using Core.DependencyResolver;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Configurations;
using WebApi.Filters;
using WebApi.Middlewares;
using WebApi.Services.Swagger; 

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options => options.Filters.Add(new LoggingFilter()))
    .AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBuinessService();

builder.Services.AddServiceFactories();

builder.Services.AddCoreService();

//Authentication Jwt 
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        };
    });

builder.Services.AddHangfire(config => config.UseMemoryStorage());
builder.Services.AddHangfireServer();

//Swager Gen
builder.Services.AddSwaggerGen(c =>
    {
        //Can you add new here like that 
        c.SwaggerDoc("public", new OpenApiInfo
        {
            Title = "Public API",
            Version = "v1"
        });

        c.DocInclusionPredicate((docName, apiDesc) =>
        {
            var multiGroupAttribute = apiDesc.ActionDescriptor
                .EndpointMetadata
                .OfType<SwaggerMultiGroupAttribute>()
                .FirstOrDefault();

            if (multiGroupAttribute != null)
            {
                if (multiGroupAttribute.GroupNames.Count() == 0)
                {
                    return true;
                }

                return multiGroupAttribute.GroupNames.Contains(docName);
            }

            return false;
        });

        c.CustomSchemaIds(type => type.FullName);
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });
builder.Services.AddCors(c =>
    {
        c.AddPolicy("AppCors", c =>
        {
            c.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
        });
    });
//Loclization Settings
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = LocalizationConfig.GetSupportedCultures();
        options.AddSupportedCultures(supportedCultures);
        options.AddSupportedUICultures(supportedCultures);

        options.FallBackToParentUICultures = true;

        options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
        {
            var defaultCulture = new CultureInfo(LocalizationConfig.GetDefaultCulture());
            return new ProviderCultureResult(defaultCulture.Name);
        }));
    });


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    //Can you add new here like that 
    c.SwaggerEndpoint("/swagger/public/swagger.json", "Public API");
    c.DefaultModelsExpandDepth(-1);
});

app.UseRouting();

app.UseHangfireDashboard("/hangfire");
#pragma warning disable CS0618 // Type or member is obsolete
app.UseHangfireServer();
#pragma warning restore CS0618 // Type or member is obsolete


app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AppCors");

var supportedCultures = LocalizationConfig.GetSupportedCultures();
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(LocalizationConfig.GetDefaultCulture()),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
};
app.UseRequestLocalization(localizationOptions);
app.UseMiddleware<LocalizationMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseStaticFiles();
app.Run();
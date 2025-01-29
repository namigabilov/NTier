using System.Threading.RateLimiting;
using DataAccess.Concrete.SQLServer;
using DataAccess.Profiles;
using DataAccess.Services;
using DataAccess.Services.Abstract;
using Entities.Concrete.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Buisness.DependencyResolver
{
    public static class ServiceRegistration
    {
        public static void AddBuinessService(this IServiceCollection service)
        {
            service.AddScoped<AppDbContext>();
            service.AddAutoMapper(typeof(MappingProfile));

            service.AddScoped<IDAtaAccessService, DataAccessService>();

            #region Localizations 

            service.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
            service.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

            #endregion

            service.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.Request.Headers.Host.ToString(), partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            AutoReplenishment = true,
                            Window = TimeSpan.FromSeconds(1)
                        });
                });
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later... ", cancellationToken: token);
                };
            });

            service.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                options.Lockout.MaxFailedAccessAttempts = 5;

            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }

    }
}

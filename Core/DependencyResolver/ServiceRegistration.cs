using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using Core.Helpers.Abstract;
using Core.Helpers.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyResolver
{
    public static class ServiceRegistration
    {
        public static void AddCoreService(this IServiceCollection service)
        {
            service.AddScoped<IEmailService, EmailManager>();

            service.AddScoped(typeof(IRepositoryBase<,,>), typeof(EFRepositoryBase<,,>));
        }
    }
}

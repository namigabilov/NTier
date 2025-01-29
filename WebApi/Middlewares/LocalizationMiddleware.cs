using System.Globalization;
using WebApi.Configurations;

namespace WebApi.Middlewares
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public LocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();

            if (!LocalizationConfig.GetSupportedCultures().Contains(acceptLanguage))
                acceptLanguage = LocalizationConfig.GetDefaultCulture();

            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                var culture = new CultureInfo(acceptLanguage);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            else
            {
                var culture = new CultureInfo(LocalizationConfig.GetDefaultCulture());
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }

            await _next(context);
        }
    }

}
using System.IdentityModel.Tokens.Jwt;

namespace Core.Helpers
{
    public static class Jwt
    {
        public static string DecodeJwt(this string? token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
            {
                return null;
            }
            var jwtToken = handler.ReadJwtToken(token);
            var type = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            if (type is null)
                return null;
            else
                return type.Value;
        }
    }
} 
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ParrotWings.WebAPI.Core.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // Константа ISSUER представляет издателя токена. Здесь можно определить любое название.

        public const string AUDIENCE = "http://localhost:51903/"; // UDIENCE представляет потребителя токена - опять же может быть любая строка, но в данном случае указан адрес текущего приложения.

        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации

        public const int LIFETIME = 1; // время жизни токена - 1 минута

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
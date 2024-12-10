using ViaAPI.Models;

namespace ViaAPI.Services.JwtService
{
    public interface IJwtService
    {
        string GerarJwt(ApplicationUser token);
    }
}

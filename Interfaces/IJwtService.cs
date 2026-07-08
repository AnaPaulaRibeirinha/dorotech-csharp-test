using DoroTech.Bookstore.Api.Entities;

namespace DoroTech.Bookstore.Api.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}

using YuyoDev.Domain.Entities;

namespace YuyoDev.Application.Interfaces;
public interface ITokenService {
    string CreateToken(ApplicationUser user, IList<string> roles);
    string GenerateToken(ApplicationUser user, IList<string> roles);
}
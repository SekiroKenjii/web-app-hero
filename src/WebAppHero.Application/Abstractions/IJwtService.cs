using System.Security.Claims;

namespace WebAppHero.Application.Abstractions;

public interface IJwtService
{
    string? GenerateAccessToken(IEnumerable<Claim> claims);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetClaimsPrincipalFormExpiredToken(string token);
}

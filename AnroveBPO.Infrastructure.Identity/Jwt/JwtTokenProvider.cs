using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AnroveBPO.Domain.Shared;
using AnroveBPO.Infrastructure.Identity.Models;
using CSharpFunctionalExtensions;
using Microsoft.IdentityModel.Tokens;

namespace AnroveBPO.Infrastructure.Identity.Jwt;

public static class CustomClaims
{
    public const string ID = "Id";

    public const string ROLE = "Role";

    public const string PERMISSION = "Permission";
}

public sealed class JwtTokenProvider(AuthOptions authOptions)
{
    private readonly AuthOptions _authOptions = authOptions;

    public Result<string, Error> GenerateAccessToken(
        User user,
        IReadOnlyCollection<string> roles,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_authOptions.SecretKey))
        {
            return Error.Failure("auth.secret.missing", "JWT SecretKey is missing");
        }

        var claims = new List<Claim>
        {
            new(CustomClaims.ID, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim(CustomClaims.ROLE, role));
        }

        var ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretKey));
        var signingCredentials = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _authOptions.Issuer,
            audience: _authOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_authOptions.TokenLifeTimeInMinutes),
            signingCredentials: signingCredentials);

        string stringToken = new JwtSecurityTokenHandler().WriteToken(token);

        return stringToken;
    }

    public Result<string, Error> GenerateRefreshToken(User user, CancellationToken ct = default)
    {
        if (user.Id == Guid.Empty)
        {
            return Error.Validation("user.id.invalid", "User id is invalid");
        }

        byte[] refreshTokenBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(refreshTokenBytes);
    }
}
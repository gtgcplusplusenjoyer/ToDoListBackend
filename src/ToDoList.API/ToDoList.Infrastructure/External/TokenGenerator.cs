using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoList.Core.Entities.User;
using ToDoList.Core.Interfaces.External;
using ToDoList.Core.Tokens;
using ToDoList.Infrastructure.Settings;

namespace ToDoList.Infrastructure.External
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly AuthSettings _settings;
        public TokenGenerator(IOptions<AuthSettings> settings)
        {
            _settings = settings.Value;
        }
        public TokenPair GenerateTokenPair(User user)
        {
            var access = GenerateAccessToken(user);
            var refresh = GenerateRefreshToken();

            return new TokenPair
            {
                AccessToken = access,
                RefreshToken = refresh,
            };
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken
            (
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}

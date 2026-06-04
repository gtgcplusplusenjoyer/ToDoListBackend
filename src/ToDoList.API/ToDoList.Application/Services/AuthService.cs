using Microsoft.Extensions.Options;
using ToDoList.Application.Dto.User;
using ToDoList.Application.Interfaces;
using ToDoList.Core.Entities.User;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Interfaces.External;
using ToDoList.Infrastructure.Settings;

namespace ToDoList.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AuthSettings _settings;
        public AuthService(
            IUserRepository userRepository,
            ITokenGenerator tokenGenerator,
            IPasswordHasher passwordHasher,
            IRefreshTokenRepository refreshTokenRepository,
            IOptions<AuthSettings> settings)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _settings = settings.Value;
        }

        public async Task<AuthResult> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(loginUserDto.Email, cancellationToken);

            if (user == null)
            {
                return AuthResult.Failure("User with this email not exist");
            }

            if (!_passwordHasher.VerifyPassword(loginUserDto.Password, user.PasswordHash))
            {
                return AuthResult.Failure("Invalid email or password");
            }

            var tokenPair = _tokenGenerator.GenerateTokenPair(user);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = tokenPair.RefreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays),
                IsRevoked = false,
            };

            await _refreshTokenRepository.CreateAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return AuthResult.Success(tokenPair, user.Id);
        }

        public async Task<AuthResult> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null)
            {
                return AuthResult.Failure("Refresh token is not found");
            }

            if (storedToken.IsRevoked == true)
            {
                return AuthResult.Failure("Refresh token has been revoked");
            }

            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return AuthResult.Failure("Refresh token has expired");
            }

            var user = await _userRepository.GetUserById(storedToken.UserId, cancellationToken);

            if (user == null)
            {
                return AuthResult.Failure("User with this id is not found");
            }

            await _refreshTokenRepository.RevokeAsync(storedToken.Id);

            var newTokenPair = _tokenGenerator.GenerateTokenPair(user);

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newTokenPair.RefreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays),
                IsRevoked = false
            };

            await _refreshTokenRepository.CreateAsync(newRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return AuthResult.Success(newTokenPair, user.Id);
        }

        public async Task<AuthResult> RegisterAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByEmail(registerUserDto.Email, cancellationToken);

            if (existingUser != null)
            {
                return AuthResult.Failure("User with this email already exist");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                PasswordHash = _passwordHasher.HashPassword(registerUserDto.Password)
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);

            var tokenPair = _tokenGenerator.GenerateTokenPair(user);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = tokenPair.RefreshToken,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays),
                IsRevoked = false,
            };

            await _refreshTokenRepository.CreateAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return AuthResult.Success(tokenPair, user.Id);
        }

        public async Task<AuthResult> LogoutAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserById(id, cancellationToken);

            if (user == null)
            {
                return AuthResult.Failure("User not found");
            }

            await _refreshTokenRepository.RevokeAllTokensByUserId(id, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return AuthResult.Success(user.Id);
        }
    }
}

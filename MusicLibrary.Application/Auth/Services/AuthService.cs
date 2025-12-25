using MusicLibrary.Application.Auth.Commands;
using MusicLibrary.Application.Auth.Interfaces;
using MusicLibrary.Domain.Entities;

namespace MusicLibrary.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IEmailConfirmationTokenRepository _tokens;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfirmationTokenGenerator _tokenGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            IUserRepository users,
            IEmailConfirmationTokenRepository tokens,
            IPasswordHasher passwordHasher,
            IConfirmationTokenGenerator tokenGenerator,
            IEmailSender emailSender, IJwtTokenGenerator jwtTokenGenerator)
        {
            _users = users;
            _tokens = tokens;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _emailSender = emailSender;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task RegisterAsync(RegisterUserCommand command, CancellationToken ct = default)
        {
            var existingUser = await _users.GetByEmailAsync(command.Email, ct);

            if (existingUser is not null && existingUser.EmailConfirmed)
                throw new InvalidOperationException("A user with this email already exists.");

            User user;

            if (existingUser is null)
            {
                user = new User
                {
                    Email = command.Email,
                    PasswordHash = _passwordHasher.Hash(command.Password),
                    EmailConfirmed = false,
                };

                await _users.AddAsync(user, ct);
            }
            else
            {
                user = existingUser;
            }

            var tokenValue = _tokenGenerator.Generate();

            var token = new EmailConfirmationToken
            {
                UserId = user.Id,
                Token = tokenValue,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            await _tokens.AddAsync(token, ct);

            await _users.SaveChangeAsync(ct);
            await _tokens.SaveChangeAsync(ct);

            var confirmationLink = $"https://localhost:5001/api/auth/confirm-email?token={tokenValue}";

            await _emailSender.SendRegistrationConfirmationAsync(
                user.Email,
                confirmationLink,
                ct
             );

        }

        public async Task ConfirmEmailAsync(ConfirmEmailCommand command, CancellationToken ct = default)
        {
            var token = await _tokens.GetByTokenAsync(command.Token, ct);

            if (token is null)
                throw new InvalidOperationException("Invalid confirmation token.");

            if (token.Used)
                throw new InvalidOperationException("This confirmation token has already been used.");

            if (token.ExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("This confirmation token has expired.");

            var user = await _users.GetByIdAsync(token.UserId, ct);

            if (user is null)
                throw new InvalidOperationException("User associated with this token does not exist.");

            user.EmailConfirmed = true;
            token.Used = true;

            await _users.SaveChangeAsync(ct);
            await _tokens.SaveChangeAsync(ct);
        }

        public async Task<string> LoginAsync(LoginCommand command, CancellationToken ct = default)
        {
            var user = await _users.GetByEmailAsync(command.Email, ct);

            if (user is null || !user.EmailConfirmed)
                throw new InvalidOperationException("Invalid email or password.");

            if (!_passwordHasher.Verify(command.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password.");

            return _jwtTokenGenerator.GenerateToken(user);

        }
    }
}

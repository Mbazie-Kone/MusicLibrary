using MusicLibrary.Application.Auth.Interfaces;
using System.Security.Cryptography;

namespace MusicLibrary.Infrastructure.Tokens
{
    public class ConfirmationTokenGenerator : IConfirmationTokenGenerator
    {
        public string Generate()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }
    }
}

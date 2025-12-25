using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Application.Auth.Commands;
using MusicLibrary.Application.Auth.Interfaces;

namespace MusicLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Dtos.RegisterRequestDto dto, CancellationToken ct)
        {
            try
            {
                var command = new RegisterUserCommand(dto.Email, dto.Password);
                await _authService.RegisterAsync(command, ct);

                return Ok("Registration started. Check your email.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token,  CancellationToken ct)
        {
            try
            {
                var command = new ConfirmEmailCommand(token);
                await _authService.ConfirmEmailAsync(command, ct);

                return Ok("Email confirmed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using DSD.Core.Entities;
using DSD.Data;
using DSD.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace DSD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;

        // simple in-memory token blacklist for logout (dev only)
        private static readonly ConcurrentDictionary<string, DateTime> BlacklistedTokens = new();

        public AuthController(AppDbContext db, IPasswordHasher hasher, ITokenService tokenService)
        {
            _db = db;
            _hasher = hasher;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("username and password required");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username && u.IsActive);
            if (user == null) return Unauthorized("invalid credentials");

            if (!_hasher.Verify(req.Password, user.PasswordHash))
                return Unauthorized("invalid credentials");

            var token = _tokenService.BuildToken(user);

            return Ok(new
            {
                token,
                expiresInHours = 24,
                user = new { user.UserId, user.Username, user.FullName, user.Email, user.Role }
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Extract JTI or token string and blacklist it.
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                BlacklistedTokens[token] = DateTime.UtcNow.AddHours(24);
            }
            return Ok();
        }

        [Authorize]
        [HttpGet("ping")]
        public IActionResult PingAuth()
        {
            // if token is blacklisted, deny
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token) && BlacklistedTokens.ContainsKey(token))
                return Unauthorized();

            return Ok(new { online = true, user = User.Identity?.Name ?? "" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

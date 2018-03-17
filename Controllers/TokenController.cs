using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using authentication_service.Models;
using authentication_service.Repositories;
using authentication_service.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace authentication_service.Controllers
{
    [Route("api/v1/[controller]")]
    public class TokenController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration, IUserRepository repository)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // POST api/v1/token
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RequestToken([FromBody] User user)
        {
            if (!await _repository.UsernameExists(user.Username))
            {
                ModelState.AddModelError("LoginCredentials", String.Format("Login credentials invalid"));
                return NotFound(ModelState);
            }

            // Will fix this later, have another method handle the claimsidentity

            User userEntity = await _repository.GetByUsernameAsync(user.Username);

            string check_hash = Encryption.GenerateSHA256Hash(user.Password, userEntity.Salt);
            List<string> permissions = AutoMapper.Mapper.Map<User, UserViewModel>(userEntity).UserPermission;

            if (check_hash == userEntity.Password)
            {
                userEntity.LastLoginTimestamp = DateTime.UtcNow;
                await _repository.UpdateUserAsync(userEntity.Id, userEntity);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, userEntity.Username),
                    new Claim(ClaimTypes.Email, userEntity.Email)
                };
                var test = _configuration.GetSection("Encryption").GetSection("SecretKey").Value;

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Encryption").GetSection("SecretKey").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "localhost",
                    audience: "localhost",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify login credentials");
        }
    }
}
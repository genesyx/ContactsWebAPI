using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contacts.Web.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Contacts.Web.API.Models.ContextConfiguration;

namespace Contacts.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ContactsWebAPIContext _context;

        private IConfiguration Configuration;

        public AuthenticationController(ContactsWebAPIContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        /// <summary>
        /// Method that returns a valid token
        /// </summary>
        /// <param name="credentials">credentials with login and password</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login([FromBody]CredentialsDTO credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Login) || string.IsNullOrWhiteSpace(credentials.Password)) return BadRequest();

            var contact = _context.ContactItems.Where(c => c.Email == credentials.Login && c.Password == credentials.Password).SingleOrDefaultAsync();

            if (contact == null) return Unauthorized();

            string tokenString = CreateToken(credentials.Login);

            return Ok(new { token = tokenString });
        }

        /// <summary>
        /// Method that create a valid token
        /// </summary>
        /// <param name="email">identity</param>
        /// <returns></returns>
        private string CreateToken(string email)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(Configuration["jwt:issuer"],
                                             Configuration["jwt:issuer"],
                                             claims: new[]
                                             {
                                                 new Claim(JwtRegisteredClaimNames.Email, email)
                                             },
                                             expires: DateTime.Now.AddMinutes(30),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contacts.Web.API.Models;
using Contacts.Web.API.Models.DTO;

namespace Contacts.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ContactsWebAPIContext _context;

        public AuthenticationController(ContactsWebAPIContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login(CredentialsDTO credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Login) || string.IsNullOrWhiteSpace(credentials.Password)) return BadRequest();

            var contact = await _context.ContactItems.Where(c => c.Email == credentials.Login && c.Password == credentials.Password).SingleOrDefaultAsync();

            if (contact == null)
            {
                return Unauthorized();
            }

            // TODO Générer un token jwt
            string token = "mon_token_est_ok";

            // TODO interceptor pour l'autorisation

            return token;
        }
    }
}

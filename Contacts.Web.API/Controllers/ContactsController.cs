using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contacts.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Contacts.Web.API.Models.ContextConfiguration;
using System.Security.Claims;
using System.Net;
using Contacts.Web.API.Models.DTO;
using Contacts.Web.API.Helpers;

namespace Contacts.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactsController : ControllerBase
    {
        private readonly ContactsWebAPIContext _context;

        public ContactsController(ContactsWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContactItems()
        {
            List<ContactDTO> result = new List<ContactDTO>();

            List<Contact> contactsList = await _context.ContactItems.Include(c => c.SkillsList).ToListAsync();
            foreach (var contact in contactsList)
            {
                result.Add(ContactHelper.ConvertContactToContactDTO(contact));
            }
            return result;
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDTO>> GetContact([FromRoute] long id)
        {
            Contact contact = _context.ContactItems.Include(c => c.SkillsList).Where(c => c.Id == id).SingleOrDefaultAsync().Result;

            if (contact == null)
            {
                return NotFound();
            }

            return ContactHelper.ConvertContactToContactDTO(contact);
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact([FromRoute] long id, [FromBody] ContactDTO contactDTO)
        {
            // Authorization
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            string currentEmailConnected = claims[0].Value;
            if (contactDTO.Email.ToLower() != currentEmailConnected.ToLower()) return StatusCode((int)HttpStatusCode.Unauthorized, "Sorry! You can't change data of another contact than yourself...");

            if (id != contactDTO.Id)
            {
                return BadRequest();
            }

            // update fields
            Contact contact = _context.ContactItems.FindAsync(contactDTO.Id).Result;
            if (contact == null) return StatusCode((int)HttpStatusCode.NotFound, $"We can't find a contact with id {contactDTO.Id}...");

            contact.FirstName = contactDTO.FirstName;
            contact.LastName = contactDTO.LastName;
            contact.FullName = contactDTO.FullName;
            contact.Address = contactDTO.Address;
            contact.Email = contactDTO.Email;
            contact.Mobile = contactDTO.Mobile;

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact([FromBody] ContactDTO contactDTO)
        {
            Contact contact = ContactHelper.ConvertContactDTOToContact(contactDTO);
            _context.ContactItems.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.Id }, ContactHelper.ConvertContactToContactDTO(contact));
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactDTO>> DeleteContact([FromRoute] long id)
        {
            var contact = await _context.ContactItems.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            // Authorization
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            string currentEmailConnected = claims[0].Value;
            if (contact.Email.ToLower() != currentEmailConnected.ToLower()) return StatusCode((int)HttpStatusCode.Unauthorized, "Sorry! You can't delete a contact that isn't yours...");

            _context.ContactItems.Remove(contact);
            await _context.SaveChangesAsync();

            return ContactHelper.ConvertContactToContactDTO(contact);
        }

        private bool ContactExists(long id)
        {
            return _context.ContactItems.Any(e => e.Id == id);
        }
    }
}

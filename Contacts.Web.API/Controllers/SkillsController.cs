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

namespace Contacts.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SkillsController : ControllerBase
    {
        private readonly ContactsWebAPIContext _context;

        public SkillsController(ContactsWebAPIContext context)
        {
            _context = context;
        }

        // GET: api/Skills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillDTO>>> GetSkillItems()
        {
            List<SkillDTO> result = new List<SkillDTO>();

            List<Skill> skillsList = await _context.SkillItems.Include(s => s.Contact).ToListAsync();
            foreach (var skill in skillsList)
            {
                result.Add(new SkillDTO() { Id = skill.Id, idContact = skill.Contact != null ? skill.Contact.Id : 0, Level = skill.Level, Name = skill.Name });
            }

            return result;
        }

        // GET: api/Skills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SkillDTO>> GetSkill([FromRoute] long id)
        {
            Skill skill = _context.SkillItems.Include(s => s.Contact).Where(s => s.Id == id).SingleOrDefaultAsync().Result;

            if (skill == null)
            {
                return NotFound();
            }

            return new SkillDTO() { Id = skill.Id, idContact = skill.Contact.Id, Level = skill.Level, Name = skill.Name };
        }

        // PUT: api/Skills/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkill([FromRoute] long id, [FromBody] SkillDTO skillDTO)
        {
            Skill skillWithContact = _context.SkillItems.Include(s => s.Contact).Where(s => s.Id == skillDTO.Id).SingleOrDefaultAsync().Result;

            if (skillWithContact == null) return StatusCode((int)HttpStatusCode.NotFound, $"Cannot find a skill with id {id}...");

            if (skillWithContact.Contact.Id != skillDTO.idContact) return StatusCode((int)HttpStatusCode.BadRequest, $"You can't update the contact of the skill...");

            // Authorization
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            string currentEmailConnected = claims[0].Value;
            if (skillWithContact.Contact?.Email.ToLower() != currentEmailConnected.ToLower()) return StatusCode((int)HttpStatusCode.Unauthorized, "Sorry! You can't change data of a skill that isn't yours...");

            if (id != skillWithContact.Id)
            {
                return BadRequest();
            }

            // update fields
            skillWithContact.Level = skillDTO.Level;
            skillWithContact.Name = skillDTO.Name;
            _context.Entry(skillWithContact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillExists(id))
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

        // POST: api/Skills
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Skill>> PostSkill([FromBody] SkillDTO skillDTO)
        {
            if (skillDTO.idContact == 0) return StatusCode((int)HttpStatusCode.BadRequest, $"A skill must have a contact...");
            Contact contact = _context.ContactItems.FindAsync(skillDTO.idContact).Result;

            if (contact == null) return StatusCode((int)HttpStatusCode.NotFound, $"We can't find a contact with id {skillDTO.idContact}...");

            Skill skill = new Skill() { Level = skillDTO.Level, Name = skillDTO.Name };
            skill.Contact = contact; // contact association
            _context.SkillItems.Add(skill);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSkill", new { id = skill.Id }, skill);
        }

        // DELETE: api/Skills/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Skill>> DeleteSkill([FromRoute] long id)
        {
            var skill = await _context.SkillItems.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }

            // Authorization
            Skill skillWithContact = _context.SkillItems.Include(s => s.Contact).Where(s => s.Id == id).SingleOrDefaultAsync().Result;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            string currentEmailConnected = claims[0].Value;
            if (skillWithContact.Contact?.Email.ToLower() != currentEmailConnected.ToLower()) return StatusCode((int)HttpStatusCode.Unauthorized, "Sorry! You can't delete a skill that isn't yours...");

            _context.SkillItems.Remove(skill);
            await _context.SaveChangesAsync();

            return skill;
        }

        private bool SkillExists(long id)
        {
            return _context.SkillItems.Any(e => e.Id == id);
        }
    }
}

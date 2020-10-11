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
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkillItems() 
        {
            return await _context.SkillItems.ToListAsync();
        }

        // GET: api/Skills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetSkill(long id)
        {
            var skill = await _context.SkillItems.FindAsync(id);

            if (skill == null)
            {
                return NotFound();
            }

            return skill;
        }

        // PUT: api/Skills/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkill(long id, Skill skill)
        {
            Skill skillWithContact = _context.SkillItems.Include(s => s.Contact).Where(s => s.Id == id).SingleOrDefaultAsync().Result;

            if (skillWithContact == null) return StatusCode((int)HttpStatusCode.NotFound, $"Cannot find a skill with id {id}...");

            // Authorization
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claims = identity.Claims.ToList();
            string currentEmailConnected = claims[0].Value;
            if (skillWithContact.Contact?.Email.ToLower() != currentEmailConnected.ToLower()) return StatusCode((int)HttpStatusCode.Unauthorized, "Sorry! You can't change data of a skill that isn't yours...");

            if (id != skill.Id)
            {
                return BadRequest();
            }

            _context.Entry(skill).State = EntityState.Modified;

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
        public async Task<ActionResult<Skill>> PostSkill(Skill skill)
        {
            _context.SkillItems.Add(skill);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSkill", new { id = skill.Id }, skill);
        }

        // DELETE: api/Skills/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Skill>> DeleteSkill(long id)
        {
            var skill = await _context.SkillItems.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }

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

namespace Contacts.Web.API.Models.DTO
{
    public class SkillDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public long idContact { get; set; }
    }
}

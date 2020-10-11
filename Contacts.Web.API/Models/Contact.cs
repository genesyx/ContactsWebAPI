using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Web.API.Models
{
    [Table("Contacts")]
    public class Contact : AbstractModel
    {
        [Required(ErrorMessage = "Your first name is required")]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Your last name is required")]
        [MaxLength(25)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string FullName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "An email address is required")]
        [MaxLength(255)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        public List<Skill> SkillsList { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [MaxLength(255)]
        [JsonIgnore] // pour ne pas renvoyer le password crypter (même s'il sera crypter côté client)
        public string Password { get; set; }
    }
}

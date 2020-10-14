using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contacts.Web.API.Models
{
    [Table("Skills")]
    public class Skill : AbstractModel
    {
        [Required(ErrorMessage = "The skill name is required")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Compris entre 1 et 10
        /// </summary>
        [Range(1, 10)]
        [Required(ErrorMessage = "A level value is required")]
        public int Level { get; set; }

        /// <summary>
        /// Contact associé
        /// </summary>
        [Required(ErrorMessage = "A contact is required to create or update a skill")]
        public Contact Contact { get; set; }
    }
}

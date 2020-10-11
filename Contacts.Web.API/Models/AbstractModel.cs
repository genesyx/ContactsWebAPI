using System.ComponentModel.DataAnnotations;

namespace Contacts.Web.API.Models
{
    public abstract class AbstractModel
    {
        [Key]
        public long Id { get; set; }
    }
}

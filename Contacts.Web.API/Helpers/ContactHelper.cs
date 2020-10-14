using Contacts.Web.API.Models;
using Contacts.Web.API.Models.DTO;
using System.Collections.Generic;

namespace Contacts.Web.API.Helpers
{
    public static class ContactHelper
    {
        public static ContactDTO ConvertContactToContactDTO(Contact contact)
        {
            ContactDTO result = new ContactDTO()
            {
                Address = contact.Address,
                Email = contact.Email,
                FirstName = contact.FirstName,
                FullName = contact.FullName,
                Id = contact.Id,
                LastName = contact.LastName,
                Mobile = contact.Mobile
            };

            if(contact.SkillsList != null && contact.SkillsList.Count > 0)
            {
                result.SkillsList = new List<SkillDTO>();
                foreach (var skill in contact.SkillsList)
                {
                    result.SkillsList.Add(new SkillDTO()
                    {
                        Id = skill.Id,
                        Level = skill.Level,
                        Name = skill.Name,
                        idContact = result.Id
                    });
                }
            }

            return result;
        }

        public static Contact ConvertContactDTOToContact(ContactDTO contactDTO)
        {
            Contact result = new Contact()
            {
                Address = contactDTO.Address,
                Email = contactDTO.Email,
                FirstName = contactDTO.FirstName,
                FullName = contactDTO.FullName,
                Id = contactDTO.Id,
                LastName = contactDTO.LastName,
                Mobile = contactDTO.Mobile,
                Password = contactDTO.Password
            };

            if (contactDTO.SkillsList != null && contactDTO.SkillsList.Count > 0)
            {
                result.SkillsList = new List<Skill>();
                foreach (var skill in contactDTO.SkillsList)
                {
                    result.SkillsList.Add(new Skill()
                    {
                        Id = skill.Id,
                        Level = skill.Level,
                        Name = skill.Name
                    });
                }
            }

            return result;
        }
    }
}

﻿namespace Contacts.Web.API.Models.DTO
{
    public class UpdatableContactDTO : AbstractModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}

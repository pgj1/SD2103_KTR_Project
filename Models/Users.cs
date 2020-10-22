using System;
using System.Collections.Generic;

namespace KTR.Models
{
    public partial class Users
    {
        public Users()
        {
            Recipes = new HashSet<Recipes>();
        }

        public int UserId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string RegId { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}

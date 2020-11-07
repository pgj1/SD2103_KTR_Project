using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace KTR.Models
{
    public partial class Users
    {
        public Users()
        {
            Recipes = new HashSet<Recipes>();
        }

        
        [DisplayName("User Id")]
        public int UserId { get; set; }

        [DisplayName("First Name")]
        public string Fname { get; set; }

        [DisplayName("Last Name")]
        public string Lname { get; set; }

        public string Email { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Registration Id")]
        public string RegId { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}

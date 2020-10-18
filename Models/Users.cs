using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KTR.Models
{
    public partial class Users
    {
        public Users()
        {
            Recipes = new HashSet<Recipes>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Lname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public string RegId { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}

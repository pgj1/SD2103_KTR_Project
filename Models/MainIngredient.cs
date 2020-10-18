using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KTR.Models
{
    public partial class MainIngredient
    {
        public MainIngredient()
        {
            Recipes = new HashSet<Recipes>();
        }

        [Key]
        public int MainId { get; set; }
        public string MainName { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}

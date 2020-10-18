using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KTR.Models
{
    public partial class RecipeStatus
    {
        public RecipeStatus()
        {
            Recipes = new HashSet<Recipes>();
        }

        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KTR.Models
{
    public partial class RecipeCategory
    {
        public RecipeCategory()
        {
            Recipes = new HashSet<Recipes>();
        }

        [Key]
        public int CategoryId { get; set; }
        public string CatName { get; set; }

        public ICollection<Recipes> Recipes { get; set; }
    }
}

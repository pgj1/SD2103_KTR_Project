using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KTR.Models
{
    public partial class Preparation
    {
        [Key]
        public int PrepId { get; set; }
        public int Step { get; set; }
        public string Instr { get; set; }
        public int RecipeId { get; set; }
        public DateTime Updated { get; set; }

        public Recipes Recipe { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace KTR.Models
{
    public partial class Preparation
    {
        public int PrepId { get; set; }
        public int Step { get; set; }
        public string Instr { get; set; }
        public int RecipeId { get; set; }
        public DateTime Updated { get; set; }

        public Recipes Recipe { get; set; }
    }
}

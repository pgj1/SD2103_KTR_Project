using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KTR.Models
{
    public partial class RecipeAuditLog
    {
        [Key]
        public int RecipeAuditId { get; set; }
        public int? RecipeId { get; set; }
        public string Rname { get; set; }
        public string RnameOld { get; set; }
        public int? Servings { get; set; }
        public int? ServingsOld { get; set; }
        public int? CategoryId { get; set; }
        public int? CategoryIdOld { get; set; }
        public int? StatusId { get; set; }
        public int? StatusIdOld { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int? MainId { get; set; }
        public int? MainIdOld { get; set; }
        public string Description { get; set; }
        public string DescriptionOld { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoPathOld { get; set; }
    }
}

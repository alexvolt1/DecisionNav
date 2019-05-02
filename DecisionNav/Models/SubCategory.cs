using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DecisionNav.Models
{
    public class SubCategory
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Camp Name")]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Column(TypeName = "nvarchar(32)")]
        //[Remote("doesSubCategoryExistAsync", "SubCategories", HttpMethod = "POST", ErrorMessage = "Camp with this name already exists.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Age From")]
        public int AgeFrom { get; set; }

        [Required]
        [Display(Name = "Age To")]
        public int AgeTo { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    } 
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeTogetherNG.Models
{
    public class AddProjectViewModel
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(3, ErrorMessage = "Title has a minimum length of 3.")]
        [MaxLength(50, ErrorMessage = "Title has a maximum length of 50.")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Description is required.")]
        [MinLength(20, ErrorMessage = "Description has a minimum length of 20.")]
        [MaxLength(1000, ErrorMessage = "Description has a maximum length of 1000.")]
        public string Description { get; set; }

        public List<int> TechList { get; set; }
    }
}
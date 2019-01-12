using System.ComponentModel.DataAnnotations;

namespace CodeTogetherNG.Models
{
    public class AddProjectViewModel
    {
            [Display(Name = "Title")]
            [Required(ErrorMessage = "Title is required.")]
            [MaxLength(50, ErrorMessage = "Title has a maximum length of 50.")]
        public string Title { get; set; }

            [Display(Name = "Description")]
            [MaxLength(1000, ErrorMessage = "Description has a maximum length of 1000.")]
        public string Description { get; set; }
    }
}
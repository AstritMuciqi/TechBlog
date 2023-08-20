


using System.ComponentModel.DataAnnotations;

namespace TechBlogApp.Domain.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "FirstName is required")]
        public string? FirstName { get; set; }
        
        [Display(Name = "LastName")]
        [Required(ErrorMessage = "LastName is required")]
        public string? LastName { get; set; }       

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string? EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "Confrim Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string? ConfirmPassword { get; set; }
    }
}

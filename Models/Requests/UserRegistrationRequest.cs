using System.ComponentModel.DataAnnotations;

namespace WebProject.Models.Requests
{
    public class UserRegistrationRequest
    {
        [Required(ErrorMessage = "Please Enter your Name")]
        public string Name {  get; set; }

        [Required(ErrorMessage = "Please Enter your email adress")]
        [RegularExpression(@"[A-Za-z0-9.%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter your password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password")]
        [Compare("Password" ,ErrorMessage = "Password mismatch")]
        public string ConfirmPassword { get; set; }
    }
}

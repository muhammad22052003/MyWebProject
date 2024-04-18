using System.ComponentModel.DataAnnotations;

namespace WebProject.Models.Requests
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Please enter your Email")]
        [RegularExpression(@"[A-Za-z0-9.%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set;}

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set;}
    }
}

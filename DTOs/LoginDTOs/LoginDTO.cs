using System.ComponentModel.DataAnnotations;

namespace STEPIFY.DTOs.RequestDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Please Enter your Email")]
        [EmailAddress]

        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter your Password")]

        public string Password { get; set; }
    }
}

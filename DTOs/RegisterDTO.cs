using System.ComponentModel.DataAnnotations;

namespace STEPIFY.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name id Required")]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(10, ErrorMessage = "PhoneNo number must be 0-10")]
        public string PhoneNo { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}

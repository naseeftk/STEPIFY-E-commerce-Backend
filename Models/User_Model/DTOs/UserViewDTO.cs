using System.ComponentModel.DataAnnotations;

namespace STEPIFY.Models.User_Model.DTOs
{
    public class UserViewDTO
    {

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public bool IsBlocked { get; set; }

        public string Role { get; set; }
    }
}

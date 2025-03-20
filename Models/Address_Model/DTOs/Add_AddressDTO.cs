using System.ComponentModel.DataAnnotations;

namespace STEPIFY.Models.Address_Model.DTOs
{
    public class Add_AddressDTO
    {

        [Required(ErrorMessage = "Full name is required")]

        public string? FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }


        public string? Pincode { get; set; }

        [Required(ErrorMessage = "House name is required")]

        public string? HouseName { get; set; }

        [Required(ErrorMessage = "Place is required")]

        public string? Place { get; set; }



        [Required(ErrorMessage = "Land mark is required")]

        public string? LandMark { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace STEPIFY.Models.Address_Model.DTOs
{
    public class AddressViewDTO
    {

        public int Id { get; set; }
        public string? FullName { get; set; }


        public string? PhoneNumber { get; set; }

        public string? Pincode { get; set; }


        public string? HouseName { get; set; }

        public string? Place { get; set; }




        public string? LandMark { get; set; }

    }
}

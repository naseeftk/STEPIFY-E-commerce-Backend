using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Models.Address_Model.DTOs;
namespace STEPIFY.Interfaces.IAddress
{
    public interface IAddressService
    {
        Task<ResultDTO<List<AddressViewDTO>>> GetAllAddress(int userId);
        Task<ResultDTO<string>> Add_Address(int userId, Add_AddressDTO addAddress);
        Task<ResultDTO<string>> RemoveAddress(int userId, int addressId);
    }
}

using STEPIFY.Models.Address_Model;

namespace STEPIFY.Interfaces.IAddress
{
    public interface IAddressRepo
    {
        Task<List<Address>> GetAllAddress(int userId);
        Task Add_Address(Address newAddress);
        Task<Address?> GetAddressById(int userId, int addressId);
        //Task<bool> IsAddressUsedInOrders(int addressId);
        Task<bool> HasMaxAddresses(int userId);
        Task RemoveAddress(int addressId);
    }
}

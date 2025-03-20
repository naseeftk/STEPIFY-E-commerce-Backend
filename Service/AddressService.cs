using AutoMapper;
using Serilog;
using STEPIFY.DTOs.ResultDTO;
using STEPIFY.Interfaces.IAddress;
using STEPIFY.Models.Address_Model;
using STEPIFY.Models.Address_Model.DTOs;
using STEPIFY.Repositories;
using STEPIFY.Service;

namespace STEPIFY.Services
{


    public class AddressService : IAddressService
    {
        private readonly IAddressRepo _addressRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;   

        public AddressService(IAddressRepo addressRepo, IMapper mapper, ILogger<ProductService> logger)
        {
            _addressRepo = addressRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResultDTO<List<AddressViewDTO>>> GetAllAddress(int userId)
        {
            try
            {
                var userAddresses = await _addressRepo.GetAllAddress(userId);
                if (!userAddresses.Any())
                {
                    return new ResultDTO<List<AddressViewDTO>>
                    {
                        Message = "Your address list is empty. Please add an address.",
                        StatusCode = 404
                    };
                }

                var mappedAddresses = _mapper.Map<List<AddressViewDTO>>(userAddresses);
                return new ResultDTO<List<AddressViewDTO>>
                {
                    Data = mappedAddresses,
                    StatusCode = 200,
                    Message = "Addresses retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving addresses.");
                return new ResultDTO<List<AddressViewDTO>>
                {
                    StatusCode = 500,
                    Message = "An error occurred while fetching addresses.",

                };
            }
        }

        public async Task<ResultDTO<string>> Add_Address(int userId, Add_AddressDTO addAddress)
        {
            try
            {
                
                if (await _addressRepo.HasMaxAddresses(userId) )
                {
                    return new ResultDTO<string>
                    {
                        StatusCode = 400,
                        Message = "Maximum limit of addresses reached."
                    };
                }

                var newAddress = _mapper.Map<Address>(addAddress);
                newAddress.UserId = userId;

                await _addressRepo.Add_Address(newAddress);

                return new ResultDTO<string>
                {
                    StatusCode = 201,
                    Message = "Address added successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding an address.");
                return new ResultDTO<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while adding the address.",
                
                };
            }
        }

        public async Task<ResultDTO<string>> RemoveAddress(int userId, int addressId)
        {
            try
            {
                var userAddress = await _addressRepo.GetAddressById(userId, addressId);
                if (userAddress == null)
                {
                    return new ResultDTO<string>
                    {
                        StatusCode = 404,
                        Message = "Address not found."
                    };
                }


                //bool isAddressUsed = await _addressRepo.IsAddressUsedInOrders(addressId);
                //if (isAddressUsed)
                //{
                //    return new ResultDTO<string>
                //    {
                //        StatusCode = 400,
                //        Message = "Address cannot be deleted as it is linked to an order."
                //    };
                //}

                await _addressRepo.RemoveAddress( addressId);

                return new ResultDTO<string>
                {
                    StatusCode = 200,
                    Message = "Address removed successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing an address.");
                return new ResultDTO<string>
                {
                    StatusCode = 500,
                    Message = "An error occurred while removing the address.",
   
                };
            }
        }
    }
}

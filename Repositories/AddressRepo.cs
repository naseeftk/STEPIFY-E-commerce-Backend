using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STEPIFY.DTOs;
using STEPIFY.Interfaces.IAddress;
using STEPIFY.Models.Address_Model;

namespace STEPIFY.Repositories
{

    public class AddressRepo : IAddressRepo
    {
        private readonly StepifyDbContext _context;

        public AddressRepo(StepifyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Address>> GetAllAddress(int userId)
        {
            try
            {
                return await _context.Address.Where(a => a.UserId == userId && a.IsDeleted==false).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching addresses", ex);
            }
        }

        public async Task Add_Address(Address newAddress)
        {
            try
            {
                _context.Address.Add(newAddress);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding new address", ex);
            }
        }

        public async Task<Address?> GetAddressById(int userId, int addressId)
        {
            try
            {
                return await _context.Address
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.Id == addressId && a.IsDeleted==false);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching address by ID", ex);
            }
        }

        //public async Task<bool> IsAddressUsedInOrders(int addressId)
        //{
        //    try
        //    {
        //        return await _context.Order.AnyAsync(o => o.AddressId == addressId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error checking if address is linked to orders", ex);
        //    }
        //}

        public async Task<bool> HasMaxAddresses(int userId)
        {
            try
            {
                return await _context.Address
                    .Where(a => a.UserId == userId && a.IsDeleted==false)
                    .CountAsync() >= 3;
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking max addresses limit", ex);
            }
        }

        public async Task RemoveAddress(int addressId)
        {
            try
            {
                 var address=await _context.Address
                      .FirstOrDefaultAsync(a => a.Id == addressId );
                if (address!=null)
                {
                    address.IsDeleted = true;
                    await _context.SaveChangesAsync();
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing address", ex);
            }
        }
    }
}

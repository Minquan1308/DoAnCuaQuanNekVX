using Microsoft.AspNetCore.Mvc;
using TH_Lap3.Models;

namespace TH_Lap3.Repositories
{
    public interface IOrderRepository
    {
        //Task<IEnumerable<Order>> GetAllAsync();
        //Task<Order> GetByIdAsync(int id);
        ////Task AddAsync(Product product);
        ////Task UpdateAsync(Product product);
        ////Task DeleteAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);

    }
}

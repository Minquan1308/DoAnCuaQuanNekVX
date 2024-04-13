using Microsoft.EntityFrameworkCore;
using TH_Lap3.DataAccess;
using TH_Lap3.Models;

namespace TH_Lap3.Repositories
{
        public class EFOrderRepository : IOrderRepository
        {
        //private readonly ApplicationDbContext _context;


        //public EFOrderRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task<IEnumerable<Order>> GetAllAsync()
        //{
        //    return await _context.Orders
        //.Include(p => p.OrderDetails) // IOlude thông tin về category
        //.ToListAsync();
        //}

        //public async Task<Order> GetByIdAsync(int id)
        //{
        //    return await _context.Orders.Include(p => p.OrderDetails).FirstOrDefaultAsync(p => p.Id == id);
        //}
        private readonly ApplicationDbContext _context;
        public EFOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();

            // Tính toán TotalQuantitySold cho mỗi đơn hàng
            foreach (var order in orders)
            {
                order.TotalQuantitySold = await GetTotalQuantitySoldAsync(order.Id);
            }

            return orders;
        }
        public async Task<Order> GetByIdAsync(int id) //trả về một đối tượng Order khi tác vụ hoàn thành
        {
            return await _context.Orders
        .Include(o => o.ApplicationUser)
        .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
        .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<Order> GetAll()
        {
            // Lấy tất cả các đơn hàng từ cơ sở dữ liệu và trả về
            return _context.Orders.ToList();
        }
        // Cài đặt phương thức tính tổng số lượng đã bán của một sản phẩm
        public async Task<int> GetTotalQuantitySoldAsync(int productId)
        {
            // Lấy tổng số lượng đã bán của sản phẩm dựa trên productId từ bảng OrderDetails
            var totalQuantitySold = await _context.OrderDetails
                .Where(od => od.ProductId == productId)
                .SumAsync(od => od.Quantity);

            return totalQuantitySold;
        }
    }
}

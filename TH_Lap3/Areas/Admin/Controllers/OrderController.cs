using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TH_Lap3.DataAccess;
using TH_Lap3.Models;
using TH_Lap3.Repositories;

namespace TH_Lap3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository, ApplicationDbContext context)
        {
            _context = context;
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }
        public IActionResult Details(int id)
        {
            //OrderDetails có một mối quan hệ với Product thông qua một khóa ngoại
            // tạo một truy vấn duy nhất để lấy dữ liệu từ cả hai bảng OrderDetails và Product
            var order = _context.OrderDetails.Where(od => od.OrderId == id)
                                                    .Include(od => od.Product)
                                                    .ToList();

            return View(order);
        }

    }
}

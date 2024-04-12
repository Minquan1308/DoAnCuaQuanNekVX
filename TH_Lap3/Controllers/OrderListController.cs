using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TH_Lap3.DataAccess;
using TH_Lap3.Models;
using TH_Lap3.Repositories;

namespace TH_Lap3.Controllers
{
    public class OrderListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOrderRepository _orderRepository;
        public OrderListController(IOrderRepository orderRepository, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _orderRepository = orderRepository;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            // Lấy thông tin người dùng hiện tại
            var user = await _userManager.GetUserAsync(HttpContext.User); // nơi lưu trữ thông tin về người dùng hiện tại đang tương tác với ứng dụng.
            //trả về một đối tượng người dùng (User) từ UserManager, đại diện cho người dùng hiện tại
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (user != null)
            {
                // Truy vấn các đơn hàng của người dùng dựa trên UserId
                var orders = await _context.Orders
                    .Where(o => o.UserId == user.Id)
                    .ToListAsync();

                return View(orders);
            }
            else
            {
                // Người dùng chưa đăng nhập, có thể xử lý tùy ý
                // Ví dụ: Redirect người dùng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Display(int id)
        {
            //OrderDetails có một mối quan hệ với Product thông qua một khóa ngoại
            // tạo một truy vấn duy nhất để lấy dữ liệu từ cả hai bảng OrderDetails và Product
            var order = _context.OrderDetails.Where(od => od.OrderId == id)
                                                    .Include(od => od.Product)
                                                    .ToList();

            return View(order);
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _orderRepository.UpdateAsync(order); //Truyền vào 1 order và nó sẽ đc lưu vào DB
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _orderRepository.GetByIdAsync(id); //Trả về 1 đối tượng Order có id = tham số id 
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

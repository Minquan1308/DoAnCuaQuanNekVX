using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TH_Lap3.Models;
using TH_Lap3.Repositories;

namespace TH_Lap3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class RevenueController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public RevenueController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }

        [HttpPost]
        public IActionResult MonthlyRevenue(int year, int month)
        {
            // Thực hiện truy vấn cơ sở dữ liệu để lấy đơn hàng trong tháng đã chọn
            var ordersInMonth = _orderRepository.GetAll().Where(o => o.OrderDate.Year == year && o.OrderDate.Month == month);

            // Tính tổng doanh thu từ các đơn hàng trong tháng
            decimal totalRevenue = ordersInMonth.Sum(o => o.TotalPrice);

            // Trả về view hiển thị thông tin doanh thu theo tháng
            return View(totalRevenue);
         }
        }
    }
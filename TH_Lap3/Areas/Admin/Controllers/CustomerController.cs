using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TH_Lap3.Models;
using TH_Lap3.Repositories;

namespace TH_Lap3.Areas.Admin.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Customer)]
    public class CustomerController : Controller
    {
        private readonly IProductRepository _productRepository;
        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {

            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
    }
}

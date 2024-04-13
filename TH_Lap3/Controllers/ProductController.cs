using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using TH_Lap3.Repositories;
using TH_Lap3.Models;
using TH_Lap3.DataAccess;

namespace TH_Lap3.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, ApplicationDbContext dbContext, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _dbContext = dbContext; // Inject đối tượng DbContext vào controller
        }

        public IActionResult Products(int id)
        {
            var productsInCategory = _productRepository.GetProductByCategoryAsync(id);
            //var categories = _categoryRepository.GetAll();

            // ViewData["Categories"] = categories; // Truyền danh sách danh mục sản phẩm vào ViewData

            return View(productsInCategory);
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            foreach (var product in products)
            {
                if (product.CategoryId != null)
                {
                    product.Category = await _categoryRepository.GetByIdAsync(product.CategoryId);
                }
                product.TotalQuantitySold = await _orderRepository.GetTotalQuantitySoldAsync(product.Id);
            }
            return View(products);
        }
        public IActionResult Display1(int id)
        {
            var product = _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Trả về trang 404 nếu không tìm thấy sản phẩm
            }

            return View(product); // Trả về view hiển thị thông tin chi tiết sản phẩm
        }
        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public async Task<IActionResult> SearchResult(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                // Nếu từ khóa tìm kiếm rỗng, trả về trang chính
                return RedirectToAction("Index");
            }

            // Gọi hàm tìm kiếm sản phẩm dựa trên keyword từ repository
            var searchResults = await _productRepository.SearchAsync(keyword);
            foreach (var product in searchResults)
            {
                if (product.CategoryId != null)
                {
                    product.Category = await _categoryRepository.GetByIdAsync(product.CategoryId);
                }
            }

            // Trả về view kết quả tìm kiếm và truyền dữ liệu tìm kiếm vào view
            return View("SearchResult", searchResults);
        }
        public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
        {
            // Truy vấn cơ sở dữ liệu để lấy danh sách sản phẩm có CategoryId tương ứng
            // Ví dụ: Sử dụng Entity Framework Core để thực hiện truy vấn dữ liệu từ cơ sở dữ liệu
            return _dbContext.Products.Where(p => p.CategoryId == categoryId).ToList();
        }
        public async Task<IActionResult> FilterByCategory(int categoryId)
        {
            // Truy vấn cơ sở dữ liệu để lấy danh sách sản phẩm có CategoryId tương ứng
            var products = _dbContext.Products.Where(p => p.CategoryId == categoryId).ToList();
            foreach (var product in products)
            {
                if (product.CategoryId != null)
                {
                    product.Category = await _categoryRepository.GetByIdAsync(product.CategoryId);
                }
            }
            // Trả về một PartialView chứa danh sách sản phẩm đã lọc
            return View("_ProductList", products);
        }
    }
}

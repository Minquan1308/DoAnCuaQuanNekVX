//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc;
//using TH_Lap3.Repositories;
//using TH_Lap3.Models;
//using Microsoft.AspNetCore.Authorization;

//namespace TH_Lap3.Controllers
//{
//    [Authorize(Roles = SD.Role_Admin)]
//    public class CategoryController : Controller
//    {

//        private readonly IProductRepository _productRepository;
//        private readonly ICategoryRepository _categoryRepository;


//        public CategoryController(IProductRepository productRepository, ICategoryRepository categoryRepository)
//        {
//            _productRepository = productRepository;
//            _categoryRepository = categoryRepository;
//        }


//        // Hiển thị danh sách sản phẩm
//        public async Task<IActionResult> Index()
//        {
//            var categories = await _categoryRepository.GetAllAsync();
//            return View(categories);
//        }
//        // Hiển thị form thêm sản phẩm mới
//        public async Task<IActionResult> Add()
//        {
//            var categories = await _categoryRepository.GetAllAsync();
//            ViewBag.Categories = new SelectList(categories, "Id", "Name");

//            return View();
//        }


//        // Xử lý thêm sản phẩm mới
//        [HttpPost]
//        public async Task<IActionResult> Add(Category category)
//        {
//            if (ModelState.IsValid)
//            {

//                await _categoryRepository.AddAsync(category);
//                return RedirectToAction(nameof(Index));
//            }

//            return View(category);
//        }

//        // Hiển thị thông tin chi tiết sản phẩm
//        public async Task<IActionResult> Display(int id)
//        {
//            var category = await _categoryRepository.GetByIdAsync(id);
//            if (category == null)
//            {
//                return NotFound();
//            }
//            return View(category);
//        }
//        // Hiển thị form cập nhật sản phẩm
//        public async Task<IActionResult> Update(int id)
//        {
//            var category = await _categoryRepository.GetByIdAsync(id);
//            if (category == null)
//            {
//                return NotFound();
//            }


//            var categories = await _categoryRepository.GetAllAsync();
//            ViewBag.Categories = new SelectList(categories, "Id", "Name", category.Id);
//            return View(category);
//        }
//        // Xử lý cập nhật sản phẩm
//        [HttpPost]
//        public async Task<IActionResult> Update(int id, Category category)

//        {
//            if (id != category.Id)
//            {
//                return NotFound();
//            }
//            if (ModelState.IsValid)
//            {
//                var existingCategory = await _categoryRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync
//                // Cập nhật các thông tin khác của sản phẩm
//                existingCategory.Name = category.Name;

//                await _categoryRepository.UpdateAsync(existingCategory);
//                return RedirectToAction(nameof(Index));
//            }
//            var categories = await _categoryRepository.GetAllAsync();
//            ViewBag.Categories = new SelectList(categories, "Id", "Name");
//            return View(category);
//        }


//        // Hiển thị form xác nhận xóa sản phẩm
//        public async Task<IActionResult> Delete(int id)
//        {
//            var category = await _categoryRepository.GetByIdAsync(id);
//            if (category == null)
//            {
//                return NotFound();
//            }
//            return View(category);
//        }


//        // Xử lý xóa sản phẩm
//        [HttpPost, ActionName("Delete")]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            await _categoryRepository.DeleteAsync(id);
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}

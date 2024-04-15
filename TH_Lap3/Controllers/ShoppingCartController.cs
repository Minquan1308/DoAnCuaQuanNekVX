using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TH_Lap3.DataAccess;
using TH_Lap3.Extensions;
using TH_Lap3.Models;
using TH_Lap3.Repositories;

namespace TH_Lap3.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
       private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, IProductRepository
        productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var product = await GetProductFromDatabase(productId);

            var imageUrl =product.ImageUrl ?? "/images/default-product-image.png";

            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                ImageUrl = imageUrl,
                Price = product.Price,
                Quantity = quantity
            };
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }
        // Các actions khác...


        //public IActionResult Checkout()
        //{
        //    return View(new Order());
        //}
        //[HttpPost]
        //public async Task<IActionResult> Checkout(Order order)
        //{
        //    var cart =
        //    HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
        //    if (cart == null || !cart.Items.Any())
        //    {
        //        // Xử lý giỏ hàng trống...
        //        return RedirectToAction("Index");
        //    }
        //    var user = await _userManager.GetUserAsync(User);
        //    order.UserId = user.Id;
        //    order.OrderDate = DateTime.UtcNow;
        //    order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
        //    order.OrderDetails = cart.Items.Select(i => new OrderDetail
        //    {
        //        ProductId = i.ProductId,
        //        Quantity = i.Quantity,
        //        Price = i.Price
        //    }).ToList();
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();
        //    HttpContext.Session.Remove("Cart");
        //    return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
        //}
        //public async Task<IActionResult> Checkout()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null || string.IsNullOrEmpty(user.FullName) || string.IsNullOrEmpty(user.Address) || string.IsNullOrEmpty(user.PhoneNumber))
        //    {
        //        // Nếu thông tin người dùng chưa đủ, chuyển hướng đến trang cập nhật thông tin
        //        return RedirectToAction("UpdateUserInfo");
        //    }
        //    return View(new Order());
        //}
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Hãy thêm sản phẩm vào giỏ hàng trước khi tiếp tục.";
                //return RedirectToAction("Index");
                return RedirectToAction("Index");
            }
            if (user == null || string.IsNullOrEmpty(user.FullName) || string.IsNullOrEmpty(user.Address) || string.IsNullOrEmpty(user.PhoneNumber))
            {
                // Nếu thông tin người dùng chưa đủ, chuyển hướng đến trang cập nhật thông tin
                return RedirectToAction("UpdateUserInfo");
            }

            // Truyền dữ liệu từ user object vào UpdateUserInfoViewModel
            var model = new UpdateUserInfoViewModel
            {
                FullName = user.FullName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Hãy thêm sản phẩm vào giỏ hàng trước khi tiếp tục.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.FullName) || string.IsNullOrEmpty(user.Address) || string.IsNullOrEmpty(user.PhoneNumber))
            {
                // Nếu thông tin người dùng chưa đủ, chuyển hướng đến trang cập nhật thông tin
                return RedirectToAction("UpdateUserInfo");
            }

            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.ShippingAddress = user.Address;
            order.Notes = "";
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
        }

        public async Task<IActionResult> UpdateUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UpdateUserInfoViewModel
            {
                FullName = user.FullName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo(UpdateUserInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                user.FullName = model.FullName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Cập nhật thông tin thành công, chuyển hướng đến trang checkout
                    return RedirectToAction("Checkout");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Nếu có lỗi, hiển thị lại form để người dùng nhập lại
            return View(model);
        }


        private async Task<Product> GetProductFromDatabase(int productId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);

                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateQuantityAsync(int productId, int quantity)
        {
            //    Nếu không có đối tượng giỏ hàng trong phiên, nó tạo một đối tượng ShoppingCart mới.
            //    Sau đó, nó cập nhật số lượng của sản phẩm cụ thể trong giỏ hàng bằng cách sử dụng
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.UpdateQuantity(productId, quantity);
            HttpContext.Session.SetObjectAsJson("Cart", cart); //lưu đối tượng giỏ hàng đã cập nhật trở lại phiên sử dụng 
            return RedirectToAction("Index");
        }

    }
}

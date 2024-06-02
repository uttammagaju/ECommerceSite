using ECommerceSiteUtiltiy;
using ECommereceSiteData.Repository;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerceSite.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            ViewBag.categories = categories;
            return View(categories);
        }
        [HttpGet]
        public IActionResult CategoryWiseProducts(int categoryId)
        {
            List<Product> product = _unitOfWork.Product.GetAll().Where(x => x.CategoryId == categoryId).ToList();
            var category = _unitOfWork.Category.Get(c => c.Id == categoryId);
            ViewBag.CategoryName = category?.CategoryName;
            return View(product);
        }
        [HttpGet]
        public IActionResult ProductDetails(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(c => c.Id == productId),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]

        public IActionResult ProductDetails(ShoppingCart shoppingCart)
        {
            //get the userId of login user
            var claimsIdentity = (ClaimsIdentity)User.Identity; //->that will have userId of login user
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;// in this way userId is populated
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u =>u.ApplicationUserId== userId && u.ProductId == shoppingCart.ProductId);
            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                
            }
            else
            {
                //shopping cart is not exists
                _unitOfWork.ShoppingCart.Add(shoppingCart);

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

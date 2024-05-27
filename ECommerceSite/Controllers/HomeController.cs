using ECommereceSiteData.Repository;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerceSite.Controllers
{
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
            List<Product> product = _unitOfWork.Product.GetAll().Where(x=>x.CategoryId == categoryId).ToList();
            var category = _unitOfWork.Category.Get(c => c.Id == categoryId);
            ViewBag.CategoryName = category?.CategoryName;
            return View(product);
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

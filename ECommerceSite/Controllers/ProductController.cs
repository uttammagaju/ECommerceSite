using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace ECommerceSite.Controllers
{
    public class ProductController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> productobj = _unitOfWork.Product.GetAll().ToList();
            return View(productobj);
        }
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.Id.ToString()
                });
            ViewBag.CategoryList = CategoryList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product emodel, IFormFile img)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(img != null && img.Length > 0)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product\");
                    using(var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        img.CopyTo(fileStream);
                    }
                    emodel.ImageUrl = @"\images\category" + filename;
                }
               _unitOfWork.Product.Add(emodel);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Product? product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(id);
        }
        #region API Call
        public IActionResult GetAll()
        {
            List<Product> objFromDb = _unitOfWork.Product.GetAll().ToList();
            return Json(new {data = objFromDb});
        }
        public IActionResult Delete(int? id)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var productToBeDelete = _unitOfWork.Product.Get(u =>u.Id==id);
            string path = wwwRootPath + productToBeDelete.ImageUrl;
            System.IO.File.Delete(path);
            _unitOfWork.Product.Remove(productToBeDelete);
            _unitOfWork.Save();
            return Json(new {data = productToBeDelete});
        }
        #endregion
    }
}

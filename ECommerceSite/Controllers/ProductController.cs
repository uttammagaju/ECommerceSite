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
            List<Product> productobj = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productobj);
        }
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.Id.ToString()
                });
            ViewBag.categoryList = categoryList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product emodel, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (img != null && img.Length > 0)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product\");
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        img.CopyTo(fileStream);
                    }
                    emodel.ImageUrl = @"\images\product\" + filename;
                }
                _unitOfWork.Product.Add(emodel);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.Id.ToString()
                });
            ViewBag.categoryList = categoryList;
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? product = _unitOfWork.Product.Get(u => u.Id == id);


            if (product == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(
               u => new SelectListItem
               {
                   Text = u.CategoryName,
                   Value = u.Id.ToString()
               });
             ViewBag.categoryList = categoryList;

            return View(product); // Ensure that the Product object is being passed here
            
        }
        [HttpPost]
        public IActionResult Edit(Product model, IFormFile? img)
        {
            
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string oldimg= model.ImageUrl;
            if (img != null && img.Length > 0)
            {
                // Update photo handling (combined and improved):
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product\");

                if (ModelState.IsValid)
                {
                    string oldImagePath = Path.Combine(wwwRootPath, model.ImageUrl.TrimStart('\\'));


                    // Delete the old image (error handling included)
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Error deleting old image: " + ex.Message);
                            return View(model); // Re-render view with error message
                        }
                    }

                    // Save the new image (error handling included)
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        try
                        {
                            img.CopyTo(fileStream);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Error saving new image: " + ex.Message);
                            return View(model); // Re-render view with error message
                        }
                    }

                    // Update model's ImageUrl with the new path
                    model.ImageUrl = @"\images\product\" + filename;


                    // Update product in database (assuming _unitOfWork.product is a repository):
                    _unitOfWork.Product.Update(model);

                    _unitOfWork.Save();

                    return RedirectToAction("Index");
                }
            }
                else
                {
                    
                    model.ImageUrl = oldimg.ToString();   
                    _unitOfWork.Product.Update(model);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }

            // Re-render the view with validation errors (if any)
            return View(model);
        }


        // Re-render the view with validation errors (if any)


        #region API Call
        public IActionResult GetAll()
        {
            List<Product> objFromDb = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objFromDb});
        }
        public IActionResult Delete(int? id)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var productToBeDelete = _unitOfWork.Product.Get(u =>u.Id==id);
            string path = wwwRootPath + productToBeDelete.ImageUrl;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error deleting old image: " + ex.Message);
                    return View(); // Re-render view with error message
                }
            }
            _unitOfWork.Product.Remove(productToBeDelete);
            _unitOfWork.Save();
            return Json(new {data = productToBeDelete});
        }
        #endregion
    }
}

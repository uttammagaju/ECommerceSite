using ECommereceSiteData.Data;
using ECommereceSiteData.Repository;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ECommerceSite.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            ViewBag.categorias = categories;    
            return View();
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category emodel, IFormFile? img)
        {
            var DataFromDB = _unitOfWork.Category.Get(u => u.CategoryName == emodel.CategoryName);
            if (DataFromDB != null) { 
                return RedirectToAction("Index");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (img != null && img.Length > 0)
                    {
                        string filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                        string categoryPath = Path.Combine(wwwRootPath, @"images\category\");
                        //var oldImagePath = Path.Combine(wwwRootPath, emodel.ImageUrl.TrimStart('\\'));
                        using (var fileStream = new FileStream(Path.Combine(categoryPath, filename), FileMode.Create))
                        {
                            img.CopyTo(fileStream);
                        }
                        emodel.ImageUrl = @"\images\category\" + filename;
                    }
                }
                _unitOfWork.Category.Add(emodel);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            
            
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category model, IFormFile? img)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string oldImage = model.ImageUrl;
            string oldImagePath = Path.Combine(wwwRootPath, model.ImageUrl.TrimStart('\\'));
            if (img != null && img.Length > 0)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                string categoryPath = Path.Combine(wwwRootPath, @"images\category\");

                if (ModelState.IsValid)
                {
                    // Update photo handling (combined and improved):

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
                    using (var fileStream = new FileStream(Path.Combine(categoryPath, filename), FileMode.Create))
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
                    model.ImageUrl = @"\images\category\" + filename;
                

                // Update category in database (assuming _unitOfWork.Category is a repository):
                _unitOfWork.Category.Update(model);
                    
                    _unitOfWork.Save();

                return RedirectToAction("Index");
            }

        }
            else
            {
                    
                model.ImageUrl= oldImage;
                    _unitOfWork.Category.Update(model);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            
            // Re-render the view with validation errors (if any)
            return View(model);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> objFromDb = _unitOfWork.Category.GetAll().ToList();
            return Json(new {data = objFromDb});
        }
        public IActionResult Delete(int id)
        {
            
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            
            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    try
            //    {
            //        System.IO.File.Delete(oldImagePath);
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("", "Error deleting old image: " + ex.Message);
            //        return View(model); // Re-render view with error message
            //    }
            //}
            var categoryToBeDelete = _unitOfWork.Category.Get(u=>u.Id == id);
            string path = wwwRootPath + categoryToBeDelete.ImageUrl; 
           // string oldImagePath = Path.Combine(wwwRootPath, categoryToBeDelete.ImageUrl);
            System.IO.File.Delete(path);
            _unitOfWork.Category.Remove(categoryToBeDelete);
            _unitOfWork.Save();
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return Json(new {data = categoryToBeDelete});
        }
        #endregion
    }


}

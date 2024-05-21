using ECommereceSiteData.Data;
using ECommereceSiteData.Repository;
using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSite.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category emodel)
        {
            if (emodel.ImageUrl == null) 
            {
                emodel.ImageUrl = ""; 
            }

            _unitOfWork.Category.Add(emodel);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
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
        public IActionResult Edit(Category model)
        {
            if ((ModelState.IsValid))
            {
                _unitOfWork.Category.Update(model);
                _unitOfWork.Save();
            }
            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> objFromDb = _unitOfWork.Category.GetAll().ToList();
            return Json(new {data = objFromDb});
        }
        #endregion
    }


}

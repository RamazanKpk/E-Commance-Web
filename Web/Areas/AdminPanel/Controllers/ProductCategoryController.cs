using DataAccess.Context;
using DataAccess;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.CategoryViewModels;
using Buisness.SessionAuth;

namespace Web.Areas.AdminPanel.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: AdminPanel/ProductCategory
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<ProductCategory>().GetAll();
            return View(result);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var model = new CategoryViewModel
            {
                Brands = GetBrand(),
                ProductCategories = new List<SelectListItem>(),
                SubCategories = new List<SelectListItem>(),
                SubSubCategories = new List<SelectListItem>(),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var newCategory = new ProductCategory
                    {
                        ParentId = model.SubSubCategoryId ?? model.SubParentId ?? model.ParentId,
                        Title = model.Title,
                        BrandId = model.BrandId.Value,
                        IsActive = model.IsActive,
                        SortOrder = model.SortOrder,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = user.Id,
                        Deleted = false,
                    };
                    unitOfWork.GetRepository<ProductCategory>().Add(newCategory);
                    unitOfWork.Complate();
                    return RedirectToAction("Index");
                }
            }
            model.Brands = GetBrand();
            if (model.BrandId.HasValue)
            {
                model.ProductCategories = GetParentCategory(model.BrandId.Value);
                if (model.ParentId.HasValue)
                {
                    model.SubCategories = GetSubCategories(model.ParentId.Value);
                    if (model.SubParentId.HasValue)
                    {
                        model.SubSubCategories = GetSubCategories(model.SubParentId.Value);
                    }
                }
            }
           

            return View(model);
        }
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<ProductCategory>().GetById(Id);
            return View(result);
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var result = unitOfWork.GetRepository<ProductCategory>().GetById(Id);
            if (result == null)
            {
                return HttpNotFound();
            } 
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            var previousRecord = unitOfWork.GetRepository<ProductCategory>().GetById(productCategory.Id);
            try
            {
                if(user != null)
                {
                    productCategory.CreatedBy = previousRecord.CreatedBy;
                    productCategory.CreatedBy = previousRecord?.CreatedBy;
                    productCategory.ModifiedDate = DateTime.Now;
                    productCategory.ModifiedBy = user.Id;

                    unitOfWork.GetRepository<ProductCategory>().Update(productCategory);
                    unitOfWork.Complate();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                
            }
            return View(productCategory);
        }
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                ProductCategory productCategory = unitOfWork.GetRepository<ProductCategory>().GetById(id);
                IEnumerable<ProductCategory> productCategories = new List<ProductCategory>() { productCategory };
                unitOfWork.GetRepository<ProductCategory>().DeleteRange(productCategories);
            }
            unitOfWork.Complate();
            return Json("Kayıt başarılı bir şekilde silindi");
        }
        private List<SelectListItem> GetSubCategories(int parentId)
        {
            return unitOfWork.GetRepository<ProductCategory>().GetAll()
                .Where(x => x.ParentId == parentId && !x.Deleted)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title,
                }).ToList();
        }
        private List<SelectListItem> GetParentCategory(int brandId)
        {
            return unitOfWork.GetRepository<ProductCategory>().GetAll() 
                .Where(x => x.ParentId == null && x.BrandId == brandId && !x.Deleted)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.Title,
                 }).ToList();
        }
        private List<SelectListItem> GetBrand()
        {
            return unitOfWork.GetRepository<Brand>().GetAll()
                .Where(x => !x.Deleted)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.Title,
                 }).ToList();
        }
         [HttpGet]
    public JsonResult GetParentCategoriesJson(int brandId)
    {
        try
        {
            var parentCategories = GetParentCategory(brandId);
            return Json(parentCategories, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            // Hata mesajını loglama veya işleme burada yapılabilir
            return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        }
    }

    [HttpGet]
    public JsonResult GetSubCategoriesJson(int parentId)
    {
        try
        {
            var subCategories = GetSubCategories(parentId);
            return Json(subCategories, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            // Hata mesajını loglama veya işleme burada yapılabilir
            return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
        }
    }
       
        [HttpPost]
        public JsonResult CreateParentCategory(string title, int? brandId, int? parentId, int sortOrder, int? subId)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            var newCategory = new ProductCategory
            {
                Title = title,
                ParentId = subId ?? parentId,
                IsActive = true,
                SortOrder = sortOrder,
                BrandId = brandId.Value,
                CreatedDate = DateTime.Now,
                CreatedBy = user.Id,
                ModifiedBy = user.Id,
                ModifiedDate = DateTime.Now,
                Deleted = false,
            };
            unitOfWork.GetRepository<ProductCategory>().Add(newCategory);
            unitOfWork.Complate();
            return Json(new { success = true, newCategoryId = newCategory.Id });
        }
    }
}
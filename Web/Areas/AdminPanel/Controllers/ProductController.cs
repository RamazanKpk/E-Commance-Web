using Buisness.SessionAuth;
using DataAccess;
using DataAccess.Context;
using Entity;
using DataModel.ProductViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Web.Areas.AdminPanel.Controllers
{
    public class ProductController : Controller
    {
        // GET: AdminPanel/Product
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ActionResult Index()
        {
            var result = unitOfWork.GetRepository<Product>().GetAll();
            return View(result);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var categoryList = unitOfWork.GetRepository<ProductCategory>().GetAll();
            var model = new ProductViewModel
            {
                Brands = GetBrand(),
                ProductCategories = new List<SelectListItem>(),
                SubCategories = new List<SelectListItem>(),
                SubSubCategories = new List<SelectListItem>(),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(ProductViewModel model)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            if (ModelState.IsValid)
            {
                var decVatRate = Convert.ToDecimal(model.VatRate);
                var product = new Product
                {
                    Title = model.Title,
                    CategoryId = model.SubSubCategoryId ?? model.SubCategoryId ??  model.MainCategoryId ?? model.CategoryId,
                    VatRate = model.VatRate,
                    SalePrice = model.Price*(1+decVatRate/100),
                    IsActive = model.IsActive,
                    IsVatInclude = model.IsVatInclude,
                    Price = model.Price,
                    StockAmount = model.StockAmount,
                    CriticalStockAmount = model.CriticalStockAmount,
                    CreatedBy = user.Id,
                    ModifiedBy = user.Id,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Description = model.Description,
                    Deleted = false,
                };
                if (model.ImageFiles != null && model.ImageFiles.Length > 0)
                {
                    foreach (var imageFile in model.ImageFiles)
                    {
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            var imageName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(imageFile.FileName);
                            string filePath = Path.Combine(Server.MapPath("~/Content/images"), imageName);
                            imageFile.SaveAs(filePath);
                            var productImage = new ProductImage
                            {
                                ImagePath = imageName,
                                Deleted = false,
                                IsDefaultImage = true,
                                ProductId = product.Id,
                            };
                            unitOfWork.GetRepository<ProductImage>().Add(productImage);
                        }
                    }

                }
                unitOfWork.GetRepository<Product>().Add(product);
                unitOfWork.Complate();
                return RedirectToAction("Index");
            }
            model.Brands = GetBrand();
            model.ProductCategories = GetParentCategory(model.BrandId);
            if (model.MainCategoryId.HasValue)
            {
                model.SubCategories = GetSubCategories(model.MainCategoryId.Value);
            }
            return View(model);
        }
        public ActionResult Details(int Id)
        {
            var result = unitOfWork.GetRepository<Product>().GetById(Id);
            var images = unitOfWork.GetRepository<ProductImage>().GetAll();
            var productImage = images.Where(x=>x.ProductId == Id).OrderByDescending(c=> c.Id).ToList();
            ViewBag.ProductImages = productImage;
            return View(result);
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var result = unitOfWork.GetRepository<Product>().GetById(Id);
            var categoryList = unitOfWork.GetRepository<ProductCategory>().GetAll();
            SelectList selectLists = new SelectList(categoryList, "Id", "Title", "ParentId");
            ViewBag.Category = selectLists;
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            var previousRecord = unitOfWork.GetRepository<Product>().GetById(product.Id);
            try
            {
                if (user != null) {
                    var decVatRate = Convert.ToDecimal(product.VatRate);
                    product.SalePrice = product.Price * (1 + decVatRate / 100);
                    product.CreatedBy = previousRecord.CreatedBy;
                    product.CreatedDate = previousRecord.CreatedDate;
                    product.ModifiedDate = DateTime.Now;
                    product.ModifiedBy = user.Id;
                    unitOfWork.GetRepository<Product>().Update(product);
                    unitOfWork.Complate();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(product);
        }
        [HttpPost]
        public ActionResult Delete(int[] Ids)
        {
            foreach (int id in Ids)
            {
                Product product = unitOfWork.GetRepository<Product>().GetById(id);
                IEnumerable<Product> products = new List<Product>() { product };
                var productImages = unitOfWork.GetRepository<ProductImage>().GetAll();
                var productImage = productImages.Where(x => x.ProductId == product.Id).SingleOrDefault();
                IEnumerable<ProductImage> image = new List<ProductImage>() { productImage };
                if (productImage != null)
                {
                    unitOfWork.GetRepository<ProductImage>().DeleteRange(image);
                }
                unitOfWork.GetRepository<Product>().DeleteRange(products);
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
            var result = unitOfWork.GetRepository<ProductCategory>().GetAll()
                .Where(x => x.ParentId == null && x.BrandId == brandId && !x.Deleted)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.Title,
                 }).ToList();
            return result;
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
        public JsonResult GetSubCategoriesJson(int Id)
        {
            try
            {
                var subCategories = GetSubCategories(Id);
                return Json(subCategories, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Hata mesajını loglama veya işleme burada yapılabilir
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetParentCategoriesJson(int Id)
        {
            try
            {
                var parentCategories = GetParentCategory(Id);
                return Json(parentCategories, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Hata mesajını loglama veya işleme burada yapılabilir
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Images()
        {
            var result =  unitOfWork.GetRepository<ProductImage>().GetAll();
            return View(result);
        }
        [HttpGet]
        public ActionResult AddImage(int id)
        {
            var product = unitOfWork.GetRepository<Product>().GetById(id);
            var model = new ProductImage { ProductId = id };
            var productName = product.Title;
            ViewBag.ProductName = productName;
            return View(model);
        }
        [HttpPost]
        public ActionResult AddImage(List<HttpPostedFileBase> imageFiles, ProductImage model)
        {
            try
            {
                var product = unitOfWork.GetRepository<Product>().GetById(model.ProductId.Value);
                if (product == null)
                {
                    return HttpNotFound("Product Not Found");
                }
                foreach (var imageFile in imageFiles)
                {
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var imageName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Content/images"), imageName);
                        imageFile.SaveAs(filePath);
                        var productImage= new ProductImage {
                            ProductId = model.ProductId.Value,
                            ImagePath = imageName,
                            IsDefaultImage = model.IsDefaultImage,
                            Deleted = model.Deleted,
                        };
                        unitOfWork.GetRepository<ProductImage>().Add(productImage);
                    }
                }
                unitOfWork.Complate();

                TempData["SuccessMessage"] = "Resimler başarıyla eklendi.";

                // Ürün detayları sayfasına yönlendir
                return RedirectToAction("Details", "Product", new { id = model.ProductId });
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "Resimler eklenirken bir hata oluştu: " + ex.Message;

                // Ürün detayları sayfasına geri yönlendir
                return RedirectToAction("Details", "Product", new { id = model.ProductId });
            }
        }
    }
}
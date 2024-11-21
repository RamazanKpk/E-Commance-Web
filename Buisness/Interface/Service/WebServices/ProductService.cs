
using Buisness.SessionAuth;
using DataAccess;
using DataAccess.Context;
using Entity;
using DataModel.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Buisness.Interface.Abstract.WebAbstract;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using DataModel.CategoryViewModels;
using System.Text;

namespace Buisness.Interface.Service.WebServices
{
    public class ProductService: IProductService, IAllWebService<ProductListModel>
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public ProductViewModel GetModel()
        {
            var categoryList = unitOfWork.GetRepository<ProductCategory>().GetAll();
            var model = new ProductViewModel
            {
                Brands = GetBrand(),
                ProductCategories = new List<SelectListItem>(),
                SubCategories = new List<SelectListItem>(),
                SubSubCategories = new List<SelectListItem>(),
            };
            return model;
        }
        public List<SelectListItem> GetSubCategories(int parentId)
        {
            return unitOfWork.GetRepository<ProductCategory>().GetAll()
                .Where(x => x.ParentId == parentId && !x.Deleted)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title,
                }).ToList();
        }
        public List<SelectListItem> GetParentCategory(int brandId)
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
        public List<SelectListItem> GetBrand()
        {
            return unitOfWork.GetRepository<Brand>().GetAll()
                .Where(x => !x.Deleted)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.Title,
                 }).ToList();
        }
        public ProductViewModel CreateModel(ProductViewModel model)
        {
            var user = unitOfWork.UserRepository.GetLogUser(SessionManeger.GetUserName());
            var product = new Product
            {
                Title = model.Title,
                CategoryId = model.SubCategoryId ?? model.SubSubCategoryId ?? model.MainCategoryId ?? model.CategoryId,
                VatRate = model.VatRate,
                SalePrice = model.SalePrice,
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
            unitOfWork.GetRepository<Product>().Add(product);
            unitOfWork.Complate();

            model.Brands = GetBrand();
            model.ProductCategories = GetParentCategory(model.BrandId);
            if (model.MainCategoryId.HasValue)
            {
                model.SubCategories = GetSubCategories(model.MainCategoryId.Value);
            }
            return model;
        }

        public List<ProductListModel> GetApiToWeb()
        {
            List<ProductListModel> products = new List<ProductListModel>();
            string api = ConfigurationManager.AppSettings["GetProductEndPoint"].ToString();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    products = JsonConvert.DeserializeObject<List<ProductListModel>>(json);
                }
            }
            return products;
        }
        public List<ProductListModel> PostProductByCategory(CategoryListModel model)
        {
            string apiUrl = ConfigurationManager.AppSettings["PostProductCategoryEndPoint"];
            var jsonContent = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var products = new List<ProductListModel>();

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage reponse = client.PostAsync(apiUrl, content).Result;
                if (reponse.IsSuccessStatusCode)
                {
                    string jsonData = reponse.Content.ReadAsStringAsync().Result;
                    products = JsonConvert.DeserializeObject<List<ProductListModel>>(jsonData);
                }
            }
            return products;
        }
    }
    
}

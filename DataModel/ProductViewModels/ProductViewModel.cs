using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataModel.ProductViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool IsVatInclude { get; set; }
        public int VatRate { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public int StockAmount { get; set; }
        public int CriticalStockAmount { get; set; }
        public bool IsActive { get; set; }
        public int BrandId { get; set; }
        public int? MainCategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? SubSubCategoryId { get; set; }
        public HttpPostedFileBase[] ImageFiles { get; set; }
        public List<SelectListItem> ProductCategories { get; set; }
        public List<SelectListItem> SubCategories { get; set; }
        public List<SelectListItem> SubSubCategories { get; set; }
        public List<SelectListItem> Brands { get; set; }
    }
}
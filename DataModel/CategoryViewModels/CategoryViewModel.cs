using System.Collections.Generic;
using System.Web.Mvc;


namespace DataModel.CategoryViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? SubParentId { get; set; }
        public int? BrandId { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public int? SubSubCategoryId { get; set; }
        public List<SelectListItem> ProductCategories{ get; set; }
        public List<SelectListItem> SubCategories { get; set; }
        public List<SelectListItem> Brands { get; set; }
        public List<SelectListItem> SubSubCategories { get; set; }
    }
}
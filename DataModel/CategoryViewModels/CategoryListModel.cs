using DataModel.BaseModel;
using DataModel.BrandViewModels;
using DataModel.ProductViewModels;
using System.Collections.Generic;

namespace DataModel.CategoryViewModels
{
    public class CategoryListModel: EntityBase
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public List<SubCategoryListModel> SubCategories { get; set; }
    }
}

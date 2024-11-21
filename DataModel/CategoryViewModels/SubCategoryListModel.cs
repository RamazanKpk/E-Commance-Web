using DataModel.BaseModel;
using DataModel.ProductViewModels;
using System.Collections.Generic;

namespace DataModel.CategoryViewModels
{
    public class SubCategoryListModel: EntityBase
    {
        public string Name { get; set; }
        public List<ProductListModel> Products { get; set; }
        public int? ParentId { get; set; }
        public List<SubCategoryListModel> SubCategories { get; set; }
    }
}

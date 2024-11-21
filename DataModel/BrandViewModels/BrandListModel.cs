using DataModel.BaseModel;
using DataModel.CategoryViewModels;
using System.Collections.Generic;

namespace DataModel.BrandViewModels
{
    public class BrandListModel :EntityBase
    {
        public string Name { get; set; }
        public List<CategoryListModel> Categories { get; set; }
    }
}

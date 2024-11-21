using DataModel.CategoryViewModels;
using DataModel.ProductViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Buisness.Interface.Abstract.WebAbstract
{
    public interface IProductService
    {
        List<SelectListItem> GetSubCategories(int Id);
        List<SelectListItem> GetParentCategory(int Id);
        List<SelectListItem> GetBrand();
        ProductViewModel GetModel();
        ProductViewModel CreateModel(ProductViewModel model);
        List<ProductListModel> PostProductByCategory(CategoryListModel model);
    }
}

using Buisness.Interface.Abstract.ApiAbstract;
using DataAccess;
using DataAccess.Context;
using DataModel.CategoryViewModels;
using DataModel.ImageViewModels;
using DataModel.ProductViewModels;
using Entity;
using System.Collections.Generic;
using System.Linq;

namespace Buisness.Interface.Service.ApiServices
{
    public class CategoryApiService: IAllApiServices<CategoryListModel>
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public List<CategoryListModel> GetList()
        {
            var category = unitOfWork.GetRepository<ProductCategory>().GetAll()
                .Where(x=> x.IsActive)
                .Select(c=> new CategoryListModel
                {
                    Id = c.Id,
                    Name = c.Title,
                    ParentId = c.ParentId,
                    SubCategories = SubCategoryRecusive(c.Id)
                    
                }).ToList();
            return category;
        }
        private List<SubCategoryListModel> SubCategoryRecusive(int? parnetId)
        {
            var subCategory = unitOfWork.GetRepository<ProductCategory>().GetAll()
                    .Where(k => k.ParentId == parnetId)
                    .Select(n => new SubCategoryListModel
                    {
                        Id = n.Id,
                        ParentId = n.ParentId,
                        Name = n.Title,
                        SubCategories = SubCategoryRecusive(n.Id),
                        Products = unitOfWork.GetRepository<Product>().GetAll()
                        .Where(x => x.IsActive && x.CategoryId == n.Id)
                        .Select(v => new ProductListModel
                        {
                            Id = v.Id,
                            Name = v.Title,
                            SalePrice = v.SalePrice,
                            StockAmount = v.StockAmount,
                            Description = v.Description,
                            VatRate = v.VatRate,
                            BrandId = v.ProductCategory.BrandId,
                            Images = unitOfWork.GetRepository<ProductImage>().GetAll()
                            .Where(b => b.IsDefaultImage && b.ProductId == v.Id)
                            .Select(a => new ImageListModel
                            {
                                ImageUrl = a.ImagePath,
                            }).ToList(),
                        }).ToList(),
                    }).ToList();
            return subCategory;
        }
       
    }
}

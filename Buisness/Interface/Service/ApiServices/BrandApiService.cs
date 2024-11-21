using Buisness.Interface.Abstract.ApiAbstract;
using DataAccess.Context;
using DataAccess;
using DataModel.BrandViewModels;
using DataModel.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DataModel.ImageViewModels;
using DataModel.ProductViewModels;

namespace Buisness.Interface.Service.ApiServices
{
    public class BrandApiService : IAllApiServices<BrandListModel>
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        private IAllApiServices<CategoryListModel> _allApiServices;
        public BrandApiService(IAllApiServices<CategoryListModel> allApiServices) 
        {
            _allApiServices = allApiServices;
        }

        public List<BrandListModel> GetList()
        {
            var brad = unitOfWork.GetRepository<Brand>().GetAll()
                .Where(x => x.IsActive)
                .Select(c => new BrandListModel
                {
                    Id = c.Id,
                    Name = c.Title,
                    Categories = unitOfWork.GetRepository<ProductCategory>().GetAll()
                .Where(x => x.IsActive && x.BrandId == c.Id)
                .Select(a => new CategoryListModel
                {
                    Id = a.Id,
                    Name = a.Title,
                    ParentId = a.ParentId,
                    SubCategories = SubCategoryRecusive(a.Id)
                }).ToList(),
                }).ToList();
            return brad;
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

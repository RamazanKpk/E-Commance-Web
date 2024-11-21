using DataModel.BaseModel;
using DataModel.ImageViewModels;
using System.Collections.Generic;

namespace DataModel.ProductViewModels
{
    public class ProductListModel: EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public int? CategoryId { get; set; }
        public int StockAmount { get; set; }
        public int VatRate { get; set; }
        public List<ImageListModel> Images { get; set; }
        public int BrandId { get; set; }
    }
}

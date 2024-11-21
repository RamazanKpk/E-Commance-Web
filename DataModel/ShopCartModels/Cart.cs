using DataModel.ProductViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.ShopCartModels
{
    public class Cart
    {
        [Key]
        public int OrderNumber { get; set; }
        public List<ShopCartListModel> Items { get; set; } = new List<ShopCartListModel>();
        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
    }
}

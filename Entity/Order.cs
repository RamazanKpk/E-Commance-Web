//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public int ProductAmount { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCompleated { get; set; }
        public bool IsCanceled { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}

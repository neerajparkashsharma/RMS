//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public long Id { get; set; }
        public Nullable<long> FoodItemId { get; set; }
        public Nullable<long> OrderId { get; set; }
        public Nullable<long> Quantity { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
    
        public virtual FoodItem FoodItem { get; set; }
        public virtual Order Order { get; set; }
    }
}

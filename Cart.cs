using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS
{
    public class Cart
    {
        public long FoodItemId { get; set; }
        public string Name { get; set; }
        public long? Price { get; set; }
        public long Quantity { get; set; }
        public string Image { get; set; }
        public float bill { get; set; }

        public long UserId { get; set; }
    }
}
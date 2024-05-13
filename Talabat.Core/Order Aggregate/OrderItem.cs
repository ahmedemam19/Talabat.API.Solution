﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Order_Aggregate
{
	public class OrderItem : BaseEntity
	{
        public ProductItemOrdered Product { get; set; } = null!;

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

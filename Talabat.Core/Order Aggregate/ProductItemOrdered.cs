﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Order_Aggregate
{
	public class ProductItemOrdered
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string PictureUrl { get; set; } = null!;
	}
}

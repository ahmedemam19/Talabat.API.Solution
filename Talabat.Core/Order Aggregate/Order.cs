using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Order_Aggregate
{
	public class Order : BaseEntity
	{
		public string BuyerEmail { get; set; } = null!;
		
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
		
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
        
		public Address ShippingAddress { get; set; } = null!;

        //public int DeliveryMethodId { get; set; } // ForeginKey
        public DeliveryMethod? DeliveryMethod { get; set; } = null!; // Navigational Property [ ONE ]

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigational Property [ MANY ]

        public decimal Subtotal { get; set; }

		//[NotMapped]
		//public decimal Total => Subtotal + DeliveryMethod.Cost;

        public decimal GetTolal() => Subtotal + DeliveryMethod.Cost;

		public string PaymentIntendId { get; set; } = string.Empty;

    }
}

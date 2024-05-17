using Talabat.Core.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
	public class OrderToReturnDto
	{
        public int Id { get; set; }

		public string BuyerEmail { get; set; } = null!;

		public DateTimeOffset OrderDate { get; set; }

		public string Status { get; set; }

		public Address ShippingAddress { get; set; }

		public string DeliveryMethod { get; set; }

		public string DeliveryMethodCost { get; set; }

		public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>(); // Navigational Property [ MANY ]

		public decimal Subtotal { get; set; }

		public decimal Total { get; set; }

		public string PaymentIntendId { get; set; } = string.Empty;
	}
}

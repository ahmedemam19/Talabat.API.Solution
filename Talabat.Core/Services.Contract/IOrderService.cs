﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
	public interface IOrderService
	{
		Task<Order> CreateOrderAsync(string BuyerEmail, string basketId, string deliveryMethod, Address shippingAddress);

		Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

		Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId);

		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

	}
}

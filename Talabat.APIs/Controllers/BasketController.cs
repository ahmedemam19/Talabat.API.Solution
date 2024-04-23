using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;

		public BasketController(IBasketRepository basketRepository)
        {
			_basketRepository = basketRepository;
		}

		[HttpGet] // GET : /api/Basket?id=
		public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
		{
			var basket = await _basketRepository.GetBasketAsync(id);

			return Ok(basket ?? new CustomerBasket(id));
		}


		[HttpPost]  // POST: /api/Basket
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
		{
			var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);

			if (createdOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));

			return Ok(createdOrUpdatedBasket);
		}

		[HttpDelete] // DELETE: /api/Basket?id=
		public async Task DeleteBasket(string id)
		{
			await _basketRepository.DeleteBasketAsync(id);
		}
    }
}

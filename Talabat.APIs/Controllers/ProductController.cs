using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{
	public class ProductController : BaseApiController
	{
		private readonly IGenericRepoistory<Product> _productsRepo;

		public ProductController(IGenericRepoistory<Product> productsRepo)
        {
			_productsRepo = productsRepo;
		}


		// /api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var Products = await _productsRepo.GetAllAsync();
			return Ok(Products);
		}

    }
}

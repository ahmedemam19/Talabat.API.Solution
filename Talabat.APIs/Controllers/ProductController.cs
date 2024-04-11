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


		// /api/Products/10
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _productsRepo.GetAsync(id);
			
			if(product is null)
				return NotFound(new {Message = "Not Found", StatusCode = 404}); // 404

			return Ok(product);
		}

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

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
			var spec = new ProductWithBrandAndCategorySpecification();

			var Products = await _productsRepo.GetAllWithSpecAsync(spec);

			return Ok(Products);
		}


		// /api/Products/10
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecification(id);

			var product = await _productsRepo.GetWithSpecAsync(spec);
			
			if(product is null)
				return NotFound(new {Message = "Not Found", StatusCode = 404}); // 404

			return Ok(product);
		}

    }
}

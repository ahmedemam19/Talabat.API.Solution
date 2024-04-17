using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{
	public class ProductController : BaseApiController
	{
		private readonly IGenericRepoistory<Product> _productsRepo;
		private readonly IMapper _mapper;

		public ProductController(IGenericRepoistory<Product> productsRepo, IMapper mapper)
        {
			_productsRepo = productsRepo;
			_mapper = mapper;
		}



		// /api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
		{
			var spec = new ProductWithBrandAndCategorySpecification();

			var products = await _productsRepo.GetAllWithSpecAsync(spec);

			return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products));
		}


		// /api/Products/10
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecification(id);

			var product = await _productsRepo.GetWithSpecAsync(spec);
			
			if(product is null)
				return NotFound(new ApiResponse(404)); // 404

			return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
		}

    }
}

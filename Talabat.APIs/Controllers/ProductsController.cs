using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IProductService _productService;

		/// private readonly IGenericRepoistory<Product> _productsRepo;
		/// private readonly IGenericRepoistory<ProductBrand> _productBrandRepo;
		/// private readonly IGenericRepoistory<ProductCategory> _productCategoryRepo;
		private readonly IMapper _mapper;


		//Constructotr
		public ProductsController(
			IProductService productService,
			/// IGenericRepoistory<Product> productsRepo,
			/// IGenericRepoistory<ProductBrand> productBrandRepo,
			/// IGenericRepoistory<ProductCategory> productCategoryRepo,
			IMapper mapper)
        {
			_productService = productService;
			/// _productsRepo = productsRepo;
			/// _productBrandRepo = productBrandRepo;
			/// _productCategoryRepo = productCategoryRepo;
			_mapper = mapper;
		}



		// /api/Products
		//[Authorize(AuthenticationSchemes = "Bearer")]
		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
		{
			// We Implemented (Sorting, Filteration and Pagination)

			var products = await _productService.GetProductsAsync(specParams);

			var count = await _productService.GetCountAsync(specParams);

			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}


		// /api/Products/10
		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var product = await _productService.GetProductAsync(id);
			
			if(product is null)
				return NotFound(new ApiResponse(404)); // 404

			return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
		}


		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _productService.GetBrandsAsync();
			return Ok(brands);
		}


		[HttpGet("categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _productService.GetCategoriesAsync();
			return Ok(categories);
		}

    }
}

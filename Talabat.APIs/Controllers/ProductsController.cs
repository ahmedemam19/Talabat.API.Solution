﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepoistory<Product> _productsRepo;
		private readonly IGenericRepoistory<ProductBrand> _productBrandRepo;
		private readonly IGenericRepoistory<ProductCategory> _productCategoryRepo;
		private readonly IMapper _mapper;


		//Constructotr
		public ProductsController(
			IGenericRepoistory<Product> productsRepo,
			IGenericRepoistory<ProductBrand> productBrandRepo,
			IGenericRepoistory<ProductCategory> productCategoryRepo,
			IMapper mapper)
        {
			_productsRepo = productsRepo;
			_productBrandRepo = productBrandRepo;
			_productCategoryRepo = productCategoryRepo;
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

			var spec = new ProductWithBrandAndCategorySpecification(specParams);

			var products = await _productsRepo.GetAllWithSpecAsync(spec);

			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			var countSpec = new ProductsWithFilterationForCountSpecification(specParams);

			var count = await _productsRepo.GetCountAsync(countSpec);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));
		}


		// /api/Products/10
		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecification(id);

			var product = await _productsRepo.GetByIdWithSpecAsync(spec);
			
			if(product is null)
				return NotFound(new ApiResponse(404)); // 404

			return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
		}


		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _productBrandRepo.GetAllAsync();
			return Ok(brands);
		}


		[HttpGet("categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _productCategoryRepo.GetAllAsync();
			return Ok(categories);
		}

    }
}

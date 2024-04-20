using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
	public class ProductWithBrandAndCategorySpecification : BaseSpecifications<Product>
	{
        // this ctor is will be used for creating object that will be used to get all products
        public ProductWithBrandAndCategorySpecification(ProductSpecParams specParams) 
			: base(p => 
						(string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) 
						&&
						(!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value)
						&&
						(!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId.Value))

		{
            Includes.Add(p =>  p.Brand);
            Includes.Add(p =>  p.Category);


			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				switch (specParams.Sort)
				{
					case "priceAsc":
						//OrderBy = p => p.Price;
						AddOrderBy(p => p.Price);
						break;
					case "priceDesc":
						//OrderByDesc = p => p.Price;
						AddOrderByDesc(p => p.Price);
						break;
					default:
						OrderBy = p => p.Name;
						break;
				}
			}
			else
				AddOrderBy(p => p.Name);


			ApplyPagination((specParams.PageIndex -1) * specParams.PageSize, specParams.PageSize);
        }

		// this ctor is will be used for creating object that will be used to get a specific product with Id
		public ProductWithBrandAndCategorySpecification(int id) : base(p => p.Id == id)
		{
			Includes.Add(p => p.Brand);
			Includes.Add(p => p.Category);
		}

	}
}

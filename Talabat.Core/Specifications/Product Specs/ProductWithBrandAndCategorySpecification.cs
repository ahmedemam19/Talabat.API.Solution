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
        public ProductWithBrandAndCategorySpecification(string sort) : base()
        {
            Includes.Add(p =>  p.Brand);
            Includes.Add(p =>  p.Category);


			if (!string.IsNullOrEmpty(sort))
			{
				switch (sort)
				{
					case "priceAsc":
						OrderBy = p => p.Price;
						break;
					case "priceDesc":
						OrderByDesc = p => p.Price;
						break;
					default:
						OrderBy = p => p.Name;
						break;
				}
			}
			else
				AddOrderBy(p => p.Name);

        }

		// this ctor is will be used for creating object that will be used to get a specific product with Id
		public ProductWithBrandAndCategorySpecification(int id) : base(p => p.Id == id)
		{
			Includes.Add(p => p.Brand);
			Includes.Add(p => p.Category);
		}

	}
}

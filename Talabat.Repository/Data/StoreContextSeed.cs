using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
	public class StoreContextSeed
	{
		public static async Task SeedAsync(StoreDbContext _dbContext)
		{
			if (_dbContext.ProductBrands.Count() == 0)
			{
				var brandsData = File.ReadAllText("../Talabat.Repository/DataSeed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

				if (brands?.Count > 0)
				{

					#region To Ignore the [ Id ] Sent from the JSON file 
					//brands = brands.Select(b => new ProductBrand()
					//{
					//	Name = b.Name,
					//}).ToList(); // to ignore the [ Id ] Sent from the JSON file 
					#endregion

					foreach (var brand in brands)
					{
						_dbContext.Set<ProductBrand>().Add(brand);
					}
					await _dbContext.SaveChangesAsync();
				} 
			}

			if (_dbContext.ProductCategories.Count() == 0)
			{
				var categoriesData = File.ReadAllText("../Talabat.Repository/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

				if (categories?.Count > 0)
				{

					#region To Ignore the [ Id ] Sent from the JSON file 
					//categories = categories.Select(b => new ProductCategory()
					//{
					//	Name = b.Name,
					//}).ToList(); // to ignore the [ Id ] Sent from the JSON file 
					#endregion

					foreach (var category in categories)
					{
						_dbContext.Set<ProductCategory>().Add(category);
					}
					await _dbContext.SaveChangesAsync();
				}
			}

			if (_dbContext.Products.Count() == 0)
			{
				var productsData = File.ReadAllText("../Talabat.Repository/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productsData);

				if (products?.Count > 0)
				{
					foreach (var product in products)
					{
						_dbContext.Set<Product>().Add(product);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
		}
	}
}

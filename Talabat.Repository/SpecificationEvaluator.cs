using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;

namespace Talabat.Repository
{
	public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
		{
			var query = inputQuery; // _dbContext.Set<Product>()

			if(spec.Criteria is not null) // p => p.Id == 1
				query = query.Where(spec.Criteria);

			// query = _dbContext.Set<Product>().Where(p => p.BrandId == 1 && p.CategoryId == 1)


			if (spec.OrderBy is not null) // to sort by price
				query = query.OrderBy(spec.OrderBy);

			// query = _dbContext.Set<Product>().Where(p => p.BrandId == 1 && true).OrderBy(p => p.Name)

			else if (spec.OrderByDesc is not null) // to sort by price Desc
				query = query.OrderByDescending(spec.OrderByDesc);


			if(spec.IsPaginationEnabled)
				query = query.Skip(spec.Skip).Take(spec.Take);


			// query = _dbContext.Set<Product>().Where(p => p.BrandId == 1 && p.CategoryId == 1).OrderBy(p => p.Price)
			// Includes
			// 1. p => p.Brand
			// 2. p => p.Category


			query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));

			// _dbContext.Product.Where(p => true && true).OrderBy(p => p.Name).Skip(5).Take(5).Include(p => p.Brand)
			// _dbContext.Product.Where(p => true && true).OrderBy(p => p.Name).Skip(5).Take(5).Include(p => p.Brand).Include(p => p.Category)


			return query;
		}
	}
}

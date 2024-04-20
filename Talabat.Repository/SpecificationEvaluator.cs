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

			// query = _dbContext.Set<Product>().Where(p => p.BrandId == 1 && true)

			else if (spec.OrderByDesc is not null) // to sort by price Desc
				query = query.OrderByDescending(spec.OrderByDesc);


			if(spec.IsPaginationEnabled)
				query = query.Skip(spec.Skip).Take(spec.Take);


			// query = _dbContext.Set<Product>().Where(p => p.BrandId == 2 && true)
			// Includes



			query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));

			// query = _dbContext.Set<Product>().Where(p => p.BrandId == 2 && true)

			return query;
		}
	}
}

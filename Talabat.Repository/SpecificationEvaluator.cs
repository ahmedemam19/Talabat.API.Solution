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
	internal static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
		{
			var query = inputQuery; // _dbContext.Set<Product>()

			if(spec.Criteria is not null) // p => p.Id == 1
				query = query.Where(spec.Criteria);

			// query = _dbContext.Set<Product>().Where(p => p.Id == 1)
			// Includes
			// 1. p => p.Brand
			// 2. p => p.Category

			query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));

			// query = _dbContext.Set<Product>().Where(p => p.Id == 1).Include(p => p.Brand)
			// query = _dbContext.Set<Product>().Where(p => p.Id == 1).Include(p => p.Brand).Include(p => p.Category)


			return query;
		}
	}
}

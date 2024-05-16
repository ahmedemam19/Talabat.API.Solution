﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
	public class GenericRepository<T> : IGenericRepoistory<T> where T : BaseEntity
	{
		private readonly StoreDbContext _dbContext;

		public GenericRepository(StoreDbContext dbContext)
        {
			_dbContext = dbContext;
		}


        public async Task<IReadOnlyList<T>> GetAllAsync()
		{ 
            return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}


		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).ToListAsync();
		}

		public async Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).FirstOrDefaultAsync();
		}


		public async Task<int> GetCountAsync(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).CountAsync();
		}


		private IQueryable<T> ApplySpecification (ISpecification<T> spec)
		{
			return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
		}


		public void AddAsync(T entity)
			=> _dbContext.Set<T>().AddAsync(entity);

		public void Update(T entity)
			=> _dbContext.Set<T>().Update(entity);

		public void Delete(T entity)
			=> _dbContext.Set<T>().Remove(entity);
	}
}

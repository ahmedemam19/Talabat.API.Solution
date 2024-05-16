using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositories.Contract
{
	public interface IGenericRepoistory<T> where T : BaseEntity
	{
		Task<T?> GetByIdAsync(int id);
		Task<IReadOnlyList<T>> GetAllAsync();

		Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec);
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

		Task<int> GetCountAsync(ISpecification<T> spec);

		void AddAsync(T entity);

		void Update(T entity);

		void Delete(T entity);

	}
}

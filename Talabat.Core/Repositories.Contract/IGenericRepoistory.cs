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
		Task<T?> GetAsync(int id);

		Task<IEnumerable<T>> GetAllAsync();

		Task<T?> GetWithSpecAsync(ISpecification<T> spec);

		Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec);

	}
}

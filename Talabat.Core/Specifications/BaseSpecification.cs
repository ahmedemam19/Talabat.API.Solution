using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
	internal class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
	{
		public Expression<Func<T, bool>> Criteria { get; set; } = null;
		public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public BaseSpecification() // to make query that gets all the products
        {
            //Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression) // to make query that gets specific product by Id
		{
            Criteria = criteriaExpression;
			//Includes = new List<Expression<Func<T, object>>>();
		}
    }
}

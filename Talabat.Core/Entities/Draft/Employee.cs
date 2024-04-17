using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Draft
{
	public class Employee : BaseEntity
	{
        public string? Name{ get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}

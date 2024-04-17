using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Draft;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.Employee_Specs;

namespace Talabat.APIs.Controllers
{
	public class EmployeeController : BaseApiController
	{
		private readonly IGenericRepoistory<Employee> _employeeRepo;

		public EmployeeController(IGenericRepoistory<Employee> employeeRepo)
		{
			_employeeRepo = employeeRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
		{
			var spec = new EmployeeWithDepartmentSpecification();
			var employees = await _employeeRepo.GetAllWithSpecAsync(spec);

			if (employees == null) { return NotFound(new ApiResponse(404)); }

			return Ok(employees);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<IEnumerable<Employee>>> GetById(int id)
		{
			var spec = new EmployeeWithDepartmentSpecification(id);
			var employee = await _employeeRepo.GetWithSpecAsync(spec);

			if (employee == null) { return NotFound(new ApiResponse(404)); }

			return Ok(employee);
		}
    }
}

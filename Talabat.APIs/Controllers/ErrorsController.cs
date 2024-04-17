using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
	[Route("errors/{code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)] // to make swagger ignore this controller and run without it
	public class ErrorsController : ControllerBase
	{
		[HttpGet]
		public ActionResult Error(int code)
		{
			if (code == 400)
				return BadRequest(new ApiResponse(400));
			else if (code == 401)
				return Unauthorized(new ApiResponse(401));
			else if (code == 404)
				return NotFound(new ApiResponse(404));
			else
				return StatusCode(code);
		}
	}
}

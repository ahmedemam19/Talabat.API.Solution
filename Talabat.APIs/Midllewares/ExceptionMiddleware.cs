using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Midllewares
{
	// Convension Based
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
			_next = next;
			_logger = logger;
			_env = env;
		}
        public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{

				// take an action with the request

				await _next.Invoke(httpContext); // Go to next middleware

				// take an action with the respone
 
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message); // Development Env
											  // log Exception in ( Database | Files ) // Production Env


				httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
				httpContext.Response.ContentType = "application/json";


				var response = _env.IsDevelopment() ?
					new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
					:
					new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

				var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

				var json = JsonSerializer.Serialize(response, options);

				await httpContext.Response.WriteAsync(json);
			}
		}
	}
}

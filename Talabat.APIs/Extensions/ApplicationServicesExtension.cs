using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			//Allowing Dependancy injection for the Basket Repo
			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));


			//Allowing Dependancy injection for the Generic Repo for all types 
			services.AddScoped(typeof(IGenericRepoistory<>), typeof(GenericRepository<>));


			//services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
			services.AddAutoMapper(typeof(MappingProfiles));


			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(parameter => parameter.Value.Errors.Count() > 0)
														 .SelectMany(parameter => parameter.Value.Errors)
														 .Select(error => error.ErrorMessage)
														 .ToArray();

					var response = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);
				};
			});

			return services;
		}
	}
}

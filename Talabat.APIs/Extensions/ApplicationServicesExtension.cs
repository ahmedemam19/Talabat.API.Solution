using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			//Allowing Dependancy injection for the UnitOfWork
			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));


			//Allowing Dependancy injection for the Basket Repo
			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));


			//Allowing Dependancy injection for the Generic Repo for all types 
			//services.AddScoped(typeof(IGenericRepoistory<>), typeof(GenericRepository<>));


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


		public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
		{
			//webApplicationBuilder.Services.AddAuthentication().AddJwtBearer("Bearer", options =>
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // For any action that take the authorize attribute without specifying which schema
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero, // to solve the change of time all over the world
					};

				});

			return services;
		}

	}
}

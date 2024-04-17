
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Midllewares;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
	public class Program
	{
		// Entry Point
		public static async Task Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);


			#region Configure Services
			// Add services to the Dipendancy Injection container.


			webApplicationBuilder.Services.AddControllers();
			// Register Reuqired Web APIs Services to the Dipendancy Injection Container


			webApplicationBuilder.Services.AddDbContext<StoreDbContext>(options  =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			webApplicationBuilder.Services.AddEndpointsApiExplorer();
			webApplicationBuilder.Services.AddSwaggerGen();

			//Allowing Dependancy injection for the Generic Repo for all types 
			webApplicationBuilder.Services.AddScoped(typeof(IGenericRepoistory<>), typeof(GenericRepository<>));


			//webApplicationBuilder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
			webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));


			webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
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

					return new  BadRequestObjectResult(response);
				};
			});

			#endregion


			var app = webApplicationBuilder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbcontext = services.GetRequiredService<StoreDbContext>();
			// Ask CLR fro creating Object From DbContext Explicitly


			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbcontext.Database.MigrateAsync(); // Update Database
				await StoreContextSeed.SeedAsync(_dbcontext); // Data Seeding
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An Error Occured during applying Migration");
			}

			#region Configure Kestrel Middlewares

			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			//app.UseAuthorization();

			app.UseStaticFiles();

			app.MapControllers(); /// It collects all the routes of the controllers
								  /// It is used instead of [ UseRouting & UseEndPoints ]
								  /// It Rely on the Attribute [ Route ] in the Controller

			#endregion


			app.Run();
		}
	}
}

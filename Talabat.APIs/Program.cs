
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Midllewares;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository._Identity;
using Talabat.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Service.AuthService;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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


			webApplicationBuilder.Services.AddSwaggerServices();


			//ApplicationServicesExtension.AddApplicationServices(webApplicationBuilder.Services);
			webApplicationBuilder.Services.AddApplicationServices();


			webApplicationBuilder.Services.AddDbContext<StoreDbContext>(options  =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});


			webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
			});
			

			webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});


			webApplicationBuilder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));

			// Adding Default identity system config for specified User and Role
			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				//options.Password.RequiredUniqueChars = 2;
			}).AddEntityFrameworkStores<ApplicationIdentityDbContext>();


			webApplicationBuilder.Services.AddAuthServices(webApplicationBuilder.Configuration); // for JWT

			#endregion


			var app = webApplicationBuilder.Build();


			#region Apply all Pending Migrations

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbcontext = services.GetRequiredService<StoreDbContext>();
			// Ask CLR fro creating Object From DbContext Explicitly for StoreDbContext

			var _identityDbcontext = services.GetRequiredService<ApplicationIdentityDbContext>();
			// Ask CLR fro creating Object From DbContext Explicitly for ApplicationIdentityDbContext

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			var logger = loggerFactory.CreateLogger<Program>();

			try
			{
				await _dbcontext.Database.MigrateAsync(); // Update Database for StoreDbContext
				await StoreContextSeed.SeedAsync(_dbcontext); // Data Seeding for StoreDbContext


				await _identityDbcontext.Database.MigrateAsync(); // Update Database for ApplicationIdentityDbContext

				var _usermanager = services.GetRequiredService<UserManager<ApplicationUser>>();
				await ApplicationIdentityContextSeed.SeedUserAsync(_usermanager); // Data Seeding for ApplicationIdentityDbContext
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An Error Occured during applying Migration");
			}

			#endregion


			#region Configure Kestrel Middlewares

			app.UseMiddleware<ExceptionMiddleware>();

			#region Implementing the middleware in the [ program file ] instead of making a class

			//app.Use(async (httpContext, _next) =>
			//{
			//	try
			//	{

			//		// take an action with the request

			//		await _next.Invoke(httpContext); // Go to next middleware

			//		// take an action with the respone

			//	}
			//	catch (Exception ex)
			//	{
			//		logger.LogError(ex.Message); // Development Env
			//									  // log Exception in ( Database | Files ) // Production Env


			//		httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			//		httpContext.Response.ContentType = "application/json";


			//		var response = webApplicationBuilder.Environment.IsDevelopment() ?
			//			new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
			//			:
			//			new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

			//		var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			//		var json = JsonSerializer.Serialize(response, options);

			//		await httpContext.Response.WriteAsync(json);
			//	}
			//}); 

			#endregion

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddlewares();
			}

			// It work in case a request sent does not match with any endpoint i the controller
			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.MapControllers(); /// It collects all the routes of the controllers
								  /// It is used instead of [ UseRouting & UseEndPoints ]
								  /// It Rely on the Attribute [ Route ] in the Controller
			#endregion


			app.Run();
		}
	}
}

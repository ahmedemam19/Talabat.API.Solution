
using Microsoft.EntityFrameworkCore;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
	public class Program
	{
		// Entry Point
		public static void Main(string[] args)
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

			#endregion


			var app = webApplicationBuilder.Build();


			#region Configure Kestrel Middlewares

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			//app.UseAuthorization();

			app.MapControllers(); /// It collects all the routes of the controllers
								  /// It is used instead of [ UseRouting & UseEndPoints ]
								  /// It Rely on the Attribute [ Route ] in the Controller

			#endregion


			app.Run();
		}
	}
}

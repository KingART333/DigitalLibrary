
using DigitalLibraryConsole.Data;
using DigitalLibraryConsole.Service;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibraryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var dbPath = builder.Configuration.GetConnectionString("LibraryDb");

            // Add services to the container.
            builder.Services.AddDbContext<LibraryContext>(options =>
                options.UseSqlite($"Data Source={dbPath}")); ;

            builder.Services.AddScoped<LibraryService>();
            builder.Services.AddControllers();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

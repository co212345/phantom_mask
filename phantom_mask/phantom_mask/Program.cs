
using Microsoft.EntityFrameworkCore;
using phantom_mask.Data;

namespace phantom_mask
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<PharmacyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<PharmacyDbContext>();
                await PharmacySeeder.SeedAsync(db, "Data/pharmacies.json");
                await UserSeeder.SeedAsync(db, "Data/Users.json");
            }



            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}


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
            if (args.Length == 2 && args[0] == "import:pharmacies")
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<PharmacyDbContext>();
                await PharmacySeeder.SeedAsync(db, args[1]);
                Console.WriteLine($"已匯入藥局資料：{args[1]}");
                return;
            }
            else if (args.Length == 2 && args[0] == "import:users")
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<PharmacyDbContext>();
                await UserSeeder.SeedAsync(db, args[1]);
                Console.WriteLine($"已匯入使用者資料：{args[1]}");
                return;
            }
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

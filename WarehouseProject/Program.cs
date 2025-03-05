

using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using WarehouseProject.Models;
using WarehouseProject.Services;
using WarehouseProject.Services.ServicesImp;
using WarehouseProject.Util;
using WarehouseProject.Controllers;

namespace WarehouseProject {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DB");
            builder.Services.AddDbContext<WarehouseDBContext>(options =>
                options.UseSqlServer(connectionString));

            //setup cloudinary
            builder.Services.AddSingleton(provider => {
                var config = builder.Configuration.GetSection("Cloudinary").Get<CloundinarySettings>();
                return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
            });

            // Register seed data
            builder.Services.AddScoped<SeedData>();

            // Register service
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            //------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope()) {
                var seedData = scope.ServiceProvider.GetRequiredService<SeedData>();
                seedData.SeedRole();
            }

            app.Run();
        }
    }
}

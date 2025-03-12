using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using WarehouseProject.Models;
using WarehouseProject.Services;
using WarehouseProject.Services.ServicesImp;
using WarehouseProject.Util;
using WarehouseProject.Controllers;
using WarehouseProject.Models.Entity;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

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

            // Register session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Register service
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            // Register JsonIgnore
            builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            //------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                //options.MapType<Enum>(() => new OpenApiSchema {
                //    Type = "string",
                //    Enum = Enum.GetNames(typeof(Enum)).Select(name => new OpenApiString(name)).ToList()
                //});
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.UseSession();

            app.MapControllers();

            using (var scope = app.Services.CreateScope()) {
                var seedData = scope.ServiceProvider.GetRequiredService<SeedData>();
                seedData.SeedRole();
                seedData.AddAdmin();
            }

            app.Run();
        }
    }
}

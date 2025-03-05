using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using WarehouseProject.Models.Entity;
using WarehouseProject.Util;

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


            // Register repository

            // Register service


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

            app.Run();
        }
    }
}

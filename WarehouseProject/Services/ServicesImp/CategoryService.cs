using Microsoft.EntityFrameworkCore;
using WarehouseProject.Models;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services.ServicesImp {
    public class CategoryService : ICategoryService {

        private readonly WarehouseDBContext _context;
        public CategoryService(WarehouseDBContext context) {
            _context = context;
        }

        //====================================================================================================

        public (bool isSuccess, string message) Create(CategoryDTO entity) {
            try {
                var category = new Category {
                    Name = entity.Name,
                    Description = entity.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                _context.Categories.Add(category);
                _context.SaveChanges();
                return (true, "Create Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public (bool isSuccess, string message) Delete(int id) {
            try {
                var category = _context.Categories.FirstOrDefault(x => x.CategoryId == id);

                if (category == null) {
                    return (false, "No found to update");
                }

                _context.Products.Where(x => x.CategoryId == id).ToList().ForEach(x => x.CategoryId = null);
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return (true, "Delete Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public List<Category> GetAll(string? search) {
            try {
                if (!string.IsNullOrEmpty(search)) {
                    return _context.Categories
                        .Where(x => x.Name.Contains(search))
                        .OrderByDescending(x => x.CreatedAt)
                        .ToList();
                }
                return _context.Categories
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList();

            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }

        public Category GetDetail(int id) {
            try {
                var category = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
                return category;
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }

        public (bool isSuccess, string message) Update(int id, CategoryDTO entity) {
            try {
                var category = _context.Categories.FirstOrDefault(x => x.CategoryId == id);

                if (category == null) {
                    return (false, "No found to update");
                }

                category.Name = entity.Name;
                category.Description = entity.Description;
                category.UpdatedAt = DateTime.Now;

                _context.SaveChangesAsync();
                return (true, "Update Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

    }
}

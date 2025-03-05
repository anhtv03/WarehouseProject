using Microsoft.EntityFrameworkCore;
using WarehouseProject.Models;
using WarehouseProject.Models.Form;

namespace WarehouseProject.Services.ServicesImp {
    public class SupplierService : ISupplierService {
        private readonly WarehouseDBContext _context;

        public SupplierService(WarehouseDBContext context) {
            _context = context;
        }
        //====================================================================================================

        public (bool isSuccess, string message) Create(SupplierDTO entity) {
            try {
                var supplier = new Supplier {
                    Name = entity.Name,
                    Phone = entity.Phone,
                    Email = entity.Email,
                    Address = entity.Address,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                _context.Suppliers.Add(supplier);
                _context.SaveChanges();
                return (true, "Create Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public (bool isSuccess, string message) Delete(int id) {
            try {
                var supplier = _context.Suppliers.FirstOrDefault(x => x.SupplierId == id);

                if (supplier == null) {
                    return (false, "No supplier found to delete");
                }

                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
                return (true, "Delete Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public List<Supplier> GetAll(string? search) {
            try {
                var query = _context.Suppliers.AsQueryable();

                if (!string.IsNullOrEmpty(search)) {
                    query = query.Where(x => x.Name.Contains(search) ||
                                             x.Address.Contains(search) ||
                                             x.Phone.Contains(search));
                }

                query = query.OrderByDescending(x => x.CreatedAt);

                return query.ToList();
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }

        public Supplier GetDetail(int id) {
            try {
                var supplier = _context.Suppliers.FirstOrDefault(x => x.SupplierId == id);
                return supplier;
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }

        public (bool isSuccess, string message) Update(int id, SupplierDTO entity) {
            try {
                var supplier = _context.Suppliers.FirstOrDefault(x => x.SupplierId == id);

                if (supplier == null) {
                    return (false, "No supplier found to update");
                }

                supplier.Name = entity.Name;
                supplier.Phone = entity.Phone;
                supplier.Email = entity.Email;
                supplier.Address = entity.Address;
                supplier.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return (true, "Update Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }
    }
}
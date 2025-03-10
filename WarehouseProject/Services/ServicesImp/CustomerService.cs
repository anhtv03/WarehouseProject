using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services.ServicesImp {
    public class CustomerService : ICustomerService {
        private readonly WarehouseDBContext _context;

        public CustomerService(WarehouseDBContext context) {
            _context = context;
        }

        //====================================================================================================
        public (bool isSuccess, string message) Create(CustomerDTO entity) {
            try {
                var cus = new Customer {
                    Address = entity.Address,
                    Email = entity.Email,
                    FullName = entity.FullName,
                    Phone = entity.Phone,
                    Note = entity.Note,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                _context.Customers.Add(cus);
                _context.SaveChanges();
                return (true, "Created Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public (bool isSuccess, string message) Delete(int id) {
            try {
                var cus = _context.Customers.FirstOrDefault(p => p.CustomerId == id);

                if (cus == null) {
                    return (false, "No found to delete.");
                }

                _context.Orders.Where(p => p.CustomerId == id).ToList().ForEach(p => p.CustomerId = null);
                _context.Customers.Remove(cus);
                _context.SaveChanges();
                return (true, "Delete successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public List<Customer> GetAll(string? search) {
            try {
                var query = _context.Customers.AsQueryable();

                if (!string.IsNullOrEmpty(search)) {
                    query = query.Where(p => p.FullName.Contains(search) ||
                                             p.Address.Contains(search) ||
                                             p.Phone.Contains(search));
                }

                query = query.OrderByDescending(p => p.CreatedAt);
                return query.ToList();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Customer GetDetail(int id) {
            try {
                var cus = _context.Customers.FirstOrDefault(p => p.CustomerId == id);
                return cus;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public (bool isSuccess, string message) Update(int id, CustomerDTO entity) {
            try {
                var cus = _context.Customers.FirstOrDefault(p => p.CustomerId == id);

                if (cus == null) {
                    return (false, "No customer found to update.");
                }

                cus.FullName = entity.FullName;
                cus.Phone = entity.Phone;
                cus.Email = entity.Email;
                cus.Address = entity.Address;
                cus.Note = entity.Note;
                cus.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return (true, "Update Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }
    }
}

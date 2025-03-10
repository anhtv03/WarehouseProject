using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models;
using WarehouseProject.Models.Entity;
using WarehouseProject.Services;

namespace WarehouseProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase {

        private readonly WarehouseDBContext _context;
        public RolesController(WarehouseDBContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRoles() {
            var roles = _context.Roles.ToList();
            return Ok(roles);
        }


    }
}

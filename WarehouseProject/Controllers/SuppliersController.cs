using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models;
using WarehouseProject.Models.Form;
using WarehouseProject.Services;

namespace WarehouseProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase {
        private readonly ISupplierService _service;
        public SuppliersController(ISupplierService service) {
            _service = service;
        }

        [HttpGet]
        public ActionResult GetAll([FromQuery] string? search) {
            var list = _service.GetAll(search);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult GetDetails([FromRoute] int id) {
            try {
                var data = _service.GetDetail(id);

                if (data == null) {
                    return NotFound("Supplier is not exised");
                }

                return Ok(data);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Create([FromBody] SupplierDTO supplier) {
            try {
                _service.Create(supplier);
                return Created("", "Created success");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] SupplierDTO supplier) {
            try {
                _service.Update(id, supplier);
                return Ok("Update success");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id) {
            try {
                _service.Delete(id);
                return Ok("Delete success");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

    }
}

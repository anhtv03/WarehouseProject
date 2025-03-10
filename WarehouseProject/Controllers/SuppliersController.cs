using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models.DTOs;
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
                var result = _service.Create(supplier);
                if (result.isSuccess) {
                    return Created("", result.message);
                } else {
                    return BadRequest(result.message);
                }
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] SupplierDTO supplier) {
            try {
                var result = _service.Update(id, supplier);
                if (result.isSuccess) {
                    return Ok(result.message);
                } else {
                    return BadRequest(result.message);
                }
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id) {
            try {
                var result = _service.Delete(id);
                if (result.isSuccess) {
                    return Ok(result.message);
                } else {
                    return BadRequest(result.message);
                }
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

    }
}

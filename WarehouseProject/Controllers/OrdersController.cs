using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Services;
using WarehouseProject.Util;

namespace WarehouseProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {
        private readonly IOrderService _service;
        public OrdersController(IOrderService service) {
            _service = service;
        }

        [HttpGet("{role}")]
        public ActionResult GetAll([FromRoute] OrderTypeEnum role, [FromQuery] string? search) {
            var list = _service.GetAll(role, search);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult GetDetails([FromRoute] int id) {
            try {
                var data = _service.GetDetail(id);

                if (data == null) {
                    return NotFound("Order is not exised");
                }

                return Ok(data);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Create([FromBody] OrderDTO order) {
            try {
                var result = _service.Create(order);
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
        public ActionResult Update([FromRoute] int id, [FromBody] OrderDTO order) {
            try {
                var result = _service.Update(id, order);
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

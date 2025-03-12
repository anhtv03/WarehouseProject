using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Services;

namespace WarehouseProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase {
        private readonly IOrderDetailService _service;
        public OrderDetailsController(IOrderDetailService service) {
            _service = service;
        }

        [HttpGet]
        public ActionResult GetAll() {
            var list = _service.GetAll();
            return Ok(list);
        }
        
        [HttpGet("Order/{id}")]
        public ActionResult GetByOrderId([FromRoute] int id) {
            var list = _service.GetByOrderId(id);
            return Ok(list);
        }
        
        [HttpGet("Product/{id}")]
        public ActionResult GetByProductId([FromRoute] int id) {
            var list = _service.GetByProductId(id);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult GetDetails([FromRoute] int id) {
            try {
                var data = _service.GetDetail(id);

                if (data == null) {
                    return NotFound("Order Detail is not exised");
                }

                return Ok(data);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Create([FromBody] OrderDetailDTO order) {
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
        public ActionResult Update([FromRoute] int id, [FromBody] OrderDetailDTO order) {
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

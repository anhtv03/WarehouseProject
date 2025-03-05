using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Form;
using WarehouseProject.Services;

namespace WarehouseProject.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly IProductService _service;
        public ProductsController(IProductService service) {
            _service = service;
        }

        [HttpGet]
        public ActionResult GetAll([FromQuery] string? search) {
            var list = _service.GetAll(search);
            return Ok(list);
        }
        
        [HttpGet("Category/{id}")]
        public ActionResult GetAllByCategory([FromRoute] int id) {
            var list = _service.GetProductByCategory(id);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult GetDetails([FromRoute] int id) {
            try {
                var data = _service.GetDetail(id);

                if (data == null) {
                    return NotFound("Product is not exised");
                }

                return Ok(data);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Create([FromBody] ProductDTO product) {
            try {
                _service.Create(product);
                return Created("", "Created success");
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] ProductDTO product) {
            try {
                _service.Update(id, product);
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

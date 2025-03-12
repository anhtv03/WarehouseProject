using Microsoft.AspNetCore.Mvc;
using WarehouseProject.Models.DTOs;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductDTO product, IFormFile? file) {
            try {
                var result = await _service.CreateAsync(product, file);
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ProductDTO product, IFormFile? file) {
            try {
                var result = await _service.UpdateAsync(id, product, file);
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

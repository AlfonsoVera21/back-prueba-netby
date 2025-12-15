using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Commands;
using ProductService.Application.Handlers;
using ProductService.Domain.Ports;

namespace ProductService.Api.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IProductSearchService _searchService;
        private readonly CreateProductHandler _createHandler;
        private readonly UpdateProductHandler _updateHandler;

        public ProductsController(
            IProductRepository repository,
            IProductSearchService searchService,
            CreateProductHandler createHandler,
            UpdateProductHandler updateHandler)
        {
            _repository = repository;
            _searchService = searchService;
            _createHandler = createHandler;
            _updateHandler = updateHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand cmd)
        {
            var id = await _createHandler.HandleAsync(cmd);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // GET api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products);
        }

        // GET api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null) return NotFound();
            return Ok(product);
        }

        // PUT api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand cmd)
        {
            if (id != cmd.Id)
                return BadRequest("Id de la ruta y del cuerpo no coinciden.");

            await _updateHandler.HandleAsync(cmd);
            return NoContent();
        }

        // DELETE api/products/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null) return NotFound();

            await _repository.DeleteAsync(product);
            return NoContent();
        }

        // POST api/products/{id}/increase-stock?quantity=10
        [HttpPost("{id:guid}/increase-stock")]
        public async Task<IActionResult> IncreaseStock(Guid id, [FromQuery] int quantity)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null) return NotFound();

            product.IncreaseStock(quantity);
            await _repository.UpdateAsync(product);

            return Ok(new { product.Id, product.Stock });
        }

        // POST api/products/{id}/decrease-stock?quantity=5
        [HttpPost("{id:guid}/decrease-stock")]
        public async Task<IActionResult> DecreaseStock(Guid id, [FromQuery] int quantity)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null) return NotFound();

            product.DecreaseStock(quantity);
            await _repository.UpdateAsync(product);

            return Ok(new { product.Id, product.Stock });
        }

        // GET api/products/search?name=mouse
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var products = await _searchService.SearchByNameAsync(name);
            return Ok(products);
        }
    }
}

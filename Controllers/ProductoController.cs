using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]

public class ProductoController : ControllerBase
{
    private ILogger<ProductoController> _logger;
    private ProductoRepository _productoRepository;

    public ProductoController(ILogger<ProductoController> logger, ProductoRepository productoRepository)
    {
        _logger = logger;
        _productoRepository = productoRepository;
    }

    [HttpGet("/api/Producto")]
    public ActionResult<List<Producto>> ListarProductos()
    {
        var productos = _productoRepository.ListarProductos();
        return Ok(productos);
    }

    [HttpPost]
    public ActionResult CrearProducto([FromBody] Producto producto)
    {
        if (producto == null) return BadRequest("El producto es inválido.");

        _productoRepository.Insert(producto);
        return CreatedAtAction(nameof(CrearProducto), producto);
    }

    [HttpPut("{idProducto}")]
    public ActionResult ModificarProducto(int idProducto, [FromBody] Producto producto)
    {
        if (producto == null) return BadRequest("El producto es inválido.");

        var buscado = _productoRepository.FindById(idProducto);
        if (buscado == null) return NotFound($"No se encontró el producto con ID {idProducto}.");

        _productoRepository.Update(producto, idProducto);
        return NoContent();
    }

    [HttpDelete("{idProducto}")]
    public ActionResult EliminarProducto(int idProducto)
    {
        var buscado = _productoRepository.FindById(idProducto);
        if (buscado == null) return NotFound($"No se encontró el producto con ID {idProducto}.");

        _productoRepository.Delete(idProducto);
        return NoContent();
    }

}
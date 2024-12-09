using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]

public class PresupuestoController : ControllerBase
{
    private ILogger<PresupuestoController> _logger;
    private PresupuestoRepository _presupuestoRepository;

    public PresupuestoController(ILogger<PresupuestoController> logger, PresupuestoRepository presupuestoRepository)
    {
        _logger = logger;
        _presupuestoRepository = presupuestoRepository;
    }

    [HttpGet("/api/Presupuesto")]
    public ActionResult<List<Presupuesto>> ListarPresupuestos()
    {
        var presupuestos = _presupuestoRepository.ListarPresupuestos();
        return Ok(presupuestos);
    }

    [HttpPost]
    public ActionResult CrearPresupuesto([FromBody] Presupuesto presupuesto)
    {
        if (presupuesto == null) return BadRequest("El Presupuesto es inválido.");

        _presupuestoRepository.Insert(presupuesto);
        return CreatedAtAction(nameof(CrearPresupuesto), presupuesto);
    }

    [HttpPut("{idPresupuesto}")]
    public ActionResult ModificarPresupuesto(int idPresupuesto, [FromBody] Presupuesto presupuesto)
    {
        if (presupuesto == null) return BadRequest("El Presupuesto es inválido.");

        var buscado = _presupuestoRepository.FindById(idPresupuesto);
        if (buscado == null) return NotFound($"No se encontró el Presupuesto con ID {idPresupuesto}.");

        _presupuestoRepository.Update(presupuesto, idPresupuesto);
        return NoContent();
    }

    [HttpDelete("{idPresupuesto}")]
    public ActionResult EliminarPresupuesto(int idPresupuesto)
    {
        var buscado = _presupuestoRepository.FindById(idPresupuesto);
        if (buscado == null) return NotFound($"No se encontró el Presupuesto con ID {idPresupuesto}.");

        _presupuestoRepository.Delete(idPresupuesto);
        return NoContent();
    }

    [HttpPost("{idPresupuesto}/Producto")]
    public ActionResult AgregarProductoAPresupuesto(int idPresupuesto, [FromBody] Producto productoDto)
    {
        if (productoDto == null) return BadRequest("Los datos del producto son inválidos.");

        var presupuesto = _presupuestoRepository.FindById(idPresupuesto);
        if (presupuesto == null) return NotFound($"No se encontró el presupuesto con ID {idPresupuesto}.");

        // Suponemos que la cantidad es siempre 1 por defecto. 
        int cantidad = 1;

        _presupuestoRepository.AgregarProductoAPresupuesto(idPresupuesto, productoDto, cantidad);
        return Ok($"Producto con ID {productoDto.IdProducto} agregado al presupuesto {idPresupuesto}.");
    }

    [HttpGet("{idPresupuesto}/Detalles")]
    public ActionResult VerPresupuestoConProductos(int idPresupuesto)
    {
        var presupuesto = _presupuestoRepository.FindById(idPresupuesto);
        if (presupuesto == null) return NotFound($"No se encontró el presupuesto con ID {idPresupuesto}.");

        return Ok(presupuesto);
    }

}
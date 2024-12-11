using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
// using tl2_tp6_2024_marianogk.Models;

[ApiController]
[Route("api/[controller]")]

public class PresupuestoController : Controller
{
    private ILogger<PresupuestoController> _logger;
    private PresupuestoRepository _presupuestoRepository;
    private readonly ProductoRepository _productoRepository;

    public PresupuestoController(ILogger<PresupuestoController> logger, PresupuestoRepository presupuestoRepository, ProductoRepository productoRepository)
    {
        _logger = logger;
        _presupuestoRepository = presupuestoRepository;
        _productoRepository = productoRepository;
    }

    [HttpGet]
    public ActionResult Index()
    {
        var presupuestos = _presupuestoRepository.ListarPresupuestos();
        return View(presupuestos);
    }

    // get crear presupuesto
    [HttpGet("Crear")]
    public ActionResult Crear()
    {
        return View();
    }

    // post crear presupuesto
    [HttpPost("Crear")]
    public IActionResult Crear(Presupuesto presupuesto)
    {
        if (!ModelState.IsValid)
        {
            return View(presupuesto);
        }

        _presupuestoRepository.Insert(presupuesto);
        return RedirectToAction("Index");
    }

    // mostrar presupuesto a editar
    [HttpGet("Editar/{idPresupuesto}")]
    public ActionResult Editar(int idPresupuesto)
    {

        var buscado = _presupuestoRepository.FindById(idPresupuesto);
        if (buscado == null) return NotFound();

        return View(buscado);
    }

    // mostrar presupuesto a editar
    [HttpPost("Editar/{idPresupuesto}")]
    public ActionResult Editar(int idPresupuesto, Presupuesto presupuesto)
    {
        if (!ModelState.IsValid)
        {
            return View(presupuesto); // Retorna la vista con errores de validación
        }

        _presupuestoRepository.Update(presupuesto, idPresupuesto);
        return RedirectToAction("Index");// Redirige a la lista de presupuestos

    }

    [HttpDelete("Eliminar/{idPresupuesto}")]
    public ActionResult Eliminar(int idPresupuesto)
    {
        _presupuestoRepository.Delete(idPresupuesto);
        return RedirectToAction("Index");
    }

    // get agregar producto
    [HttpGet("{idPresupuesto}/Producto")]
    public ActionResult AgregarProducto(int idPresupuesto)
    {
        var presupuesto = _presupuestoRepository.FindById(idPresupuesto);
        if (presupuesto == null)
        {
            return NotFound($"No se encontró el presupuesto con ID {idPresupuesto}.");
        }

        ViewBag.Productos = _productoRepository.ListarProductos(); // Obtener lista de productos
        return View(presupuesto);
    }

    // post agregar producto
    [HttpPost("{idPresupuesto}/Producto")]
    public ActionResult AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        var presupuesto = _presupuestoRepository.FindById(idPresupuesto);
        var producto = _productoRepository.FindById(idProducto);

        if (producto == null)
        {
            return NotFound($"No se encontró el producto con ID {producto.IdProducto}.");
        }

        _presupuestoRepository.AgregarProductoAPresupuesto(idPresupuesto, producto, cantidad);

        return RedirectToAction("VerPresupuestoConProductos", new { idPresupuesto });
    }

    //mostrar presupuestos con productos
    [HttpGet("{idPresupuesto}/Detalles")]
    public IActionResult VerPresupuestoConProductos(int idPresupuesto)
    {
        var presupuesto = _presupuestoRepository.FindById(idPresupuesto); // Método que incluye productos
        if (presupuesto == null)
        {
            return NotFound($"No se encontró el presupuesto con ID {idPresupuesto}.");
        }

        return View(presupuesto);
    }


}
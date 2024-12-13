using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


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
    [HttpGet("Presupuesto/Crear")]
    public ActionResult Crear()
    {
        return View();
    }

    // post crear presupuesto
    [HttpPost("Presupuesto/Crear")]
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
    [HttpGet("Presupuesto/Editar/{idPresupuesto}")]
    public ActionResult Editar(int idPresupuesto)
    {

        var buscado = _presupuestoRepository.FindById(idPresupuesto);
        if (buscado == null) return NotFound();

        return View(buscado);
    }

    // mostrar presupuesto a editar
    [HttpPost("Presupuesto/Editar/{idPresupuesto}")]
    public ActionResult Editar(int idPresupuesto, Presupuesto presupuesto)
    {
        if (!ModelState.IsValid)
        {
            return View(presupuesto); // Retorna la vista con errores de validación
        }

        _presupuestoRepository.Update(presupuesto, idPresupuesto);
        return RedirectToAction("Index");// Redirige a la lista de presupuestos

    }

    [HttpGet("Presupuesto/Eliminar/{idPresupuesto}")]
    public ActionResult Eliminar(int idPresupuesto)
    {
        var presupuesto = _presupuestoRepository.FindById(idPresupuesto);
        if (presupuesto == null) return NotFound();

        return View(presupuesto);
    }

    [HttpPost("Presupuesto/Eliminar/{idProducto}")]
    public ActionResult ConfirmarEliminar(int idPresupuesto)
    {
        _presupuestoRepository.Delete(idPresupuesto);
        return RedirectToAction("Index");
    }

    // get agregar producto
    [HttpGet("Presupuesto/{idPresupuesto}/Producto")]
    public IActionResult AgregarProducto(int idPresupuesto)
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
    [HttpPost("Presupuesto/{idPresupuesto}/Producto")]
    public IActionResult AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        var presupuesto = _presupuestoRepository.FindById(idPresupuesto);
        var producto = _productoRepository.FindById(idProducto);

        if (producto == null)
        {
            return NotFound($"No se encontró el producto con ID {producto.IdProducto}.");
        }

        if (cantidad <= 0)
        {
            ModelState.AddModelError("", "La cantidad debe ser mayor a 0.");
            ViewBag.Productos = _productoRepository.ListarProductos();
            return View(presupuesto);
        }

        _presupuestoRepository.AgregarProductoAPresupuesto(idPresupuesto, producto, cantidad);

        return RedirectToAction("Detalles", new { idPresupuesto });
    }

    //mostrar presupuestos con productos
    [HttpGet("Presupuesto/{idPresupuesto}/Detalles")]
    public IActionResult Detalles(int idPresupuesto)
    {
        var presupuesto = _presupuestoRepository.FindById(idPresupuesto); // Método que incluye productos
        if (presupuesto == null)
        {
            return NotFound($"No se encontró el presupuesto con ID {idPresupuesto}.");
        }

        return View(presupuesto);
    }


}
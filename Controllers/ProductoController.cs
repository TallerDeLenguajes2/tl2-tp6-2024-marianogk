using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

// [ApiController]
// [Route("api/[controller]")]

public class ProductoController : Controller
{
    private ILogger<ProductoController> _logger;
    private ProductoRepository _productoRepository;

    public ProductoController(ILogger<ProductoController> logger, ProductoRepository productoRepository)
    {
        _logger = logger;
        _productoRepository = productoRepository;
    }

    [HttpGet]
    public ActionResult Index()
    {
        var productos = _productoRepository.ListarProductos();
        return View(productos);
    }

    [HttpGet]
    public ActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Crear(Producto producto)
    {
        if (!ModelState.IsValid)
        {
            return View(producto);
        }

        _productoRepository.Insert(producto);
        return RedirectToAction("Index");
    }

    [HttpGet("Editar/{idProducto}")]
    public ActionResult Editar(int idProducto)
    {
        var producto = _productoRepository.FindById(idProducto);
        if (producto == null) return NotFound();

        return View(producto);
    }

    [HttpPost("Editar/{idProducto}")]
    public ActionResult Editar(int idProducto, Producto producto)
    {
        if (!ModelState.IsValid)
        {
            return View(producto);
        }

        _productoRepository.Update(producto, idProducto);
        return RedirectToAction("Index");
    }

    [HttpDelete("Eliminar/{idProducto}")]
    public ActionResult Eliminar(int idProducto)
    {
        _productoRepository.Delete(idProducto);
        return RedirectToAction("Index");
    }

}
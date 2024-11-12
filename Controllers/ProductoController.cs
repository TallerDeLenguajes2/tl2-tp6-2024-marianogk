using Microsoft.AspNetCore.Mvc;

public class ProductoController : ControllerBase
{
    private ILogger<ProductoController> _logger;
    private ProductoRepository productoRepository;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        productoRepository = new ProductoRepository();
    }

    [HttpGet("/api/Producto")]
    public ActionResult<List<Producto>> ListarProductos()
    {
        Productos = productoRepository.ListarProductos();
        return Ok(Productos);
    }





}
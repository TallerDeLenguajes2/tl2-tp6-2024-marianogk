using EspacioPresupuestoDetalle;
public class Presupuesto
{
    private List<PresupuestoDetalle> detalles;
    public int IdPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public List<PresupuestoDetalle> Detalles { get => detalles; }
    public Presupuesto()
    {
        detalles = new List<PresupuestoDetalle>();
    }

    public void AgregarProducto(Producto prod, int cant)
    {
        PresupuestoDetalle pd = new PresupuestoDetalle(prod,cant);
        // pd.CargarProducto(prod, cant);
        pd.Cantidad = cant;
        detalles.Add(pd);
    }

    public float MontoPresupuesto()
    {
        float total = 0;
        foreach (var p in detalles)
        {
            total += p.Cantidad * p.Producto.Precio;
        }
        return total;
    }
    public double MontoPresupuestoConIva()
    {
        double total = 0;
        const double iva = 1.21;
        foreach (var p in detalles)
        {
            total += p.Cantidad * p.Producto.Precio * iva;
        }
        return total;
    }

    public int CantidadProductos()
    {
        return detalles.Count;
    }
}
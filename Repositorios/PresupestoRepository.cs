using EspacioPresupuestoDetalle;

public class PresupuestoRepository : IRepositoryPresupuesto
{
    private List<Presupuesto> presupuestos;

    public PresupuestoRepository()
    {
        presupuestos = new List<Presupuesto>();
    }
    public void Create(Presupuesto presupuesto)
    {
        presupuestos.Add(presupuesto);
    }

    public List<Presupuesto> FindAll()
    {
        return new List<Presupuesto>(presupuestos);
    }
    public Presupuesto FindById(int idPresupuesto)
    {
        Presupuesto presupuesto = presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);
        return presupuesto;
    }
    public void Insert(int idPresupuesto, Producto producto, int cantidad)
    {
        var presupuesto = FindById(idPresupuesto);
        if (presupuesto != null)
        {
            var detalle = presupuesto.Detalles.FirstOrDefault(d => d.Producto.IdProducto == producto.IdProducto);
            if (detalle != null)
            {
                detalle.Cantidad += cantidad; // Si el producto ya existe, actualiza la cantidad
            }
            else
            {
                presupuesto.Detalles.Add(new PresupuestoDetalle(producto,cantidad));
            }
        }
    }
    public void Delete(int idPresupuesto)
    {
        var presupuesto = presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);
        if (presupuesto != null)
        {
            presupuestos.Remove(presupuesto);
        }
    }

}

public interface IRepositoryPresupuesto
{
    void Create(Presupuesto presupuesto);
    Presupuesto FindById(int idPresupuesto);
    List<Presupuesto> FindAll();
    void Insert(int idPresupuesto, Producto producto, int cantidad);
    void Delete(int idPresupuesto);
}
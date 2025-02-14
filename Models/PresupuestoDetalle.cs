namespace EspacioPresupuestoDetalle
{
    public class PresupuestoDetalle
    {
        public int Cantidad { get; set; }
        public Producto Producto { get; set;}
        public PresupuestoDetalle(Producto prod, int cant)
        {
            Producto = prod;
            Cantidad = cant;
        }
    }
}
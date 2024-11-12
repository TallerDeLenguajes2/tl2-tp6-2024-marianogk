namespace EspacioPresupuestoDetalle
{
    public class PresupuestoDetalle
    {
        private Producto producto;
        public int Cantidad { get; set; }
        public Producto Producto { get; }
        public PresupuestoDetalle(Producto prod, int cant)
        {
            Producto = prod;
            Cantidad = cant;
        }
    }
}
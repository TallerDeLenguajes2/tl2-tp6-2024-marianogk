using EspacioPresupuestoDetalle;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

public class PresupuestoRepository : IRepositoryR
{
    private readonly string cadenaConexion = "Data Source=db/Tienda.db";
    public PresupuestoRepository(string connectionString)
    {
        cadenaConexion = connectionString;
    }
    public void Insert(Presupuesto presupuesto)
    {
        var queryString = @"INSERT INTO Presupuestos(NombreDestinatario, FechaCreacion) VALUES (@NombreDestinatario, @FechaCreacion);";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            DateTime fechaHoy = DateTime.Today;


            command.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@FechaCreacion", fechaHoy);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void Update(Presupuesto presupuesto, int idPresupuesto)
    {
        var queryString = @"UPDATE Presupuestos SET NombreDestinatario = @NombreDestinatario WHERE idPresupuesto = @idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
    public List<Presupuesto> ListarPresupuestos()
    {
        var queryString = @"SELECT * FROM Presupuestos;";
        List<Presupuesto> presupuestos = new List<Presupuesto>();

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuesto presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();

                    presupuestos.Add(presupuesto);

                }
            }
        }
        return presupuestos;
    }
    public Presupuesto FindById(int idPresupuesto)
    {
        var queryString = @"SELECT * FROM Presupuestos p
        LEFT JOIN PresupuestosDetalle d ON p.idPresupuesto = d.idPresupuesto
        LEFT JOIN Productos prod ON d.idProducto = prod.idProducto
        WHERE p.idPresupuesto = @idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                Presupuesto presupuesto = null;
                while (reader.Read())
                {
                    if (presupuesto == null)
                    {
                        presupuesto = new Presupuesto
                        {
                            IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                            NombreDestinatario = reader["NombreDestinatario"].ToString(),
                            Detalles = new List<PresupuestoDetalle>()
                        };
                    }
                    // Si hay detalles, agregarlos
                    if (!reader.IsDBNull(reader.GetOrdinal("idProducto")))
                    {
                        var producto = new Producto
                        {
                            IdProducto = Convert.ToInt32(reader["idProducto"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            Precio = Convert.ToInt32(reader["Precio"])
                        };

                        var detalle = new PresupuestoDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                        presupuesto.Detalles.Add(detalle);
                    }
                }
                return presupuesto;
            }
        }
    }


    public void Delete(int idPresupuesto)
    {
        var queryDeleteProductos = "DELETE FROM PresupuestosDetalle WHERE IdPresupuesto = @idPresupuesto";

        using (var connection = new SqliteConnection(cadenaConexion))
        {
            var command = new SqliteCommand(queryDeleteProductos, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

            connection.Open();
            command.ExecuteNonQuery();
        }

        var queryString = @"DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

            connection.Open();
            command.ExecuteNonQuery();
        }

    }

    public void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        var queryString = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) 
                        VALUES (@idPresupuesto, @idProducto, @Cantidad);";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
            command.Parameters.AddWithValue("@idProducto", idProducto);
            command.Parameters.AddWithValue("@Cantidad", cantidad);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
public interface IRepositoryR
{
    void Insert(Presupuesto presupuesto);
    void Update(Presupuesto presupuesto, int idPresupuesto);
    List<Presupuesto> ListarPresupuestos();
    Presupuesto FindById(int idPresupuesto);
    void Delete(int idPresupuesto);
    public void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad);
}

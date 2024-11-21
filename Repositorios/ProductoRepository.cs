using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

public class ProductoRepository : IRepository
{
    private readonly string cadenaConexion;

    public ProductoRepository(string connectionString)
    {
        cadenaConexion = connectionString;
    }
    public void Insert(Producto producto)
    {
        var queryString = @"INSERT INTO Productos(Descripcion, Precio) VALUES (@Descripcion, @Precio);";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);

            connection.Open();
            command.ExecuteNonQuery();
            // connection.Close();
        }
    }

    public void Update(Producto producto, int idProducto)
    {
        var queryString = @"UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @idProducto;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@idProducto", idProducto);

            connection.Open();
            command.ExecuteNonQuery();
            // connection.Close();
        }
    }
    public List<Producto> ListarProductos()
    {
        var queryString = @"SELECT * FROM Productos;";
        List<Producto> productos = new List<Producto>();

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Producto producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);

                    productos.Add(producto);

                }
            }
            // connection.Close();
        }
        return productos;
    }
    public Producto FindById(int idProducto)
    {
        var queryString = @"SELECT * FROM Productos WHERE idProducto = @idProducto;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            command.Parameters.AddWithValue("@idProducto", idProducto);

            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Producto producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                    return producto;
                }
            }
            // connection.Close();
        }
        return null;
    }


    public void Delete(int idProducto)
    {
        var queryString = @"DELETE FROM Productos WHERE idProducto = @idProducto;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idProducto", idProducto);

            connection.Open();
            command.ExecuteNonQuery();
            // connection.Close();
        }

    }
}
public interface IRepository
{
    void Insert(Producto producto);
    void Update(Producto producto, int idProducto);
    List<Producto> ListarProductos();
    Producto FindById(int idProducto);
    void Delete(int idProducto);
}

using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

public class PresupuestoRepository : IRepositoryR
{
    private readonly string cadenaConexion;

    public PresupuestoRepository(string connectionString)
    {
        cadenaConexion = connectionString;
    }
    public void Insert(Presupuesto presupuesto)
    {
        var queryString = @"INSERT INTO Presupuestos(NombreDestinatario) VALUES (@NombreDestinatario);";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);

            connection.Open();
            command.ExecuteNonQuery();
            // connection.Close();
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
            // connection.Close();
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
            // connection.Close();
        }
        return presupuestos;
    }
    public Presupuesto FindById(int idPresupuesto)
    {
        var queryString = @"SELECT * FROM Presupuestos WHERE idPresupuesto = @idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Presupuesto presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    return presupuesto;
                }
            }
            // connection.Close();
        }
        return null;
    }


    public void Delete(int idPresupuesto)
    {
        var queryString = @"DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);

            connection.Open();
            command.ExecuteNonQuery();
            // connection.Close();
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
}

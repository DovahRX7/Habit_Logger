using Microsoft.Data.Sqlite;

namespace Habit_Logger;

//SQL del programa
public static class HabitoSQL
{
    //Insertar
    public static void Insertar(Habito habito)
    {
        using SqliteConnection conexion = Database.Abrir();
        using SqliteCommand comando = conexion.CreateCommand();

        // Los valores se pasan con parámetros
        comando.CommandText = "INSERT INTO Habitos (Nombre, Fecha, Cantidad) VALUES ($nombre, $fecha, $cantidad)";
        comando.Parameters.AddWithValue("$nombre", habito.Nombre);
        comando.Parameters.AddWithValue("$fecha", FechaParaBD(habito.Fecha));
        comando.Parameters.AddWithValue("$cantidad", habito.Cantidad);

        comando.ExecuteNonQuery();
    }

    //Ver historial
    public static List<Habito> ObtenerTodos()
    {
        List<Habito> lista = new List<Habito>();

        using SqliteConnection conexion = Database.Abrir();
        using SqliteCommand comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre, Fecha, Cantidad FROM Habitos ORDER BY Fecha, Id";

        using SqliteDataReader lector = comando.ExecuteReader();
        while (lector.Read()) //Read() avanza fila a fila 
        {
            lista.Add(LeerHabito(lector));
        }

        return lista;
    }

    //Ver hábito por id
    //El "?" en Habito significa que puede devolver null si no existe ese id
    public static Habito? ObtenerPorId(int id)
    {
        using SqliteConnection conexion = Database.Abrir();
        using SqliteCommand comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre, Fecha, Cantidad FROM Habitos WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);

        using SqliteDataReader lector = comando.ExecuteReader();
        if (lector.Read())
        {
            return LeerHabito(lector);
        }
        return null;   // No se encontró
    }


    //Actualizar
    //Devuelve true si se modificó alguna fila
    public static bool Actualizar(Habito habito)
    {
        using SqliteConnection conexion = Database.Abrir();
        using SqliteCommand comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Habitos SET Nombre = $nombre, Fecha = $fecha, Cantidad = $cantidad WHERE Id = $id";
        comando.Parameters.AddWithValue("$nombre", habito.Nombre);
        comando.Parameters.AddWithValue("$fecha", FechaParaBD(habito.Fecha));
        comando.Parameters.AddWithValue("$cantidad", habito.Cantidad);
        comando.Parameters.AddWithValue("$id", habito.Id);

        int filasAfectadas = comando.ExecuteNonQuery();
        return filasAfectadas > 0;
    }

    //Eliminar
    //Devuelve true si se borró alguna fila 
    public static bool Eliminar(int id)
    {
        using SqliteConnection conexion = Database.Abrir();
        using SqliteCommand comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Habitos WHERE Id = $id";
        comando.Parameters.AddWithValue("$id", id);

        int filasAfectadas = comando.ExecuteNonQuery();
        return filasAfectadas > 0;
    }

    //Convierte la fila actual  en un objeto Habito
    private static Habito LeerHabito(SqliteDataReader lector)
    {
        Habito habito = new Habito();
        habito.Id = lector.GetInt32(0);                     //Id
        habito.Nombre = lector.GetString(1);                //Nombre
        habito.Fecha = DateTime.Parse(lector.GetString(2)); //Fecha 
        habito.Cantidad = lector.GetInt32(3);               //Cantidad
        return habito;
    }

    // Las fechas se guardan como texto 
    private static string FechaParaBD(DateTime fecha)
    {
        return fecha.ToString("yyyy-MM-dd");
    }
}
using Microsoft.Data.Sqlite;

namespace Habit_Logger;

//Guarda los errores del programa en la tabla "Errores" de la base de datos
public static class ErrorLog
{
    public static void Registrar(Exception error, string contexto)
    {
        try
        {
            using SqliteConnection conexion = Database.Abrir();
            using SqliteCommand comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO Errores (FechaHora, Contexto, Mensaje) VALUES ($fechaHora, $contexto, $mensaje)";
            comando.Parameters.AddWithValue("$fechaHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            comando.Parameters.AddWithValue("$contexto", contexto);
            comando.Parameters.AddWithValue("$mensaje", error.Message);
            comando.ExecuteNonQuery();
        }
        catch (Exception errorAlGuardar)
        {
            //Si falla la base de datos lo anotamos en el archivo de log 
            FileLog.Registrar("ERROR en '" + contexto + "': " + error.Message + " (y además no se pudo guardar en la BD: " + errorAlGuardar.Message + ")");
        }
    }
}
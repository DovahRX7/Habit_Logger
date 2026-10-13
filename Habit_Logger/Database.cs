using Microsoft.Data.Sqlite;

namespace Habit_Logger;


//Conecta con la bd y crea las tablas
public static class Database
{
    //Nombre del archivo de la base de datos.
    private const string CadenaConexion = "Data Source=habitos.db";

    
    //Abre una conexión con la base de datos.
    public static SqliteConnection Abrir()
    {
        SqliteConnection conexion = new SqliteConnection(CadenaConexion);
        conexion.Open();
        return conexion;
    }

    
    //Crea las tablas 
    public static void Inicializar()
    {
        using SqliteConnection conexion = Abrir();
        using SqliteCommand comando = conexion.CreateCommand();

        //IF NOT EXISTS evita error si la tabla ya estaba creada
        comando.CommandText = @"
            CREATE TABLE IF NOT EXISTS Habitos (
                Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre   TEXT    NOT NULL,
                Fecha    TEXT    NOT NULL,
                Cantidad INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Errores (
                Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                FechaHora  TEXT NOT NULL,
                Contexto   TEXT NOT NULL,
                Mensaje    TEXT NOT NULL
            );";

        comando.ExecuteNonQuery();   //Ejecuta el SQL anterior
    }
}
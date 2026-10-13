namespace Habit_Logger;

//Guarda las operaciones del usuario en un archivo .log
//Crea un archivo por día dentro de la carpeta "logs"
public static class FileLog
{
    public static void Registrar(string mensaje)
    {
        try
        {
            //Crea la carpeta "logs" 
            Directory.CreateDirectory("logs");

            //Nombre del archivo según el día de hoy
            string nombreArchivo = DateTime.Now.ToString("dd_MM_yyyy") + ".log";
            string ruta = Path.Combine("logs", nombreArchivo);

            //Cada línea lleva la hora delante
            string linea = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + mensaje + Environment.NewLine;

            //Añade al final del archivo
            File.AppendAllText(ruta, linea);
        }
        catch
        {
            //Si el log falla no se hace nada para evitar un crash
        }
    }
}

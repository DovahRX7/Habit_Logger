using System.Globalization;

namespace Habit_Logger;

//Métodos para pedir datos al usuario y validarlos
public static class Entrada
{
    
    //Lee una respuesta del usuario
    private static string LeerRespuesta(string pregunta)
    {
        Console.Write(pregunta);
        string? texto = Console.ReadLine();

        //ReadLine() devuelve null si se cierra la entrada 
        if (texto == null)
        {
            Environment.Exit(0);
        }

        return texto.Trim();   //Quita espacios sobrantes al principio y al final
    }

    //Pide un texto que no esté vacío y no sea demasiado largo 
    public static string LeerTexto(string pregunta, int longitudMaxima)
    {
        while (true)
        {
            string texto = LeerRespuesta(pregunta);

            if (texto == "")
            {
                Console.WriteLine("Error: No puede estar vacío.");
            }
            else if (texto.Length > longitudMaxima)
            {
                Console.WriteLine($"Error: Máximo {longitudMaxima} caracteres.");
            }
            else
            {
                return texto;   
            }
        }
    }
    
    //Pide un número entero entre un min y max
    public static int LeerEntero(string pregunta, int min, int max)
    {
        while (true)
        {
            string texto = LeerRespuesta(pregunta);

            // TryParse intenta convertir el texto a número 
            if (!int.TryParse(texto, out int numero))
            {
                Console.WriteLine("Error: Introduce un número entero.");
            }
            else if (numero < min || numero > max)
            {
                Console.WriteLine($"Error: El número debe estar entre {min} y {max}.");
            }
            else
            {
                return numero;
            }
        }
    }

    /// Pide una fecha con formato dia/mes/año. Si el usuario escribe "hoy", usa la fecha actual
    public static DateTime LeerFecha(string pregunta)
    {
        while (true)
        {
            string texto = LeerRespuesta(pregunta + " (dd/mm/aaaa): ");

            //Comprueba el formato exacto y que la fecha exista
            if (!DateTime.TryParseExact(texto, "d/M/yyyy", CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out DateTime fecha))
            {
                Console.WriteLine("Error: Fecha no válida.");
            }
            else if (fecha > DateTime.Today)
            {
                Console.WriteLine("Error: La fecha no puede ser futura.");
            }
            else
            {
                return fecha;
            }
        }
    }

    //Pregunta sí/no, devuelve true si el usuario responde "s"
    public static bool Confirmar(string pregunta)
    {
        while (true)
        {
            string texto = LeerRespuesta(pregunta + " (s/n): ").ToLower();

            if (texto == "s") return true;
            if (texto == "n") return false;

            Console.WriteLine("Error: Responde 's' o 'n'.");
        }
    }
}

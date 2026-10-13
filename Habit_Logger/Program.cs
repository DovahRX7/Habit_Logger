using System.Text;

namespace Habit_Logger;

class Program
{
    static void Main()
    {
        //Crear la base de datos y las tablas
        try
        {
            Database.Inicializar();
        }
        catch (Exception error)
        {
            ErrorLog.Registrar(error, "Inicializar la base de datos");
            Console.WriteLine("No se pudo preparar la base de datos. El programa se cerrará.");
            return;
        }

        FileLog.Registrar("Programa iniciado");

        //Bucle del menú: se repite hasta que el usuario elige salir
        bool salir = false;
        while (!salir)
        {
            MostrarMenu();
            int opcion = Entrada.LeerEntero("Elige una opción: ", 0, 4);

            //Si algo falla, se guarda el error y se vuelve al menú (el programa no crashea)
            try
            {
                switch (opcion)
                {
                    case 1: 
                        Anadir(); 
                        break;
                    case 2:
                        Ver(); 
                        break;
                    case 3: 
                        Actualizar(); 
                        break;
                    case 4: 
                        Eliminar();
                        break;
                    case 0: 
                        salir = true;
                        break;
                }
            }
            catch (Exception error)
            {
                ErrorLog.Registrar(error, "Opción de menú " + opcion);
                Console.WriteLine("Ha ocurrido un error. Se ha guardado en el registro de errores.");
            }
        }

        FileLog.Registrar("Saliendo...");
    }

    //Muestra las opciones del menú
    static void MostrarMenu()
    {
        Console.WriteLine();
        Console.WriteLine("===== REGISTRO DE HÁBITOS =====");
        Console.WriteLine("1. Añadir hábito");
        Console.WriteLine("2. Ver hábitos");
        Console.WriteLine("3. Actualizar hábito");
        Console.WriteLine("4. Eliminar hábito");
        Console.WriteLine("0. Salir");
    }

    //Opciones del menú

    static void Anadir()
    {
        Habito habito = new Habito();
        habito.Nombre = Entrada.LeerTexto("Nombre del hábito: ", 30);
        habito.Fecha = Entrada.LeerFecha("Fecha: ");
        habito.Cantidad = Entrada.LeerEntero("Cantidad: ", 1, 1000);

        HabitoSQL.Insertar(habito);

        Console.WriteLine("Hábito añadido correctamente.");
        FileLog.Registrar("AÑADIR: " + habito.Nombre + ", " + habito.Fecha.ToString("dd/MM/yyyy") + ", " + habito.Cantidad);
    }

    static void Ver()
    {
        Listar();
        FileLog.Registrar("Ver listado de hábitos");
    }

    static void Actualizar()
    {
        Habito? habito = ElegirHabito("actualizar");
        if (habito == null) return;   //Si el ID no existe vuelve al menú

        //Modificamos el mismo objeto con los nuevos valores
        habito.Nombre = Entrada.LeerTexto("Nuevo nombre: ", 30);
        habito.Fecha = Entrada.LeerFecha("Nueva fecha");
        habito.Cantidad = Entrada.LeerEntero("Nueva cantidad: ", 1, 1000);

        if (HabitoSQL.Actualizar(habito))
        {
            Console.WriteLine("Hábito actualizado correctamente.");
            FileLog.Registrar("ACTUALIZAR: Id " + habito.Id);
        }
        else
        {
            Console.WriteLine("No se pudo actualizar: el hábito ya no existe.");
        }
    }

    static void Eliminar()
    {
        Habito? habito = ElegirHabito("eliminar");
        if (habito == null) return;

        if (!Entrada.Confirmar("Eliminar '" + habito.Nombre + "'?"))
        {
            Console.WriteLine("Operación cancelada.");
            return;
        }

        if (HabitoSQL.Eliminar(habito.Id))
        {
            Console.WriteLine("Hábito eliminado correctamente.");
            FileLog.Registrar("ELIMINAR: Id " + habito.Id);
        }
        else
        {
            Console.WriteLine("No se pudo eliminar: el hábito ya no existe.");
        }
    }

    //Muestra todos los hábitos en forma de tabla
    static bool Listar()
    {
        List<Habito> habitos = HabitoSQL.ObtenerTodos();

        if (habitos.Count == 0)
        {
            Console.WriteLine("Todavía no hay hábitos registrados.");
            return false;
        }

        Console.WriteLine();
        //El número después de la coma es el ancho de la columna
        Console.WriteLine($"{"ID",-5}{"HÁBITO",-32}{"FECHA",-13}{"CANTIDAD"}");
        Console.WriteLine(new string('-', 62));

        foreach (Habito h in habitos)
        {
            //Primero va el ancho (,-13) y después el formato de la fecha
            Console.WriteLine($"{h.Id,-5}{h.Nombre,-32}{h.Fecha,-13:dd/MM/yyyy}{h.Cantidad}");
        }

        return true;
    }

    //Muestra la lista, pide un Id y devuelve el hábito elegido
    static Habito ElegirHabito(string accion)
    {
        if (!Listar()) return null;

        int id = Entrada.LeerEntero("Id del hábito a " + accion + ": ", 1, int.MaxValue);
        Habito? habito = HabitoSQL.ObtenerPorId(id);

        if (habito == null)
        {
            Console.WriteLine("No existe ningún hábito con Id " + id + ".");
        }

        return habito;
    }
}
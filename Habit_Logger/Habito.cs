namespace Habit_Logger;

//Representa una fila de la tabla Habitos
public class Habito
{
    public int Id { get; set; }                //Número de ID que asigna la base de datos
    public string Nombre { get; set; } = "";   //Nombre del hábito
    public DateTime Fecha { get; set; }        //Día en que ocurrió
    public int Cantidad { get; set; }          //Cantidad de veces
}

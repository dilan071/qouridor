using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("¡Bienvenido al juego!");
        int tamano;
        while (true)
        {
            Console.Write("Por favor, introduce el tamaño del tablero (nxn): ");
            if (int.TryParse(Console.ReadLine(), out tamano) && tamano >= 5)
            {
                break;
            }
            Console.WriteLine("Entrada no válida. Por favor, introduce un número entero mayor o igual a 5.");
        }

        Juego juego = new Juego(tamano);
        juego.Iniciar();
    }
}

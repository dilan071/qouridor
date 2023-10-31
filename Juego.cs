using System;
using System.Collections.Generic;

public class Juego
{
    private MatrizListaEnlazada matriz;
    private Nodo posicionX;
    private Nodo posicionY;
    private Nodo cursor;
    private Nodo arroba;
    private bool XPasadoArroba = false;
    private bool YPasadoArroba = false;

    public Juego(int n)
    {
        matriz = new MatrizListaEnlazada(n);
        posicionX = matriz.ObtenerNodoEn(0, n / 2);
        posicionY = matriz.ObtenerNodoEn(n - 1, n / 2);

        Random rnd = new Random();
        int arrobaX = rnd.Next(0, n);
        int arrobaY = rnd.Next(0, n);
        arroba = matriz.ObtenerNodoEn(arrobaX, arrobaY);
        arroba.Valor = "@";
        Console.WriteLine($"Inicializando arroba en ({arrobaX}, {arrobaY}), Valor: {arroba.Valor}");
    }


    public bool MoverJugador(string jugador, string direccion)
    {
        Nodo posicionJugador = jugador == "X" ? posicionX : posicionY;
        Nodo otroJugador = jugador == "X" ? posicionY : posicionX;
        Nodo nuevoNodo = null;

        switch (direccion)
        {
            case "DIAGONAL DERECHA":
            case "DIAGONAL IZQUIERDA":
                if (!EstanCaraACara(posicionJugador, otroJugador))
                {
                    Console.WriteLine("El movimiento en diagonal no es permitido ya que no existe un jugador de frente.");
                    return false;
                }

                if (jugador == "X" && direccion == "DIAGONAL DERECHA" && posicionX.Y < matriz.Tamano - 1 && posicionX.X < matriz.Tamano - 1)
                {
                    nuevoNodo = matriz.ObtenerNodoEn(posicionX.X + 1, posicionX.Y + 1);
                }
                else if (jugador == "Y" && direccion == "DIAGONAL DERECHA" && posicionY.Y < matriz.Tamano - 1 && posicionY.X > 0)
                {
                    nuevoNodo = matriz.ObtenerNodoEn(posicionY.X - 1, posicionY.Y + 1);
                }
                else if (jugador == "X" && direccion == "DIAGONAL IZQUIERDA" && posicionX.Y > 0 && posicionX.X < matriz.Tamano - 1)
                {
                    nuevoNodo = matriz.ObtenerNodoEn(posicionX.X + 1, posicionX.Y - 1);
                }
                else if (jugador == "Y" && direccion == "DIAGONAL IZQUIERDA" && posicionY.Y > 0 && posicionY.X > 0)
                {
                    nuevoNodo = matriz.ObtenerNodoEn(posicionY.X - 1, posicionY.Y - 1);
                }
                break;

            case "DERECHA":
                if (posicionJugador.Y < matriz.Tamano - 1)
                {
                    nuevoNodo = matriz.ObtenerNodoEn(posicionJugador.X, posicionJugador.Y + 1);
                }
                break;

            case "IZQUIERDA":
                if (posicionJugador.Y > 0)
                {
                    nuevoNodo = matriz.ObtenerNodoEn(posicionJugador.X, posicionJugador.Y - 1);
                }
                break;

            case "ARRIBA":
                if (posicionJugador.X > 0)
                {
                    nuevoNodo = matriz.ObtenerNodoEn(posicionJugador.X - 1, posicionJugador.Y);
                }
                break;

            case "ABAJO":
                if (posicionJugador.X < matriz.Tamano - 1)
                {
                    nuevoNodo = matriz.ObtenerNodoEn(posicionJugador.X + 1, posicionJugador.Y);
                }
                break;

            default:
                break;
        }


        if (nuevoNodo != null && nuevoNodo == arroba)
        {
            if (jugador == "X")
                XPasadoArroba = true;
            else
                YPasadoArroba = true;
        }


        if (nuevoNodo == otroJugador)
        {
            Console.WriteLine("Este movimiento no es permitido porque hay otro jugador en ese nodo.");
            return false;
        }
        else if (nuevoNodo != null && nuevoNodo.Valor == "#")
        {
            Console.WriteLine("No te puedes mover ahi porque hay un bloqueo");
            return false;
        }
        else if (nuevoNodo != null && nuevoNodo.Valor != "#" && nuevoNodo != cursor)
        {
            if (jugador == "X")
                posicionX = nuevoNodo;
            else
                posicionY = nuevoNodo;
            return true;
        }

        return false;
    }

    private bool EstanCaraACara(Nodo nodo1, Nodo nodo2)
    {

        return nodo1.Y == nodo2.Y && (Math.Abs(nodo1.X - nodo2.X) == 1);
    }



    public bool EliminarNodo(int x, int y, string jugador)
    {
        Nodo nodoAEliminar = matriz.ObtenerNodoEn(x, y);
        Nodo jugadorActual = (jugador == "X") ? posicionX : posicionY;

        if (nodoAEliminar == posicionX || nodoAEliminar == posicionY)
        {
            Console.WriteLine("Intento de bloquear un nodo con un jugador.");
            return false;
        }


        nodoAEliminar.Valor = "#";

        if (!AmbosJugadoresTienenCamino())
        {

            nodoAEliminar.Valor = " ";
            Console.WriteLine("No puedes bloquear este nodo, este nodo es el unico disponible pa tu victoria");
            return false;
        }

        return true;
    }



    private bool ExisteCamino(Nodo inicio, Nodo destino)
    {
        Queue<Nodo> lista = new Queue<Nodo>();
        HashSet<Nodo> visitados = new HashSet<Nodo>();

        lista.Enqueue(inicio);
        visitados.Add(inicio);

        while (lista.Count > 0)
        {
            Nodo actual = lista.Dequeue();

            if (actual == destino) return true;


            if (actual.Derecha != null && actual.Derecha.Valor != "#" && !visitados.Contains(actual.Derecha))
            {
                lista.Enqueue(actual.Derecha);
                visitados.Add(actual.Derecha);
            }
            if (actual.Abajo != null && actual.Abajo.Valor != "#" && !visitados.Contains(actual.Abajo))
            {
                lista.Enqueue(actual.Abajo);
                visitados.Add(actual.Abajo);
            }

            if (actual.Izquierda != null && actual.Izquierda.Valor != "#" && !visitados.Contains(actual.Izquierda))
            {
                lista.Enqueue(actual.Izquierda);
                visitados.Add(actual.Izquierda);
            }
            if (actual.Arriba != null && actual.Arriba.Valor != "#" && !visitados.Contains(actual.Arriba))
            {
                lista.Enqueue(actual.Arriba);
                visitados.Add(actual.Arriba);
            }
        }
        return false;
    }

    private bool AmbosJugadoresTienenCamino()
    {
        return
            (ExisteCamino(posicionX, arroba) && ExisteCamino(arroba, matriz.ObtenerNodoEn(matriz.Tamano - 1, arroba.Y))) &&
            (ExisteCamino(posicionY, arroba) && ExisteCamino(arroba, matriz.ObtenerNodoEn(0, arroba.Y)));
    }



    public void Iniciar()
    {
        bool juegoTerminado = false;
        string jugadorActual = "X";

        while (!juegoTerminado)
        {
            MostrarTablero();
            Console.WriteLine($"Turno del jugador {jugadorActual}");

            if (Mover(jugadorActual))
            {

                if (HaGanado(jugadorActual))
                {
                    juegoTerminado = true;
                    Console.WriteLine($"¡El jugador {jugadorActual} ha ganado!");
                }
                else
                {
                    jugadorActual = jugadorActual == "X" ? "Y" : "X"; // Cambiamos el turno dependiendo del jugador que pasea el truno
                }
            }
        }
    }

    private bool Mover(string jugador)
    {
        Console.WriteLine($"Turno de {jugador}. Usa las flechas para moverte, i/d para movimientos diagonales, o B para bloquear una casilla.");
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.RightArrow:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.UpArrow:
            case ConsoleKey.DownArrow:

                string dir = key switch
                {
                    ConsoleKey.RightArrow => "DERECHA",
                    ConsoleKey.LeftArrow => "IZQUIERDA",
                    ConsoleKey.UpArrow => "ARRIBA",
                    ConsoleKey.DownArrow => "ABAJO",
                    _ => throw new InvalidOperationException()
                };
                return MoverJugador(jugador, dir);

            case ConsoleKey.I:
                return MoverJugador(jugador, "DIAGONAL IZQUIERDA");
            case ConsoleKey.D:
                return MoverJugador(jugador, "DIAGONAL DERECHA");
            case ConsoleKey.B:
                cursor = jugador == "X" ? posicionX : posicionY;
                MostrarTableroConCursor();

                while (true)
                {
                    var seleccionKey = Console.ReadKey(true).Key;
                    switch (seleccionKey)
                    {
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.DownArrow:
                            MoverCursor(seleccionKey);
                            MostrarTableroConCursor();
                            break;
                        case ConsoleKey.Spacebar:
                            if (EliminarNodo(cursor.X, cursor.Y, jugador))
                            {
                                cursor = null;
                                MostrarTablero();
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("¡Movimiento no válido!");
                            }
                            break;
                        case ConsoleKey.Escape:
                            return false;
                    }
                }
            default:
                Console.WriteLine("Entrada no válida.");
                return false;
        }
    }


    private void MoverCursor(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.RightArrow:
                if (cursor.Y < matriz.Tamano - 1 && cursor.Derecha != null) cursor = cursor.Derecha;
                break;
            case ConsoleKey.LeftArrow:
                if (cursor.Y > 0 && cursor.Izquierda != null) cursor = cursor.Izquierda;
                break;
            case ConsoleKey.UpArrow:
                if (cursor.X > 0 && cursor.Arriba != null) cursor = cursor.Arriba;
                break;
            case ConsoleKey.DownArrow:
                if (cursor.X < matriz.Tamano - 1 && cursor.Abajo != null) cursor = cursor.Abajo;
                break;
        }
    }

    private void MostrarTablero()
    {
        Console.WriteLine($"Arroba en ({arroba.X}, {arroba.Y}), Valor: {arroba.Valor}");
        for (int i = 0; i < matriz.Tamano; i++)
        {
            Nodo nodoActual = matriz.ObtenerNodoEn(i, 0);
            for (int j = 0; j < matriz.Tamano && nodoActual != null; j++)
            {
                if (cursor != null && cursor.X == i && cursor.Y == j)
                    Console.Write("* ");
                else if (posicionX.X == i && posicionX.Y == j)
                    Console.Write("X ");
                else if (posicionY.X == i && posicionY.Y == j)
                    Console.Write("Y ");
                else if (nodoActual == arroba)
                    Console.Write("@ ");
                else if (nodoActual.Valor == "#")
                    Console.Write("# ");
                else
                    Console.Write("o ");

                nodoActual = nodoActual.Derecha;
            }
            Console.WriteLine();
        }
    }

    private void MostrarTableroConCursor()
    {
        Console.Clear();

        for (int i = 0; i < matriz.Tamano; i++)
        {
            Nodo nodoActual = matriz.ObtenerNodoEn(i, 0);
            for (int j = 0; j < matriz.Tamano && nodoActual != null; j++)
            {
                if (nodoActual.Valor == "#")
                {
                    Console.Write("# ");
                }
                else if (cursor.X == i && cursor.Y == j)
                {
                    Console.Write("* ");
                }
                else
                {
                    if (posicionX.X == i && posicionX.Y == j)
                    {
                        Console.Write("X ");
                    }

                    else if (posicionY.X == i && posicionY.Y == j)
                    {
                        Console.Write("Y ");
                    }
                    else
                    {
                        Console.Write("O ");
                    }
                }
                nodoActual = nodoActual.Derecha;
            }
            Console.WriteLine();
        }

        Console.WriteLine("Usa las flechas para mover el cursor, barra espaciadora para bloquear y Esc para cancelar.");
    }




    private bool HaGanado(string jugador)
    {
        if (jugador == "X" && posicionX.X == matriz.Tamano - 1 && XPasadoArroba) return true;
        if (jugador == "Y" && posicionY.X == 0 && YPasadoArroba) return true;
        return false;
    }
}






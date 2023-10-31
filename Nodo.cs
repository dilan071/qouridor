using System;
public class Nodo
{
    public Nodo Izquierda { get; set; }
    public Nodo Arriba { get; set; }
    public string Valor { get; set; }
    public Nodo Derecha { get; set; }
    public Nodo Abajo { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Nodo(int x, int y)
    {
        X = x;
        Y = y;
        Valor = " ";
    }
}


public class MatrizListaEnlazada
{
    public int Tamano { get; private set; }
    public ListaDeFilas ListaDeFilas { get; set; }

    public MatrizListaEnlazada(int n)
    {
        Tamano = n;
        ListaDeFilas = new ListaDeFilas();

        ListaEnlazada filaAnterior = null;

        for (int i = 0; i < n; i++)
        {
            ListaEnlazada nuevaFila = new ListaEnlazada();
            Nodo prev = null;

            for (int j = 0; j < n; j++)
            {
                Nodo newNode = new Nodo(i, j);
                if (j == 0)
                {
                    nuevaFila.Cabeza = newNode;
                }
                else
                {
                    prev.Derecha = newNode;
                }

                if (j > 0)
                {
                    newNode.Izquierda = prev;
                }

                if (i > 0)
                {
                    Nodo nodoSuperior = filaAnterior.Cabeza;
                    for (int col = 0; col < j; col++)
                    {
                        nodoSuperior = nodoSuperior.Derecha;
                    }
                    nodoSuperior.Abajo = newNode;
                    newNode.Arriba = nodoSuperior;
                }

                prev = newNode;
            }

            if (filaAnterior == null)
            {
                ListaDeFilas.Cabeza = nuevaFila;
            }
            else
            {
                filaAnterior.Siguiente = nuevaFila;
                nuevaFila.Anterior = filaAnterior;
            }

            filaAnterior = nuevaFila;
        }
    }

    public Nodo ObtenerNodoEn(int x, int y)
    {
        ListaEnlazada filaActual = ListaDeFilas.Cabeza;
        for (int i = 0; i < x && filaActual.Siguiente != null; i++)
        {
            filaActual = filaActual.Siguiente;
        }

        Nodo nodoActual = filaActual.Cabeza;
        for (int i = 0; i < y && nodoActual.Derecha != null; i++)
        {
            nodoActual = nodoActual.Derecha;
        }
        return nodoActual;
    }
}


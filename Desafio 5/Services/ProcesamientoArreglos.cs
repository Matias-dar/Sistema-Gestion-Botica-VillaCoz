
using System;

namespace SistemaBotica.Services
{
    public class ProcesamientoArreglos
    {
        // Arreglo unidimensional para almacenar codigos
        private string[] codigos = new string[5];

        // Matriz bidimensional para almacenar datos
        // Columnas: Codigo, Nombre, Precio y Stock
        private string[,] medicamentos = new string[5, 4];

        private int cantidad = 0;

        // Insertar un medicamento

        public bool Insertar(string codigo, string nombre,
                             decimal precio, int stock)
        {
            if (cantidad >= codigos.Length)
                return false;

            if (string.IsNullOrWhiteSpace(codigo) ||
                string.IsNullOrWhiteSpace(nombre))
                return false;

            if (precio <= 0 || stock < 0)
                return false;

            if (Buscar(codigo) != -1)
                return false;

            codigos[cantidad] = codigo;

            medicamentos[cantidad, 0] = codigo;
            medicamentos[cantidad, 1] = nombre;
            medicamentos[cantidad, 2] = precio.ToString("0.00");
            medicamentos[cantidad, 3] = stock.ToString();

            cantidad++;

            return true;
        }

        // Buscar un medicamento por codigo
        public int Buscar(string codigo)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (codigos[i].Equals(
                    codigo,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        // Mostrar los medicamentos almacenados
        public string Mostrar()
        {
            string resultado = "";

            for (int i = 0; i < cantidad; i++)
            {
                resultado +=
                    "Codigo: " + medicamentos[i, 0] +
                    " | Nombre: " + medicamentos[i, 1] +
                    " | Precio: " + medicamentos[i, 2] +
                    " | Stock: " + medicamentos[i, 3] +
                    Environment.NewLine;
            }

            return resultado;
        }

        // Ordenamiento Burbuja por nombre
        public void OrdenarBurbuja()
        {
            for (int i = 0; i < cantidad - 1; i++)
            {
                for (int j = 0; j < cantidad - i - 1; j++)
                {
                    if (string.Compare(
                        medicamentos[j, 1],
                        medicamentos[j + 1, 1],
                        StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        // Intercambiar filas de la matriz
                        for (int k = 0; k < 4; k++)
                        {
                            string temporal = medicamentos[j, k];
                            medicamentos[j, k] = medicamentos[j + 1, k];
                            medicamentos[j + 1, k] = temporal;
                        }

                        // Mantener sincronizado el arreglo de códigos
                        string tempCodigo = codigos[j];
                        codigos[j] = codigos[j + 1];
                        codigos[j + 1] = tempCodigo;
                    }
                }
            }
        }

        // Ordenamiento por Seleccion: precio de menor a mayor
        public void OrdenarSeleccion()
        {
            for (int i = 0; i < cantidad - 1; i++)
            {
                int menor = i;

                for (int j = i + 1; j < cantidad; j++)
                {
                    decimal precioActual =
                        decimal.Parse(medicamentos[j, 2]);

                    decimal precioMenor =
                        decimal.Parse(medicamentos[menor, 2]);

                    if (precioActual < precioMenor)
                    {
                        menor = j;
                    }
                }

                if (menor != i)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        string temporal = medicamentos[i, k];
                        medicamentos[i, k] = medicamentos[menor, k];
                        medicamentos[menor, k] = temporal;
                    }

                    string tempCodigo = codigos[i];
                    codigos[i] = codigos[menor];
                    codigos[menor] = tempCodigo;
                }
            }
        }
    }
}


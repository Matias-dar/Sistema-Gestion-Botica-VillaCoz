
using System;

namespace SistemaBotica.Services
{
    public class ArreglosParalelos
    {
        // Arreglos paralelos de medicamentos
        private string[] codigos = new string[5];
        private string[] nombres = new string[5];
        private decimal[] precios = new decimal[5];

        private int cantidad = 0;

        // Registrar un medicamento
        public bool Registrar(string codigo, string nombre, decimal precio)
        {
            if (cantidad >= codigos.Length)
                return false;

            if (string.IsNullOrWhiteSpace(codigo) ||
                string.IsNullOrWhiteSpace(nombre) ||
                precio <= 0)
                return false;

            if (Buscar(codigo) != -1)
                return false;

            codigos[cantidad] = codigo;
            nombres[cantidad] = nombre;
            precios[cantidad] = precio;

            cantidad++;

            return true;
        }

        // Buscar medicamento por codigo
        public int Buscar(string codigo)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (codigos[i].Equals(
                    codigo,
                    StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }

        // Mostrar los registros
        public string Mostrar()
        {
            string resultado = "";

            for (int i = 0; i < cantidad; i++)
            {
                resultado +=
                    "Codigo: " + codigos[i] +
                    " | Nombre: " + nombres[i] +
                    " | Precio: S/ " + precios[i].ToString("0.00") +
                    Environment.NewLine;
            }

            return resultado;
        }
    }
}

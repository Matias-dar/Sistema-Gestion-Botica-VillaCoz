
using System;

namespace SistemaBotica.Services
{
    public class PruebaArreglos
    {
        public static void Ejecutar()
        {
            ProcesamientoArreglos prueba =
                new ProcesamientoArreglos();

            // Insertar tres medicamentos
            bool r1 = prueba.Insertar(
                "MED001", "Paracetamol", 5.50m, 97);

            bool r2 = prueba.Insertar(
                "MED002", "Ibuprofeno", 8.50m, 55);

            bool r3 = prueba.Insertar(
                "MED003", "Amoxicilina", 12.00m, 40);

            Console.WriteLine("RESULTADO DE INSERCION:");
            Console.WriteLine(r1);
            Console.WriteLine(r2);
            Console.WriteLine(r3);

            // Mostrar medicamentos
            Console.WriteLine("\nLISTA DE MEDICAMENTOS:");
            Console.WriteLine(prueba.Mostrar());

            // Ordenar medicamentos por nombre
            prueba.OrdenarBurbuja();

            Console.WriteLine("\nMEDICAMENTOS ORDENADOS POR BURBUJA:");
            Console.WriteLine(prueba.Mostrar());

            // Ordenar medicamentos por precio
            prueba.OrdenarSeleccion();

            Console.WriteLine("\nMEDICAMENTOS ORDENADOS POR SELECCION:");
            Console.WriteLine(prueba.Mostrar());

            // Buscar un medicamento
            int posicion = prueba.Buscar("MED002");

            Console.WriteLine("POSICION DE MED002:");
            Console.WriteLine(posicion);

            // Prueba 1: intentar registrar un codigo duplicado
            bool duplicado = prueba.Insertar(
                "MED001", "Paracetamol", 5.50m, 97);

            Console.WriteLine("\nPRUEBA DE CODIGO DUPLICADO:");
            Console.WriteLine(duplicado);

            // Prueba 2: completar los espacios disponibles
            bool r4 = prueba.Insertar(
                "MED004", "Loratadina", 9.00m, 30);

            bool r5 = prueba.Insertar(
                "MED005", "Omeprazol", 15.00m, 25);

            Console.WriteLine("\nPRUEBA DE CAPACIDAD:");
            Console.WriteLine("Cuarto registro: " + r4);
            Console.WriteLine("Quinto registro: " + r5);

            // Prueba 3: intentar insertar un sexto medicamento
            bool sexto = prueba.Insertar(
                "MED006", "Vitamina C", 7.00m, 20);

            Console.WriteLine("Sexto registro: " + sexto);

            Console.WriteLine("\nLISTA FINAL:");
            Console.WriteLine(prueba.Mostrar());

            // PRUEBA DE ARREGLOS PARALELOS
            ArreglosParalelos paralelo = new ArreglosParalelos();

            bool p1 = paralelo.Registrar("MED001", "Paracetamol", 5.50m);
            bool p2 = paralelo.Registrar("MED002", "Ibuprofeno", 8.50m);
            bool p3 = paralelo.Registrar("MED003", "Amoxicilina", 12.00m);

            Console.WriteLine("\nPRUEBA DE ARREGLOS PARALELOS:");
            Console.WriteLine(p1);
            Console.WriteLine(p2);
            Console.WriteLine(p3);

            Console.WriteLine("\nREGISTROS PARALELOS:");
            Console.WriteLine(paralelo.Mostrar());

            int posicionParalela = paralelo.Buscar("MED002");

            Console.WriteLine("BUSQUEDA MED002:");
            Console.WriteLine(posicionParalela);

        }
    }
}

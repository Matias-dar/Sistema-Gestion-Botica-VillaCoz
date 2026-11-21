using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SistemaBotica.Models;

namespace SistemaBotica.Services
{
    public class GestionCatalogo
    {
        private readonly string archivo;
        private readonly List<CatalogoProducto> productos = new List<CatalogoProducto>();

        public GestionCatalogo()
        {
            string carpeta = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data"));
            archivo = Path.Combine(carpeta, "catalogo_productos.txt");
            Cargar();
        }

        private void Cargar()
        {
            productos.Clear();
            if (!File.Exists(archivo)) return;

            string[] lineas = File.ReadAllLines(archivo);
            // La primera fila contiene los nombres de las columnas.
            for (int i = 1; i < lineas.Length; i++)
            {
                string[] p = lineas[i].Split('|');
                if (p.Length < 14) continue;
                productos.Add(new CatalogoProducto
                {
                    NumeroFactura = p[0], Laboratorio = p[1], Producto = p[2],
                    FechaVencimiento = p[3], Lote = p[4], Cantidad = p[5],
                    UnidadesPresentacion = p[6], Unidades = p[7],
                    PrecioCompraUnidad = p[8], Margen = p[9], PVP = p[10],
                    FechaIngreso = p[11], StockFuente = p[12], MejorPrecio = p[13]
                });
            }
        }

        public List<CatalogoProducto> ObtenerTodos()
        {
            return productos.ToList();
        }

        public List<CatalogoProducto> Buscar(string texto)
        {
            texto = (texto ?? "").Trim();
            if (texto.Length == 0) return ObtenerTodos();
            return productos.Where(p =>
                Contiene(p.Producto, texto) || Contiene(p.Laboratorio, texto) ||
                Contiene(p.Lote, texto) || Contiene(p.NumeroFactura, texto))
                .ToList();
        }

        private bool Contiene(string valor, string texto)
        {
            return (valor ?? "").IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}

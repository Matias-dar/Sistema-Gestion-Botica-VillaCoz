namespace SistemaBotica.Models
{
    // Registro de referencia importado del Excel de Boticas VillaCoz.
    // Se mantiene separado de Medicamento para no tratar datos historicos como stock vigente.
    public class CatalogoProducto
    {
        public string NumeroFactura { get; set; }
        public string Laboratorio { get; set; }
        public string Producto { get; set; }
        public string FechaVencimiento { get; set; }
        public string Lote { get; set; }
        public string Cantidad { get; set; }
        public string UnidadesPresentacion { get; set; }
        public string Unidades { get; set; }
        public string PrecioCompraUnidad { get; set; }
        public string Margen { get; set; }
        public string PVP { get; set; }
        public string FechaIngreso { get; set; }
        public string StockFuente { get; set; }
        public string MejorPrecio { get; set; }
    }
}

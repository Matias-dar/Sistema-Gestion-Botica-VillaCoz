using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using SistemaBotica.Models;
using SistemaBotica.Services;

namespace SistemaBotica.Forms
{
    public class FrmCatalogo : Form
    {
        private readonly GestionCatalogo gestion = new GestionCatalogo();
        private readonly GestionMedicamentos gestionMedicamentos = new GestionMedicamentos();
        private readonly DataGridView tabla = new DataGridView();
        private readonly TextBox txtBuscar = new TextBox();
        private readonly Label lblCantidad = new Label();
        private readonly Button btnRegistrarInventario = new Button();

        public FrmCatalogo()
        {
            Text = "Botica VillaCoz - Catálogo de productos";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1250, 650);
            MinimumSize = new Size(950, 500);
            Font = new Font("Segoe UI", 9F);

            var titulo = new Label
            {
                Text = "CATÁLOGO DE PRODUCTOS - BOTICAS VILLACOZ",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 18)
            };
            Controls.Add(titulo);

            Controls.Add(new Label { Text = "Buscar por producto, laboratorio, lote o factura:", AutoSize = true, Location = new Point(22, 65) });
            txtBuscar.Location = new Point(22, 90);
            txtBuscar.Width = 420;
            txtBuscar.TextChanged += (s, e) => MostrarResultados();
            Controls.Add(txtBuscar);

            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(465, 94);
            Controls.Add(lblCantidad);

            tabla.Location = new Point(20, 130);
            tabla.Size = new Size(1190, 420);
            tabla.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabla.ReadOnly = true;
            tabla.AllowUserToAddRows = false;
            tabla.AllowUserToDeleteRows = false;
            tabla.AutoGenerateColumns = false;
            tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabla.MultiSelect = false;
            tabla.BackgroundColor = Color.White;
            AgregarColumna("Factura", "NumeroFactura");
            AgregarColumna("Laboratorio", "Laboratorio");
            AgregarColumna("Producto", "Producto");
            AgregarColumna("Vencimiento (fuente)", "FechaVencimiento");
            AgregarColumna("Lote", "Lote");
            AgregarColumna("Cantidad", "Cantidad");
            AgregarColumna("Unid. presentación", "UnidadesPresentacion");
            AgregarColumna("Unidades", "Unidades");
            AgregarColumna("PVF x unidad (S/)", "PrecioCompraUnidad");
            AgregarColumna("Margen", "Margen");
            AgregarColumna("PVP (S/)", "PVP");
            AgregarColumna("Ingreso (fuente)", "FechaIngreso");
            AgregarColumna("Stock (fuente)", "StockFuente");
            AgregarColumna("Mejor precio", "MejorPrecio");
            Controls.Add(tabla);

            btnRegistrarInventario.Text = "Registrar en inventario";
            btnRegistrarInventario.Size = new Size(190, 32);
            btnRegistrarInventario.Location = new Point(20, 560);
            btnRegistrarInventario.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRegistrarInventario.Click += BtnRegistrarInventario_Click;
            Controls.Add(btnRegistrarInventario);

            var nota = new Label
            {
                Text = "El catálogo es histórico y referencial. Para registrar, verifica el vencimiento y escribe el stock físico actual.",
                AutoSize = true,
                ForeColor = Color.DarkSlateGray,
                Location = new Point(225, 568),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            Controls.Add(nota);
            MostrarResultados();
        }

        private void AgregarColumna(string titulo, string propiedad)
        {
            tabla.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = titulo,
                DataPropertyName = propiedad,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
        }

        private void MostrarResultados()
        {
            var lista = gestion.Buscar(txtBuscar.Text);
            tabla.DataSource = null;
            tabla.DataSource = lista;
            lblCantidad.Text = "Registros mostrados: " + lista.Count + " / " + gestion.ObtenerTodos().Count;
        }

        private void BtnRegistrarInventario_Click(object sender, EventArgs e)
        {
            CatalogoProducto producto = tabla.CurrentRow == null
                ? null
                : tabla.CurrentRow.DataBoundItem as CatalogoProducto;

            if (producto == null)
            {
                MessageBox.Show("Seleccione un producto del catálogo.", "Registro en inventario",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dialogo = new FrmAltaDesdeCatalogo(producto, gestionMedicamentos))
            {
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Producto registrado en el inventario. El precio guardado corresponde al PVP del catálogo.",
                        "Registro completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }

    // Ventana de confirmación: los datos históricos no se convierten automáticamente en stock vigente.
    internal class FrmAltaDesdeCatalogo : Form
    {
        private readonly CatalogoProducto producto;
        private readonly GestionMedicamentos gestion;
        private readonly TextBox txtCodigo = new TextBox();
        private readonly TextBox txtNombre = new TextBox();
        private readonly TextBox txtLaboratorio = new TextBox();
        private readonly TextBox txtPrecio = new TextBox();
        private readonly TextBox txtStock = new TextBox();
        private readonly DateTimePicker dtpVencimiento = new DateTimePicker();

        public FrmAltaDesdeCatalogo(CatalogoProducto producto, GestionMedicamentos gestion)
        {
            this.producto = producto;
            this.gestion = gestion;

            Text = "Registrar producto en inventario";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(510, 390);
            Font = new Font("Segoe UI", 9F);

            int y = 18;
            AgregarCampo("Código (generado)", txtCodigo, ref y, true);
            AgregarCampo("Nombre del producto", txtNombre, ref y, true);
            AgregarCampo("Laboratorio", txtLaboratorio, ref y, true);
            AgregarCampo("Precio de venta PVP (S/)", txtPrecio, ref y, true);
            AgregarCampo("Stock físico actual (ingrésalo)", txtStock, ref y, false);

            var lblFecha = new Label { Text = "Fecha de vencimiento REAL del lote:", AutoSize = true, Location = new Point(20, y + 5) };
            Controls.Add(lblFecha);
            dtpVencimiento.Location = new Point(265, y);
            dtpVencimiento.Width = 220;
            dtpVencimiento.Format = DateTimePickerFormat.Custom;
            dtpVencimiento.CustomFormat = "dd/MM/yyyy";
            dtpVencimiento.MinDate = DateTime.Today;
            Controls.Add(dtpVencimiento);
            y += 48;

            Controls.Add(new Label
            {
                Text = "Importante: el vencimiento del Excel es histórico. Verifica la fecha del envase físico antes de guardar.",
                AutoSize = false, Size = new Size(465, 42), Location = new Point(20, y),
                ForeColor = Color.DarkRed
            });

            var btnGuardar = new Button { Text = "Guardar en inventario", Size = new Size(155, 32), Location = new Point(205, 335) };
            btnGuardar.Click += Guardar_Click;
            Controls.Add(btnGuardar);
            var btnCancelar = new Button { Text = "Cancelar", Size = new Size(90, 32), Location = new Point(370, 335), DialogResult = DialogResult.Cancel };
            Controls.Add(btnCancelar);
            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;

            CargarDatos();
        }

        private void AgregarCampo(string etiqueta, TextBox caja, ref int y, bool soloLectura)
        {
            Controls.Add(new Label { Text = etiqueta + ":", AutoSize = true, Location = new Point(20, y + 5) });
            caja.Location = new Point(265, y);
            caja.Size = new Size(220, 23);
            caja.ReadOnly = soloLectura;
            Controls.Add(caja);
            y += 43;
        }

        private void CargarDatos()
        {
            string baseCodigo = "CAT-" + new string((producto.Lote ?? "").Where(char.IsLetterOrDigit).ToArray());
            if (baseCodigo == "CAT-") baseCodigo = "CAT-PRODUCTO";
            string codigo = baseCodigo;
            int consecutivo = 2;
            while (gestion.BuscarPorCodigo(codigo) != null)
                codigo = baseCodigo + "-" + consecutivo++;

            txtCodigo.Text = codigo;
            txtNombre.Text = producto.Producto ?? "";
            txtLaboratorio.Text = producto.Laboratorio ?? "";

            decimal pvp;
            if (!decimal.TryParse(producto.PVP, NumberStyles.Any, CultureInfo.InvariantCulture, out pvp) || pvp <= 0)
            {
                txtPrecio.Text = producto.PVP ?? "";
                txtPrecio.BackColor = Color.MistyRose;
            }
            else
            {
                txtPrecio.Text = pvp.ToString("0.00##", CultureInfo.InvariantCulture);
            }

            // No se copia el stock del Excel: el usuario debe indicar el stock físico actual.
            txtStock.Text = "0";

            DateTime vencimientoFuente;
            if (DateTime.TryParseExact(producto.FechaVencimiento,
                new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd" },
                CultureInfo.InvariantCulture, DateTimeStyles.None, out vencimientoFuente)
                && vencimientoFuente.Date >= DateTime.Today)
            {
                dtpVencimiento.Value = vencimientoFuente.Date;
            }
            else
            {
                dtpVencimiento.Value = DateTime.Today;
            }
        }

        private void Guardar_Click(object sender, EventArgs e)
        {
            decimal precio;
            int stock;
            if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out precio) || precio <= 0)
            {
                MessageBox.Show("El PVP del catálogo no es válido. Revise el precio antes de continuar.");
                return;
            }
            if (!int.TryParse(txtStock.Text, out stock) || stock < 0)
            {
                MessageBox.Show("Ingrese un stock físico actual válido (cero o mayor).");
                return;
            }
            if (dtpVencimiento.Value.Date <= DateTime.Today)
            {
                MessageBox.Show("La fecha de vencimiento debe ser posterior a hoy. Verifique el envase físico.");
                return;
            }

            var nuevo = new Medicamento
            {
                Codigo = txtCodigo.Text.Trim(),
                Nombre = txtNombre.Text.Trim(),
                Categoria = "Sin especificar",
                Laboratorio = txtLaboratorio.Text.Trim(),
                Precio = precio,
                Stock = stock,
                FechaVencimiento = dtpVencimiento.Value.Date
            };

            if (!gestion.RegistrarMedicamento(nuevo))
            {
                MessageBox.Show("No se pudo registrar el producto. Compruebe que el código no esté repetido.");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

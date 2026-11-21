using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desafio_5
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SistemaBotica.Services.PruebaArreglos.Ejecutar();
            Application.Run(new SistemaBotica.Forms.FrmMedicamentos());
        }
    }
}

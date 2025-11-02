using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoEventos.Controlador;
using proyectoEventos.Modelo;
using proyectoEventos.vista;

namespace proyectoEventos
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

            // Instanciar solo el Facade (ControladorPrincipal)
            ControladorPrincipal sistema = new ControladorPrincipal();

            // Correr la aplicación con la vista principal
            Application.Run(sistema.ObtenerVistaInicial());
        }
    }
}

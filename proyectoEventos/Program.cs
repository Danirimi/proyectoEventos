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

            // crear instancia de una sola vez IMPORTANTE!!!!
            // crear intancia de crearUsuario y de otras vistas
            CrearUsuario crearUsuarioVista = new CrearUsuario();
            inicio inicioVista = new inicio();
            IUsuario repo = new IUsuarioMemoria();
            //crear intancia de controlador Usuario donde se va pasar como parametro la instancia de crearUsuario
            ControladorUsuario controladorUsu = new ControladorUsuario(crearUsuarioVista,repo);

            inicioVista.configurarControlador(controladorUsu);


            Application.Run(inicioVista);
        }
    }
}

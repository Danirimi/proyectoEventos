using proyectoEventos.Controlador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoEventos.vista
{
    public partial class inicio : Form
    {
        private ControladorUsuario _controladorUsuario;
        private PaginaInicial _paginaInicialExistente;

        public inicio(PaginaInicial paginaInicial)
        {
            InitializeComponent();
            _paginaInicialExistente = paginaInicial;
        }
        public void configurarControlador(ControladorUsuario controlador)
        {
            _controladorUsuario = controlador;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Evita que la ventana se muestre más de una vez o que se haya cerrado mal
            if (_paginaInicialExistente.IsDisposed)
            {
                MessageBox.Show("La ventana principal fue cerrada. Reinicie la aplicación para continuar.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mostrar la instancia existente
            _paginaInicialExistente.Show();
            _paginaInicialExistente.BringToFront(); // La trae al frente por si ya estaba abierta
            this.Hide(); // Oculta la ventana actual
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

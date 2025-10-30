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
            _paginaInicialExistente.Show();
        }
    }
}

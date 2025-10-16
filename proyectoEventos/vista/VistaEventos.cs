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
    public partial class VistaEventos : Form, IEventoVista
    {
        //Eventos de la interfaz

        public event EventHandler AgregarEvento;
        public event EventHandler BuscarEvento;
        public event EventHandler editarEvento;
        public event EventHandler eliminarEvento;


        public VistaEventos()
        {
            InitializeComponent();
        }
        public void notify(string message)
        {
            MessageBox.Show(message, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void VistaEventos_Load(object sender, EventArgs e)
        {

        }
    }
}

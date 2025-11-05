using proyectoEventos.vista.Argumentos;
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
    public partial class cambiarContraseña : Form
    {
        public event EventHandler<ArgumentosContraseña> CambiarContraseñaE;
        public cambiarContraseña()
        {
            
            InitializeComponent();
        }
        public void LimpiarCampos()
        {
            txtCorreo.Text = "";
            txtContrseña.Text = "";
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text;
            string contrasena = txtContrseña.Text;
            CambiarContraseñaE?.Invoke(this, new ArgumentosContraseña(correo,  contrasena));
            this.LimpiarCampos();
            this.Hide();
        }
    }
}

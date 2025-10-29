using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoEventos.Modelo;

namespace proyectoEventos.vista
{
    public partial class CrearUsuario : Form
    {
        public event EventHandler<UsuarioEventArgs> UsuarioCrearE;
        public CrearUsuario()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Los administradores tienen permisos especiales para gestionar eventos y usuarios en la plataforma. Acambio tu alma nos pertenece","Informacion Administradores",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string correo = txtCorreo.Text;
            int cedula = int.Parse(txtCedula.Text);
            int edad = int.Parse(txtEdad.Text);
            string contrasena = txtContrasena.Text;
            bool esadmin = boolAdmin.Checked;
            // Disparar el evento Usuario
            UsuarioCrearE?.Invoke(this, new UsuarioEventArgs(nombre, correo, cedula, edad, contrasena, esadmin));
            MessageBox.Show("Usuario creado con exito","Exito",MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Hide();

        }

        public void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtCedula.Text = "";
            txtEdad.Text = "";
            txtContrasena.Text = "";
            boolAdmin.Checked = false;
        }

        private void CrearUsuario_Load(object sender, EventArgs e)
        {

        }

    }
}

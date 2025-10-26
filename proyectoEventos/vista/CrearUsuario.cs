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
using proyectoEventos.Controlador;

namespace proyectoEventos.vista
{    
    public partial class CrearUsuario : Form
    {   IUsuarioMemoria usumemoria = new IUsuarioMemoria();
        public CrearUsuario()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.ToString();
            string correo = txtCorreo.ToString();
            string contrasena = txtContrasena.ToString();
            string cedula = txtCedula.ToString();
            string edadT = txtEdad.ToString();
            int  edad = int.Parse(edadT);
            bool admin = boolAdmin.Checked;
            bool exito 
            // Lógica para crear el usuario con los datos proporcionados
            


        }

        private void CrearUsuario_Load(object sender, EventArgs e)
        {

        }
    }
}

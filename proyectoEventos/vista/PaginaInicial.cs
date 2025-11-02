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
    public partial class PaginaInicial : Form
    {
        private ControladorUsuario _controladorUsuario;
        //Creo el evento para iniciar sesion
        public event EventHandler <ArgumentoIniciarSesion> IniciarSesionE;

        public PaginaInicial()
        {
            InitializeComponent();
        }
        public void ConfigurarControlador(ControladorUsuario controlador)
        {
            _controladorUsuario = controlador;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            _controladorUsuario.MostrarVentanaCrearUsuario();


        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void PaginaInicial_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            String Correo = tbCorreo.Text;
            String Contraseña = tbContraseña.Text;
            // Disparar el evento de iniciar sesión
            IniciarSesionE?.Invoke(this, new ArgumentoIniciarSesion(Correo, Contraseña));
            

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

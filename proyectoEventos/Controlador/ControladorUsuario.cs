using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoEventos.Modelo;
using proyectoEventos.vista;

namespace proyectoEventos.Controlador
{
    public class ControladorUsuario
    {
        //Creacion de eventos
        //Evento para crear usuario
        private readonly CrearUsuario _VistaCrearUsuario;
        private readonly IUsuario _repo;
        private readonly PaginaInicial _PaginaInicial;
        //Evento para inicio de sesion



        public ControladorUsuario(CrearUsuario Vista, IUsuario repo, PaginaInicial paginaInicial)
        {
            _VistaCrearUsuario = Vista;
            _repo = repo;
            _PaginaInicial = paginaInicial;
            _VistaCrearUsuario.UsuarioCrearE += OnUsuarioCrear;
            _PaginaInicial.IniciarSesionE += LogicaSesion;
        }
        private void LogicaSesion(object sender, ArgumentoIniciarSesion e) {
            bool valido = _repo.ValidarUsuarioDirecto(e.Correo, e.Contrasena);
            if (valido) MessageBox.Show("Éxito");
            else MessageBox.Show("Error");
        }
        private void OnUsuarioCrear(object sender, UsuarioEventArgs e)
        {
            try
            {
                bool exito = crearUsuarioM(e.Nombre, e.Correo, e.Cedula, e.Edad, e.Contrasena, e.Esadmin);
                if (exito)
                {
                    MessageBox.Show("Usuario creado con éxito.", "Confirmación",
                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo crear el usuario.", "Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al crear el usuario: {ex.Message}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Lógica para manejar la creación de un usuario


        }
        public bool crearUsuarioM(string nombre, string correo, int cedula, int edad, string contrasena, bool esadmin)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo))
                throw new ArgumentException("El nombre y el correo no pueden estar vacíos.");

            if (edad <= 0)
                throw new ArgumentException("La edad debe ser un número positivo.");
            // Lógica para crear un usuario
            Modelo.Usuario nuevoUsuario = new Modelo.Usuario(nombre, correo, cedula, edad, contrasena, esadmin);

            _repo.AgregarUsuario(nuevoUsuario);


            return true; // Retorna true si la creación fue exitosa
        }


        public void MostrarVentanaCrearUsuario()
        {
            _VistaCrearUsuario.LimpiarCampos();
            _VistaCrearUsuario.Show();
        }
    }
            
    }

          

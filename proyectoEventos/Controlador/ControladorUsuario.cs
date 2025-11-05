using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoEventos.Modelo;
using proyectoEventos.vista;
using proyectoEventos.vista.Argumentos;



namespace proyectoEventos.Controlador
{
    public class ControladorUsuario
    {
        //Creacion de eventos
        //Evento para crear usuario
        private readonly CrearUsuario _VistaCrearUsuario;
        private readonly IUsuario _repo;
        private readonly PaginaInicial _PaginaInicial;
        private readonly cambiarContraseña _vistaCambiarContraseña;
        private VistaEventos _vistaEventos;
        private VistaEventosUsuario _vistaEventosUsuario;
        private ControladorEvento _controladorEvento;
        private ControladorEventoUsuario _controladorEventoUsuario;
        private readonly InterfaceEvento _repoEventos;
        private readonly ITicket _repoTickets;
        //Evento para inicio de sesion



        public ControladorUsuario(CrearUsuario Vista, IUsuario repo, PaginaInicial paginaInicial, InterfaceEvento repoEventos, ITicket repoTickets, cambiarContraseña cambiarContraseña)
        {
            _VistaCrearUsuario = Vista;
            _repo = repo;
            _PaginaInicial = paginaInicial;
            _repoEventos = repoEventos;
            _repoTickets = repoTickets;
            _vistaCambiarContraseña = cambiarContraseña;
            _VistaCrearUsuario.UsuarioCrearE += OnUsuarioCrear;
            _PaginaInicial.IniciarSesionE += LogicaSesion;
            _vistaCambiarContraseña.CambiarContraseñaE += OnCambiarContraseña;
        }

        private void LogicaSesion(object sender, ArgumentoIniciarSesion e)
        {
            bool valido = _repo.ValidarUsuarioDirecto(e.Correo, e.Contrasena);
            if (valido)
            {
                Usuario usuarioActual = _repo.ObtenerUsuarioPorCredenciales(e.Correo, e.Contrasena);

                if (usuarioActual == null)
                {
                    MessageBox.Show("Error al obtener información del usuario", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show($"Bienvenido, {usuarioActual.Nombre}!", "Inicio de sesión exitoso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _PaginaInicial.Hide();

                if (usuarioActual.esadmin)
                {
                    // Admin - sin cierre de sesión por ahora
                    if (_vistaEventos == null || _vistaEventos.IsDisposed)
                    {
                        _vistaEventos = new VistaEventos();
                        _controladorEvento = new ControladorEvento(_vistaEventos, _repoEventos);
                    }
                    _vistaEventos.Show();
                }
                else
                {
                    // Usuario normal - CON cierre de sesión
                    if (_vistaEventosUsuario == null || _vistaEventosUsuario.IsDisposed)
                    {
                        _vistaEventosUsuario = new VistaEventosUsuario(usuarioActual);
                        _controladorEventoUsuario = new ControladorEventoUsuario(_vistaEventosUsuario, _repoEventos, usuarioActual, _repoTickets);

                        // ✅ SUSCRIBIR EVENTO DE CERRAR SESIÓN SOLO PARA USUARIOS NORMALES
                        _vistaEventosUsuario.CerrarSesionE += onCerrarSesionE;
                    }
                    _vistaEventosUsuario.Show();
                }
            }
            else
            {
                MessageBox.Show("Correo o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void onCerrarSesionE(object sender, EventArgs e)
        {
            if (_vistaEventosUsuario != null && !_vistaEventosUsuario.IsDisposed)
            {
                _vistaEventosUsuario.Hide();
            }

            if (_PaginaInicial != null && !_PaginaInicial.IsDisposed)
            {
                // ✅ USA EL MÉTODO PÚBLICO QUE YA CREASTE
                _PaginaInicial.LimpiarCamposLogin();  // Este método SÍ es público

                _PaginaInicial.Show();
                _PaginaInicial.BringToFront();
            }

            _vistaEventosUsuario = null;
        }

        private void OnUsuarioCrear(object sender, UsuarioEventArgs e)
        {
            try
            {
                bool verificacion = _repo.Verificar(e.Correo, e.Nombre, e.Cedula); 
                if (!verificacion)
                {
                    bool exito = crearUsuarioM(e.Nombre, e.Correo, e.Cedula, e.Edad, e.Contrasena, e.Esadmin);
                    if (!exito)
                    {
                        MessageBox.Show("No se pudo crear el usuario.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al crear el usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnCambiarContraseña(object sender, ArgumentosContraseña e)
        {
            try
            {
                bool exito = _repo.CambiarContraseña(e.Correo, e.Contraseña);
                if (exito)
                {
                    MessageBox.Show("Contraseña cambiada exitosamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo cambiar la contraseña. Verifique el correo.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cambiar la contraseña: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

            // Lógica para manejar la creación de un usuario



        public bool crearUsuarioM(string nombre, string correo, string cedula, int edad, string contrasena, bool esadmin)
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

        public void MostrarPaginaInicial()
        {
            _PaginaInicial.Show();
        }
        public void MostrarVentanaCambiarContraseña()
        {
            _vistaCambiarContraseña.LimpiarCampos();
            _vistaCambiarContraseña.Show();
        }

    }
}




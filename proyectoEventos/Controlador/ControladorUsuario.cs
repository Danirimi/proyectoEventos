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

        // Timer para verificar expiración de sesión periódicamente
        private Timer _sessionTimer;

        public ControladorUsuario(CrearUsuario Vista, IUsuario repo, PaginaInicial paginaInicial, InterfaceEvento repoEventos, ITicket repoTickets, cambiarContraseña vistaCambiarContrasena)
        {

            if (repo == null)
            {
                throw new ArgumentNullException(nameof(repo), "El repositorio de usuarios no puede ser null");
            }

            _VistaCrearUsuario = Vista;
            _repo = repo;  // ✅ Ahora sabemos que no es null
            _PaginaInicial = paginaInicial;
            _repoEventos = repoEventos;
            _repoTickets = repoTickets;
            _vistaCambiarContraseña = vistaCambiarContrasena;

            _VistaCrearUsuario.UsuarioCrearE += OnUsuarioCrear;
            _PaginaInicial.IniciarSesionE += LogicaSesion;

            if (_vistaCambiarContraseña != null)
            {
                _vistaCambiarContraseña.CambiarContraseñaE += OnCambiarContraseña;
            }
        }

        private void LogicaSesion(object sender, ArgumentoIniciarSesion e)
        {
            try
            {
                bool valido = _repo.ValidarUsuarioDirecto(e.Correo, e.Contrasena);
                if (valido)
                {
                    // Obtener el usuario completo
                    Usuario usuarioActual = _repo.ObtenerUsuarioPorCredenciales(e.Correo, e.Contrasena);

                    if (usuarioActual == null)
                    {
                        MessageBox.Show("Error al obtener información del usuario", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // ✅ NUEVO: Crear token de sesión por 7 MINUTOS en BD
                    int minutosExpiracion = 7; // Aumentado de 1 a 7 minutos
                    string tokenSesion = SesionService.CrearSesion(usuarioActual, minutosExpiracion);

                    if (string.IsNullOrEmpty(tokenSesion))
                    {
                        MessageBox.Show("Error al crear sesión en base de datos", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // ✅ GUARDAR EN SesionManager (persistente)
                    SesionManager.TokenActual = tokenSesion;

                    // Iniciar/Resetear el timer que vigila la sesión
                    if (_sessionTimer != null)
                    {
                        _sessionTimer.Stop();
                        _sessionTimer.Tick -= SessionTimer_Tick;
                        _sessionTimer.Dispose();
                        _sessionTimer = null;
                    }
                    _sessionTimer = new Timer { Interval = 5000 }; // comprobar cada 5s
                    _sessionTimer.Tick += SessionTimer_Tick;
                    _sessionTimer.Start();

                    // ✅ MENSAJE DINÁMICO con el tiempo real de expiración
                    MessageBox.Show($"Bienvenido, {usuarioActual.Nombre}!\nSesión válida por {minutosExpiracion} minuto(s)",
                        "Inicio de sesión exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Ocultar la página inicial
                    _PaginaInicial.Hide();

                    // Verificar si es administrador
                    if (usuarioActual.esadmin)
                    {
                        if (_vistaEventos == null || _vistaEventos.IsDisposed)
                        {
                            _vistaEventos = new VistaEventos();
                            _controladorEvento = new ControladorEvento(_vistaEventos, _repoEventos);
                        }
                        _vistaEventos.Show();
                    }
                    else
                    {
                        if (_vistaEventosUsuario == null || _vistaEventosUsuario.IsDisposed)
                        {
                            _vistaEventosUsuario = new VistaEventosUsuario(usuarioActual);
                            _controladorEventoUsuario = new ControladorEventoUsuario(_vistaEventosUsuario, _repoEventos, usuarioActual, _repoTickets);

                            // Suscribirse al evento de cerrar sesión
                            _vistaEventosUsuario.CerrarSesionE += OnCerrarSesion;
                        }
                        _vistaEventosUsuario.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Correo o contraseña incorrectos", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante el inicio de sesión: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tick del timer: si la sesión ya no es válida, forzar cierre
        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SesionManager.TokenActual) || !SesionManager.SesionActiva)
                {
                    // Detener y limpiar timer antes de cerrar sesión
                    if (_sessionTimer != null)
                    {
                        _sessionTimer.Stop();
                        _sessionTimer.Tick -= SessionTimer_Tick;
                        _sessionTimer.Dispose();
                        _sessionTimer = null;
                    }

                    // Ejecuta cierre de sesión (misma lógica que cierre manual)
                    OnCerrarSesion(this, EventArgs.Empty);
                }
            }
            catch
            {
                // No lanzar excepciones desde el timer
            }
        }

        private void OnCerrarSesion(object sender, EventArgs e)
        {
            try
            {
                // Detener timer si estaba activo
                if (_sessionTimer != null)
                {
                    _sessionTimer.Stop();
                    _sessionTimer.Tick -= SessionTimer_Tick;
                    _sessionTimer.Dispose();
                    _sessionTimer = null;
                }

                // ✅ NUEVO: Cerrar sesión usando SesionManager
                SesionManager.CerrarSesion();

                // Ocultar vista de usuario si está abierta
                if (_vistaEventosUsuario != null && !_vistaEventosUsuario.IsDisposed)
                {
                    _vistaEventosUsuario.Hide();
                    _vistaEventosUsuario = null;
                }

                // Limpiar campos de login y mostrar inicio
                _PaginaInicial.LimpiarCamposLogin();
                if (_PaginaInicial != null && !_PaginaInicial.IsDisposed)
                {
                    _PaginaInicial.Show();
                    _PaginaInicial.BringToFront();
                }

                MessageBox.Show("Sesión cerrada exitosamente", "Cerrar Sesión",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar sesión: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnUsuarioCrear(object sender, UsuarioEventArgs e)
        {
            try
            {
                // NOTA: No validar sesión aquí para permitir registro de nuevos usuarios sin iniciar sesión

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
                // ✅ NUEVO: Validar sesión usando SesionManager
                if (!SesionManager.SesionActiva)
                {
                    MessageBox.Show("Sesión expirada. Por favor, inicie sesión nuevamente.", "Sesión Expirada",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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

        public bool crearUsuarioM(string nombre, string correo, string cedula, int edad, string contrasena, bool esadmin)
        {
            try
            {
                // Nota: El registro de nuevos usuarios no requiere sesión activa.

                // ✅ VALIDAR que el repositorio esté inicializado
                if (_repo == null)
                {
                    MessageBox.Show("ERROR: Repositorio no inicializado", "ERROR");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo))
                    throw new ArgumentException("El nombre y el correo no pueden estar vacíos.");

                if (edad <= 0)
                    throw new ArgumentException("La edad debe ser un número positivo.");

                Usuario nuevoUsuario = new Usuario(nombre, correo, cedula, edad, contrasena, esadmin);
                _repo.AgregarUsuario(nuevoUsuario);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en crearUsuarioM: {ex.Message}", "Error Detallado");
                return false;
            }
        }

        // ✅ NUEVO: Método para probar expiración de sesión
        public void ProbarExpiracionSesion()
        {
            try
            {
                if (string.IsNullOrEmpty(SesionManager.TokenActual))
                {
                    MessageBox.Show("No hay sesión activa", "Estado Sesión");
                    return;
                }

                bool sesionActiva = SesionManager.SesionActiva;

                if (!sesionActiva)
                {
                    MessageBox.Show("❌ SESIÓN EXPIRADA\nPor favor, inicie sesión nuevamente",
                        "Sesión Expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Obtener información adicional del token
                    string correoUsuario = SesionService.ObtenerCorreoDesdeToken(SesionManager.TokenActual);
                    MessageBox.Show($"✅ SESIÓN ACTIVA\nUsuario: {correoUsuario ?? "Desconocido"}",
                        "Sesión Activa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar sesión: {ex.Message}", "Error");
            }
        }

        public void MostrarVentanaCrearUsuario()
        {
            _VistaCrearUsuario.LimpiarCampos();
            _VistaCrearUsuario.Show();
        }

        public void MostrarVentanaCambiarContraseña()
        {
            if (_vistaCambiarContraseña == null || _vistaCambiarContraseña.IsDisposed)
                return;

            _vistaCambiarContraseña.LimpiarCampos();
            _vistaCambiarContraseña.Show();
            _vistaCambiarContraseña.BringToFront();
        }
    }
}
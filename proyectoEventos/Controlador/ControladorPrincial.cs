using proyectoEventos.Modelo;
using proyectoEventos.vista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoEventos.Controlador
{
    public class ControladorPrincipal
    {
        // === Vistas ===
        private CrearUsuario _vistaCrearUsuario;
        private PaginaInicial _vistaPaginaInicial;
        private inicio _vistaInicio;
        private cambiarContraseña _vistaCambiarContraseña;
        private VistaEventos _vistaEventos;
        private VistaEventosUsuario _vistaEventosUsuario;
        private vista.VerHistorial _vistaVerHistorial;

        // === Repositorios ===
        private IUsuario _repoUsuarios;
        private InterfaceEvento _repoEventos;
        private ITicket _repoTickets;

        // === Controladores ===
        private ControladorUsuario _controladorUsuario;
        private ControladorEvento _controladorEvento;
        private ControladorEventoUsuario _controladorEventoUsuario;


        public ControladorPrincipal()
        {
            // Verificar conexión a MySQL al iniciar
            if (!MySQLConexion.ProbarConexion())
            {
                MessageBox.Show(
                    "No se pudo conectar a la base de datos MySQL.\n\n" +
                    "Verifique que:\n" +
                    "1. MySQL esté ejecutándose\n" +
                    "2. La base de datos 'Eventos' exista\n" +
                    "3. Las credenciales en MySQLConexion.cs sean correctas",
                    "Error de Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                Application.Exit();
                return;
            }

            // 1. Crear vistas
            _vistaCrearUsuario = new CrearUsuario();
            _vistaPaginaInicial = new PaginaInicial();
            _vistaInicio = new inicio(_vistaPaginaInicial);
            _vistaCambiarContraseña = new cambiarContraseña();
            _vistaVerHistorial = new vista.VerHistorial();

            // 2. Crear repositorios MySQL (CAMBIO AQUÍ)
            _repoUsuarios = new IUsuarioMySQL();
            _repoEventos = new IEventoMySQL();
            

            // 3. Crear controladores y pasar dependencias
            _controladorUsuario = new ControladorUsuario(
                _vistaCrearUsuario,
                _repoUsuarios,
                _vistaPaginaInicial,
                _repoEventos,
                _repoTickets,
                _vistaCambiarContraseña
            );

            // 4. Conectar controladores a vistas
            _vistaPaginaInicial.ConfigurarControlador(_controladorUsuario);
            _vistaInicio.configurarControlador(_controladorUsuario);
        }

        // Método público para iniciar la app
        public Form ObtenerVistaInicial()
        {
            return _vistaInicio;
        }
    }
}

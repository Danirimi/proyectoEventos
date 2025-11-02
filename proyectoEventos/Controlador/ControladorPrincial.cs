using proyectoEventos.Modelo;
using proyectoEventos.vista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoEventos.vista.Argumentos;

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

        // === Repositorios ===
        private IUsuario _repoUsuarios;
        private InterfaceEvento _repoEventos;

        // === Controladores ===
        private ControladorUsuario _controladorUsuario;
        private ControladorEvento _controladorEvento;
        private ControladorEventoUsuario _controladorEventoUsuario;


        public ControladorPrincipal()
        {
            // 1. Crear vistas
            _vistaCrearUsuario = new CrearUsuario();
            _vistaPaginaInicial = new PaginaInicial();
            _vistaInicio = new inicio(_vistaPaginaInicial);
            _vistaCambiarContraseña = new cambiarContraseña();

            // 2. Crear repositorios
            _repoUsuarios = new IUsuarioMemoria();
            _repoEventos = new InterfazEventoMemoria();

            // 3. Crear controladores y pasar dependencias
            _controladorUsuario = new ControladorUsuario(
                _vistaCrearUsuario,
                _repoUsuarios,
                _vistaPaginaInicial,
                _repoEventos,
                _vistaCambiarContraseña
            );

            // 4. Conectar controladores a vistas
            _vistaPaginaInicial.ConfigurarControlador(_controladorUsuario);
            _vistaInicio.configurarControlador(_controladorUsuario);

            SuscribirEventos();
        }

        // Método público para iniciar la app
        public Form ObtenerVistaInicial()
        {
            return _vistaInicio;
        }
        private void SuscribirEventos()
        {
            _controladorUsuario.SesionIniciada += OnSesionIniciada;
        }

        private void OnSesionIniciada(object sender, secioniniciadaArgs e)
        {
            Usuario usuarioActual = e.Usuario;
            _vistaPaginaInicial.Hide();

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
                    _controladorEventoUsuario = new ControladorEventoUsuario(_vistaEventosUsuario, _repoEventos, usuarioActual);
                }

                _vistaEventosUsuario.Show();
            }
        }

    }
}

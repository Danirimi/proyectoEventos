using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using proyectoEventos.Modelo.Seguridad;


namespace proyectoEventos.Modelo
{
    public class IUsuarioMemoria : IUsuario
    {
        private readonly string rutaUsuarios;
        private readonly string rutaAdministradores;

        private readonly List<Usuario> _usuarios;
        private readonly List<Usuario> _administradores;

        public IUsuarioMemoria()
        {
            // Construye las rutas dinámicamente (no se usan para leer/escribir ahora)
            rutaUsuarios = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modelo", "Datos", "usuarios.json");
            rutaAdministradores = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modelo", "Datos", "administradores.json");
            // Inicialización en memoria (sin JSON)
            _usuarios = new List<Usuario>();
            _administradores = new List<Usuario>();
        }

        public void AgregarUsuario(Usuario usuario)
        {
            // HASHEAR CONTRASEÑA ANTES DE GUARDAR
            string contrasenaHash = PasswordHasher.HashPassword(usuario.Contrasena);

            // Crear nuevo usuario con contraseña hasheada
            var usuarioConHash = new Usuario(usuario.Nombre, usuario.Correo, usuario.Cedula,
                                           usuario.Edad, contrasenaHash, usuario.esadmin);

            if (usuario.esadmin)
            {
                _administradores.Add(usuarioConHash);
                // Persistencia deshabilitada: almacenamiento en memoria
            }
            else
            {
                _usuarios.Add(usuarioConHash);
                // Persistencia deshabilitada: almacenamiento en memoria
            }
            MessageBox.Show("Usuario creado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void EditarUsuario(Usuario usuario)
        {
            // Determinar la lista y el nombre del archivo correspondiente
            List<Usuario> lista = usuario.esadmin ? _administradores : _usuarios;
            string archivo = usuario.esadmin ? "administradores.json" : "usuarios.json";

            // Buscar el usuario existente
            var existente = lista.FirstOrDefault(u => u.Cedula == usuario.Cedula);
            if (existente != null)
            {
                // Actualizar los datos del usuario
                existente.Nombre = usuario.Nombre;
                existente.Correo = usuario.Correo;
                existente.Edad = usuario.Edad;
                existente.Contrasena = usuario.Contrasena;

                // Persistencia deshabilitada: almacenamiento en memoria

                MessageBox.Show("Usuario actualizado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se encontró el usuario a editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void EliminarUsuario(string cedula)
        {
            bool eliminado = false;

            // Buscar en usuarios normales
            var usuario = _usuarios.FirstOrDefault(u => u.Cedula.ToString() == cedula);
            if (usuario != null)
            {
                _usuarios.Remove(usuario);
                // Persistencia deshabilitada
                eliminado = true;
            }

            // Buscar en administradores
            var admin = _administradores.FirstOrDefault(a => a.Cedula.ToString() == cedula);
            if (admin != null)
            {
                _administradores.Remove(admin);
                // Persistencia deshabilitada
                eliminado = true;
            }

            // Mostrar mensaje según corresponda
            if (eliminado)
                MessageBox.Show("Usuario eliminado correctamente.", "Confirmación",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se encontró el usuario.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _usuarios;
        }

        public void verHistorialcompra(string cedula)
        {
            throw new NotImplementedException();
        }

        //este es para el inicio de sesion y verifica el correo y la contraseña
        public bool ValidarUsuarioDirecto(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
                return false;

            var listaUsuarios = _usuarios;
            var listaAdmins = _administradores;

            // Buscar en usuarios normales
            var usuario = listaUsuarios.FirstOrDefault(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));

            if (usuario != null)
            {
                // VERIFICAR CON HASH
                return PasswordHasher.VerifyPassword(contrasena, usuario.Contrasena);
            }

            // Buscar en administradores
            var admin = listaAdmins.FirstOrDefault(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));

            if (admin != null)
            {
                // VERIFICAR CON HASH
                return PasswordHasher.VerifyPassword(contrasena, admin.Contrasena);
            }

            return false;
        }

        public Usuario ObtenerUsuarioPorCredenciales(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
                return null;

            var listaUsuarios = _usuarios;
            var listaAdmins = _administradores;

            var usuario = listaUsuarios.FirstOrDefault(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase) &&
                u.Contrasena == contrasena);

            if (usuario != null)
                return usuario;

            var admin = listaAdmins.FirstOrDefault(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase) &&
                u.Contrasena == contrasena);

            return admin;
        }
        
        // este es para verificar si un usuario ya existe por su correo, usuario y cedula

        public bool Verificar(string correo, string nombre, string cedula)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(cedula))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return true;
            }

            var listaUsuarios = _usuarios;
            var listaAdmins = _administradores;

            bool existeCorreo = listaUsuarios.Any(u => u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase)) ||
                                listaAdmins.Any(u => u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));

            bool existeCedula = listaUsuarios.Any(u => u.Cedula == cedula) ||
                                listaAdmins.Any(u => u.Cedula == cedula);

            bool existeNombre = listaUsuarios.Any(u => u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)) ||
                                listaAdmins.Any(u => u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            if (existeCorreo)
            {
                MessageBox.Show("El correo ya está en uso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (existeCedula)
            {
                MessageBox.Show("La cédula ya está en uso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (existeNombre)
            {
                MessageBox.Show("El usuario ya está en uso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            return false;
        }

        public bool CambiarContraseña(string correo, string nuevaContraseña)
        {
            // HASHEAR NUEVA CONTRASEÑA
            string nuevaContrasenaHash = PasswordHasher.HashPassword(nuevaContraseña);

            // Buscar en usuarios normales
            var usuario = _usuarios.FirstOrDefault(u => u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
            if (usuario != null)
            {
                usuario.Contrasena = nuevaContrasenaHash;
                // Persistencia deshabilitada
                return true;
            }

            // Buscar en administradores
            var admin = _administradores.FirstOrDefault(a => a.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
            if (admin != null)
            {
                admin.Contrasena = nuevaContrasenaHash;
                // Persistencia deshabilitada
                return true;
            }

            return false;
        }
        public List<Ticket> ObtenerHistorialCompras(int usuarioId)
        {
            ITicket repositorioTickets = new ITicketmemoria();
            return repositorioTickets.ObtenerTicketsPorUsuario(usuarioId);
        }
    }

}


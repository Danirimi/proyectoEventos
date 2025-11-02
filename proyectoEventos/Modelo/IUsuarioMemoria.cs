using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

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
            // Construye las rutas dinámicamente
            rutaUsuarios = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modelo", "Datos", "usuarios.json");
            rutaAdministradores = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modelo", "Datos", "administradores.json");
            // Carga los datos usando esas rutas
            _usuarios = JsonDataManager.CargarDatos<Usuario>(rutaUsuarios);
            _administradores = JsonDataManager.CargarDatos<Usuario>(rutaAdministradores);
        }

        public void AgregarUsuario(Usuario usuario)
        {
            if (usuario.esadmin)
            {
                _administradores.Add(usuario);
              
                JsonDataManager.GuardarDatos(_administradores, "administradores.json");
            }
            else
            {
                _usuarios.Add(usuario);
               
                JsonDataManager.GuardarDatos(_usuarios, "usuarios.json");
            }
            MessageBox.Show("Usuario creado con exito", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // Guardar toda la lista actualizada usando la clase JsonDataManager
                JsonDataManager.GuardarDatos(lista, archivo);

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
                JsonDataManager.GuardarDatos(_usuarios, "usuarios.json");
                eliminado = true;
            }

            // Buscar en administradores
            var admin = _administradores.FirstOrDefault(a => a.Cedula.ToString() == cedula);
            if (admin != null)
            {
                _administradores.Remove(admin);
                JsonDataManager.GuardarDatos(_administradores, "administradores.json");
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
            //  Verificar que ambos parámetros no estén vacíos
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
                return false;
            //  Leer los archivos JSON
            var listaUsuarios = LeerArchivoJSON(rutaUsuarios);
            var listaAdmins = LeerArchivoJSON(rutaAdministradores);

            //  Buscar coincidencia en ambos archivos
            var usuario = listaUsuarios.FirstOrDefault(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase) &&
                u.Contrasena == contrasena);

            var admin = listaAdmins.FirstOrDefault(u =>
              u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase) &&
              u.Contrasena == contrasena);

            //  Si se encuentra en alguno de los dos archivos
            if (usuario != null || admin != null)
                return true;
            return false;
        }
        
        public Usuario ObtenerUsuarioPorCredenciales(string correo, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
                return null;

            var listaUsuarios = LeerArchivoJSON(rutaUsuarios);
            var listaAdmins = LeerArchivoJSON(rutaAdministradores);

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

            var listaUsuarios = LeerArchivoJSON(rutaUsuarios);
            var listaAdmins = LeerArchivoJSON(rutaAdministradores);

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



        private List<Usuario> LeerArchivoJSON(string ruta)
        {
            if (!File.Exists(ruta))
                return new List<Usuario>(); // si el archivo no existe, devolver lista vacía

            string contenido = File.ReadAllText(ruta);
            return JsonConvert.DeserializeObject<List<Usuario>>(contenido) ?? new List<Usuario>();


        }
    }

}


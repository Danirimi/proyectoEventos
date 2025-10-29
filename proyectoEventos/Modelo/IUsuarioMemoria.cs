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
            // Definimos las rutas dentro de bin\Debug\net8.0-windows\
            rutaUsuarios = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "usuarios.json");
            rutaAdministradores = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "administradores.json");

            // Cargamos los datos existentes (si los archivos existen)
            _usuarios = CargarDesdeJson(rutaUsuarios);
            _administradores = CargarDesdeJson(rutaAdministradores);
        }

        public void AgregarUsuario(Usuario usuario)
        {
            if (usuario.esadmin)
            {
                _administradores.Add(usuario);
                GuardarEnJson(_administradores, rutaAdministradores);
            }
            else
            {
                _usuarios.Add(usuario);
                GuardarEnJson(_usuarios, rutaUsuarios);
            }

            MessageBox.Show("Usuario guardado correctamente en JSON.", "Confirmación",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void EditarUsuario(Usuario usuario)
        {
            List<Usuario> lista = usuario.esadmin ? _administradores : _usuarios;
            string ruta = usuario.esadmin ? rutaAdministradores : rutaUsuarios;

            var existente = lista.FirstOrDefault(u => u.Cedula == usuario.Cedula);
            if (existente != null)
            {
                existente.Nombre = usuario.Nombre;
                existente.Correo = usuario.Correo;
                existente.Edad = usuario.Edad;
                existente.Contrasena = usuario.Contrasena;

                GuardarEnJson(lista, ruta);
                MessageBox.Show("Usuario actualizado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void EliminarUsuario(string cedula)
        {
            bool eliminado = false;

            var usuario = _usuarios.FirstOrDefault(u => u.Cedula.ToString() == cedula);
            if (usuario != null)
            {
                _usuarios.Remove(usuario);
                GuardarEnJson(_usuarios, rutaUsuarios);
                eliminado = true;
            }

            var admin = _administradores.FirstOrDefault(a => a.Cedula.ToString() == cedula);
            if (admin != null)
            {
                _administradores.Remove(admin);
                GuardarEnJson(_administradores, rutaAdministradores);
                eliminado = true;
            }

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

        // Métodos auxiliares JSON
        private List<Usuario> CargarDesdeJson(string ruta)
        {
            try
            {
                if (File.Exists(ruta))
                {
                    string json = File.ReadAllText(ruta);
                    var datos = JsonSerializer.Deserialize<List<Usuario>>(json);
                    return datos ?? new List<Usuario>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al leer el archivo JSON ({ruta}): {ex.Message}");
            }
            return new List<Usuario>();
        }

        private void GuardarEnJson(List<Usuario> lista, string ruta)
        {
            try
            {
                string json = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ruta, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el archivo JSON ({ruta}): {ex.Message}");
            }
        }
    }
}


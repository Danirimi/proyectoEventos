using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace proyectoEventos.Modelo
{
    public class IUsuarioMySQL : IUsuario
    {
        public void AgregarUsuario(Usuario usuario)
        {
            try
            {
                // Encriptar antes de guardar
                usuario.Contrasena = PasswordSecurity.HashPassword(usuario.Contrasena);

                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = @"INSERT INTO usuarios (nombre, correo, cedula, edad, contrasena, esadmin) 
                           VALUES (@nombre, @correo, @cedula, @edad, @contrasena, @esadmin)";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        // ? ESTE CÓDIGO FALTA Y ES ESENCIAL
                        cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("@cedula", usuario.Cedula);
                        cmd.Parameters.AddWithValue("@edad", usuario.Edad);
                        cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
                        cmd.Parameters.AddWithValue("@esadmin", usuario.esadmin);

                        cmd.ExecuteNonQuery(); // ? ESTE EJECUTA EL INSERT

                        MessageBox.Show("Usuario creado con éxito", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show("El correo o cédula ya está registrado.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Error al agregar usuario: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void EditarUsuario(Usuario usuario)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = @"UPDATE usuarios 
                                   SET nombre = @nombre, correo = @correo, edad = @edad, contrasena = @contrasena 
                                   WHERE cedula = @cedula";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@correo", usuario.Correo);
                        cmd.Parameters.AddWithValue("@edad", usuario.Edad);
                        cmd.Parameters.AddWithValue("@contrasena", usuario.Contrasena);
                        cmd.Parameters.AddWithValue("@cedula", usuario.Cedula);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Usuario actualizado correctamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el usuario a editar.", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarUsuario(string cedula)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = "DELETE FROM usuarios WHERE cedula = @cedula";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@cedula", cedula);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Usuario eliminado correctamente.", "Confirmación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el usuario.", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<Usuario> ObtenerUsuarios()
        {
            var usuarios = new List<Usuario>();
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return usuarios;

                    string query = "SELECT id, nombre, correo, cedula, edad, contrasena, esadmin FROM usuarios";

                    using (var cmd = new MySqlCommand(query, conexion))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuario(
                                reader.GetString("nombre"),
                                reader.GetString("correo"),
                                reader.GetString("cedula"),
                                reader.GetInt32("edad"),
                                reader.GetString("contrasena"),
                                reader.GetBoolean("esadmin")
                            );
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener usuarios: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return usuarios;
        }

        public bool ValidarUsuarioDirecto(string correo, string contrasena)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return false;

                   
                    string query = "SELECT contrasena FROM usuarios WHERE correo = @correo";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            string contrasenaEncriptada = result.ToString();
                           
                            return PasswordSecurity.VerifyPassword(contrasena, contrasenaEncriptada);
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                // ... (código existente IGUAL)
                return false;
            }
        }

        public Usuario ObtenerUsuarioPorCredenciales(string correo, string contrasena)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return null;

                    string query = @"SELECT id, nombre, correo, cedula, edad, contrasena, esadmin 
                                   FROM usuarios 
                                   WHERE correo = @correo AND contrasena = @contrasena";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@contrasena", contrasena);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Usuario(
                                    reader.GetString("nombre"),
                                    reader.GetString("correo"),
                                    reader.GetString("cedula"),
                                    reader.GetInt32("edad"),
                                    reader.GetString("contrasena"),
                                    reader.GetBoolean("esadmin")
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public bool Verificar(string correo, string nombre, string cedula)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return true;

                    string query = @"SELECT 
                                    (SELECT COUNT(*) FROM usuarios WHERE correo = @correo) as correoExiste,
                                    (SELECT COUNT(*) FROM usuarios WHERE cedula = @cedula) as cedulaExiste,
                                    (SELECT COUNT(*) FROM usuarios WHERE nombre = @nombre) as nombreExiste";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@cedula", cedula);
                        cmd.Parameters.AddWithValue("@nombre", nombre);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int correoExiste = reader.GetInt32("correoExiste");
                                int cedulaExiste = reader.GetInt32("cedulaExiste");
                                int nombreExiste = reader.GetInt32("nombreExiste");

                                if (correoExiste > 0)
                                {
                                    MessageBox.Show("El correo ya está en uso.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return true;
                                }
                                if (cedulaExiste > 0)
                                {
                                    MessageBox.Show("La cédula ya está en uso.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return true;
                                }
                                if (nombreExiste > 0)
                                {
                                    MessageBox.Show("El usuario ya está en uso.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            return false;
        }

        public bool CambiarContraseña(string correo, string nuevaContraseña)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return false;

                    string query = "UPDATE usuarios SET contrasena = @contrasena WHERE correo = @correo";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@contrasena", nuevaContraseña);
                        cmd.Parameters.AddWithValue("@correo", correo);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar contraseña: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public List<Ticket> ObtenerHistorialCompras(int usuarioId)
        {
            ITicket repositorioTickets = new ITicketMySQL();
            return repositorioTickets.ObtenerTicketsPorUsuario(usuarioId);
        }

        public void verHistorialcompra(string cedula)
        {
            throw new NotImplementedException();
        }
    }
}
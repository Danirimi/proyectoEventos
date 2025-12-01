using MySql.Data.MySqlClient;
using proyectoEventos.Modelo;
using proyectoEventos.Modelo.Seguridad;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace proyectoEventos.Modelo
{
    public class UsuarioMySQL : IUsuario
    {
        // MÉTODO AGREGAR USUARIO (YA EXISTENTE)
        public void AgregarUsuario(Usuario usuario)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    // HASHEAR CONTRASEÑA
                    string contrasenaHash = PasswordHasher.HashPassword(usuario.Contrasena);

                    string query = @"INSERT INTO usuarios (nombre, correo, cedula, edad, contrasena, esadmin) 
                                   VALUES (@nombre, @correo, @cedula, @edad, @contrasena, @esadmin)";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.Add("@nombre", MySqlDbType.VarChar, 200).Value = usuario.Nombre ?? string.Empty;
                        cmd.Parameters.Add("@correo", MySqlDbType.VarChar, 200).Value = usuario.Correo ?? string.Empty;
                        cmd.Parameters.Add("@cedula", MySqlDbType.VarChar, 50).Value = usuario.Cedula ?? string.Empty;
                        cmd.Parameters.Add("@edad", MySqlDbType.Int32).Value = usuario.Edad;
                        cmd.Parameters.Add("@contrasena", MySqlDbType.VarChar, 255).Value = contrasenaHash;
                        cmd.Parameters.Add("@esadmin", MySqlDbType.Bit).Value = usuario.esadmin;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario creado con éxito", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error de base de datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // MÉTODO EDITAR USUARIO
        public void EditarUsuario(Usuario usuario)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = @"UPDATE usuarios SET nombre = @nombre, correo = @correo, 
                                   cedula = @cedula, edad = @edad, esadmin = @esadmin 
                                   WHERE cedula = @cedulaBuscar";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.Add("@nombre", MySqlDbType.VarChar, 200).Value = usuario.Nombre ?? string.Empty;
                        cmd.Parameters.Add("@correo", MySqlDbType.VarChar, 200).Value = usuario.Correo ?? string.Empty;
                        cmd.Parameters.Add("@cedula", MySqlDbType.VarChar, 50).Value = usuario.Cedula ?? string.Empty;
                        cmd.Parameters.Add("@edad", MySqlDbType.Int32).Value = usuario.Edad;
                        cmd.Parameters.Add("@esadmin", MySqlDbType.Bit).Value = usuario.esadmin;
                        cmd.Parameters.Add("@cedulaBuscar", MySqlDbType.VarChar, 50).Value = usuario.Cedula ?? string.Empty;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario actualizado con éxito", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // MÉTODO ELIMINAR USUARIO
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
                        cmd.Parameters.Add("@cedula", MySqlDbType.VarChar, 50).Value = cedula ?? string.Empty;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario eliminado con éxito", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // MÉTODO VER HISTORIAL COMPRA
        public void verHistorialcompra(string cedula)
        {
            // Implementación básica - puedes expandir esto
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = "SELECT * FROM tickets WHERE usuario_cedula = @cedula";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.Add("@cedula", MySqlDbType.VarChar, 50).Value = cedula ?? string.Empty;
                        var reader = cmd.ExecuteReader();

                        // Aquí procesarías los resultados del historial
                        // Por ahora solo mostramos un mensaje
                        MessageBox.Show($"Mostrando historial para cédula: {cedula}", "Historial",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener historial: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // MÉTODO VALIDAR USUARIO DIRECTO (CORREGIDO)
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
                        cmd.Parameters.Add("@correo", MySqlDbType.VarChar, 200).Value = correo ?? string.Empty;

                        string hashedPassword = cmd.ExecuteScalar()?.ToString();

                        if (string.IsNullOrEmpty(hashedPassword))
                            return false;

                        return PasswordHasher.VerifyPassword(contrasena, hashedPassword);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // MÉTODO VERIFICAR
        public bool Verificar(string correo, string nombre, string cedula)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return false;

                    string query = "SELECT COUNT(*) FROM usuarios WHERE correo = @correo AND nombre = @nombre AND cedula = @cedula";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.Add("@correo", MySqlDbType.VarChar, 200).Value = correo ?? string.Empty;
                        cmd.Parameters.Add("@nombre", MySqlDbType.VarChar, 200).Value = nombre ?? string.Empty;
                        cmd.Parameters.Add("@cedula", MySqlDbType.VarChar, 50).Value = cedula ?? string.Empty;

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // MÉTODO OBTENER USUARIOS
        public List<Usuario> ObtenerUsuarios()
        {
            var usuarios = new List<Usuario>();

            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return usuarios;

                    string query = "SELECT nombre, correo, cedula, edad, esadmin FROM usuarios";

                    using (var cmd = new MySqlCommand(query, conexion))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario(
                                reader["nombre"].ToString(),
                                reader["correo"].ToString(),
                                reader["cedula"].ToString(),
                                Convert.ToInt32(reader["edad"]),
                                "", // Contraseña no se devuelve por seguridad
                                Convert.ToBoolean(reader["esadmin"])
                            ));
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

        // MÉTODO OBTENER USUARIO POR CREDENCIALES
        public Usuario ObtenerUsuarioPorCredenciales(string correo, string contrasena)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return null;

                    string query = "SELECT nombre, correo, cedula, edad, esadmin, contrasena FROM usuarios WHERE correo = @correo";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.Add("@correo", MySqlDbType.VarChar, 200).Value = correo ?? string.Empty;

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["contrasena"].ToString();

                                // Verificar contraseña
                                if (PasswordHasher.VerifyPassword(contrasena, storedPassword))
                                {
                                    return new Usuario(
                                        reader["nombre"].ToString(),
                                        reader["correo"].ToString(),
                                        reader["cedula"].ToString(),
                                        Convert.ToInt32(reader["edad"]),
                                        "", // No devolver contraseña
                                        Convert.ToBoolean(reader["esadmin"])
                                    );
                                }
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

        // MÉTODO CAMBIAR CONTRASEÑA (YA EXISTENTE)
        public bool CambiarContraseña(string correo, string nuevaContraseña)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return false;

                    string nuevaContrasenaHash = PasswordHasher.HashPassword(nuevaContraseña);
                    string query = "UPDATE usuarios SET contrasena = @contrasena WHERE correo = @correo";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.Add("@contrasena", MySqlDbType.VarChar, 255).Value = nuevaContrasenaHash;
                        cmd.Parameters.Add("@correo", MySqlDbType.VarChar, 200).Value = correo ?? string.Empty;

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

        // MÉTODO OBTENER HISTORIAL COMPRAS
        public List<Ticket> ObtenerHistorialCompras(int usuarioId)
        {
            var tickets = new List<Ticket>();

            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return tickets;

                    string query = "SELECT * FROM tickets WHERE usuario_id = @usuarioId";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.Add("@usuarioId", MySqlDbType.Int32).Value = usuarioId;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Crear objeto Ticket con los datos - ajusta según tu clase Ticket
                                var ticket = new Ticket
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    // Agrega más propiedades según tu clase Ticket
                                };
                                tickets.Add(ticket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener historial: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return tickets;
        }
    }
}
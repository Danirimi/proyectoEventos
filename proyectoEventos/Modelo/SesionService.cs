using MySql.Data.MySqlClient;
using System;

namespace proyectoEventos.Modelo
{
    public static class SesionService
    {
        public static string CrearSesion(Usuario usuario, int minutosExpiracion = 1)
        {
            try
            {
                LimpiarSesionesExpiradas();
                string token = Guid.NewGuid().ToString();
                DateTime expiracion = DateTime.Now.AddMinutes(minutosExpiracion);

                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return null;

                    // ✅ USAR usuario_id en lugar de usuario_correo
                    string query = @"INSERT INTO sesiones (token, usuario_id, fecha_expiracion) 
                           VALUES (@token, @usuario_id, @expiracion)";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@token", token);
                        cmd.Parameters.AddWithValue("@usuario_id", usuario.id); // ← NECESITA usuario.id
                        cmd.Parameters.AddWithValue("@expiracion", expiracion);

                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine($"✅ Sesión BD creada - Token: {token}");
                return token;
            }
            catch (MySqlException mex)
            {
                ErrorLogger.LogException(mex, "CrearSesion MySQL");
                Console.WriteLine($"❌ Error al crear sesión BD: {mex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex, "CrearSesion General");
                Console.WriteLine($"❌ Error al crear sesión BD: {ex.Message}");
                return null;
            }
        }

        public static bool ValidarSesion(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return false;

                    string query = @"SELECT COUNT(*) FROM sesiones 
                                   WHERE token = @token AND activa = 1 
                                   AND fecha_expiracion > NOW()";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@token", token);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        bool valida = count > 0;

                        Console.WriteLine($"🔍 Validar sesión: {token} -> {valida}");
                        return valida;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex, "ValidarSesion");
                Console.WriteLine($"❌ Error al validar sesión BD: {ex.Message}");
                return false;
            }
        }

        public static void CerrarSesion(string token)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = "UPDATE sesiones SET activa = 0 WHERE token = @token";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@token", token);
                        int afectadas = cmd.ExecuteNonQuery();
                        Console.WriteLine($"🔒 Sesiones cerradas: {afectadas}");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex, "CerrarSesion");
                Console.WriteLine($"❌ Error al cerrar sesión BD: {ex.Message}");
            }
        }

        private static void LimpiarSesionesExpiradas()
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = "DELETE FROM sesiones WHERE fecha_expiracion <= NOW() OR activa = 0";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        int eliminadas = cmd.ExecuteNonQuery();
                        if (eliminadas > 0)
                            Console.WriteLine($"🗑️ Sesiones limpiadas: {eliminadas}");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex, "LimpiarSesionesExpiradas");
                Console.WriteLine($"❌ Error al limpiar sesiones BD: {ex.Message}");
            }
        }

        // Obtener correo del usuario desde token
        public static string ObtenerCorreoDesdeToken(string token)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return null;

                    string query = @"SELECT usuario_correo FROM sesiones 
                                   WHERE token = @token AND activa = 1 
                                   AND fecha_expiracion > NOW()";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@token", token);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogException(ex, "ObtenerCorreoDesdeToken");
                Console.WriteLine($"❌ Error al obtener correo desde token: {ex.Message}");
                return null;
            }
        }
    }
}
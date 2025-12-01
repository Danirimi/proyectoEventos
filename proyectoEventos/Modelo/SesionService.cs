using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace proyectoEventos.Modelo
{
    public static class SesionService
    {
        // Clave secreta para HMAC (debería almacenarse en configuración segura en producción)
        private static readonly byte[] SecretKey = Encoding.UTF8.GetBytes("c9f1b2a7-SECRET-CHANGE-2025-please-change");

        public static string CrearSesion(Usuario usuario, int minutosExpiracion = 1)
        {
            try
            {
                LimpiarSesionesExpiradas();
                string rawToken = Guid.NewGuid().ToString();
                string tokenHash = ComputeHmac(rawToken);
                DateTime expiracion = DateTime.Now.AddMinutes(minutosExpiracion);

                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return null;

                    // Usar usuario_id en lugar de usuario_correo
                    string query = @"INSERT INTO sesiones (token, usuario_id, fecha_expiracion) 
                           VALUES (@token, @usuario_id, @expiracion)";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        // Guardamos el hash del token en BD, no el token en claro
                        cmd.Parameters.AddWithValue("@token", tokenHash);
                        cmd.Parameters.AddWithValue("@usuario_id", usuario.id);
                        cmd.Parameters.AddWithValue("@expiracion", expiracion);

                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine($"✅ Sesión BD creada - Token (raw): {rawToken}");
                // Devolver el token en claro al cliente; en BD sólo queda el hash
                return rawToken;
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

                    string tokenHash = ComputeHmac(token);

                    string query = @"SELECT COUNT(*) FROM sesiones 
                                   WHERE token = @token AND activa = 1 
                                   AND fecha_expiracion > NOW()";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@token", tokenHash);
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

                    string tokenHash = ComputeHmac(token);

                    string query = "UPDATE sesiones SET activa = 0 WHERE token = @token";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@token", tokenHash);
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

        // Obtener correo del usuario desde token (token en claro proporcionado por cliente)
        public static string ObtenerCorreoDesdeToken(string token)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return null;

                    string tokenHash = ComputeHmac(token);

                    // Unir con la tabla usuarios para obtener el correo
                    string query = @"SELECT u.correo FROM sesiones s 
                                   JOIN usuarios u ON s.usuario_id = u.id
                                   WHERE s.token = @token AND s.activa = 1 
                                   AND s.fecha_expiracion > NOW()";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@token", tokenHash);
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

        // Helper: calcular HMAC-SHA256 del token con la clave secreta
        private static string ComputeHmac(string token)
        {
            if (string.IsNullOrEmpty(token)) return string.Empty;

            using (var hmac = new HMACSHA256(SecretKey))
            {
                byte[] data = Encoding.UTF8.GetBytes(token);
                byte[] hash = hmac.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
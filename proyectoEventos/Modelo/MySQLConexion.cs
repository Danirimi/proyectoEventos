using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace proyectoEventos.Modelo
{
    public static class MySQLConexion
    {
        // Cadena de conexión - ajusta según tu configuración
        private static readonly string connectionString = 
            "server=localhost;port=3306;database=eventos;user=root;password=admin;";

        public static MySqlConnection ObtenerConexion()
        {
            try
            {
                var conexion = new MySqlConnection(connectionString);
                conexion.Open();
                return conexion;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}", 
                    "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool ProbarConexion()
        {
            try
            {
                using (var conexion = ObtenerConexion())
                {
                    return conexion != null && conexion.State == System.Data.ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows.Forms;
using proyectoEventos.Modelo;

namespace proyectoEventos.vista
{
    public partial class VerHistorial : Form
    {
        public VerHistorial()
        {
            InitializeComponent();
        }

        private void VerHistorial_Load(object sender, EventArgs e)
        {
            richTextBox1.Font = new System.Drawing.Font("Consolas", 10);
        }

        private void button1_Click(object sender, EventArgs e) // Botón "Generar"
        {
            string usuario = txtUsuario.Text.Trim();

            if (string.IsNullOrEmpty(usuario))
            {
                MessageBox.Show("Ingrese un usuario");
                return;
            }

            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = @"SELECT t.id, t.fechacompra, t.precio, t.cantidadentradas, 
                                    e.nombreevento 
                                    FROM tickets t
                                    INNER JOIN eventos e ON t.eventoid = e.id
                                    INNER JOIN usuarios u ON t.usuarioid = u.id
                                    WHERE u.nombre = @usuario
                                    ORDER BY t.fechacompra DESC";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);

                        using (var reader = cmd.ExecuteReader())
                        {
                            richTextBox1.Text = $"Historial de: {usuario}\n\n";
                            bool encontrado = false;

                            while (reader.Read())
                            {
                                encontrado = true;
                                richTextBox1.Text += $"ID: {reader.GetInt32("id")}\n";
                                richTextBox1.Text += $"Evento: {reader.GetString("nombreevento")}\n";
                                richTextBox1.Text += $"Fecha: {reader.GetDateTime("fechacompra"):yyyy-MM-dd HH:mm:ss}\n";
                                richTextBox1.Text += $"Precio: ${reader.GetDecimal("precio")}\n";
                                richTextBox1.Text += $"Cantidad: {reader.GetInt32("cantidadentradas")}\n";
                                richTextBox1.Text += "------------------------\n";
                            }

                            if (!encontrado)
                            {
                                richTextBox1.Text = $"No se encontraron tickets para el usuario: {usuario}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e) // Botón "Descargar"
        {
            string idTexto = txtId.Text.Trim();

            if (string.IsNullOrEmpty(idTexto))
            {
                MessageBox.Show("Ingrese un ID de ticket para descargar");
                return;
            }

            if (!int.TryParse(idTexto, out int idTicket))
            {
                MessageBox.Show("El ID debe ser un número válido");
                return;
            }

            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = @"SELECT t.id, t.fechacompra, t.precio, t.cantidadentradas, 
                                    e.nombreevento, u.nombre as nombreusuario
                                    FROM tickets t
                                    INNER JOIN eventos e ON t.eventoid = e.id
                                    INNER JOIN usuarios u ON t.usuarioid = u.id
                                    WHERE t.id = @id";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", idTicket);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string infoTicket = "";
                                infoTicket += "======= Ticket =======\n";
                                infoTicket += $"ID: {reader.GetInt32("id")}\n";
                                infoTicket += $"Usuario: {reader.GetString("nombreusuario")}\n";
                                infoTicket += $"Evento: {reader.GetString("nombreevento")}\n";
                                infoTicket += $"Fecha: {reader.GetDateTime("fechacompra"):yyyy-MM-dd HH:mm:ss}\n";
                                infoTicket += $"Precio: ${reader.GetDecimal("precio")}\n";
                                infoTicket += $"Cantidad: {reader.GetInt32("cantidadentradas")}\n";
                                infoTicket += "======================\n";

                                string carpetaDescargas = Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                                string rutaArchivo = Path.Combine(carpetaDescargas, $"Ticket_{idTicket}.txt");
                                File.WriteAllText(rutaArchivo, infoTicket);

                                MessageBox.Show($"Ticket guardado correctamente en tu carpeta de Descargas:\n{rutaArchivo}",
                                    "Descarga exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show($"No se encontró ningún ticket con el ID {idTicket}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar el ticket: {ex.Message}");
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
        private void txtUsuario_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
    }
}
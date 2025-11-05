using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

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
            // Configuración básica del RichTextBox
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
                // Cargar tickets desde JSON
                string rutaJSON = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modelo", "Datos", "tickets.json");
                string json = File.ReadAllText(rutaJSON);
                var tickets = JsonSerializer.Deserialize<JsonElement>(json);

                richTextBox1.Text = $"Historial de: {usuario}\n\n";

                bool encontrado = false;

                foreach (var ticket in tickets.EnumerateArray())
                {
                    if (ticket.GetProperty("NombreUsuario").GetString() == usuario)
                    {
                        encontrado = true;
                        richTextBox1.Text += $"ID: {ticket.GetProperty("id").GetInt32()}\n";
                        richTextBox1.Text += $"Evento: {ticket.GetProperty("EventoN").GetString()}\n";
                        richTextBox1.Text += $"Fecha: {ticket.GetProperty("FechaCompra").GetString()}\n";
                        richTextBox1.Text += $"Precio: ${ticket.GetProperty("Precio").GetDouble()}\n";
                        richTextBox1.Text += $"Cantidad: {ticket.GetProperty("CantidadEntradas").GetInt32()}\n";
                        richTextBox1.Text += "------------------------\n";
                    }
                }

                if (!encontrado)
                {
                    richTextBox1.Text = $"No se encontraron tickets para el usuario: {usuario}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Estos métodos deben existir aunque estén vacíos
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
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
                // Ruta del archivo JSON (igual que en el botón Generar)
                string rutaJSON = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modelo", "Datos", "tickets.json");

                if (!File.Exists(rutaJSON))
                {
                    MessageBox.Show("No se encontró el archivo tickets.json");
                    return;
                }

                // Leer y deserializar JSON
                string json = File.ReadAllText(rutaJSON);
                var tickets = JsonSerializer.Deserialize<JsonElement>(json);

                bool encontrado = false;
                string infoTicket = "";

                // Buscar ticket por ID
                foreach (var ticket in tickets.EnumerateArray())
                {
                    if (ticket.GetProperty("id").GetInt32() == idTicket)
                    {
                        encontrado = true;
                        infoTicket += "======= Ticket =======\n";
                        infoTicket += $"ID: {ticket.GetProperty("id").GetInt32()}\n";
                        infoTicket += $"Usuario: {ticket.GetProperty("NombreUsuario").GetString()}\n";
                        infoTicket += $"Evento: {ticket.GetProperty("EventoN").GetString()}\n";
                        infoTicket += $"Fecha: {ticket.GetProperty("FechaCompra").GetString()}\n";
                        infoTicket += $"Precio: ${ticket.GetProperty("Precio").GetDouble()}\n";
                        infoTicket += $"Cantidad: {ticket.GetProperty("CantidadEntradas").GetInt32()}\n";
                        infoTicket += "======================\n";
                        break;
                    }
                }

                if (!encontrado)
                {
                    MessageBox.Show($"No se encontró ningún ticket con el ID {idTicket}");
                    return;
                }

                // Obtener la ruta de la carpeta "Descargas" del usuario
                string carpetaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                // Crear el archivo en Descargas
                string rutaArchivo = Path.Combine(carpetaDescargas, $"Ticket_{idTicket}.txt");
                File.WriteAllText(rutaArchivo, infoTicket);

                MessageBox.Show($"Ticket guardado correctamente en tu carpeta de Descargas:\n{rutaArchivo}",
                                "Descarga exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar el ticket: {ex.Message}");
            }
        }
    }
}
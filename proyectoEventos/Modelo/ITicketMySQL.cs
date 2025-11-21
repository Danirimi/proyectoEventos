using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace proyectoEventos.Modelo
{
    public class ITicketMySQL : ITicket
    {
        public void GenerarTicket(Ticket ticket)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = @"INSERT INTO tickets 
                                   (eventoid, usuarioid, fechacompra, cantidadentradas, precio, metodopago) 
                                   VALUES (@eventoid, @usuarioid, @fechacompra, @cantidad, @precio, @metodopago)";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@eventoid", ticket.EventoId);
                        cmd.Parameters.AddWithValue("@usuarioid", ticket.UsuarioC);
                        cmd.Parameters.AddWithValue("@fechacompra", ticket.FechaCompra);
                        cmd.Parameters.AddWithValue("@cantidad", ticket.CantidadEntradas);
                        cmd.Parameters.AddWithValue("@precio", ticket.Precio);
                        cmd.Parameters.AddWithValue("@metodopago", ticket.MetodoPago);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Ticket generado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar ticket: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<Ticket> ObtenerTicketsPorUsuario(int usuarioId)
        {
            var tickets = new List<Ticket>();
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return tickets;

                    string query = @"SELECT t.id, t.eventoid, t.usuarioid, t.fechacompra, t.cantidadentradas, 
                                    t.precio, t.metodopago, e.nombreevento, e.lugarevento, u.nombre as nombreusuario
                                    FROM tickets t
                                    INNER JOIN eventos e ON t.eventoid = e.id
                                    INNER JOIN usuarios u ON t.usuarioid = u.id
                                    WHERE t.usuarioid = @usuarioid
                                    ORDER BY t.fechacompra DESC";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@usuarioid", usuarioId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var ticket = new Ticket(
                                    reader.GetInt32("eventoid"),
                                    reader.GetString("nombreevento"),
                                    reader.GetInt32("usuarioid"),
                                    reader.GetString("nombreusuario"),
                                    reader.GetDateTime("fechacompra"),
                                    reader.GetDecimal("precio"),
                                    reader.GetString("lugarevento"),
                                    reader.GetInt32("cantidadentradas"),
                                    reader.GetString("metodopago")
                                );
                                ticket.id = reader.GetInt32("id");
                                tickets.Add(ticket);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener tickets: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tickets;
        }

        public List<Ticket> ObtenerTodosLosTickets()
        {
            var tickets = new List<Ticket>();
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return tickets;

                    string query = @"SELECT t.id, t.eventoid, t.usuarioid, t.fechacompra, t.cantidadentradas, 
                                    t.precio, t.metodopago, e.nombreevento, e.lugarevento, u.nombre as nombreusuario
                                    FROM tickets t
                                    INNER JOIN eventos e ON t.eventoid = e.id
                                    INNER JOIN usuarios u ON t.usuarioid = u.id
                                    ORDER BY t.fechacompra DESC";

                    using (var cmd = new MySqlCommand(query, conexion))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ticket = new Ticket(
                                reader.GetInt32("eventoid"),
                                reader.GetString("nombreevento"),
                                reader.GetInt32("usuarioid"),
                                reader.GetString("nombreusuario"),
                                reader.GetDateTime("fechacompra"),
                                reader.GetDecimal("precio"),
                                reader.GetString("lugarevento"),
                                reader.GetInt32("cantidadentradas"),
                                reader.GetString("metodopago")
                            );
                            ticket.id = reader.GetInt32("id");
                            tickets.Add(ticket);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener todos los tickets: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tickets;
        }

        public Ticket BuscarTicketPorId(int id)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return null;

                    string query = @"SELECT t.id, t.eventoid, t.usuarioid, t.fechacompra, t.cantidadentradas, 
                                    t.precio, t.metodopago, e.nombreevento, e.lugarevento, u.nombre as nombreusuario
                                    FROM tickets t
                                    INNER JOIN eventos e ON t.eventoid = e.id
                                    INNER JOIN usuarios u ON t.usuarioid = u.id
                                    WHERE t.id = @id";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var ticket = new Ticket(
                                    reader.GetInt32("eventoid"),
                                    reader.GetString("nombreevento"),
                                    reader.GetInt32("usuarioid"),
                                    reader.GetString("nombreusuario"),
                                    reader.GetDateTime("fechacompra"),
                                    reader.GetDecimal("precio"),
                                    reader.GetString("lugarevento"),
                                    reader.GetInt32("cantidadentradas"),
                                    reader.GetString("metodopago")
                                );
                                ticket.id = reader.GetInt32("id");
                                return ticket;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar ticket: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public void EliminarTicket(int id)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = "DELETE FROM tickets WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Ticket eliminado exitosamente", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"No se encontró ningún ticket con ID = {id}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar ticket: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
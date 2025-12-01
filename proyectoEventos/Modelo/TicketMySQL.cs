using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using proyectoEventos.Util;

namespace proyectoEventos.Modelo
{
    public class TicketMySQL : ITicket
    {
        public void GenerarTicket(Ticket ticket)
        {
            if (ticket == null) return;
            using (var conn = MySQLConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo obtener conexión con la BD.", "Error BD",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    string sql = @"INSERT INTO tickets
                                   (eventoid, usuarioid, fechacompra, cantidadentradas, precio, metodopago)
                                   VALUES (@EventoId, @UsuarioC, @FechaCompra, @CantidadEntradas, @Precio, @MetodoPago);";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventoId", ticket.EventoId);
                        cmd.Parameters.AddWithValue("@UsuarioC", ticket.UsuarioC);
                        cmd.Parameters.AddWithValue("@FechaCompra", ticket.FechaCompra == default(DateTime) ? DateTime.Now : ticket.FechaCompra);
                        cmd.Parameters.AddWithValue("@CantidadEntradas", ticket.CantidadEntradas);
                        cmd.Parameters.AddWithValue("@Precio", ticket.Precio);

                        // APLICAR ENCODING XSS antes de almacenar
                        var metodoEncoded = EncoderHelper.EncodeForHtml(ticket.MetodoPago ?? string.Empty);
                        cmd.Parameters.AddWithValue("@MetodoPago", metodoEncoded);

                        int afectados = cmd.ExecuteNonQuery();

                        if (afectados == 0)
                        {
                            MessageBox.Show("La inserción no afectó filas. Verifique constraints y datos.", "Advertencia BD",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            using (var idCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
                            {
                                var result = idCmd.ExecuteScalar();
                                if (result != null && int.TryParse(result.ToString(), out int id))
                                {
                                    ticket.id = id;
                                }
                            }
                        }
                    }
                }
                catch (MySqlException mex)
                {
                    MessageBox.Show($"Error MySQL al guardar ticket: {mex.Message}\nCódigo: {mex.Number}", "Error BD",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al guardar ticket en BD: {ex.Message}\n{ex}", "Error BD",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private bool ColumnExists(MySqlDataReader rdr, string columnName)
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                if (string.Equals(rdr.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public List<Ticket> ObtenerTicketsPorUsuario(int usuarioId)
        {
            var lista = new List<Ticket>();
            using (var conn = MySQLConexion.ObtenerConexion())
            {
                if (conn == null) return lista;
                try
                {
                    string sql = "SELECT id, eventoid, usuarioid, fechacompra, cantidadentradas, precio, metodopago FROM tickets WHERE usuarioid = @UsuarioC";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UsuarioC", usuarioId);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var t = new Ticket();
                                t.id = rdr.GetInt32("id");
                                t.EventoId = rdr.GetInt32("eventoid");
                                t.UsuarioC = rdr.GetInt32("usuarioid");
                                t.FechaCompra = rdr.GetDateTime("fechacompra");
                                t.CantidadEntradas = rdr.GetInt32("cantidadentradas");
                                t.Precio = rdr.GetDecimal("precio");

                                if (ColumnExists(rdr, "metodopago") && !rdr.IsDBNull(rdr.GetOrdinal("metodopago")))
                                {
                                    var stored = rdr.GetString("metodopago");
                                    // Decodificar al devolver para que la aplicación trabaje con el valor original
                                    t.MetodoPago = EncoderHelper.DecodeFromHtml(stored);
                                }
                                else
                                {
                                    t.MetodoPago = null;
                                }

                                lista.Add(t);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al leer tickets: {ex.Message}", "Error BD",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return lista;
        }

        public List<Ticket> ObtenerTodosLosTickets()
        {
            var lista = new List<Ticket>();
            using (var conn = MySQLConexion.ObtenerConexion())
            {
                if (conn == null) return lista;
                try
                {
                    string sql = "SELECT id, eventoid, usuarioid, fechacompra, cantidadentradas, precio, metodopago FROM tickets";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var t = new Ticket();
                                t.id = rdr.GetInt32("id");
                                t.EventoId = rdr.GetInt32("eventoid");
                                t.UsuarioC = rdr.GetInt32("usuarioid");
                                t.FechaCompra = rdr.GetDateTime("fechacompra");
                                t.CantidadEntradas = rdr.GetInt32("cantidadentradas");
                                t.Precio = rdr.GetDecimal("precio");

                                if (ColumnExists(rdr, "metodopago") && !rdr.IsDBNull(rdr.GetOrdinal("metodopago")))
                                {
                                    var stored = rdr.GetString("metodopago");
                                    t.MetodoPago = EncoderHelper.DecodeFromHtml(stored);
                                }
                                else
                                {
                                    t.MetodoPago = null;
                                }

                                lista.Add(t);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al leer tickets: {ex.Message}", "Error BD",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return lista;
        }

        public Ticket BuscarTicketPorId(int id)
        {
            using (var conn = MySQLConexion.ObtenerConexion())
            {
                if (conn == null) return null;
                try
                {
                    string sql = "SELECT id, eventoid, usuarioid, fechacompra, cantidadentradas, precio, metodopago FROM tickets WHERE id = @id LIMIT 1";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                var t = new Ticket();
                                t.id = rdr.GetInt32("id");
                                t.EventoId = rdr.GetInt32("eventoid");
                                t.UsuarioC = rdr.GetInt32("usuarioid");
                                t.FechaCompra = rdr.GetDateTime("fechacompra");
                                t.CantidadEntradas = rdr.GetInt32("cantidadentradas");
                                t.Precio = rdr.GetDecimal("precio");

                                if (ColumnExists(rdr, "metodopago") && !rdr.IsDBNull(rdr.GetOrdinal("metodopago")))
                                {
                                    var stored = rdr.GetString("metodopago");
                                    t.MetodoPago = EncoderHelper.DecodeFromHtml(stored);
                                }
                                else
                                {
                                    t.MetodoPago = null;
                                }

                                return t;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al buscar ticket: {ex.Message}", "Error BD",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return null;
        }

        public void EliminarTicket(int id)
        {
            using (var conn = MySQLConexion.ObtenerConexion())
            {
                if (conn == null) return;
                try
                {
                    string sql = "DELETE FROM tickets WHERE id = @id";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar ticket: {ex.Message}", "Error BD",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace proyectoEventos.Modelo
{
    public class IEventoMySQL : InterfaceEvento
    {
        public void agregarEvento(Evento evento)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = @"INSERT INTO eventos 
                                   (nombreevento, fechaevento, lugarevento, descripcionevento, 
                                    entradastotales, entradasdisponibles, precioentrada) 
                                   VALUES (@nombre, @fecha, @lugar, @descripcion, @totales, @disponibles, @precio)";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", evento.NombreEvento);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.Parse(evento.FechaEvento));
                        cmd.Parameters.AddWithValue("@lugar", evento.LugarEvento);
                        cmd.Parameters.AddWithValue("@descripcion", evento.DescripcionEvento);
                        cmd.Parameters.AddWithValue("@totales", evento.entradastotales);
                        cmd.Parameters.AddWithValue("@disponibles", evento.entradasdisponibles);
                        cmd.Parameters.AddWithValue("@precio", evento.PrecioEntrada);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Evento agregado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar evento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void actualizarEvento(Evento evento)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = @"UPDATE eventos 
                                   SET nombreevento = @nombre, fechaevento = @fecha, lugarevento = @lugar, 
                                       descripcionevento = @descripcion, entradastotales = @totales, 
                                       entradasdisponibles = @disponibles, precioentrada = @precio 
                                   WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", evento.Id);
                        cmd.Parameters.AddWithValue("@nombre", evento.NombreEvento);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.Parse(evento.FechaEvento));
                        cmd.Parameters.AddWithValue("@lugar", evento.LugarEvento);
                        cmd.Parameters.AddWithValue("@descripcion", evento.DescripcionEvento);
                        cmd.Parameters.AddWithValue("@totales", evento.entradastotales);
                        cmd.Parameters.AddWithValue("@disponibles", evento.entradasdisponibles);
                        cmd.Parameters.AddWithValue("@precio", evento.PrecioEntrada);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Evento actualizado exitosamente", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"No se encontró ningún evento con Id = {evento.Id}.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar evento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Evento buscarEventoId(int id)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return null;

                    string query = @"SELECT id, nombreevento, fechaevento, lugarevento, descripcionevento, 
                                    entradastotales, entradasdisponibles, precioentrada 
                                    FROM eventos WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Evento(
                                    reader.GetInt32("id"),
                                    reader.GetString("nombreevento"),
                                    reader.GetDateTime("fechaevento").ToString("yyyy-MM-dd HH:mm:ss"),
                                    reader.GetString("lugarevento"),
                                    reader.GetString("descripcionevento"),
                                    reader.GetInt32("entradastotales"),
                                    reader.GetInt32("entradasdisponibles"),
                                    reader.GetDecimal("precioentrada")
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar evento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public void eliminarEvento(int id)
        {
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return;

                    string query = "DELETE FROM eventos WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int filasAfectadas = cmd.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Evento eliminado exitosamente", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"No se encontró ningún evento con Id = {id}.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar evento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public IEnumerable<Evento> mostrarEventos()
        {
            var eventos = new List<Evento>();
            try
            {
                using (var conexion = MySQLConexion.ObtenerConexion())
                {
                    if (conexion == null) return eventos;

                    string query = @"SELECT id, nombreevento, fechaevento, lugarevento, descripcionevento, 
                                    entradastotales, entradasdisponibles, precioentrada 
                                    FROM eventos ORDER BY fechaevento";

                    using (var cmd = new MySqlCommand(query, conexion))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            eventos.Add(new Evento(
                                reader.GetInt32("id"),
                                reader.GetString("nombreevento"),
                                reader.GetDateTime("fechaevento").ToString("yyyy-MM-dd HH:mm:ss"),
                                reader.GetString("lugarevento"),
                                reader.GetString("descripcionevento"),
                                reader.GetInt32("entradastotales"),
                                reader.GetInt32("entradasdisponibles"),
                                reader.GetDecimal("precioentrada")
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener eventos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return eventos;
        }
    }
}
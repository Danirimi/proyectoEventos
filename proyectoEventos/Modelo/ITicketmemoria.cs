using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoEventos.Modelo
{
    public class ITicketmemoria : ITicket
    {
        private readonly List<Ticket> _tickets = new List<Ticket>();
        private int _siguienteId = 1;
        private readonly string _nombreArchivoJson = "tickets.json";

        public ITicketmemoria()
        {
            // Cargar tickets desde el archivo JSON
            _tickets = JsonDataManager.CargarDatos<Ticket>(_nombreArchivoJson);

            // Calcular el siguiente ID basado en los tickets existentes
            _siguienteId = _tickets.Any() ? _tickets.Max(t => t.id) + 1 : 1;
        }

        public void GenerarTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                MessageBox.Show("El ticket no puede ser nulo.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Asignar ID automáticamente si no tiene
            if (ticket.id == 0)
            {
                ticket.id = _siguienteId++;
            }

            _tickets.Add(ticket);

            // Guardar en JSON
            JsonDataManager.GuardarDatos(_tickets, _nombreArchivoJson);

            MessageBox.Show("Ticket generado exitosamente", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public List<Ticket> ObtenerTicketsPorUsuario(int usuarioId)
        {
            return _tickets.Where(t => t.UsuarioC == usuarioId).ToList();
        }

        public List<Ticket> ObtenerTodosLosTickets()
        {
            return _tickets;
        }

        public Ticket BuscarTicketPorId(int id)
        {
            return _tickets.FirstOrDefault(t => t.id == id);
        }

        public void EliminarTicket(int id)
        {
            var ticket = _tickets.FirstOrDefault(t => t.id == id);
            if (ticket != null)
            {
                _tickets.Remove(ticket);
                JsonDataManager.GuardarDatos(_tickets, _nombreArchivoJson);

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
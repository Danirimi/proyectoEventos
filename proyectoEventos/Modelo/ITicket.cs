using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    public interface ITicket
    {
        void GenerarTicket(Ticket ticket);
        List<Ticket> ObtenerTicketsPorUsuario(int usuarioId);
        List<Ticket> ObtenerTodosLosTickets();
        Ticket BuscarTicketPorId(int id);
        void EliminarTicket(int id);
    }
}
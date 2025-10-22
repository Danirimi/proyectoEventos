using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    public class ITicketmemoria: ITicket
    {
        private readonly List<Ticket> _tickets = new List<Ticket>();
        public void generarTicket(Ticket ticket)
        {
            _tickets.Add(ticket);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    public class Ticket
    {
        private static int contadorIds = 0;
        public int id { get; private set; }
        public string EventoN { get; set; }             
        public int UsuarioC { get; set; }

        public DateTime FechaCompra { get; set; }

        public decimal Precio { get; set; }

       

        public string EventoL { get; set; }

        // Constructor
        public Ticket(int id, string eventoN, int usuarioC, DateTime fechaCompra, decimal precio,  string eventoL)
        {
            contadorIds++;
            this.id = contadorIds;
            this.EventoN = eventoN;
            this.UsuarioC = usuarioC;
            this.FechaCompra = fechaCompra;
            this.Precio = precio;
            this.EventoL = eventoL;
        }

        // Método para mostrar información del ticket
        public override string ToString()
        {
            return
                "================= DETALLES DEL TICKET =================\n" +
            $"ID Ticket: {id}\n" +
            $"Nombre del Evento: {EventoN}\n" +
            $"ID del Usuario: {UsuarioC}\n" +
            $"Fecha de Compra: {FechaCompra:dd/MM/yyyy HH:mm:ss}\n" +
            $"Precio: {Precio:C2}\n" +
            $"Lugar del Evento: {EventoL}\n" +
            "========================================================";
        }
    }
}

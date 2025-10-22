using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos
{
    internal class Ticket
    {
        public int id { get; set; }
        public int EventoId { get; set; }             
        public int UsuarioId { get; set; }

        public DateTime FechaCompra { get; set; }

        public decimal Precio { get; set; }

        public bool Disponible { get; set; }

        // Constructor
        public Ticket(int id, int eventoId, int usuarioId, DateTime fechaCompra, decimal precio, bool disponible)
        {
            this.id = id;
            this.EventoId = eventoId;
            this.UsuarioId = usuarioId;
            this.FechaCompra = fechaCompra;
            this.Precio = precio;
            this.Disponible = disponible;
        }

        // Método para mostrar información del ticket
        public override string ToString()
        {
            return
                "================= DETALLES DE LA COMPRA =================\n" +
                $"ID Interno: {id}\n" +
                $"ID del Evento: {EventoId}\n" +
                $"ID del Usuario: {UsuarioId}\n" +
                $"Fecha de Compra: {FechaCompra:dd/MM/yyyy HH:mm:ss}\n" +
                $"Precio: {Precio:C2}\n" +
                $"Disponible: {(Disponible ? "Sí" : "No")}\n" +
                "==========================================================";
        }
    }
}

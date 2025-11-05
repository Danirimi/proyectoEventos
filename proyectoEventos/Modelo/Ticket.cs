using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    public class Ticket
    {
        public int id { get; set; }
        public string EventoN { get; set; }
        public int UsuarioC { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal Precio { get; set; }
        public string EventoL { get; set; }
        public int CantidadEntradas { get; set; }
        public int EventoId { get; set; }
        public string MetodoPago { get; set; } // "Efectivo" o "Tarjeta"

        // Constructor sin parámetros para JSON
        public Ticket()
        {
        }

        // Constructor completo
        public Ticket(int eventoId, string eventoN, int usuarioC, string nombreUsuario,
                     DateTime fechaCompra, decimal precio, string eventoL, int cantidadEntradas, string metodoPago)
        {
            EventoId = eventoId;
            EventoN = eventoN;
            UsuarioC = usuarioC;
            NombreUsuario = nombreUsuario;
            FechaCompra = fechaCompra;
            Precio = precio;
            EventoL = eventoL;
            CantidadEntradas = cantidadEntradas;
            MetodoPago = metodoPago;
        }

        // Método para mostrar información del ticket
        public override string ToString()
        {
            return
                "================= DETALLES DEL TICKET =================\n" +
                $"ID Ticket: {id}\n" +
                $"Evento: {EventoN}\n" +
                $"Lugar: {EventoL}\n" +
                $"ID Usuario: {UsuarioC}\n" +
                $"Nombre Usuario: {NombreUsuario}\n" +
                $"Fecha de Compra: {FechaCompra:dd/MM/yyyy HH:mm:ss}\n" +
                $"Cantidad de Entradas: {CantidadEntradas}\n" +
                $"Precio Total: {Precio:C2}\n" +
                $"Método de Pago: {MetodoPago}\n" +
                "========================================================";
        }
    }
}
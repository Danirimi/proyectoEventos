using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proyectoEventos.Modelo; // <-- Asegúrate de que esta línea esté presente

namespace proyectoEventos.vista
{
    // EventArgs personalizados para manejar eventos de la aplicación
    public class EventoEventArgs : EventArgs
    {
        public int Id { get; }
        public string NombreEvento { get; }
        public string FechaEvento { get; }
        public string LugarEvento { get; }
        public string DescripcionEvento { get; }
        public int EntradasTotales { get; }
        public int EntradasDisponibles { get; }
        public decimal PrecioEntrada { get; }

        public EventoEventArgs(int id, string nombreEvento, string fechaEvento, string lugarEvento, 
            string descripcionEvento, int entradasTotales, int entradasDisponibles, decimal precioEntrada)
        {
            Id = id;
            NombreEvento = nombreEvento;
            FechaEvento = fechaEvento;
            LugarEvento = lugarEvento;
            DescripcionEvento = descripcionEvento;
            EntradasTotales = entradasTotales;
            EntradasDisponibles = entradasDisponibles;
            PrecioEntrada = precioEntrada;
        }
    }

    public class EventoSeleccionadoArgs : EventArgs
    {
        public Evento EventoSeleccionado { get; }

        public EventoSeleccionadoArgs(Evento evento)
        {
            EventoSeleccionado = evento;
        }
    }

    public class CompraEventoArgs : EventArgs
    {
        public Evento Evento { get; }
        public int Cantidad { get; }
        public Usuario Usuario { get; }
        public string MetodoPago { get; }

        public CompraEventoArgs(Evento evento, int cantidad, Usuario usuario, string metodoPago)
        {
            Evento = evento;
            Cantidad = cantidad;
            Usuario = usuario;
            MetodoPago = metodoPago;
        }
    }
}

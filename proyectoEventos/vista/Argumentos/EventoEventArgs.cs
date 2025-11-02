using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public EventoEventArgs(int id, string nombreEvento, string fechaEvento, string lugarEvento, 
            string descripcionEvento, int entradasTotales, int entradasDisponibles)
        {
            Id = id;
            NombreEvento = nombreEvento;
            FechaEvento = fechaEvento;
            LugarEvento = lugarEvento;
            DescripcionEvento = descripcionEvento;
            EntradasTotales = entradasTotales;
            EntradasDisponibles = entradasDisponibles;
        }
    }

    public class EventoSeleccionadoArgs : EventArgs
    {
        public Modelo.Evento EventoSeleccionado { get; }

        public EventoSeleccionadoArgs(Modelo.Evento evento)
        {
            EventoSeleccionado = evento;
        }
    }

    public class CompraEventoArgs : EventArgs
    {
        public Modelo.Evento Evento { get; }
        public int Cantidad { get; }
        public Modelo.Usuario Usuario { get; }

        public CompraEventoArgs(Modelo.Evento evento, int cantidad, Modelo.Usuario usuario)
        {
            Evento = evento;
            Cantidad = cantidad;
            Usuario = usuario;
        }
    }
}

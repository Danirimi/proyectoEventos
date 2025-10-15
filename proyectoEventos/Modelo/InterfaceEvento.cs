using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    internal interface InterfaceEvento
    {
        IEnumerable<Evento> mostrarEventos();

        Evento buscarEventoId(int id);

        void agregarEvento(Evento evento);

        void actualizarEvento(Evento evento);

        void eliminarEvento(int id);


    }
}

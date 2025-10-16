using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proyectoEventos.Modelo;

namespace proyectoEventos.vista
{
    internal interface IEventoVista
    {

        event EventHandler AgregarEvento;
        event EventHandler BuscarEvento;
        event EventHandler editarEvento;
        event EventHandler eliminarEvento;

        //lectrua de datos
        Evento AbrirEdicion();

        void notify(string message);
    }
}

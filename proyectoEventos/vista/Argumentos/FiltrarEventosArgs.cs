using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.vista.Argumentos
{
    public class FiltrarEventosArgs: EventArgs

    {
        public string Nombre { get; set; }
        public string Fecha { get; set; }
        public string Lugar { get; set; }

        public FiltrarEventosArgs(string nombre, string fecha, string lugar)
        {
            Nombre = nombre;
            Fecha = fecha;
            Lugar = lugar;
        }

    }
}

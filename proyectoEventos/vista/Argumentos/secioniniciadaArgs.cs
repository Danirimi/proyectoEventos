using proyectoEventos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.vista.Argumentos
{
    public class secioniniciadaArgs : EventArgs
    {
        public Usuario Usuario { get; }

        public secioniniciadaArgs(Usuario usuario)
        {
            Usuario = usuario;
        }
    }
}

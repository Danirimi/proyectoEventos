using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.vista
{
    public class ArgumentoIniciarSesion : EventArgs
    {
        public string Correo { get; }
        public string Contrasena { get; }
        public ArgumentoIniciarSesion(string correo, string contrasena)
        {
            Correo = correo;
            Contrasena = contrasena;
        }
    }
}

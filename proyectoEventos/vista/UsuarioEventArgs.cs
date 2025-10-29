using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.vista
{
    public class UsuarioEventArgs : EventArgs   
    {
        public string Nombre { get; }
        public int Cedula { get; }
        public int Edad { get; }
        public string Correo { get; }
        public string Contrasena { get; }
        public bool Esadmin { get; }
        public UsuarioEventArgs(string nombre, string correo, int cedula, int edad, string contrasena, bool esadmin)
        {
            Nombre = nombre;
            Correo = correo;
            Cedula = cedula;
            Edad = edad;
            Contrasena = contrasena;
            Esadmin = esadmin;
        }
    }
}

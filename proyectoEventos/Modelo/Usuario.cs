using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Cedula { get; set; }
        public int Edad { get; set; }
        public string Contrasena { get; set; }

        public bool esadmin { get; set; }

        public Usuario(string Nombre, string Correo, string Cedula, int Edad,string Contrasena, bool Esadmin) 
        { 
            this.Nombre = Nombre;
            this.Correo = Correo;
            this.Cedula = Cedula;
            this.Edad = Edad;
            this.Contrasena = Contrasena;
            this.esadmin = Esadmin;

        }

        public override string ToString()
        {
            return
                "================= DATOS DEL USUARIO =================\n" +
                $"Nombre: {Nombre}\n" +
                $"Correo: {Correo}\n" +
                $"Cédula: {Cedula}\n" +
                $"Edad: {Edad} años\n" +
                 $"Contraseña: {Contrasena}\n" +
                  $"Es admin?: {esadmin}\n" +
                "======================================================";
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.vista.Argumentos
{
    public class ArgumentosContraseña
    {
        public string Correo { get; }
     

        public string Contraseña { get; set; }
        public ArgumentosContraseña(string correo, string contraseña)
        {
            Correo = correo;
          
            Contraseña = contraseña;
        }
    }
}

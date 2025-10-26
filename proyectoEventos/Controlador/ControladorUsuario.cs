using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proyectoEventos.Modelo;
using proyectoEventos.vista;

namespace proyectoEventos.Controlador
{
    internal class ControladorUsuario
    {   private readonly IUsuarioMemoria usuarioMemoria;
        public bool crearUsuario(string nombre, string correo, string cedula, int edad, string contrasena, bool esadmin)
        {
            // Lógica para crear un usuario
            Modelo.Usuario nuevoUsuario = new Modelo.Usuario(nombre, correo, cedula, edad, contrasena, esadmin);
            usuarioMemoria.AgregarUsuario(nuevoUsuario);
            

            return true; // Retorna true si la creación fue exitosa
        }
    }
}

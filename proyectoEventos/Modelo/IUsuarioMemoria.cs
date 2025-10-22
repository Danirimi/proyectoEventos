using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    public class IUsuarioMemoria : IUsuario
    {
       private readonly List<Usuario> _usuarios = new List<Usuario>();
        private readonly List<Usuario> _administradores = new List<Usuario>();
 
        public void AgregarUsuario(Usuario usuario)
        {
            if (usuario.esadmin==false)
            {
             _usuarios.Add(usuario);
            }
            else
            {
                _administradores.Add(usuario);
            }
        }
        public void EditarUsuario(Usuario usuario)
        {
            Usuario usuarioExistente;
            if (usuario.esadmin == false)
            {
                usuarioExistente = _usuarios.FirstOrDefault(u => u.Cedula == usuario.Cedula);
                if (usuarioExistente != null)
                {
                    usuarioExistente.Nombre = usuario.Nombre;
                    usuarioExistente.Correo = usuario.Correo;
                    usuarioExistente.Edad = usuario.Edad;
                    usuarioExistente.Contrasena = usuario.Contrasena;
                }
            }
            else
            {
               usuarioExistente = _administradores.FirstOrDefault(u => u.Cedula == usuario.Cedula);
                if (usuarioExistente != null)
                {
                    usuarioExistente.Nombre = usuario.Nombre;
                    usuarioExistente.Correo = usuario.Correo;
                    usuarioExistente.Edad = usuario.Edad;
                    usuarioExistente.Contrasena = usuario.Contrasena;
                }
            }

            
            
        }
        public void EliminarUsuario(string cedula)
        {

            var usuarioAEliminar = _usuarios.FirstOrDefault(u => u.Cedula == cedula);
            if (usuarioAEliminar != null)
            {
                _usuarios.Remove(usuarioAEliminar);
            }
            else
            {
                var adminAEliminar = _administradores.FirstOrDefault(u => u.Cedula == cedula);
                if (adminAEliminar != null)
                {
                    _administradores.Remove(adminAEliminar);
                }
            }
        }
        public void verHistorialcompra(string cedula)
        {
            // Implementación pendiente
            throw new NotImplementedException();
        }
       
        


    }
}

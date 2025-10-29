using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                MessageBox.Show("CONFIRMACION");
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
            Usuario usuarioExistente = _usuarios.FirstOrDefault(u => u.Cedula.ToString() == cedula);
            if (usuarioExistente != null)
            {
                _usuarios.Remove(usuarioExistente);
            }
            else
            {
                Usuario adminExistente = _administradores.FirstOrDefault(u => u.Cedula.ToString() == cedula);
                if (adminExistente != null)
                {
                    _administradores.Remove(adminExistente);
                }
            }
        }
        public void verHistorialcompra(string cedula)
        {
            // Implementación pendiente
            throw new NotImplementedException();
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return _usuarios;
        }




    }
}

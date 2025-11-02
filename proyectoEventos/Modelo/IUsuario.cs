using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    public interface IUsuario
    {
        void AgregarUsuario(Usuario usuario);
        void EditarUsuario(Usuario usuario);
        void EliminarUsuario(string cedula);

        void verHistorialcompra(string cedula);
        bool ValidarUsuarioDirecto(string nombre, string contrasena);

        bool Verificar(string correo, string nombre, string cedula);
        List<Usuario> ObtenerUsuarios();
        
        Usuario ObtenerUsuarioPorCredenciales(string correo, string contrasena);
    }
}

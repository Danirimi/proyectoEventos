using System;

namespace proyectoEventos.Modelo
{
    public class SesionUsuario
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
        public Usuario Usuario { get; set; }

        public bool EstaExpirada()
        {
            return DateTime.Now > Expiracion;
        }

        // Constructor
        public SesionUsuario(string token, Usuario usuario, int minutosExpiracion = 30)
        {
            Token = token;
            Usuario = usuario;
            Expiracion = DateTime.Now.AddMinutes(minutosExpiracion);
        }

        // Constructor vacío para serialización
        public SesionUsuario() { }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

namespace proyectoEventos.Modelo
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Cedula { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }

        public Usuario()
        {
        }

        public Usuario(string nombre, string correo, string cedula, int edad, string genero)
        {
            Nombre = nombre;
            Correo = correo;
            Cedula = cedula;
            Edad = edad;
            Genero = genero;
        }

        // ==== MÉTODOS JSON ====

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Usuario FromJson(string json)
        {
            return JsonSerializer.Deserialize<Usuario>(json);
        }

        public override string ToString()
        {
            return $"Nombre: {Nombre}\n" +
                   $"Correo: {Correo}\n" +
                   $"Cédula: {Cedula}\n" +
                   $"Edad: {Edad}\n" +
                   $"Género: {Genero}";
        }
    }
}
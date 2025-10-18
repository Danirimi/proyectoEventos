using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace proyectoEventos.Modelo
{
    public class Tiquete
    {
        public string CodigoTiquete { get; set; }
        public DateTime FechaCompra { get; set; }
        public string Cedula { get; set; }
        public Usuario UsuarioAsociado { get; set; }
        
        public Tiquete()
        {
        }

        public Tiquete(string codigoTiquete, DateTime fechaCompra, string cedula, Usuario usuario = null)
        {
            CodigoTiquete = codigoTiquete;
            FechaCompra = fechaCompra;
            Cedula = cedula;
            UsuarioAsociado = usuario;
        }

        // ==== MÉTODOS JSON SIMPLES ====
        
        // Convertir este tiquete a JSON (sin options)
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // Crear tiquete desde JSON
        public static Tiquete FromJson(string json)
        {
            return JsonSerializer.Deserialize<Tiquete>(json);
        }


        
        public void EditarCodigo(string nuevoCodigo)
        {
            if (string.IsNullOrWhiteSpace(nuevoCodigo))
            {
                Console.WriteLine(" El nuevo código no puede estar vacío.");
                return;
            }

           
            CodigoTiquete = nuevoCodigo;
    
        }

        public void Eliminar()
        {
            Console.WriteLine($"🗑️ Eliminando tiquete con código: {CodigoTiquete}...");
            CodigoTiquete = null;
            FechaCompra = default(DateTime);
            Cedula = null;
            UsuarioAsociado = null;
            Console.WriteLine("✅ Tiquete eliminado correctamente (datos limpiados).");
        }

        public override string ToString()
        {
            string info = $"Código de Tiquete: {CodigoTiquete}\n" +
                          $"Fecha de Compra: {FechaCompra}\n" +
                          $"Cédula del Usuario: {Cedula}";

            if (UsuarioAsociado != null)
            {
                info += "\n--- Datos del Usuario Asociado ---\n" + UsuarioAsociado.ToString();
            }

            return info;
        }
    }
}




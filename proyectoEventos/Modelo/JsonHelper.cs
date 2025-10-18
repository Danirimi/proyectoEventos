using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace proyectoEventos.Modelo
{
    public static class JsonHelper
    {
        public static void Guardar<T>(List<T> lista, string rutaArchivo)
        {
            string carpeta = Path.GetDirectoryName(rutaArchivo);
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string json = System.Text.Json.JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(rutaArchivo, json);
        }

        public static List<T> Cargar<T>(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
                return new List<T>();

            string json = File.ReadAllText(rutaArchivo);
            List<T> lista = System.Text.Json.JsonSerializer.Deserialize<List<T>>(json);
            return lista ?? new List<T>();
        }

    }

}

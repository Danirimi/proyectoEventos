using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    public static class JsonDataManager
    {
        // Obtiene la ruta absoluta al directorio donde se ejecuta el programa
        private static string baseRuta = AppDomain.CurrentDomain.BaseDirectory;

        // Genera el nombre de archivo según el tipo de dato (Usuario -> usuarios.json)
        private static string ObtenerRuta<T>()
        {
            string nombre = typeof(T).Name.ToLower();
            return Path.Combine(baseRuta, $"{nombre}s.json");


        }

        public static void GuardarDatos<T>(List<T> datos)
        {
            string ruta = ObtenerRuta<T>();
            string json = JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ruta, json);
        }

        // Carga los datos desde el JSON correspondiente
        public static List<T> CargarDatos<T>()
        {
            string ruta = ObtenerRuta<T>();
            if (!File.Exists(ruta))
                return new List<T>();

            string json = File.ReadAllText(ruta);
            return JsonSerializer.Deserialize<List<T>>(json);
        }

    }
}

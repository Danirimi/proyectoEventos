using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace proyectoEventos.Modelo
{
    public static class JsonDataManager
    {
        private static string ObtenerRutaCompleta(string nombreArchivo)
        {
            string carpetaDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modelo", "Datos");
            Directory.CreateDirectory(carpetaDatos); // Garantiza que exista
            return Path.Combine(carpetaDatos, nombreArchivo);
        }

        public static void GuardarDatos<T>(List<T> datos, string nombreArchivo)
        {
            string ruta = ObtenerRutaCompleta(nombreArchivo);
            string json = JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ruta, json);
        }

        public static List<T> CargarDatos<T>(string nombreArchivo)
        {
            string ruta = ObtenerRutaCompleta(nombreArchivo);
            if (!File.Exists(ruta))
                return new List<T>();

            string json = File.ReadAllText(ruta);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }
}

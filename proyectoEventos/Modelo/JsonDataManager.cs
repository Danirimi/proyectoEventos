using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace proyectoEventos.Modelo
{
    public static class JsonDataManager
    {
        private static string ObtenerRutaCompleta(string nombreArchivo)
        {
            string carpetaDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modelo", "Datos");
            // devuelve la ruta completa donde esta el proyecto junto con las carpetas Modelo y Datos

            Directory.CreateDirectory(carpetaDatos); // Garantiza que exista y crea la carpeta si no existe
            return Path.Combine(carpetaDatos, nombreArchivo);
        }

        public static void GuardarDatos<T>(List<T> datos, string nombreArchivo)
        //GuardarDatos<T> indica que es un método genérico que puede trabajar con cualquier tipo de datos.
        {
            string ruta = ObtenerRutaCompleta(nombreArchivo);
            
            try
            {
                // Asegurar que el directorio exista
                string directorio = Path.GetDirectoryName(ruta);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }
                
                string json = JsonSerializer.Serialize(datos, new JsonSerializerOptions { WriteIndented = true });
                // convertimos la lista de datos en una cadena JSON con formato indentado para mejor legibilidad.
                File.WriteAllText(ruta, json);
                // esciribimos el JSON en el archivo que indicamos en la ruta y si ya existe lo sobrescribe
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar datos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static List<T> CargarDatos<T>(string nombreArchivo)
        {
            string ruta = ObtenerRutaCompleta(nombreArchivo);
            
            try
            {
                if (!File.Exists(ruta))
                    return new List<T>();
                // si no existe el archivo, devuelve una lista vacía

                string json = File.ReadAllText(ruta);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                // si el archivo existe, lee su contenido y lo deserializa en una lista del tipo T
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<T>();
            }
        }
        
      
       
    }
}

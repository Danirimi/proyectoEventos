using System;
using System.IO;
using System.Text;

namespace proyectoEventos.Modelo
{
    public static class ErrorLogger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "errors.log");

        private static void EnsureDirectory()
        {
            var dir = Path.GetDirectoryName(LogFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static void LogError(string message)
        {
            try
            {
                EnsureDirectory();
                string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}";
                File.AppendAllText(LogFilePath, line + Environment.NewLine, Encoding.UTF8);
            }
            catch
            {
                // No-op
            }
        }

        public static void LogException(Exception ex, string context = null)
        {
            try
            {
                EnsureDirectory();
                var sb = new StringBuilder();
                sb.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss}] EXCEPTION", DateTime.Now);
                if (!string.IsNullOrEmpty(context)) sb.Append($" Context: {context}");
                sb.AppendLine();
                sb.AppendLine(ex.ToString());
                sb.AppendLine(new string('-', 80));
                File.AppendAllText(LogFilePath, sb.ToString(), Encoding.UTF8);
            }
            catch
            {
                // No-op
            }
        }

        public static string GetLastLines(int lines = 50)
        {
            try
            {
                if (!File.Exists(LogFilePath))
                    return "No hay registros de errores.";

                var allLines = File.ReadAllLines(LogFilePath, Encoding.UTF8);
                int start = Math.Max(0, allLines.Length - lines);
                return string.Join(Environment.NewLine, allLines, start, allLines.Length - start);
            }
            catch (Exception ex)
            {
                return $"No se pudo leer el archivo de logs: {ex.Message}";
            }
        }
    }
}

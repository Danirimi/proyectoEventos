using System;
using System.Net;
using System.Text;

namespace proyectoEventos.Util
{
    public static class EncoderHelper
    {
        // Codifica para contenido HTML (texto dentro de elementos)
        public static string EncodeForHtml(string input)
        {
            if (input == null) return null;
            return WebUtility.HtmlEncode(input);
        }

        // Decodifica HTML (revertir HtmlEncode)
        public static string DecodeFromHtml(string input)
        {
            if (input == null) return null;
            return WebUtility.HtmlDecode(input);
        }

        // Codifica para atributos HTML (añade protección extra contra comillas)
        public static string EncodeForHtmlAttribute(string input)
        {
            if (input == null) return null;
            string encoded = WebUtility.HtmlEncode(input);
            // Reemplazar comillas para evitar romper el atributo
            encoded = encoded.Replace("\"", "&quot;").Replace("'", "&#39;");
            return encoded;
        }

        // Codifica para valores incluidos en URLs
        public static string EncodeForUrl(string input)
        {
            if (input == null) return null;
            return Uri.EscapeDataString(input);
        }

        // Escape simple para cadenas que se incrustan en JavaScript (no reemplaza todo)
        public static string EncodeForJavaScript(string input)
        {
            if (input == null) return null;
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                switch (c)
                {
                    case '\\': sb.Append("\\\\"); break;
                    case '\"': sb.Append("\\\""); break;
                    case '\'': sb.Append("\\\'"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < 32 || c > 126) sb.AppendFormat("\\u{0:X4}", (int)c);
                        else sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
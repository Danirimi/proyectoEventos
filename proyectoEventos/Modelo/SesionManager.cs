namespace proyectoEventos.Modelo
{
    public static class SesionManager
    {
        public static string TokenActual { get; set; }

        public static bool SesionActiva
        {
            get
            {
                return SesionService.ValidarSesion(TokenActual);
            }
        }

        public static void CerrarSesion()
        {
            if (!string.IsNullOrEmpty(TokenActual))
            {
                SesionService.CerrarSesion(TokenActual);
                TokenActual = null;
            }
        }
    }
}
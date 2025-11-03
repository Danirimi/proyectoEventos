using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoEventos.Modelo;
using proyectoEventos.vista;

namespace proyectoEventos.Controlador
{
    public class ControladorEventoUsuario
    {
        private readonly VistaEventosUsuario _vistaEventosUsuario;
        private readonly InterfaceEvento _repoEventos;
        private readonly Usuario _usuarioActual;

        public ControladorEventoUsuario(VistaEventosUsuario vistaEventosUsuario, InterfaceEvento repoEventos, Usuario usuario)
        {
            _vistaEventosUsuario = vistaEventosUsuario;
            _repoEventos = repoEventos;
            _usuarioActual = usuario;

            // Suscribirse a eventos de la vista
            _vistaEventosUsuario.ComprarEntradaE += OnComprarEntrada;
            _vistaEventosUsuario.FiltrarEventosE += OnFiltrarEventos;

            // Cargar eventos iniciales
            CargarEventos();
        }

        private void OnComprarEntrada(object sender, CompraEventoArgs e)
        {
            try
            {
                var evento = _repoEventos.buscarEventoId(e.Evento.Id);

                if (evento == null)
                {
                    MessageBox.Show("El evento no existe", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (evento.entradasdisponibles < e.Cantidad)
                {
                    MessageBox.Show($"Solo hay {evento.entradasdisponibles} entradas disponibles",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Calcular precio (puedes personalizar esta lógica)
                decimal precioPorEntrada = 50.00m; // Precio base
                decimal precioTotal = precioPorEntrada * e.Cantidad;

                // Crear ticket
                ITicket repositorioTickets = new ITicketmemoria();
                Ticket nuevoTicket = new Ticket(
                    evento.Id,
                    evento.NombreEvento,
                    _usuarioActual.id,
                    _usuarioActual.Nombre,
                    DateTime.Now,
                    precioTotal,
                    evento.LugarEvento,
                    e.Cantidad
                );

                // Guardar ticket
                repositorioTickets.GenerarTicket(nuevoTicket);

                // Actualizar entradas disponibles del evento
                evento.entradasdisponibles -= e.Cantidad;
                _repoEventos.actualizarEvento(evento);

                MessageBox.Show(
                    $"¡Compra exitosa!\n\n" +
                    $"Evento: {evento.NombreEvento}\n" +
                    $"Cantidad de entradas: {e.Cantidad}\n" +
                    $"Precio total: {precioTotal:C2}\n" +
                    $"Usuario: {e.Usuario.Nombre}\n" +
                    $"Correo: {e.Usuario.Correo}\n\n" +
                    $"Entradas restantes: {evento.entradasdisponibles}",
                    "Compra Confirmada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Recargar eventos para mostrar entradas actualizadas
                CargarEventos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al realizar la compra: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEventos()
        {
            var eventos = _repoEventos.mostrarEventos();
            _vistaEventosUsuario.MostrarEventos(eventos);
        }
        private List<Evento> FiltrarEventosRecursivo(List<Evento> eventos, int index, string nombre, string fecha, string lugar)
        {
            if (index >= eventos.Count)
                return new List<Evento>();

            var eventoActual = eventos[index];

            bool coincide =
               (string.IsNullOrEmpty(nombre) || eventoActual.NombreEvento.IndexOf(nombre, StringComparison.OrdinalIgnoreCase) >= 0) &&
    (string.IsNullOrEmpty(fecha) || eventoActual.FechaEvento.IndexOf(fecha, StringComparison.OrdinalIgnoreCase) >= 0) &&
    (string.IsNullOrEmpty(lugar) || eventoActual.LugarEvento.IndexOf(lugar, StringComparison.OrdinalIgnoreCase) >= 0);

            var resultado = FiltrarEventosRecursivo(eventos, index + 1, nombre, fecha, lugar);

            if (coincide)
                resultado.Insert(0, eventoActual);

            return resultado;
        }




        private void OnFiltrarEventos(object sender, vista.Argumentos.FiltrarEventosArgs e)
        {
            var eventos = _repoEventos.mostrarEventos().ToList();
            var listaFiltrada = FiltrarEventosRecursivo(eventos, 0, e.Nombre, e.Fecha, e.Lugar);
            _vistaEventosUsuario.MostrarEventos(listaFiltrada);
        }
    }
}

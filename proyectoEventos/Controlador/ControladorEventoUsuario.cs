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
        private readonly ITicket _repoTickets;
        private readonly Usuario _usuarioActual;

        public ControladorEventoUsuario(VistaEventosUsuario vistaEventosUsuario, InterfaceEvento repoEventos, Usuario usuario, ITicket repoTickets)
        {
            _vistaEventosUsuario = vistaEventosUsuario;
            _repoEventos = repoEventos;
            _usuarioActual = usuario;
            _repoTickets = repoTickets;

            // Suscribirse a eventos de la vista
            _vistaEventosUsuario.ComprarEntradaE += OnComprarEntrada;

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

                // Calcular precio total
                decimal precioTotal = evento.PrecioEntrada * e.Cantidad;

                // Actualizar entradas disponibles
                evento.entradasdisponibles -= e.Cantidad;
                _repoEventos.actualizarEvento(evento);

                // Crear y guardar el ticket
                Ticket nuevoTicket = new Ticket(
                    evento.Id,
                    evento.NombreEvento,
                    _usuarioActual.id,
                    _usuarioActual.Nombre,
                    DateTime.Now,
                    precioTotal,
                    evento.LugarEvento,
                    e.Cantidad,
                    e.MetodoPago
                );

                _repoTickets.GenerarTicket(nuevoTicket);

                MessageBox.Show(
                    $"¡Compra exitosa!\n\n" +
                    $"Evento: {evento.NombreEvento}\n" +
                    $"Cantidad de entradas: {e.Cantidad}\n" +
                    $"Precio total: {precioTotal:C2}\n" +
                    $"Método de pago: {e.MetodoPago}\n" +
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
    }
}

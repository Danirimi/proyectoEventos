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
    public class ControladorEvento
    {
        private readonly VistaEventos _vistaEventos;
        private readonly InterfaceEvento _repoEventos;
        private EditarEvento _editarEvento;
        private AgregarEvento _agregarEvento;
        private Evento _eventoSeleccionado;

        public ControladorEvento(VistaEventos vistaEventos, InterfaceEvento repoEventos)
        {
            _vistaEventos = vistaEventos;
            _repoEventos = repoEventos;

            // Suscribirse a eventos de la vista
            _vistaEventos.AgregarEventoE += OnAgregarEvento;
            _vistaEventos.EditarEventoE += OnEditarEvento;
            _vistaEventos.EliminarEventoE += OnEliminarEvento;
            _vistaEventos.EventoSeleccionadoE += OnEventoSeleccionado;

            // Cargar eventos iniciales
            CargarEventos();
        }

        private void OnEventoSeleccionado(object sender, EventoSeleccionadoArgs e)
        {
            _eventoSeleccionado = e.EventoSeleccionado;
        }

        private void OnAgregarEvento(object sender, EventArgs e)
        {
            if (_agregarEvento == null || _agregarEvento.IsDisposed)
            {
                _agregarEvento = new AgregarEvento();
                _agregarEvento.EventoGuardadoE += OnEventoGuardado;
            }

            _agregarEvento.LimpiarCampos();
            _agregarEvento.ShowDialog();
        }

        private void OnEventoGuardado(object sender, EventoEventArgs e)
        {
            try
            {
                Evento nuevoEvento = new Evento(
                    0, // El ID se asignará automáticamente
                    e.NombreEvento,
                    e.FechaEvento,
                    e.LugarEvento,
                    e.DescripcionEvento,
                    e.EntradasTotales,
                    e.EntradasTotales // Al crear, las disponibles son iguales a las totales
                );

                _repoEventos.agregarEvento(nuevoEvento);
                CargarEventos();
                _vistaEventos.notify("Evento agregado exitosamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar evento: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnEditarEvento(object sender, EventArgs e)
        {
            if (_eventoSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un evento para editar", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_editarEvento == null || _editarEvento.IsDisposed)
            {
                _editarEvento = new EditarEvento();
                _editarEvento.EventoActualizadoE += OnEventoActualizado;
            }

            _editarEvento.CargarEvento(_eventoSeleccionado);
            _editarEvento.ShowDialog();
        }

        private void OnEventoActualizado(object sender, EventoEventArgs e)
        {
            try
            {
                Evento eventoActualizado = new Evento(
                    e.Id,
                    e.NombreEvento,
                    e.FechaEvento,
                    e.LugarEvento,
                    e.DescripcionEvento,
                    e.EntradasTotales,
                    e.EntradasDisponibles
                );

                _repoEventos.actualizarEvento(eventoActualizado);
                CargarEventos();
                _vistaEventos.notify("Evento actualizado exitosamente");
                _eventoSeleccionado = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar evento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnEliminarEvento(object sender, EventArgs e)
        {
            if (_eventoSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un evento para eliminar", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Está seguro de que desea eliminar el evento '{_eventoSeleccionado.NombreEvento}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    _repoEventos.eliminarEvento(_eventoSeleccionado.Id);
                    CargarEventos();
                    _vistaEventos.notify("Evento eliminado exitosamente");
                    _eventoSeleccionado = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar evento: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CargarEventos()
        {
            var eventos = _repoEventos.mostrarEventos();
            _vistaEventos.MostrarEventos(eventos);
        }
    }
}

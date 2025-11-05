using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoEventos.Modelo;

namespace proyectoEventos.vista
{
    public partial class EditarEvento : Form
    {
        public event EventHandler<EventoEventArgs> EventoActualizadoE;
        private int _eventoId;

        public EditarEvento()
        {
            InitializeComponent();
        }

        public void CargarEvento(Evento evento)
        {
            if (evento == null) return;

            _eventoId = evento.Id;
            txtnombre.Text = evento.NombreEvento;
            txtevento.Text = evento.FechaEvento;
            txtlugar.Text = evento.LugarEvento;
            txtdescripcion.Text = evento.DescripcionEvento;
            txtentradas.Text = evento.entradastotales.ToString();
            txtentradasdisponibles.Text = evento.entradasdisponibles.ToString();
            txtprecio.Text = evento.PrecioEntrada.ToString();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    MessageBox.Show("El nombre del evento es obligatorio", "Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtevento.Text))
                {
                    MessageBox.Show("La fecha del evento es obligatoria", "Validación", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtentradas.Text, out int entradasTotales) || entradasTotales <= 0)
                {
                    MessageBox.Show("Las entradas totales deben ser un número válido mayor a 0", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtentradasdisponibles.Text, out int entradasDisponibles) || entradasDisponibles < 0)
                {
                    MessageBox.Show("Las entradas disponibles deben ser un número válido mayor o igual a 0", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (entradasDisponibles > entradasTotales)
                {
                    MessageBox.Show("Las entradas disponibles no pueden ser mayores a las entradas totales", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtprecio.Text, out decimal precioEntrada) || precioEntrada <= 0)
                {
                    MessageBox.Show("El precio por entrada debe ser un número válido mayor a 0", 
                        "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Disparar evento con los datos actualizados
                EventoActualizadoE?.Invoke(this, new EventoEventArgs(
                    _eventoId,
                    txtnombre.Text.Trim(),
                    txtevento.Text.Trim(),
                    txtlugar.Text.Trim(),
                    txtdescripcion.Text.Trim(),
                    entradasTotales,
                    entradasDisponibles,
                    precioEntrada
                ));

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar cambios: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

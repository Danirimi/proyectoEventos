using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyectoEventos.vista
{
    public partial class AgregarEvento : Form
    {
        public event EventHandler<EventoEventArgs> EventoGuardadoE;

        public AgregarEvento()
        {
            InitializeComponent();
        }

        public void LimpiarCampos()
        {
            txtnombre.Text = "";
            txtfecha.Text = "";
            txtlugar.Text = "";
            txtdescripcion.Text = "";
            txtentradas.Text = "";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    MessageBox.Show("El nombre del evento es obligatorio", "Validaci�n", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtfecha.Text))
                {
                    MessageBox.Show("La fecha del evento es obligatoria", "Validaci�n", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtentradas.Text, out int entradasTotales) || entradasTotales <= 0)
                {
                    MessageBox.Show("Las entradas totales deben ser un n�mero v�lido mayor a 0", 
                        "Validaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Disparar evento con los datos del nuevo evento
                EventoGuardadoE?.Invoke(this, new EventoEventArgs(
                    0, // El ID se asignar� autom�ticamente
                    txtnombre.Text.Trim(),
                    txtfecha.Text.Trim(),
                    txtlugar.Text.Trim(),
                    txtdescripcion.Text.Trim(),
                    entradasTotales,
                    entradasTotales // Al crear, las disponibles son iguales a las totales
                ));

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar evento: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

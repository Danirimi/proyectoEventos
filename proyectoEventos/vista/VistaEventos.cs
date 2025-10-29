using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using proyectoEventos.Modelo;
using System.IO;

namespace proyectoEventos.vista
{
    public partial class VistaEventos : Form
    {
        private readonly string _carpetaImagenes;
        private ToolTip toolTip;
        private Dictionary<PictureBox, Evento> eventosAsociados;
        private Form formDetalles = null;

        //Eventos de la interfaz

      


        public VistaEventos()
        {
            InitializeComponent();
            _carpetaImagenes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", "Eventos");
            toolTip = new ToolTip();
            eventosAsociados = new Dictionary<PictureBox, Evento>();
            ConfigurarFlowLayout();
        }

        private void ConfigurarFlowLayout()
        {
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
        }

        public void MostrarEventos(IEnumerable<Evento> eventos)
        {
            flowLayoutPanel1.Controls.Clear();
            eventosAsociados.Clear();

            foreach (var evento in eventos)
            {
                var pictureBox = CrearPictureBoxEvento(evento);
                flowLayoutPanel1.Controls.Add(pictureBox);
                eventosAsociados[pictureBox] = evento;
            }
        }

        private PictureBox CrearPictureBoxEvento(Evento evento)
        {
            var pictureBox = new PictureBox
            {
                Width = 200,
                Height = 150,
                SizeMode = PictureBoxSizeMode.Zoom,
                Margin = new Padding(10),
                Cursor = Cursors.Hand,
                Tag = evento
            };

            // Cargar imagen del evento si existe
            string rutaImagen = Path.Combine(_carpetaImagenes, $"evento_{evento.Id}.jpg");
            if (File.Exists(rutaImagen))
            {
                try
                {
                    using (var stream = new FileStream(rutaImagen, FileMode.Open, FileAccess.Read))
                    {
                        pictureBox.Image = Image.FromStream(stream);
                    }
                }
                catch
                {
                    pictureBox.Image = Properties.Resources.evento_default; // Necesitarás agregar una imagen por defecto
                }
            }
            else
            {
                pictureBox.Image = Properties.Resources.evento_default; // Necesitarás agregar una imagen por defecto
            }

            // Configurar eventos
            pictureBox.MouseEnter += PictureBox_MouseEnter;
            pictureBox.MouseLeave += PictureBox_MouseLeave;
            pictureBox.Click += PictureBox_Click;

            return pictureBox;
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var evento = (Evento)pictureBox.Tag;
            
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            toolTip.SetToolTip(pictureBox, 
                $"Nombre: {evento.NombreEvento}\n" +
                $"Fecha: {evento.FechaEvento:dd/MM/yyyy}\n" +
                $"Entradas disponibles: {evento.entradasdisponibles}");
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            pictureBox.BorderStyle = BorderStyle.None;
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var evento = (Evento)pictureBox.Tag;

            // Cerrar el form de detalles anterior si existe
            formDetalles?.Close();

            // Crear y mostrar el nuevo form de detalles
            formDetalles = new Form
            {
                Width = 600,
                Height = 400,
                Text = evento.NombreEvento,
                StartPosition = FormStartPosition.CenterParent
            };

            var panelDetalles = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(10)
            };

            // Agregar imagen ampliada
            var imagenAmpliada = new PictureBox
            {
                Image = pictureBox.Image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            // Agregar controles con la información del evento
            var labelInfo = new Label
            {
                Text = $"Nombre: {evento.NombreEvento}\n\n" +
                       $"Fecha: {evento.FechaEvento:dd/MM/yyyy}\n\n" +
                       $"Lugar: {evento.LugarEvento}\n\n" +
                       $"Descripción: {evento.DescripcionEvento}\n\n" +
                       $"Entradas disponibles: {evento.entradasdisponibles}",
                AutoSize = true,
                Dock = DockStyle.Fill
            };

            panelDetalles.Controls.Add(imagenAmpliada, 0, 0);
            panelDetalles.Controls.Add(labelInfo, 1, 0);
            formDetalles.Controls.Add(panelDetalles);
            formDetalles.ShowDialog();
        }

        public void notify(string message)
        {
            MessageBox.Show(message, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void VistaEventos_Load(object sender, EventArgs e)
        {
            ConfigurarFlowLayout();
        }

        // Método para actualizar un evento específico
        public void ActualizarEvento(Evento evento)
        {
            foreach (PictureBox pictureBox in flowLayoutPanel1.Controls)
            {
                if (pictureBox.Tag is Evento eventoActual && eventoActual.Id == evento.Id)
                {
                    pictureBox.Tag = evento;
                    // Actualizar la imagen si es necesario
                    string rutaImagen = Path.Combine(_carpetaImagenes, $"evento_{evento.Id}.jpg");
                    if (File.Exists(rutaImagen))
                    {
                        using (var stream = new FileStream(rutaImagen, FileMode.Open, FileAccess.Read))
                        {
                            pictureBox.Image = Image.FromStream(stream);
                        }
                    }
                    break;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using proyectoEventos.Modelo;
using System.IO;
using System.Linq;

namespace proyectoEventos.vista
{
    public partial class VistaEventos : Form
    {
        private readonly string _carpetaImagenes;
        private ToolTip toolTip;
        private Dictionary<PictureBox, Evento> eventosAsociados;
        private Form formDetalles = null;
        private Evento _eventoSeleccionado;

        // Eventos de la interfaz
        public event EventHandler AgregarEventoE;
        public event EventHandler EditarEventoE;
        public event EventHandler EliminarEventoE;
        public event EventHandler<EventoSeleccionadoArgs> EventoSeleccionadoE;
        public event EventHandler CerrarSesionE;  // ✅ NUEVO: Evento para cerrar sesión

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

            if (eventos == null || !eventos.Any())
            {
                var lblNoEventos = new Label
                {
                    Text = "No hay eventos disponibles",
                    Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(20)
                };
                flowLayoutPanel1.Controls.Add(lblNoEventos);
                return;
            }

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
                Tag = evento,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
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
                    // Si hay error al cargar la imagen, usar un panel con texto
                    pictureBox.Image = CrearImagenPorDefecto(evento.NombreEvento);
                }
            }
            else
            {
                // Crear imagen por defecto con el nombre del evento
                pictureBox.Image = CrearImagenPorDefecto(evento.NombreEvento);
            }

            // Configurar eventos
            pictureBox.MouseEnter += PictureBox_MouseEnter;
            pictureBox.MouseLeave += PictureBox_MouseLeave;
            pictureBox.Click += PictureBox_Click;

            return pictureBox;
        }

        private Image CrearImagenPorDefecto(string nombreEvento)
        {
            Bitmap bmp = new Bitmap(200, 150);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                using (Font font = new Font("Arial", 12, FontStyle.Bold))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(nombreEvento, font, Brushes.Black,
                        new RectangleF(0, 0, 200, 150), sf);
                }
            }
            return bmp;
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var evento = (Evento)pictureBox.Tag;

            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            toolTip.SetToolTip(pictureBox,
                $"Nombre: {evento.NombreEvento}\n" +
                $"Fecha: {evento.FechaEvento}\n" +
                $"Precio: {evento.PrecioEntrada:C2}\n" +
                $"Entradas disponibles: {evento.entradasdisponibles}");
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var evento = (Evento)pictureBox.Tag;

            // Marcar el evento como seleccionado
            _eventoSeleccionado = evento;
            EventoSeleccionadoE?.Invoke(this, new EventoSeleccionadoArgs(evento));

            // Resaltar el evento seleccionado
            foreach (PictureBox pb in flowLayoutPanel1.Controls.OfType<PictureBox>())
            {
                pb.BackColor = Color.White;
            }
            pictureBox.BackColor = Color.LightBlue;

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
                RowCount = 1,
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
                       $"Fecha: {evento.FechaEvento}\n\n" +
                       $"Lugar: {evento.LugarEvento}\n\n" +
                       $"Descripción: {evento.DescripcionEvento}\n\n" +
                       $"Precio por entrada: {evento.PrecioEntrada:C2}\n\n" +
                       $"Entradas totales: {evento.entradastotales}\n\n" +
                       $"Entradas disponibles: {evento.entradasdisponibles}",
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft Sans Serif", 10F)
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
            foreach (PictureBox pictureBox in flowLayoutPanel1.Controls.OfType<PictureBox>())
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AgregarEventoE?.Invoke(this, EventArgs.Empty);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            EditarEventoE?.Invoke(this, EventArgs.Empty);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarEventoE?.Invoke(this, EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        
            CerrarSesionE?.Invoke(this, EventArgs.Empty);
        }
    }
}
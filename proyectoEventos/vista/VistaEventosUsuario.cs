using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using proyectoEventos.Modelo;
using System.IO;
using System.Linq;

namespace proyectoEventos.vista
{
    public partial class VistaEventosUsuario : Form
    {
        private readonly string _carpetaImagenes;
        private ToolTip toolTip;
        private Dictionary<PictureBox, Evento> eventosAsociados;
        private Form formDetalles = null;
        private Evento _eventoSeleccionado;
        private Usuario _usuarioActual;

        // Eventos de la interfaz
        public event EventHandler<CompraEventoArgs> ComprarEntradaE;
        public event EventHandler<EventoSeleccionadoArgs> EventoSeleccionadoE;

        public VistaEventosUsuario(Usuario usuario)
        {
            InitializeComponent();
            _usuarioActual = usuario;
            _carpetaImagenes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagenes", "Eventos");
            toolTip = new ToolTip();
            eventosAsociados = new Dictionary<PictureBox, Evento>();
            ConfigurarFlowLayout();
            
            // Mostrar información del usuario
            lblBienvenida.Text = $"Bienvenido, {usuario.Nombre}";
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
                    pictureBox.Image = CrearImagenPorDefecto(evento.NombreEvento);
                }
            }
            else
            {
                pictureBox.Image = CrearImagenPorDefecto(evento.NombreEvento);
            }

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

            _eventoSeleccionado = evento;
            EventoSeleccionadoE?.Invoke(this, new EventoSeleccionadoArgs(evento));

            foreach (PictureBox pb in flowLayoutPanel1.Controls.OfType<PictureBox>())
            {
                pb.BackColor = Color.White;
            }
            pictureBox.BackColor = Color.LightBlue;

            MostrarDetallesEvento(evento, pictureBox.Image);
        }

        private void MostrarDetallesEvento(Evento evento, Image imagen)
        {
            formDetalles?.Close();

            formDetalles = new Form
            {
                Width = 700,
                Height = 500,
                Text = evento.NombreEvento,
                StartPosition = FormStartPosition.CenterParent
            };

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(10)
            };

            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

            var imagenAmpliada = new PictureBox
            {
                Image = imagen,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            var labelInfo = new Label
            {
                Text = $"Nombre: {evento.NombreEvento}\n\n" +
                       $"Fecha: {evento.FechaEvento}\n\n" +
                       $"Lugar: {evento.LugarEvento}\n\n" +
                       $"Descripción: {evento.DescripcionEvento}\n\n" +
                       $"Entradas totales: {evento.entradastotales}\n\n" +
                       $"Entradas disponibles: {evento.entradasdisponibles}",
                AutoSize = false,
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft Sans Serif", 10F)
            };

            var btnComprar = new Button
            {
                Text = "Comprar Entrada",
                Dock = DockStyle.Fill,
                BackColor = Color.Green,
                ForeColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold),
                Enabled = evento.entradasdisponibles > 0
            };

            btnComprar.Click += (s, ev) =>
            {
                if (evento.entradasdisponibles > 0)
                {
                    var cantidad = MostrarDialogoCantidad(evento.entradasdisponibles);
                    if (cantidad > 0)
                    {
                        ComprarEntradaE?.Invoke(this, new CompraEventoArgs(evento, cantidad, _usuarioActual));
                        formDetalles.Close();
                    }
                }
                else
                {
                    MessageBox.Show("No hay entradas disponibles", "Información", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            mainPanel.Controls.Add(imagenAmpliada, 0, 0);
            mainPanel.Controls.Add(labelInfo, 1, 0);
            mainPanel.SetColumnSpan(btnComprar, 2);
            mainPanel.Controls.Add(btnComprar, 0, 1);

            formDetalles.Controls.Add(mainPanel);
            formDetalles.ShowDialog();
        }

        private int MostrarDialogoCantidad(int maxDisponibles)
        {
            Form prompt = new Form
            {
                Width = 350,
                Height = 150,
                Text = "Cantidad de Entradas",
                StartPosition = FormStartPosition.CenterParent
            };

            Label textLabel = new Label
            {
                Left = 20,
                Top = 20,
                Text = $"¿Cuántas entradas desea comprar? (Máx: {maxDisponibles})",
                AutoSize = true
            };

            NumericUpDown numericUpDown = new NumericUpDown
            {
                Left = 20,
                Top = 50,
                Width = 100,
                Minimum = 1,
                Maximum = maxDisponibles,
                Value = 1
            };

            Button confirmation = new Button
            {
                Text = "Confirmar",
                Left = 150,
                Width = 80,
                Top = 50,
                DialogResult = DialogResult.OK
            };

            Button cancelButton = new Button
            {
                Text = "Cancelar",
                Left = 240,
                Width = 80,
                Top = 50,
                DialogResult = DialogResult.Cancel
            };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            cancelButton.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(numericUpDown);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancelButton);

            return prompt.ShowDialog() == DialogResult.OK ? (int)numericUpDown.Value : 0;
        }

        public void notify(string message)
        {
            MessageBox.Show(message, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void VistaEventosUsuario_Load(object sender, EventArgs e)
        {
            ConfigurarFlowLayout();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            // Este evento será manejado por el controlador para recargar eventos
            OnLoad(EventArgs.Empty);
        }
    }
}

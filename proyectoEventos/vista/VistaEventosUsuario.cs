using proyectoEventos.Controlador;
using proyectoEventos.Modelo;
using proyectoEventos.vista.Argumentos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace proyectoEventos.vista
{

    public partial class VistaEventosUsuario : Form


    {
        public event EventHandler CerrarSesionE;
        public event EventHandler<FiltrarEventosArgs> FiltrarEventosE;// Evento para filtrar eventos
        private ControladorEventoUsuario _controladorEventoUsuario;
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

            // El controlador será asignado desde ControladorUsuario
            // InterfaceEvento repoEventos = new InterfazEventoMemoria();
            // _controladorEventoUsuario = new ControladorEventoUsuario(this, repoEventos, _usuarioActual);

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
                       $"Precio por entrada: {evento.PrecioEntrada:C2}\n\n" +
                       $"Entradas disponibles: {evento.entradasdisponibles} / {evento.entradastotales}",
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
                    var (cantidad, metodoPago) = MostrarDialogoCompra(evento);
                    if (cantidad > 0 && !string.IsNullOrEmpty(metodoPago))
                    {
                        ComprarEntradaE?.Invoke(this, new CompraEventoArgs(evento, cantidad, _usuarioActual, metodoPago));
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

        private (int cantidad, string metodoPago) MostrarDialogoCompra(Evento evento)
        {
            Form prompt = new Form
            {
                Width = 400,
                Height = 250,
                Text = "Comprar Entradas",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblCantidad = new Label
            {
                Left = 20,
                Top = 20,
                Width = 350,
                Text = $"¿Cuántas entradas desea comprar? (Máx: {evento.entradasdisponibles})"
            };

            NumericUpDown numCantidad = new NumericUpDown
            {
                Left = 20,
                Top = 50,
                Width = 100,
                Minimum = 1,
                Maximum = evento.entradasdisponibles,
                Value = 1
            };

            Label lblPrecioTotal = new Label
            {
                Left = 140,
                Top = 50,
                Width = 230,
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold),
                Text = $"Total: {evento.PrecioEntrada:C2}"
            };

            // Actualizar precio total al cambiar cantidad
            numCantidad.ValueChanged += (s, e) =>
            {
                decimal total = evento.PrecioEntrada * numCantidad.Value;
                lblPrecioTotal.Text = $"Total: {total:C2}";
            };

            Label lblMetodoPago = new Label
            {
                Left = 20,
                Top = 90,
                Width = 200,
                Text = "Método de Pago:"
            };

            RadioButton rbEfectivo = new RadioButton
            {
                Left = 20,
                Top = 120,
                Width = 150,
                Text = "Efectivo",
                Checked = true
            };

            RadioButton rbTarjeta = new RadioButton
            {
                Left = 20,
                Top = 150,
                Width = 150,
                Text = "Tarjeta"
            };

            Button btnConfirmar = new Button
            {
                Text = "Confirmar",
                Left = 200,
                Width = 80,
                Top = 180,
                DialogResult = DialogResult.OK,
                BackColor = Color.Green,
                ForeColor = Color.White
            };

            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Left = 290,
                Width = 80,
                Top = 180,
                DialogResult = DialogResult.Cancel,
                BackColor = Color.Red,
                ForeColor = Color.White
            };

            prompt.Controls.Add(lblCantidad);
            prompt.Controls.Add(numCantidad);
            prompt.Controls.Add(lblPrecioTotal);
            prompt.Controls.Add(lblMetodoPago);
            prompt.Controls.Add(rbEfectivo);
            prompt.Controls.Add(rbTarjeta);
            prompt.Controls.Add(btnConfirmar);
            prompt.Controls.Add(btnCancelar);

            prompt.AcceptButton = btnConfirmar;
            prompt.CancelButton = btnCancelar;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                string metodoPago = rbEfectivo.Checked ? "Efectivo" : "Tarjeta";
                return ((int)numCantidad.Value, metodoPago);
            }

            return (0, null);
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblBienvenida_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
           /* string nombre = txtNombre.Text.Trim();
            string fecha = txtFecha.Text.Trim();
            string lugar = txtLugar.Text.Trim();
            FiltrarEventosE?.Invoke(this, new FiltrarEventosArgs(nombre, fecha, lugar));*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VerHistorial Historial = new VerHistorial();
            Historial.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CerrarSesionE?.Invoke(this, EventArgs.Empty);

        }
    }
}

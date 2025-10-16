using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
namespace proyectoEventos.Modelo
{
    internal class InterfazEventoMemoria : InterfaceEvento
    {
        private readonly List<Evento> _eventos = new List<Evento>();
        private int _siguenteId = 1;
        private readonly string _rutaArchivo;

        public InterfazEventoMemoria()
        {
            string carpetaDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datos");
            _rutaArchivo = Path.Combine(carpetaDatos, "eventos.json");

            _eventos = JsonHelper.Cargar<Evento>(_rutaArchivo);
            _siguenteId = _eventos.Any() ? _eventos.Max(e => e.Id) + 1 : 1;
        }

        public IEnumerable<Evento> mostrarEventos()
        {
            return _eventos;
        }  
        
        public Evento buscarEventoId(int id)
        {
            if (_eventos == null)

            {
                MessageBox.Show("La colección de eventos no está inicializada.");
            }

            Evento evento = _eventos.FirstOrDefault(e => e.Id == id);

            if (evento == null)
            {

                MessageBox.Show($"No se encontró ningún evento con Id = {id}.");
            }
            MessageBox.Show(evento.ToString(), "Detalles del Evento", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return evento;
        }
        public void agregarEvento(Evento evento)
        {
            if (evento == null)
            {
               MessageBox.Show("El evento no puede ser nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            evento.Id = _siguenteId++;
            _eventos.Add(evento);
            JsonHelper.Guardar(_eventos, _rutaArchivo);
        }
        public void actualizarEvento(Evento evento)
        {
            if (evento == null)
            {
                MessageBox.Show("El evento no puede ser nulo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            var eventoExistente = _eventos.FirstOrDefault(e => e.Id == evento.Id);
            if (eventoExistente == null)
            {
                MessageBox.Show($"No se encontró ningún evento con Id = {evento.Id}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            eventoExistente.NombreEvento = evento.NombreEvento;
            eventoExistente.FechaEvento = evento.FechaEvento;
            eventoExistente.LugarEvento = evento.LugarEvento;
            eventoExistente.DescripcionEvento = evento.DescripcionEvento;
            eventoExistente.entradastotales = evento.entradastotales;
            eventoExistente.entradasdisponibles = evento.entradasdisponibles;

            JsonHelper.Guardar(_eventos, _rutaArchivo);
        }
        public void eliminarEvento(int id)
        {
            var evento = _eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null)
            {
                MessageBox.Show($"No se encontró ningún evento con Id = {id}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _eventos.Remove(evento);
            JsonHelper.Guardar(_eventos, _rutaArchivo);
        }





    }
}

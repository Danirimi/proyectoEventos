using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using proyectoEventos.Modelo; // Asegúrate de que JsonDataManager esté en este namespace o ajusta según corresponda

namespace proyectoEventos.Modelo
{
    public class InterfazEventoMemoria : InterfaceEvento
    {
        private List<Evento> _eventos;
        private int _siguenteId = 1;
        private readonly string _nombreArchivoJson = "eventos.json";

        public InterfazEventoMemoria()
        {
            // Inicialización en memoria (sin JSON)
            _eventos = new List<Evento>();
            
            // Calcular el siguiente ID basado en los eventos existentes
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
                MessageBox.Show("La colección de eventos no está inicializada.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            Evento evento = _eventos.FirstOrDefault(e => e.Id == id);

            if (evento == null)
            {
                return null;
            }

            return evento;
        }
        
        public void agregarEvento(Evento evento)
        {
            if (evento == null)
            {
                MessageBox.Show("El evento no puede ser nulo.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Asignar ID automáticamente
            evento.Id = _siguenteId++;
            _eventos.Add(evento);
            
            // Persistencia deshabilitada: almacenamiento en memoria
        }
        
        public void actualizarEvento(Evento evento)
        {
            if (evento == null)
            {
                MessageBox.Show("El evento no puede ser nulo.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var eventoExistente = _eventos.FirstOrDefault(e => e.Id == evento.Id);
            if (eventoExistente == null)
            {
                MessageBox.Show($"No se encontró ningún evento con Id = {evento.Id}.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Actualizar propiedades
            eventoExistente.NombreEvento = evento.NombreEvento;
            eventoExistente.FechaEvento = evento.FechaEvento;
            eventoExistente.LugarEvento = evento.LugarEvento;
            eventoExistente.DescripcionEvento = evento.DescripcionEvento;
            eventoExistente.entradastotales = evento.entradastotales;
            eventoExistente.entradasdisponibles = evento.entradasdisponibles;

            // Persistencia deshabilitada: almacenamiento en memoria
        }
        
        public void eliminarEvento(int id)
        {
            var evento = _eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null)
            {
                MessageBox.Show($"No se encontró ningún evento con Id = {id}.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            _eventos.Remove(evento);
            
            // Persistencia deshabilitada: almacenamiento en memoria
        }
    }
}

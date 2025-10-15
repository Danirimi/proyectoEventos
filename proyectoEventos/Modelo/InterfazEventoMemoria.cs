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



    }
}

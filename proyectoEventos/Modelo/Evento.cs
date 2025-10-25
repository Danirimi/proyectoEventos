using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoEventos.Modelo
{
    public class Evento
    {
        public int Id { get; set; }

        public string NombreEvento { get; set; }

        public string FechaEvento { get; set; }

        public string LugarEvento { get; set; }

        public string DescripcionEvento { get; set; }

        public int entradastotales { get; set; }

        public int entradasdisponibles { get; set; }

        public Evento(int id, string nombreEvento, string fechaEvento, string lugarEvento, string descripcionEvento, int entradastotales, int entradasdisponibles)
        {
            Id = id;
            NombreEvento = nombreEvento;
            FechaEvento = fechaEvento;
            LugarEvento = lugarEvento;
            DescripcionEvento = descripcionEvento;
            this.entradastotales = entradastotales;
            this.entradasdisponibles = entradasdisponibles;

        }
        public override string ToString()
        {
            return
                "\n========= INFORMACIÓN DEL EVENTO =========\n" +
                $"ID: {Id}\n" +
                $"Nombre del Evento: {NombreEvento}\n" +
                $"Fecha: {FechaEvento}\n" +
                $"Lugar: {LugarEvento}\n" +
                $"Descripción:\n   {DescripcionEvento}\n" +
                $"Entradas Totales: {entradastotales}\n" +
                $"Entradas Disponibles: {entradasdisponibles}\n" +
                "==========================================\n";
        }




    }
}

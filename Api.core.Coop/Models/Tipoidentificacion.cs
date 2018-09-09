using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class Tipoidentificacion
    {
        public Tipoidentificacion()
        {
            Persona = new HashSet<Persona>();
        }

        public int Secuencial { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Parapersonanatural { get; set; }
        public int Numerominimorepresentantes { get; set; }
        public int Numerominimoreferenciaspersona { get; set; }
        public int Numerominimoreferenciasbancari { get; set; }
        public int Numerominimoreferenciascomerci { get; set; }
        public bool Estaactiva { get; set; }
        public int Numeroverificador { get; set; }
        public string Codigosbs { get; set; }

        public ICollection<Persona> Persona { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Division
    {
        public Division()
        {
            Cliente = new HashSet<Cliente>();
            PersonaSecuencialdivactividadeconNavigation = new HashSet<Persona>();
            PersonaSecuencialdivpolresidenciaNavigation = new HashSet<Persona>();
        }

        public int Secuencial { get; set; }
        public int Secuencialnivel { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Numeroverificador { get; set; }

        public Oficina Oficina { get; set; }
        public ICollection<Cliente> Cliente { get; set; }
        public ICollection<Persona> PersonaSecuencialdivactividadeconNavigation { get; set; }
        public ICollection<Persona> PersonaSecuencialdivpolresidenciaNavigation { get; set; }
    }
}

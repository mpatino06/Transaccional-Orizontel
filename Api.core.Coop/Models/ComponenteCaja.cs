using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class ComponenteCaja
    {
        public ComponenteCaja()
        {
            VentanillacomponenteCaja = new HashSet<VentanillacomponenteCaja>();
        }

        public int Secuencialcomponente { get; set; }
        public string Codigotipocomponentecaja { get; set; }

        public ICollection<VentanillacomponenteCaja> VentanillacomponenteCaja { get; set; }
    }
}

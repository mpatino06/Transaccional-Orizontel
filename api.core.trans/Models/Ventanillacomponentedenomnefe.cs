using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Ventanillacomponentedenomnefe
    {
        public Ventanillacomponentedenomnefe()
        {
            MovimientoventcompCajadet = new HashSet<MovimientoventcompCajadet>();
        }

        public int Secuencial { get; set; }
        public int Secuencialventanillacompcaja { get; set; }
        public decimal Denominacion { get; set; }
        public int Cantidad { get; set; }

        public VentanillacomponenteCaja SecuencialventanillacompcajaNavigation { get; set; }
        public ICollection<MovimientoventcompCajadet> MovimientoventcompCajadet { get; set; }
    }
}

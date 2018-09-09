using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class VentanillacomponenteCaja
    {
        public VentanillacomponenteCaja()
        {
            MovimientoventanillacompCaja = new HashSet<MovimientoventanillacompCaja>();
            Ventanillacomponentedenomnefe = new HashSet<Ventanillacomponentedenomnefe>();
        }

        public int Secuencial { get; set; }
        public int Secuencialventanilla { get; set; }
        public int Secuencialcomponentecaja { get; set; }
        public int Secuencialmoneda { get; set; }
        public int Cantidad { get; set; }
        public decimal Saldo { get; set; }
        public decimal Valorcuadre { get; set; }

        public ComponenteCaja SecuencialcomponentecajaNavigation { get; set; }
        public Ventanilla SecuencialventanillaNavigation { get; set; }
        public ICollection<MovimientoventanillacompCaja> MovimientoventanillacompCaja { get; set; }
        public ICollection<Ventanillacomponentedenomnefe> Ventanillacomponentedenomnefe { get; set; }
    }
}

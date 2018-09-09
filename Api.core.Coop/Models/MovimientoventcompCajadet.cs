using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class MovimientoventcompCajadet
    {
        public int Secuencial { get; set; }
        public int Secuencialmovventcompcaja { get; set; }
        public int Secuencialventcompdenomefe { get; set; }
        public int Cantidad { get; set; }
        public decimal Saldo { get; set; }

        public MovimientoventanillacompCaja SecuencialmovventcompcajaNavigation { get; set; }
        public Ventanillacomponentedenomnefe SecuencialventcompdenomefeNavigation { get; set; }
    }
}

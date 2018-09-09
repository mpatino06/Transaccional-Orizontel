using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class MovimientoventanillacompCaja
    {
        public MovimientoventanillacompCaja()
        {
            MovimientoventcompCajadet = new HashSet<MovimientoventcompCajadet>();
        }

        public int Secuencial { get; set; }
        public int Secuencialmovimientodetalle { get; set; }
        public int Secuencialventanillacompcaja { get; set; }
        public string Codigotipomovimientocaja { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }

        public Movimientodetalle SecuencialmovimientodetalleNavigation { get; set; }
        public VentanillacomponenteCaja SecuencialventanillacompcajaNavigation { get; set; }
        public ICollection<MovimientoventcompCajadet> MovimientoventcompCajadet { get; set; }
    }
}

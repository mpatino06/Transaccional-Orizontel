using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Movimientodetalle
    {
        public Movimientodetalle()
        {
            MovimientocuentacompVista = new HashSet<MovimientocuentacompVista>();
            MovimientoventanillacompCaja = new HashSet<MovimientoventanillacompCaja>();
        }

        public int Secuencial { get; set; }
        public int Secuencialmovimiento { get; set; }
        public int Secuencialtransaccion { get; set; }
        public int Secuencialmoneda { get; set; }
        public decimal Valor { get; set; }
        public int Secuencialoficinaafectada { get; set; }

        public Movimiento SecuencialmovimientoNavigation { get; set; }
        public Oficina SecuencialoficinaafectadaNavigation { get; set; }
        public Transaccion SecuencialtransaccionNavigation { get; set; }
        public MovimientodetalleCuenta MovimientodetalleCuenta { get; set; }
        public ICollection<MovimientocuentacompVista> MovimientocuentacompVista { get; set; }
        public ICollection<MovimientoventanillacompCaja> MovimientoventanillacompCaja { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class MovimientodetalleCuenta
    {
        public int Secuencialmovimientodetalle { get; set; }
        public int Secuencialcuenta { get; set; }
        public decimal Saldocuenta { get; set; }
        public string Codigoestadocuenta { get; set; }

        public Estadocuenta CodigoestadocuentaNavigation { get; set; }
        public Cuentamaestro SecuencialcuentaNavigation { get; set; }
        public Movimientodetalle SecuencialmovimientodetalleNavigation { get; set; }
    }
}

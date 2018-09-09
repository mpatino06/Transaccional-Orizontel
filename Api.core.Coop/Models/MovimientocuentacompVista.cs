using System;
using System.Collections.Generic;

namespace Api.core.Coop.Models
{
    public partial class MovimientocuentacompVista
    {
        public int Secuencial { get; set; }
        public int Secuencialmovimientodetalle { get; set; }
        public int Secuencialcuenta { get; set; }
        public int Secuencialcomponentevista { get; set; }
        public string Codigotipomovimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public decimal Saldocuenta { get; set; }

        public ComponenteVista SecuencialcomponentevistaNavigation { get; set; }
        public Cuentamaestro SecuencialcuentaNavigation { get; set; }
        public Movimientodetalle SecuencialmovimientodetalleNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class CuentacomponenteVista
    {
        public int Secuencial { get; set; }
        public int Secuencialcuenta { get; set; }
        public int Secuencialcomponentevista { get; set; }
        public decimal Saldo { get; set; }
        public int Numeroverificador { get; set; }

        public ComponenteVista SecuencialcomponentevistaNavigation { get; set; }
        public Cuentamaestro SecuencialcuentaNavigation { get; set; }
    }
}

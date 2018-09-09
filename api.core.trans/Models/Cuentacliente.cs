using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Cuentacliente
    {
        public int Secuencial { get; set; }
        public int Secuencialcuenta { get; set; }
        public int Secuencialcliente { get; set; }
        public bool Esprincipal { get; set; }
        public bool Estaactivo { get; set; }
        public int Numeroverificador { get; set; }

        public Cliente SecuencialclienteNavigation { get; set; }
        public Cuentamaestro SecuencialcuentaNavigation { get; set; }
    }
}

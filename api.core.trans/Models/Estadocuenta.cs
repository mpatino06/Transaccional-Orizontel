using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Estadocuenta
    {
        public Estadocuenta()
        {
            Cuentamaestro = new HashSet<Cuentamaestro>();
            MovimientodetalleCuenta = new HashSet<MovimientodetalleCuenta>();
        }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Estaactivo { get; set; }
        public int Numeroverificador { get; set; }

        public ICollection<Cuentamaestro> Cuentamaestro { get; set; }
        public ICollection<MovimientodetalleCuenta> MovimientodetalleCuenta { get; set; }
    }
}

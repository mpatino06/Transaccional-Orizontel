using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class TransaccionVista
    {
        public int Secuencialtransaccion { get; set; }
        public bool Esaperturacuenta { get; set; }
        public bool Escierrecuenta { get; set; }
        public bool Paraprotestocheque { get; set; }
        public bool Paraefectivizacioncheque { get; set; }
        public bool Paraacreditarprestamoautomatic { get; set; }
        public bool Enlote { get; set; }
        public int Numeroverificador { get; set; }
        public bool Paraanexo2 { get; set; }
        public bool Trabajaconcodigocuenta { get; set; }
        public bool? Esconactivacion { get; set; }
        public bool? Esconinstitucionexterna { get; set; }

        public Transaccion SecuencialtransaccionNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Transaccioncomponente
    {
        public int Secuencial { get; set; }
        public int Secuencialtransaccion { get; set; }
        public int Secuencialcomponente { get; set; }
        public bool Estaactiva { get; set; }
        public int Numeroverificador { get; set; }

        public Transaccion SecuencialtransaccionNavigation { get; set; }
    }
}

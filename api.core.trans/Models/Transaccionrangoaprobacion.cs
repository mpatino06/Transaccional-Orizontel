using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class Transaccionrangoaprobacion
    {
        public int Secuencial { get; set; }
        public int Secuencialtransaccion { get; set; }
        public decimal Montoinicio { get; set; }
        public decimal Montofin { get; set; }
        public string Vistaimprimedocumento { get; set; }
        public bool Clientedefinecontrapartida { get; set; }
        public bool Estaactivo { get; set; }
        public int Numeroverificador { get; set; }

        public Transaccion SecuencialtransaccionNavigation { get; set; }
    }
}

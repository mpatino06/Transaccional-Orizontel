using System;
using System.Collections.Generic;

namespace api.core.trans.Models
{
    public partial class MovimientocomponenteCausal
    {
        public int Secuencial { get; set; }
        public int Secuencialcomponentecausal { get; set; }
        public int Secuencialmovimientodetalle { get; set; }
        public string Concepto { get; set; }
        public decimal Valor { get; set; }
        public string Codigotipomovimiento { get; set; }
        public string Documentoinstitucional { get; set; }
    }
}

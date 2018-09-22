using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.core.trans.Models
{
    public partial class Empresadenominacionfija
    {
        public int Secuencial { get; set; }
        public int Secuencialempresa { get; set; }
        public decimal Denominacion { get; set; }
        public int Orden { get; set; }
        public bool Estaactiva { get; set; }
        public int Numeroverificador { get; set; }
		[NotMapped]
		public int ValueInsert { get; set; }
	}
}

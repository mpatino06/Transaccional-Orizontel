using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public partial class Cuentacontable
	{
		public int SecuecialDivision { get; set; }
		public bool EsDeudora { get; set; }
		public int SecuencialEmpresa { get; set; }
		public bool AfectaManualmente { get; set; }
		public bool RequiereAuxiliar { get; set; }
		public bool EsdeTotal { get; set; }
		public bool EstaActiva { get; set; }
		public int NumeroVerificador { get; set; }
		public DateTime FechaCreacion { get; set; }

	}
}

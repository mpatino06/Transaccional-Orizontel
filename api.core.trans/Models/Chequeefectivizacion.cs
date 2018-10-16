using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class Chequeefectivizacion
	{
		public int SecuencialCheque { get; set; }
		public DateTime FechaSistema { get; set; }
		public DateTime FechaMaquina { get; set; }
		public string CodigoUsuario { get; set; }
		public int SecuencialOficina { get; set; }
		public string  Documento { get; set; }
		public bool EsManual { get; set; }
		public bool EstuvoenTransito { get; set; }
	}
}

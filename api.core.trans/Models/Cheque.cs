using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class Cheque
	{
		public int Secuencial { get; set; }
		public string CodigoCuentaCorriente { get; set; }
		public string CodigoCheque { get; set; }
		public int SecuencialBancoEmisor { get; set; }
		public int SecuencialMoneda { get; set; }
		public decimal Valor { get; set; }
		public string CodigoUsuario { get; set; }
		public bool Estaenboveda { get; set; }
		public DateTime FechaSistemaIngreso { get; set; }
		public DateTime FechaMaquinaIngreso { get; set; }
		public string CodigoEstadoCheque { get; set; }
		public int NumeroVerificador { get; set; }
	}
}

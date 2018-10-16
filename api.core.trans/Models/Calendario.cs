using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class Calendario
	{
		public int Secuencial { get; set; }
		public int SecuencialEmpresa { get; set; }
		public DateTime FechaSistema { get; set; }
		public bool EstaCerrado { get; set; }
		public bool EsFeriado { get; set; }
		public int NumeroVerificador { get; set; }
		public bool EsHabil { get; set; }

	}
}

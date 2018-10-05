using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class ComponenteCuentaContable
	{
		public int Secuencialcomponente { get; set; }
		public int SecuencialCuentaContable { get; set; }
		public bool EstaActivo { get; set; }
		public int NumeroVerificador { get; set; }
	}
}

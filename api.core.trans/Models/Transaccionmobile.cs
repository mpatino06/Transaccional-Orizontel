using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class Transaccionmobile
	{
		public int Secuencial { get; set; }
		public string CodigoUsuario { get; set; }
		public int NumeroCliente { get; set; }
		public DateTime Fecha { get; set; }
		public decimal Monto { get; set; }
		public int Longitud { get; set; }
		public int Latitud { get; set; }
	}
}

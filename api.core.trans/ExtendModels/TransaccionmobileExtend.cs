using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.ExtendModels
{
	public class TransaccionmobileExtend
	{
		public int Secuencial { get; set; }
		public string CodigoUsuario { get; set; }
		public int NumeroCliente { get; set; }
		public string NombreCliente { get; set; }
		public DateTime Fecha { get; set; }
		public decimal Monto { get; set; }
		public decimal Longitud { get; set; }
		public decimal Latitud { get; set; }
	}
}

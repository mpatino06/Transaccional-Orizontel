using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class Ruta
	{
		public int Secuencial { get; set; }
		public int Secuencialbancoemisor { get; set; }
		public int Secuencialbancodeposito { get; set; }
		public int Diastransito { get; set; }
		public bool Estaactivo { get; set; }
		public int Numeroverificador { get; set; }
	}
}

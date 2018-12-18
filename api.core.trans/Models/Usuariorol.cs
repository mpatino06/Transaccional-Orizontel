using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{

	public partial class Usuariorol
	{
		public int Secuencial { get; set; }
		public string CodigoUsuario { get; set; }
		public string CodigoRol { get; set; }
		public bool EstaActivo { get; set; }
		public int NumeroVerificador { get; set; }
	}
}

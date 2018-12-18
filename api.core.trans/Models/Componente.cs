using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public partial class Componente
	{
		public int Secuencial { get; set; }
		public string Nombre { get; set; }
		public string Siglas { get; set; }
		public int SecuencialEmpresa { get; set; }
		public bool EsOperativo { get; set; }
		public bool MovimientoenBloque {get; set;}
		public string CodigoTipoProducto { get; set; }
		public bool RequiereCuentaContable { get; set; }
		public bool EstaActivo { get; set; }
		public int NumeroVerificador { get; set; }
	}
}

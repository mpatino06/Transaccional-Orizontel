using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.ExtendModels
{
	public class ClienteCuentas
	{
		public int Secuencial { get; set; }
		public string Codigo { get; set; }
		public string NombreCuenta { get; set; }
		public int CodigoTipoCuenta { get; set; }
		public int CodigoProductoVista { get; set; }
		public string NombreProducto { get; set; }
		public string CodigoEstado { get; set; }
		public string NombreEstado { get; set; }
		public int SecuencialMoneda { get; set; }
		public int SecuencialOficina { get; set; }
		public string NombreDivision { get; set; }
		public string CodigoUsuarioOficial { get; set; }
		public DateTime FechaSistemaCreacion { get; set; }
		public DateTime FechaMaquinaCreacion { get; set; }
		public int NumeroLibreta { get; set; }
		public bool EsAnverso { get; set; }
		public bool TieneSeguroActivo { get; set; }
		public DateTime FechaCorte { get; set; }
		public bool BloqeadaTransaccionOperativa { get; set; }
		public int NumeroVerificador { get; set; }

	}
}

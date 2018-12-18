using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public partial class Movimientocontrol
	{
		public int Secuencial { get; set; }
		public bool EsDebito { get; set; }
		public decimal Valor { get; set; }
		public int SecuencialOficina { get; set; }
		public int SecuencialMoneda { get; set; }
		public string Documento { get; set; }
		public DateTime Fecha { get; set; }
		public DateTime FechaMaquina { get; set; }
		public string CodigoUsuario { get; set; }
		public int NumeroVerificador { get; set; }
	}
}

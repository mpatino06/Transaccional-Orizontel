using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class Transacciontipomovimiento
	{
		public int Secuencial { get; set; }
		public int SecuencialTransaccion { get; set; }
		public string Codigotipomovimiento { get; set; }
		public bool Estaactivo { get; set; }
		public int Numeroverificador { get; set; }
		[NotMapped]
		public int ValueInsert { get; set; }
	}
}

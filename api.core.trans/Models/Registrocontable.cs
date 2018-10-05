using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class Registrocontable
	{
		public int Secuencial { get; set; }
		public decimal Valor { get; set; }
		public bool Esdebito { get; set; }
		public string Documento { get; set; }
		public string Detalle { get; set; }
		public bool Estacontabilizado { get; set; }
		public int Secuencialcuentacontable { get; set; }
		public int Secuencialoficina { get; set; }
		public int Secuencialperfilcontable { get; set; }
		public string Codigousuario { get; set; }
		public int Secuencialmoneda { get; set; }
		public DateTime Fechasistemaregistro { get; set; }
		public DateTime Fechamaquinaregistro { get; set; }
		public int Secuencialmovimientodetalle { get; set; }
		public int Secuencialmovimientocontrol { get; set; }
		public bool Estaactiva { get; set; }
		public bool Generarcheque { get; set; }
		public bool Esreverso { get; set; }
		public int Numeroverificador { get; set; }

	}
}

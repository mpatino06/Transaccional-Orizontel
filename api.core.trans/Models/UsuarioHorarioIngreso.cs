using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.Models
{
	public class UsuarioHorarioIngreso
	{
		public int Secuencial { get; set; }
		public string CodigoUsuario { get; set; }
		public string Dia { get; set; }
		public int HoraInicio { get; set; }
		public int MinutoInicio { get; set; }
		public int HorasValidez { get; set; }
		public int MinutosValidez { get; set; }
		public bool EstaActivo { get; set; }
		public int NumeroVerificador { get; set; }


	}
}

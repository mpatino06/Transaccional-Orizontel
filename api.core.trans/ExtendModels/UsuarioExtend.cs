using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.Models;

namespace api.core.trans.ExtendModels
{
	public class UsuarioExtend
	{
		public Usuario usuario { get; set; }

		public UsuarioComplemento usuarioComplemento {get; set;}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.ExtendModels;

namespace api.core.trans.Interface
{
	public interface IUsuario
	{
		UsuarioExtend GetUsuarioByCode(string code);
	}
}

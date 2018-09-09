using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.ExtendModels;
using api.core.trans.Interface;
using api.core.trans.Models;

namespace api.core.trans.Services
{
	public class UsuarioServices : IUsuario
	{
		private FBS_SacPelileoContext context;
		public UsuarioServices()
		{
			context = new FBS_SacPelileoContext();
		}

		public UsuarioExtend GetUsuarioByCode(string code)
		{
			UsuarioExtend usuarioExtend = new UsuarioExtend();
			try
			{
				usuarioExtend.usuario = context.Usuario.FirstOrDefault(a => a.Codigo.ToUpper() == code.ToUpper());
				if (usuarioExtend != null)
				{
					usuarioExtend.usuarioComplemento = context.UsuarioComplemento.FirstOrDefault(a => a.Codigousuario.ToUpper() == code.ToUpper());
				}
			}
			catch (Exception ex)
			{
				usuarioExtend = null;
				throw;
			}
			return usuarioExtend;
		}
	}
}

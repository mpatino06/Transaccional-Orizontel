using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.Interface;
using api.core.trans.Models;
using api.core.trans.Services;
using api.core.trans.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.core.trans.Controllers
{
    [Produces("application/json")]
    [Route("OR/Usuario")]
    public class UsuarioController : Controller
    {
		private IUsuario context;

		public UsuarioController()
		{
			context = new UsuarioServices();
		}

		//[HttpGet("GetUsuarioByCode/{code}/{code2}")]
		//public async Task<IActionResult> GetUsuarioByCode([FromRoute] string code, string code2)
		//{
		//	var result = await Task.Run(() => context.GetUsuarioByCode(code, code2));
		//	if (result == null)
		//		return NotFound("User code not Found");

		//	return Ok(result);
		//}

		[HttpPost("GetUsuarioByCode")]
		public async Task<IActionResult> GetUsuarioByCode([FromBody] Login usuario)
		{
			var result = await Task.Run(() => context.GetUsuarioByCode(usuario));
			if (result == null)
				return NotFound("User code not Found");

			return Ok(result);
		}

	}
}
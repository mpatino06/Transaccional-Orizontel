using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.Interface;
using api.core.trans.Services;
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

		[HttpGet("GetUsuarioByCode/{code}")]
		public async Task<IActionResult> GetUsuarioByCode([FromRoute] string code)
		{
			var result = await Task.Run(() => context.GetUsuarioByCode(code));
			if (result == null)
				return NotFound("User code not Found");

			return Ok(result);
		}

	}
}
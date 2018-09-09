using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.core.trans.Services;

namespace api.core.trans.Controllers
{
    [Produces("application/json")]
    [Route("OR/Cliente")]
    public class ClienteController : Controller
    {

		private ICliente context;

		public ClienteController()
		{
			context = new ClienteServices();
		}

		[HttpGet("GetCliente01/{code}/{seleccion}")]
		public async Task<IActionResult> GetCliente01([FromRoute] int code, int seleccion)
		{
			var result = await Task.Run(() => context.GetCliente(code, seleccion));
			if (result == null)
				return NotFound("Cliente not Found");

			return Ok(result);
		}

		[HttpGet("GetCliente02/{empresa}/{cliente}")]
		public async Task<IActionResult> GetCliente02([FromRoute] int code, int cliente)
		{
			var result = await Task.Run(() => context.GetClienteBySecEmpresaYNumeroCliente(code, cliente));
			if (result == null)
				return NotFound("Cliente not Found");

			return Ok(result);
		}

	}
}
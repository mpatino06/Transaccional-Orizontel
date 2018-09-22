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
    [Route("OR/Transaccion")]
    public class TransaccionController : Controller
    {

		private ITransaccion context;

		public TransaccionController()
		{
			context = new TransaccionServices();
		}


		[HttpGet("GetTransaccion/{code}")]
		public async Task<IActionResult> GetTransaccion([FromRoute] int code)
		{
			var result = await Task.Run(() => context.GetBySecuencial(code));
			if (result == null)
				return NotFound("Transaccion not Found");

			return Ok(result);
		}

		[HttpGet("GetMoneda/{empresa}")]
		public async Task<IActionResult> GetMoneda([FromRoute] int empresa)
		{
			var result = await Task.Run(() => context.GetDenominacionMoneda(empresa));
			if (result == null)
				return NotFound("Transaccion not Found");

			return Ok(result);
		}

		[HttpGet("GetTransaccionTipoMovimiento/{secuencial}")]
		public async Task<IActionResult> GetTransaccionTipoMovimiento([FromRoute] int secuencial)
		{
			var result = await Task.Run(() => context.GetTransacciontipomovimientos(secuencial));
			if (result == null)
				return NotFound("Transaccion not Found");

			return Ok(result);
		}

		[HttpGet("GetTransaccionMoneda/{secuencial}/{empresa}")]
		public async Task<IActionResult> GetTransaccionMoneda([FromRoute] int secuencial, int empresa)
		{
			var result = await Task.Run(() => context.GetTransaccionMonedas(empresa, secuencial));
			if (result == null)
				return NotFound("Transaccion not Found");

			return Ok(result);
		}
	}
}
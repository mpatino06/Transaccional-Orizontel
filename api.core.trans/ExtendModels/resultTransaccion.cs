using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.core.trans.ExtendModels
{
	public class ResultTransaccion
	{
		public bool Result { get; set; }
		public string SecuencialDocumento { get; set; }
		public Decimal Saldodeposito { get; set; }
	}
}

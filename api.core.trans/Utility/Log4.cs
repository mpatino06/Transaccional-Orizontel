using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace api.core.trans.Utility
{
	public class Log4
	{
		public void MainLog(string message)
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

			var logger = LogManager.GetLogger(typeof(Program));

			logger.Error(message);
		}

	}
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using api.core.trans.ExtendModels;
using api.core.trans.Helper;
using api.core.trans.Interface;
using api.core.trans.Models;
using api.core.trans.Utility;

namespace api.core.trans.Services
{
	public class UsuarioServices : IUsuario
	{
		private FBS_SacPelileoContext context;
		private Security security;
		private Log4 errorLog;
		public UsuarioServices()
		{
			context = new FBS_SacPelileoContext();
			security = new Security();
			errorLog = new Log4();

		}

		public UsuarioExtend GetUsuarioByCode(Login login)
		{
			UsuarioExtend usuarioExtend = new UsuarioExtend();
			try
			{
				string diasemana = DateTime.Now.ToString("dddd", new CultureInfo("es-ES")).Replace("é", "e").Replace("á","a");
				string userpass = login.User.ToUpper().Trim() + login.Pass;
				string resultsha1 = security.HashSHA1Decryption(userpass);

				string qry = "";
				qry = "SELECT U.CODIGO, U.NOMBRE ";
				qry += "FROM FBS_SEGURIDADES.USUARIO U JOIN FBS_SEGURIDADES.USUARIO_COMPLEMENTO UC on u.CODIGO = uc.CODIGOUSUARIO ";
				qry += "where u.CODIGO = '" + login.User + "' AND UC.CLAVE = '" + resultsha1 + "'";
				bool horaPermitida = false;
				using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionString))
				{
					SqlDataReader dr = SQLHelper.ExecuteReader(conn, System.Data.CommandType.Text, qry, null);
					if (dr.HasRows)
					{
						while (dr.Read())
						{

							UsuarioComplemento uc = new UsuarioComplemento
							{
								Codigousuario = dr.GetString(0),
								Clave = dr.GetString(1)
							};
							usuarioExtend.usuarioComplemento = uc;
							var resultHorario = context.UsuarioHorarioIngreso.FirstOrDefault(a => a.CodigoUsuario == login.User && a.Dia == diasemana);
							if (resultHorario != null)
							{
								
								var dateHorarioUsuario = TimeSpan.FromHours(resultHorario.HoraInicio) + TimeSpan.FromMinutes(resultHorario.MinutoInicio);
								var dateHorasValides = dateHorarioUsuario + TimeSpan.FromHours(resultHorario.HorasValidez) + TimeSpan.FromMinutes(resultHorario.MinutosValidez);
								var horaActual = TimeSpan.FromHours(DateTime.Now.Hour) + TimeSpan.FromMinutes(DateTime.Now.Minute);

								if ((dateHorarioUsuario <= horaActual) && (dateHorasValides >= horaActual))
									horaPermitida = true;
							}
							usuarioExtend.AccesoUsuario = horaPermitida;

						}

					}
				}
			}
			catch (Exception ex)
			{
				usuarioExtend = null;
				errorLog.MainLog("GetUsuarioByCode ERROR " + ex.Message.ToString());
			}
			return usuarioExtend;
		}
	}
}

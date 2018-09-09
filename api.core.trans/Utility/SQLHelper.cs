using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace api.core.trans.Utility
{
	public class SQLHelper
	{
		private static string CONNECTION_STRING = getConnection();

		public static string ConnectionString { get { return CONNECTION_STRING; } }

		public static int ExecuteNonQuery(SqlConnection conn, string cmdText, SqlParameter[] cmdParms)
		{
			SqlCommand cmd = conn.CreateCommand();
			PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			SqlCommand cmd = conn.CreateCommand();
			using (conn)
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				int val = cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				return val;
			}
		}

		public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			SqlCommand cmd = conn.CreateCommand();
			PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
			var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			return rdr;
		}

		public static DataTable ExecuteDataTable(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			DataTable dt = new DataTable();
			// just doing this cause dr.load fails
			dt.Columns.Add("CustomerID");
			dt.Columns.Add("CustomerName");
			SqlDataReader dr = ExecuteReader(conn, cmdType, cmdText, cmdParms);
			// as of now dr.Load throws a big nasty exception saying its not supported. wip.
			// dt.Load(dr);
			while (dr.Read())
			{
				dt.Rows.Add(dr[0], dr[1]);
			}
			return dt;
		}

		public static DataTable ExecuteDataTableSqlDA(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			System.Data.DataTable dt = new DataTable();
			System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(cmdText, conn);
			da.Fill(dt);
			return dt;
		}

		public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
		{
			SqlCommand cmd = conn.CreateCommand();
			PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
			object val = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			return val;
		}

		private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] commandParameters)
		{
			if (conn.State != ConnectionState.Open)
			{
				conn.Open();
			}
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
			{
				cmd.Transaction = trans;
			}
			cmd.CommandType = cmdType;
			//attach the command parameters if they are provided
			if (commandParameters != null)
			{
				AttachParameters(cmd, commandParameters);
			}
		}

		private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
		{
			foreach (SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}
				command.Parameters.Add(p);
			}
		}

		public static string getConnection()
		{
			var builder = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("appsettings.json");

			var connectionStringConfig = builder.Build();

			return connectionStringConfig.GetConnectionString("DefaultConnection");
		}

	}
}
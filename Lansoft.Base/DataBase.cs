/* -------------------------------------------------------------
	Archivo: DataBase.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/22
----------------------------------------------------------------*/

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Lansoft.Base
{
	public class DataBase : IDataBase
	{
		#region IDataBase

		public void BeginTransaction()
		{
			if (ConnectionState == DBConnectionState.Open)
				Transaction = Connection.BeginTransaction();
		}
		public void Close()
		{
			// Si conexion esta ya cerrada, no hace nada
			if (ConnectionState == DBConnectionState.Closed)
				return;

			Connection.Close();
		}
		public DbConnection Connection { get; set; }
		public DBConnectionState ConnectionState
		{
			get
			{
				return Connection != null ?
					(DBConnectionState)Enum.Parse(typeof(DBConnectionState), Connection.State.ToString())
					: DBConnectionState.Closed;
			}
		}
		public string ConnectionString { get; set; }
		public void Commit()
		{
			if (Transaction != null)
			{
				Transaction.Commit();
				Transaction.Dispose();
				Transaction = null;
			}
		}
		public void Dispose()
		{
			Close();
		}
		public void Execute(string comando)
		{
			Open();
			DbCommand dbCmd = Connection.CreateCommand();
			dbCmd.CommandText = comando;
			dbCmd.CommandType = CommandType.Text;
			dbCmd.ExecuteNonQuery();
			Close();
		}
		public virtual void Open()
		{
			// Si conexion esta ya abierta, no hace nada
			if (ConnectionState == DBConnectionState.Open)
				return;

			// Connection String
			ConnectionStringSettings connStrSetts = ConfigurationManager.ConnectionStrings[ConnectionString];
			string connectionString = connStrSetts != null ? connStrSetts.ConnectionString : ConnectionString;
			string providerName = ProviderName != null ? ProviderName : (connStrSetts != null && connStrSetts.ProviderName != null ? connStrSetts.ProviderName : string.Empty);

			// Open connection
			try
			{
				DbProviderFactory DbFactory = DbProviderFactories.GetFactory(providerName);
				Connection = DbFactory.CreateConnection();
				Connection.ConnectionString = connectionString;
				Connection.Open();
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error en Connection.Open(). providerName = '{0}', connectionString = '{1}'", providerName, connectionString), ex);
			}
		}
		public string ProviderName { get; set; }
		public void RollBack()
		{
			if (Transaction != null)
			{
				Transaction.Rollback();
				Transaction.Dispose();
				Transaction = null;
			}
		}
		public DbTransaction Transaction { get; private set; }

		#endregion

		#region Constructores

		public DataBase() : this(null, null) { }
		public DataBase(string connectionString) : this(connectionString, null) { }
		public DataBase(string connectionString, string providerName)
		{
			ConnectionString = connectionString;
			ProviderName = providerName;
		}

		#endregion
	}
}

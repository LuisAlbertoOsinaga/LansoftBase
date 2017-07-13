/* -------------------------------------------------------------
	Archivo: DataBaseSQLite.cs
	Modulo: Lansoft.Base.SQLite
	Autor: LAOS
	Actualizado: 2010/septiembre/27
----------------------------------------------------------------*/

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace Lansoft.Base
{
	public class DataBaseSQLite : DataBase, IDataBaseSQLite
	{
		#region Propiedades

		private int BlockSize { get; set; }
		private byte[] ByteArrayPwd { get; set; }
		private string StringPwd { get; set; }

		#endregion

		#region IDataBaseSQLite

		public void SetPassword()
		{
			SetPassword(1001, "PASE_SECRETO", "GUSTO_SALADO");
		}
		public void SetPassword(int iteraciones, string password, string saltValue)
		{
			BlockSize = 128;
			byte[] bytesSalt = Encoding.Unicode.GetBytes(saltValue);
			Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, bytesSalt, iteraciones);
			SetPassword(deriveBytes.GetBytes(BlockSize / 8));
		}
		public void SetPassword(byte[] password)
		{
			ByteArrayPwd = password;
		}
		public void SetPassword(string password)
		{
			StringPwd = password;
		}
 
		#endregion

		#region IDataBase

		public override void  Open()
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
				if (ByteArrayPwd != null && ByteArrayPwd.Length > 0)
				{
					SQLiteConnection liteConnection = Connection as SQLiteConnection;
					liteConnection.SetPassword(ByteArrayPwd);
				}
				else if (StringPwd != null && !string.IsNullOrEmpty(StringPwd))
				{
					SQLiteConnection liteConnection = Connection as SQLiteConnection;
					liteConnection.SetPassword(StringPwd);
				}
				Connection.Open();
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error en Connection.Open(). providerName = '{0}', connectionString = '{1}'", providerName, connectionString), ex);
			}
		}

		#endregion

		#region Constructores

		public DataBaseSQLite() : this(null, null) { }
		public DataBaseSQLite(string connectionString) : this(connectionString, null) { }
		public DataBaseSQLite(string connectionString, string providerName)
			: base(connectionString, providerName)
		{
		}

		#endregion
	}
}

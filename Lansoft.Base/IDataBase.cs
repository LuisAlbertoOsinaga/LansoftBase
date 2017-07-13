/* -------------------------------------------------------------
	Archivo: IDataBase.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/22
----------------------------------------------------------------*/

using System;
using System.Data.Common;

namespace Lansoft.Base
{
	public interface IDataBase : IDisposable
	{
		void BeginTransaction();
		void Close();
		DbConnection Connection { get; set; }
		DBConnectionState ConnectionState { get; }
		string ConnectionString { get; set; }
		void Commit();
		void Execute(string comando);
		void Open();
		string ProviderName { get; set; }
		void RollBack();
		DbTransaction Transaction { get; }
	}
}

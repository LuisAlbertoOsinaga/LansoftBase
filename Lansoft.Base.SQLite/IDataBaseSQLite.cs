/* -------------------------------------------------------------
	Archivo: IDataBaseSQLite.cs
	Modulo: Lansoft.Base.SQLite
	Autor: LAOS
	Actualizado: 2010/septiembre/27
----------------------------------------------------------------*/

using System;
using System.Data.Common;

namespace Lansoft.Base
{
	public interface IDataBaseSQLite : IDataBase
	{
		void SetPassword();
		void SetPassword(int iteraciones, string password, string saltValue);
		void SetPassword(byte[] password);
		void SetPassword(string password);
	}
}
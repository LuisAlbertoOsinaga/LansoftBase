/* -------------------------------------------------------------
	Archivo: ICrudReader.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/octubre/01
----------------------------------------------------------------*/

using System;

namespace Lansoft.Base
{
	public interface ICrudReader<TEntity> : ICrud<TEntity>
	{
		void CloseReader();
		TEntity Read();
		void OpenReaderAll();
		void OpenReaderWhere(string where);
		void OpenReaderWhereOrderBy(string where, string orderby);
	}
}

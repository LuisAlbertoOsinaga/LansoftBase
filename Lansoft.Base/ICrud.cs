/* -------------------------------------------------------------
	Archivo: ICrud.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/mayo/21
----------------------------------------------------------------*/

using System;

namespace Lansoft.Base
{
	public interface ICrud<TEntity>
	{
		// DataBase
		IDataBase DB { get; set; }

		// Create
		bool Insert(TEntity entity);

		// Retrieve
		TEntity Get(TEntity entity);
		TEntity[] GetAll();
		TEntity[] GetWhere(string where);
		TEntity[] GetWhereOrderBy(string where, string orderby);
		
		// Update
		bool Update(TEntity entity);
		
		// Delete
		bool Delete(TEntity entity);
		bool DeleteAll();
	}
}


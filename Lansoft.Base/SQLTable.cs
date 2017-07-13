/* -------------------------------------------------------------
	Archivo: SQLTable.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/octubre/01
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Lansoft.Base
{
	public class SQLTable<TEntity> : ICrudReader<TEntity>
		where TEntity : class, IEntity, new()
	{
		#region ICrud

		public IDataBase DB { get; set; }
		public bool Delete(TEntity entity)
		{
			return Execute(entity, DeleteText);
		}
		public bool DeleteAll()
		{
			return Execute(null, DeleteAllText);
		}
		public TEntity Get(TEntity entity)
		{
			return ReadEntity(entity, SelectText);
		}
		public TEntity[] GetAll()
		{
			return ReadEntities(null, null, SelectAllText);
		}
		public TEntity[] GetWhere(string where)
		{
			return ReadEntities(where, null, SelectWhereText);
		}
		public TEntity[] GetWhereOrderBy(string where, string orderby)
		{
			return ReadEntities(where, orderby, SelectWhereOrderByText);
		}
		public bool Insert(TEntity entity)
		{
			return Execute(entity, InsertText);
		}
		public bool Update(TEntity entity)
		{
			return Execute(entity, UpdateText);
		}

		#endregion

		#region ICrudReader

		public void CloseReader()
		{
			if (Reader != null)
			{
				Reader.Close();
				Reader = null;
			}
		}
		public TEntity Read()
		{
			if (Reader == null)
				throw new ApplicationException("Error en SQLTable.Read. Reader null.");
			
			if (!Reader.Read())
				return null;
			
			TEntity entity = new TEntity();
			FillFields(Reader, entity);
			return entity;
		}
		public void OpenReaderAll()
		{
			SetReader(null, null, SelectAllText);
		}
		public void OpenReaderWhere(string where)
		{
			SetReader(where, null, SelectWhereText);
		}
		public void OpenReaderWhereOrderBy(string where, string orderby)
		{
			SetReader(where, orderby, SelectWhereOrderByText);
		}
		
		#endregion

		#region Constructors

		public SQLTable()
			: this(null)
		{
		}
		public SQLTable(IDataBase dataBase)
		{
			DB = dataBase;
		}

		#endregion

		#region Private Methods


		protected virtual string DeleteAllText(TEntity entity)
		{
			// Arma comando delete
			string tableName = typeof(TEntity).Name;
			return string.Concat("DELETE FROM ", tableName);
		}
		protected virtual string DeleteText(TEntity entity)
		{
			// Arma clausula where
			string where = WhereKeys(entity);

			// Arma comando delete
			string tableName = typeof(TEntity).Name;
			return string.Concat("DELETE FROM ", tableName, " WHERE ", where);
		}
		protected virtual bool Execute(DbCommand dbCmd)
		{
			dbCmd.ExecuteNonQuery();
			return true;
		}
		protected virtual bool Execute(TEntity entity, Func<TEntity, string> CommandText)
		{
			DbCommand dbCmd = null;
			try
			{
				dbCmd = DB.Connection.CreateCommand();
				dbCmd.CommandText = CommandText(entity);
				if (entity != null)
				{
					List<string> fields = entity.Fields;
					if (!string.IsNullOrEmpty(entity.FieldHash))
						fields.Add(entity.FieldHash);
					foreach (string field in fields)
					{
						DbParameter param = dbCmd.CreateParameter();
						param.ParameterName = string.Format("@{0}",field);
						param.Value = field != entity.FieldHash ? entity.GetValue(field) : entity.FieldsHashCode;
						dbCmd.Parameters.Add(param);
					}
				}

				return Execute(dbCmd);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error en SQLTable.Execute(entity, commandText). {0}", dbCmd.CommandText), ex);
			}
		}
		protected virtual void FillFields(DbDataReader dbReader, TEntity entityGet)
		{
			// Llena campos
			List<string> fields = entityGet.Fields;
			fields.ForEach(propertyName => entityGet.SetValue(propertyName, dbReader[propertyName] != DBNull.Value ? dbReader[propertyName] : null));

			// Verifica hash
			if (!string.IsNullOrEmpty(entityGet.FieldHash))
			{
				entityGet.SetValue(entityGet.FieldHash, dbReader[entityGet.FieldHash]);
				string hash = (string) entityGet.GetValue(entityGet.FieldHash);
				if(hash != entityGet.FieldsHashCode)
				{
					string mensajeError = string.Format("Error en FillFields(dbReader, entityGet). Entity de tipo '{0}' no verifica codigo hash.", entityGet.GetType().Name);
					throw new System.Security.SecurityException(mensajeError);
				}
			}
		}
		protected virtual string InsertText(TEntity entity)
		{
			// Arma clausulas fields y values
			string fields = NameFields(entity);
			string values = ValueFields(entity);

			// Arma comando insert
			string tableName = typeof(TEntity).Name;
			return string.Concat("INSERT INTO ", tableName, fields, " VALUES ", values);
		}
		protected virtual string NameFields(TEntity entity)
		{
			List<string> fields = entity.Fields;
			if (!string.IsNullOrEmpty(entity.FieldHash))
				fields.Add(entity.FieldHash);
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach (string key in fields)
			{
				if (i == 0)
					sb.Append("(");
				
				sb.Append(key);
				
				if (i++ == fields.Count - 1)
					sb.Append(")");
				else
					sb.Append(",");
			}
			return sb.ToString();
		}
		protected virtual TEntity[] ReadEntities(DbCommand dbCommand)
		{
			using (DbDataReader dbReader = dbCommand.ExecuteReader())
			{
				List<TEntity> entities = new List<TEntity>();
				while (dbReader.Read())
				{
					TEntity entity = new TEntity();
					FillFields(dbReader, entity);
					entities.Add(entity);
				}

				return entities.Count > 0 ? entities.ToArray<TEntity>() : null;
			}
		}
		protected virtual TEntity[] ReadEntities(string where, string orderby, Func<string, string, string> commandText)
		{
			DbCommand dbCmd = null;
			try
			{
				dbCmd = DB.Connection.CreateCommand();
				dbCmd.CommandText = commandText(where, orderby);
				return ReadEntities(dbCmd);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error en SQLTable.ReadEntities(where, orderby, commandText). {0}", dbCmd.CommandText), ex);
			}
		}
		protected virtual TEntity ReadEntity(DbCommand dbCommand)
		{
			using (DbDataReader dbReader = dbCommand.ExecuteReader())
			{
				if (!dbReader.Read())
					return null;
				TEntity entityGet = new TEntity();
				FillFields(dbReader, entityGet);

				return entityGet;
			}
		}
		protected virtual TEntity ReadEntity(TEntity entity, Func<TEntity, string> commandText)
		{
			DbCommand dbCmd = null;
			try
			{
				dbCmd = DB.Connection.CreateCommand();
				dbCmd.CommandText = commandText(entity);
				return ReadEntity(dbCmd);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error en SQLTable.ReadEntity(entity, commandText). {0}", dbCmd.CommandText), ex);
			}
		}
		protected virtual string SelectAllText(string where, string orderby)
		{
			// Arma comando select
			string tableName = typeof(TEntity).Name;
			return string.Concat("SELECT * FROM ", tableName);
		}
		protected virtual string SelectText(TEntity entity)
		{
			// Arma clausula where
			string where = WhereKeys(entity);

			// Arma comando select
			string tableName = typeof(TEntity).Name;
			return string.Concat("SELECT * FROM ", tableName, " WHERE ", where);
		}
		protected virtual string SelectWhereText(string where, string orderby)
		{
			// Arma comando select
			string tableName = typeof(TEntity).Name;
			return string.Concat("SELECT * FROM ", tableName, " WHERE ", where);
		}
		protected virtual string SelectWhereOrderByText(string where, string orderby)
		{
			// Arma comando select
			string tableName = typeof(TEntity).Name;
			return string.Concat("SELECT * FROM ", tableName, " WHERE ", where, " ORDER BY ", orderby);
		}
		protected virtual string SetFields(TEntity entity)
		{
			List<string> fields = entity.Fields;
			if (!string.IsNullOrEmpty(entity.FieldHash))
				fields.Add(entity.FieldHash);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < fields.Count; i++)
			{
				if (i == 0) sb.Append(" SET ");
				sb.AppendFormat("{0}=", fields[i]);
				sb.AppendFormat("@{0}", fields[i]);
				sb.Append(i == fields.Count - 1 ? " " : ",");
			}
			return sb.ToString();
		}
		protected virtual void SetReader(string where, string orderby, Func<string, string, string> commandText)
		{
			DbCommand dbCmd = null;
			try
			{
				dbCmd = DB.Connection.CreateCommand();
				dbCmd.CommandText = commandText(where, orderby);
				Reader = dbCmd.ExecuteReader();
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error en SQLTable.SetReader(where, orderby, commandText). {0}", dbCmd.CommandText), ex);
			}
		}
		protected virtual string UpdateText(TEntity entity)
		{
			// Arma clausula where y set
			string where = WhereKeys(entity);
			string sets = SetFields(entity);

			// Arma comando update
			string tableName = typeof(TEntity).Name;
			return string.Concat("UPDATE ", tableName, sets, " WHERE ", where);
		}
		protected virtual string ValueFields(TEntity entity)
		{
			List<string> fields = entity.Fields;
			if (!string.IsNullOrEmpty(entity.FieldHash))
				fields.Add(entity.FieldHash);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < fields.Count; i++)
			{
				if (i == 0) sb.Append("(");
				sb.AppendFormat("@{0}", fields[i]);
				sb.Append(i == fields.Count - 1 ? ")" : ",");
			}
			return sb.ToString();
		}
		protected virtual string WhereKeys(TEntity entity)
		{
			List<string> keys = entity.FieldKeys;
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach (string key in keys)
			{
				if (i++ > 0)
					sb.Append(" AND ");
				sb.Append(key);
				sb.Append(" = ");
				object value = entity.GetValue(key);
				if (value.GetType().Name == "String")
				{
					sb.Append("'");
					sb.Append((string)value);
					sb.Append("'");
				}
				else
				{
					sb.Append(value.ToString());
				}
			}
			return sb.ToString();
		}

		#endregion

		#region Private Properties

		private DbDataReader Reader { get; set; }

		#endregion
	}
}
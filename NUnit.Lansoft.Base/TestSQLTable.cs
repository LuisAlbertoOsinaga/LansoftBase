/* -------------------------------------------------------------
	Archivo: TestSQLTable.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/octubre/04
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Lansoft.Base;

namespace NUnit.Lansoft.Base
{
	[TestFixture]
	public class TestSQLTable
	{
		[Test]
		public void SQLTable_Delete_TEntity_ret_bool()
		{
			#region Prepara

			EntityDemo entity = new EntityDemo
			{
				Clave1 = "KeyDel01",
				Clave2 = "KeyDel02",
				Campo1 = "FieldDel01",
				Campo2 = "FielDeld02",
				Campo3 = "Fieldeld03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.Insert(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			bool result = tableDemo.Delete(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			dbDemo.Open();
			EntityDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();
			Assert.IsNull(entityGet);
		}

		[Test]
		public void SQLTable_DeleteAll_ret_bool()
		{
			#region Prepara

			EntityDemo entity = new EntityDemo
			{
				Clave1 = "KeyDelAll01",
				Clave2 = "KeyDelAll02",
				Campo1 = "FieldDelAll01",
				Campo2 = "FieldDelAll02",
				Campo3 = "FieldDelAll03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.Insert(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			bool result = tableDemo.DeleteAll();
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			dbDemo.Open();
			EntityDemo[] entityGets = tableDemo.GetAll();
			dbDemo.Close();
			Assert.IsNull(entityGets);
		}

		[Test]
		public void SQLTable_Get_TEntity_ret_TEntity()
		{
			#region Prepara

			EntityDemo entity = new EntityDemo
			{
				Clave1 = "KeyGet01",
				Clave2 = "KeyGet02",
				Campo1 = "FieldGet01",
				Campo2 = "FieldGet02",
				Campo3 = "FieldGet03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.Insert(entity);
				entity.Campo1 = string.Empty;
				entity.Campo2 = string.Empty;
				entity.Campo3 = string.Empty;
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			EntityDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual("KeyGet01", entityGet.Clave1);
			Assert.AreEqual("KeyGet02", entityGet.Clave2);
			Assert.AreEqual("FieldGet01", entityGet.Campo1);
			Assert.AreEqual("FieldGet02", entityGet.Campo2);
			Assert.AreEqual("FieldGet03", entityGet.Campo3);
		}

		[Test]
		public void SQLTable_Get_TEntity_SQLServer_DBNull_ret_TEntity()
		{
			#region Prepara

			EntityDemo2 entity = new EntityDemo2
			{
				Clave = 4010000511,
				// Campo = System.DBNull
				// Imagen = System.DBNull
			};

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB_SQL" };
			dbDemo.Open();
			SQLTable<EntityDemo2> tableDemo = new SQLTable<EntityDemo2> { DB = dbDemo };
			EntityDemo2 entityGet = tableDemo.Get(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(4010000511, entityGet.Clave);
			Assert.AreEqual(null, entityGet.Campo);
			Assert.AreEqual(null, entityGet.Imagen);
		}

		[Test]
		public void SQLTable_Get_TEntity_WithHash_ret_TEntity()
		{
			#region Prepara

			EntityHashDemo entity = new EntityHashDemo
			{
				Clave1 = "KeyGet01",
				Clave2 = "KeyGet02",
				Campo1 = "FieldGet01",
				Campo2 = "FieldGet02",
				Campo3 = "FieldGet03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityHashDemo> table = new SQLTable<EntityHashDemo> { DB = db };
				table.Insert(entity);
				entity.Campo1 = string.Empty;
				entity.Campo2 = string.Empty;
				entity.Campo3 = string.Empty;
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityHashDemo> tableDemo = new SQLTable<EntityHashDemo> { DB = dbDemo };
			EntityHashDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual("KeyGet01", entityGet.Clave1);
			Assert.AreEqual("KeyGet02", entityGet.Clave2);
			Assert.AreEqual("FieldGet01", entityGet.Campo1);
			Assert.AreEqual("FieldGet02", entityGet.Campo2);
			Assert.AreEqual("FieldGet03", entityGet.Campo3);
			Assert.AreEqual(entityGet.Hash, entityGet.FieldsHashCode);
		}

		[Test]
		public void SQLTable_Get_TEntity_WithHashEncrypt_ret_TEntity()
		{
			#region Prepara

			EntityEncryptHashDemo entity = new EntityEncryptHashDemo
			{
				Clave1 = "KeyGet_EHD_01",
				Clave2 = "KeyGet_EHD_02",
				Campo1 = "FieldGet_EHD_01",
				Campo2 = "FieldGet_EHD_02",
				Campo3 = "FieldGet_EHD_03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityEncryptHashDemo> table = new SQLTable<EntityEncryptHashDemo> { DB = db };
				table.Insert(entity);
				entity.Campo1 = string.Empty;
				entity.Campo2 = string.Empty;
				entity.Campo3 = string.Empty;
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityEncryptHashDemo> tableDemo = new SQLTable<EntityEncryptHashDemo> { DB = dbDemo };
			EntityEncryptHashDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual("KeyGet_EHD_01", entityGet.Clave1);
			Assert.AreEqual("KeyGet_EHD_02", entityGet.Clave2);
			Assert.AreEqual("FieldGet_EHD_01", entityGet.Campo1);
			Assert.AreEqual("FieldGet_EHD_02", entityGet.Campo2);
			Assert.AreEqual("FieldGet_EHD_03", entityGet.Campo3);
			Assert.AreEqual(entityGet.Hash, entityGet.FieldsHashCode);
		}

		[Test]
		public void SQLTable_GetAll_ret_ArrayTEntity()
		{
			#region Prepara

			EntityDemo entity1 = new EntityDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "Field1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityDemo entity2 = new EntityDemo
			{
				Clave1 = "Key2Get01",
				Clave2 = "Key2Get02",
				Campo1 = "Field2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityDemo entity3 = new EntityDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "Field3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "Field3Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			EntityDemo[] entityGets = tableDemo.GetAll();
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(3, entityGets.Length);
			Assert.AreEqual("Key1Get01", entityGets[0].Clave1);
			Assert.AreEqual("Key1Get02", entityGets[0].Clave2);
			Assert.AreEqual("Field1Get01", entityGets[0].Campo1);
			Assert.AreEqual("Field1Get02", entityGets[0].Campo2);
			Assert.AreEqual("Field1Get03", entityGets[0].Campo3);
			Assert.AreEqual("Key2Get01", entityGets[1].Clave1);
			Assert.AreEqual("Key2Get02", entityGets[1].Clave2);
			Assert.AreEqual("Field2Get01", entityGets[1].Campo1);
			Assert.AreEqual("Field2Get02", entityGets[1].Campo2);
			Assert.AreEqual("Field2Get03", entityGets[1].Campo3);
			Assert.AreEqual("Key3Get01", entityGets[2].Clave1);
			Assert.AreEqual("Key3Get02", entityGets[2].Clave2);
			Assert.AreEqual("Field3Get01", entityGets[2].Campo1);
			Assert.AreEqual("Field3Get02", entityGets[2].Campo2);
			Assert.AreEqual("Field3Get03", entityGets[2].Campo3);
		}

		[Test]
		public void SQLTable_GetAll_ret_WithEncryptHash_TEntity()
		{
			#region Prepara

			EntityEncryptHashDemo entity1 = new EntityEncryptHashDemo
			{
				Clave1 = "Key1Get_EHD_01",
				Clave2 = "Key1Get_EHD_02",
				Campo1 = "Field1Get_EHD_01",
				Campo2 = "Field1Get_EHD_02",
				Campo3 = "Field1Get_EHD_03"
			};
			EntityEncryptHashDemo entity2 = new EntityEncryptHashDemo
			{
				Clave1 = "Key2Get_EHD_01",
				Clave2 = "Key2Get_EHD_02",
				Campo1 = "Field2Get_EHD_01",
				Campo2 = "Field2Get_EHD_02",
				Campo3 = "Field2Get_EHD_03"
			};
			EntityEncryptHashDemo entity3 = new EntityEncryptHashDemo
			{
				Clave1 = "Key3Get_EHD_01",
				Clave2 = "Key3Get_EHD_02",
				Campo1 = "Field3Get_EHD_01",
				Campo2 = "Field3Get_EHD_02",
				Campo3 = "Field3Get_EHD_03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityEncryptHashDemo> table = new SQLTable<EntityEncryptHashDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityEncryptHashDemo> tableDemo = new SQLTable<EntityEncryptHashDemo> { DB = dbDemo };
			EntityEncryptHashDemo[] entityGets = tableDemo.GetAll();
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(3, entityGets.Length);
			Assert.AreEqual("Key1Get_EHD_01", entityGets[0].Clave1);
			Assert.AreEqual("Key1Get_EHD_02", entityGets[0].Clave2);
			Assert.AreEqual("Field1Get_EHD_01", entityGets[0].Campo1);
			Assert.AreEqual("Field1Get_EHD_02", entityGets[0].Campo2);
			Assert.AreEqual("Field1Get_EHD_03", entityGets[0].Campo3);
			Assert.AreEqual(entityGets[0].Hash, entityGets[0].FieldsHashCode);
			Assert.AreEqual("Key2Get_EHD_01", entityGets[1].Clave1);
			Assert.AreEqual("Key2Get_EHD_02", entityGets[1].Clave2);
			Assert.AreEqual("Field2Get_EHD_01", entityGets[1].Campo1);
			Assert.AreEqual("Field2Get_EHD_02", entityGets[1].Campo2);
			Assert.AreEqual("Field2Get_EHD_03", entityGets[1].Campo3);
			Assert.AreEqual(entityGets[1].Hash, entityGets[1].FieldsHashCode);
			Assert.AreEqual("Key3Get_EHD_01", entityGets[2].Clave1);
			Assert.AreEqual("Key3Get_EHD_02", entityGets[2].Clave2);
			Assert.AreEqual("Field3Get_EHD_01", entityGets[2].Campo1);
			Assert.AreEqual("Field3Get_EHD_02", entityGets[2].Campo2);
			Assert.AreEqual("Field3Get_EHD_03", entityGets[2].Campo3);
			Assert.AreEqual(entityGets[2].Hash, entityGets[2].FieldsHashCode);
		}

		[Test]
		public void SQLTable_GetAll_ret_WithHash_TEntity()
		{
			#region Prepara

			EntityHashDemo entity1 = new EntityHashDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "Field1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityHashDemo entity2 = new EntityHashDemo
			{
				Clave1 = "Key2Get01",
				Clave2 = "Key2Get02",
				Campo1 = "Field2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityHashDemo entity3 = new EntityHashDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "Field3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "Field3Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityHashDemo> table = new SQLTable<EntityHashDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityHashDemo> tableDemo = new SQLTable<EntityHashDemo> { DB = dbDemo };
			EntityHashDemo[] entityGets = tableDemo.GetAll();
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(3, entityGets.Length);
			Assert.AreEqual("Key1Get01", entityGets[0].Clave1);
			Assert.AreEqual("Key1Get02", entityGets[0].Clave2);
			Assert.AreEqual("Field1Get01", entityGets[0].Campo1);
			Assert.AreEqual("Field1Get02", entityGets[0].Campo2);
			Assert.AreEqual("Field1Get03", entityGets[0].Campo3);
			Assert.AreEqual(entityGets[0].Hash, entityGets[0].FieldsHashCode);
			Assert.AreEqual("Key2Get01", entityGets[1].Clave1);
			Assert.AreEqual("Key2Get02", entityGets[1].Clave2);
			Assert.AreEqual("Field2Get01", entityGets[1].Campo1);
			Assert.AreEqual("Field2Get02", entityGets[1].Campo2);
			Assert.AreEqual("Field2Get03", entityGets[1].Campo3);
			Assert.AreEqual(entityGets[1].Hash, entityGets[1].FieldsHashCode);
			Assert.AreEqual("Key3Get01", entityGets[2].Clave1);
			Assert.AreEqual("Key3Get02", entityGets[2].Clave2);
			Assert.AreEqual("Field3Get01", entityGets[2].Campo1);
			Assert.AreEqual("Field3Get02", entityGets[2].Campo2);
			Assert.AreEqual("Field3Get03", entityGets[2].Campo3);
			Assert.AreEqual(entityGets[2].Hash, entityGets[2].FieldsHashCode);
		}

		[Test]
		public void SQLTable_GetWhere_ret_TEntity()
		{
			#region Prepara

			EntityDemo entity1 = new EntityDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "Field1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityDemo entity2 = new EntityDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key2Get02",
				Campo1 = "Field2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityDemo entity3 = new EntityDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "Field3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "Field3Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			EntityDemo[] entityGets = tableDemo.GetWhere("Clave1='Key1Get01'");
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(2, entityGets.Length);
			Assert.AreEqual("Key1Get01", entityGets[0].Clave1);
			Assert.AreEqual("Key1Get02", entityGets[0].Clave2);
			Assert.AreEqual("Field1Get01", entityGets[0].Campo1);
			Assert.AreEqual("Field1Get02", entityGets[0].Campo2);
			Assert.AreEqual("Field1Get03", entityGets[0].Campo3);
			Assert.AreEqual("Key1Get01", entityGets[1].Clave1);
			Assert.AreEqual("Key2Get02", entityGets[1].Clave2);
			Assert.AreEqual("Field2Get01", entityGets[1].Campo1);
			Assert.AreEqual("Field2Get02", entityGets[1].Campo2);
			Assert.AreEqual("Field2Get03", entityGets[1].Campo3);
		}

		[Test]
		public void SQLTable_GetWhere_WithEncryptHash_ret_TEntity()
		{
			#region Prepara

			EntityEncryptHashDemo entity1 = new EntityEncryptHashDemo
			{
				Clave1 = "Key1GetWhere_EHD_01",
				Clave2 = "Key1GetWhere_EHD_02",
				Campo1 = "Field1GetWhere_EHD_01",
				Campo2 = "Field1GetWhere_EHD_02",
				Campo3 = "Field1GetWhere_EHD_03"
			};
			EntityEncryptHashDemo entity2 = new EntityEncryptHashDemo
			{
				Clave1 = "Key1GetWhere_EHD_01",
				Clave2 = "Key2GetWhere_EHD_02",
				Campo1 = "Field2GetWhere_EHD_01",
				Campo2 = "Field2GetWhere_EHD_02",
				Campo3 = "Field2GetWhere_EHD_03"
			};
			EntityEncryptHashDemo entity3 = new EntityEncryptHashDemo
			{
				Clave1 = "Key3GetWhere_EHD_01",
				Clave2 = "Key3GetWhere_EHD_02",
				Campo1 = "Field3GetWhere_EHD_01",
				Campo2 = "Field3GetWhere_EHD_02",
				Campo3 = "Field3GetWhere_EHD_03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityEncryptHashDemo> table = new SQLTable<EntityEncryptHashDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityEncryptHashDemo> tableDemo = new SQLTable<EntityEncryptHashDemo> { DB = dbDemo };
			EntityEncryptHashDemo[] entityGets = tableDemo.GetWhere("Clave1='Key1GetWhere_EHD_01'");
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(2, entityGets.Length);
			Assert.AreEqual("Key1GetWhere_EHD_01", entityGets[0].Clave1);
			Assert.AreEqual("Key1GetWhere_EHD_02", entityGets[0].Clave2);
			Assert.AreEqual("Field1GetWhere_EHD_01", entityGets[0].Campo1);
			Assert.AreEqual("Field1GetWhere_EHD_02", entityGets[0].Campo2);
			Assert.AreEqual("Field1GetWhere_EHD_03", entityGets[0].Campo3);
			Assert.AreEqual(entityGets[0].Hash, entityGets[0].FieldsHashCode);
			Assert.AreEqual("Key1GetWhere_EHD_01", entityGets[1].Clave1);
			Assert.AreEqual("Key2GetWhere_EHD_02", entityGets[1].Clave2);
			Assert.AreEqual("Field2GetWhere_EHD_01", entityGets[1].Campo1);
			Assert.AreEqual("Field2GetWhere_EHD_02", entityGets[1].Campo2);
			Assert.AreEqual("Field2GetWhere_EHD_03", entityGets[1].Campo3);
			Assert.AreEqual(entityGets[1].Hash, entityGets[1].FieldsHashCode);
		}

		[Test]
		public void SQLTable_GetWhere_WithHash_ret_TEntity()
		{
			#region Prepara

			EntityHashDemo entity1 = new EntityHashDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "Field1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityHashDemo entity2 = new EntityHashDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key2Get02",
				Campo1 = "Field2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityHashDemo entity3 = new EntityHashDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "Field3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "Field3Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityHashDemo> table = new SQLTable<EntityHashDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityHashDemo> tableDemo = new SQLTable<EntityHashDemo> { DB = dbDemo };
			EntityHashDemo[] entityGets = tableDemo.GetWhere("Clave1='Key1Get01'");
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(2, entityGets.Length);
			Assert.AreEqual("Key1Get01", entityGets[0].Clave1);
			Assert.AreEqual("Key1Get02", entityGets[0].Clave2);
			Assert.AreEqual("Field1Get01", entityGets[0].Campo1);
			Assert.AreEqual("Field1Get02", entityGets[0].Campo2);
			Assert.AreEqual("Field1Get03", entityGets[0].Campo3);
			Assert.AreEqual(entityGets[0].Hash, entityGets[0].FieldsHashCode);
			Assert.AreEqual("Key1Get01", entityGets[1].Clave1);
			Assert.AreEqual("Key2Get02", entityGets[1].Clave2);
			Assert.AreEqual("Field2Get01", entityGets[1].Campo1);
			Assert.AreEqual("Field2Get02", entityGets[1].Campo2);
			Assert.AreEqual("Field2Get03", entityGets[1].Campo3);
			Assert.AreEqual(entityGets[1].Hash, entityGets[1].FieldsHashCode);
		}

		[Test]
		public void SQLTable_GetWhereOrderBy_ret_TEntity()
		{
			#region Prepara

			EntityDemo entity1 = new EntityDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "BField1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityDemo entity2 = new EntityDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key2Get02",
				Campo1 = "AField2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityDemo entity3 = new EntityDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "Field3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "Field3Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			EntityDemo[] entityGets = tableDemo.GetWhereOrderBy("Clave1='Key1Get01'", "Campo1");
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(2, entityGets.Length);
			Assert.AreEqual("Key1Get01", entityGets[0].Clave1);
			Assert.AreEqual("Key2Get02", entityGets[0].Clave2);
			Assert.AreEqual("AField2Get01", entityGets[0].Campo1);
			Assert.AreEqual("Field2Get02", entityGets[0].Campo2);
			Assert.AreEqual("Field2Get03", entityGets[0].Campo3);
			Assert.AreEqual("Key1Get01", entityGets[1].Clave1);
			Assert.AreEqual("Key1Get02", entityGets[1].Clave2);
			Assert.AreEqual("BField1Get01", entityGets[1].Campo1);
			Assert.AreEqual("Field1Get02", entityGets[1].Campo2);
			Assert.AreEqual("Field1Get03", entityGets[1].Campo3);
		}

		[Test]
		public void SQLTable_GetWhereOrderBy_WithEncryptHash_ret_TEntity()
		{
			#region Prepara

			EntityEncryptHashDemo entity1 = new EntityEncryptHashDemo
			{
				Clave1 = "Key1Get_EHF_01",
				Clave2 = "Key1Get_EHF_02",
				Campo1 = "BField1Get_EHF_01",
				Campo2 = "Field1Get_EHF_02",
				Campo3 = "Field1Get_EHF_03"
			};
			EntityEncryptHashDemo entity2 = new EntityEncryptHashDemo
			{
				Clave1 = "Key1Get_EHF_01",
				Clave2 = "Key2Get_EHF_02",
				Campo1 = "AField2Get_EHF_01",
				Campo2 = "Field2Get_EHF_02",
				Campo3 = "Field2Get_EHF_03"
			};
			EntityEncryptHashDemo entity3 = new EntityEncryptHashDemo
			{
				Clave1 = "Key3Get_EHF_01",
				Clave2 = "Key3Get_EHF_02",
				Campo1 = "Field3Get_EHF_01",
				Campo2 = "Field3Get_EHF_02",
				Campo3 = "Field3Get_EHF_03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityEncryptHashDemo> table = new SQLTable<EntityEncryptHashDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityEncryptHashDemo> tableDemo = new SQLTable<EntityEncryptHashDemo> { DB = dbDemo };
			EntityEncryptHashDemo[] entityGets = tableDemo.GetWhereOrderBy("Clave1='Key1Get_EHF_01'", "Campo1");
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(2, entityGets.Length);
			Assert.AreEqual("Key1Get_EHF_01", entityGets[0].Clave1);
			Assert.AreEqual("Key2Get_EHF_02", entityGets[0].Clave2);
			Assert.AreEqual("AField2Get_EHF_01", entityGets[0].Campo1);
			Assert.AreEqual("Field2Get_EHF_02", entityGets[0].Campo2);
			Assert.AreEqual("Field2Get_EHF_03", entityGets[0].Campo3);
			Assert.AreEqual("Key1Get_EHF_01", entityGets[1].Clave1);
			Assert.AreEqual("Key1Get_EHF_02", entityGets[1].Clave2);
			Assert.AreEqual("BField1Get_EHF_01", entityGets[1].Campo1);
			Assert.AreEqual("Field1Get_EHF_02", entityGets[1].Campo2);
			Assert.AreEqual("Field1Get_EHF_03", entityGets[1].Campo3);
		}

		[Test]
		public void SQLTable_GetWhereOrderBy_WithHash_ret_TEntity()
		{
			#region Prepara

			EntityHashDemo entity1 = new EntityHashDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "BField1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityHashDemo entity2 = new EntityHashDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key2Get02",
				Campo1 = "AField2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityHashDemo entity3 = new EntityHashDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "Field3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "Field3Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityHashDemo> table = new SQLTable<EntityHashDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityHashDemo> tableDemo = new SQLTable<EntityHashDemo> { DB = dbDemo };
			EntityHashDemo[] entityGets = tableDemo.GetWhereOrderBy("Clave1='Key1Get01'", "Campo1");
			dbDemo.Close();

			// Comprueba
			Assert.IsNotNull(entityGets);
			Assert.AreEqual(2, entityGets.Length);
			Assert.AreEqual("Key1Get01", entityGets[0].Clave1);
			Assert.AreEqual("Key2Get02", entityGets[0].Clave2);
			Assert.AreEqual("AField2Get01", entityGets[0].Campo1);
			Assert.AreEqual("Field2Get02", entityGets[0].Campo2);
			Assert.AreEqual("Field2Get03", entityGets[0].Campo3);
			Assert.AreEqual(entityGets[0].Hash, entityGets[0].FieldsHashCode);
			Assert.AreEqual("Key1Get01", entityGets[1].Clave1);
			Assert.AreEqual("Key1Get02", entityGets[1].Clave2);
			Assert.AreEqual("BField1Get01", entityGets[1].Campo1);
			Assert.AreEqual("Field1Get02", entityGets[1].Campo2);
			Assert.AreEqual("Field1Get03", entityGets[1].Campo3);
			Assert.AreEqual(entityGets[1].Hash, entityGets[1].FieldsHashCode);
		}

		[Test]
		public void SQLTable_Insert_TEntity_FieldsSoloClaves_ret_bool()
		{
			#region Prepara

			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key01",
				Clave2 = "Key02",
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NULL,
								[Campo2] NVARCHAR(100) NULL,
								[Campo3] NVARCHAR(100) NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.Delete(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			bool result = tableDemo.Insert(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			entity.Campo1 = string.Empty;
			entity.Campo2 = string.Empty;
			entity.Campo3 = string.Empty;
			dbDemo.Open();
			EntityDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();
			Assert.AreEqual("Key01", entityGet.Clave1);
			Assert.AreEqual("Key02", entityGet.Clave2);
			Assert.IsNull(entityGet.Campo1);
			Assert.IsNull(entityGet.Campo2);
			Assert.IsNull(entityGet.Campo3);
		}

		[Test]
		public void SQLTable_Insert_TEntity_FieldsTodos_ret_bool()
		{
			#region Prepara

			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key01",
				Clave2 = "Key02",
				Campo1 = "Field01",
				Campo2 = "Field02",
				Campo3 = "Field03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.Delete(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			bool result = tableDemo.Insert(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			entity.Campo1 = string.Empty;
			entity.Campo2 = string.Empty;
			entity.Campo3 = string.Empty;
			dbDemo.Open();
			EntityDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();
			Assert.AreEqual("Key01", entityGet.Clave1);
			Assert.AreEqual("Key02", entityGet.Clave2);
			Assert.AreEqual("Field01", entityGet.Campo1);
			Assert.AreEqual("Field02", entityGet.Campo2);
			Assert.AreEqual("Field03", entityGet.Campo3);
		}

		[Test]
		public void SQLTable_Insert_TEntity_WithEncryptHash_ret_bool()
		{
			#region Prepara

			EntityEncryptHashDemo entity = new EntityEncryptHashDemo
			{
				Clave1 = "Key_EHDI_01",
				Clave2 = "Key_EHDI_02",
				Campo1 = "Field_EHDI_01",
				Campo2 = "Field_EHDI_02",
				Campo3 = "Field_EHDI_03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityEncryptHashDemo> table = new SQLTable<EntityEncryptHashDemo> { DB = db };
				table.Delete(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityEncryptHashDemo> tableDemo = new SQLTable<EntityEncryptHashDemo> { DB = dbDemo };
			bool result = tableDemo.Insert(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			entity.Campo1 = string.Empty;
			entity.Campo2 = string.Empty;
			entity.Campo3 = string.Empty;
			dbDemo.Open();
			EntityEncryptHashDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();
			Assert.AreEqual("Key_EHDI_01", entityGet.Clave1);
			Assert.AreEqual("Key_EHDI_02", entityGet.Clave2);
			Assert.AreEqual("Field_EHDI_01", entityGet.Campo1);
			Assert.AreEqual("Field_EHDI_02", entityGet.Campo2);
			Assert.AreEqual("Field_EHDI_03", entityGet.Campo3);
			Assert.AreEqual(entityGet.Hash, entityGet.FieldsHashCode);
		}

		[Test]
		public void SQLTable_Insert_TEntity_WithHash_ret_bool()
		{
			#region Prepara

			EntityHashDemo entity = new EntityHashDemo
			{
				Clave1 = "Key01",
				Clave2 = "Key02",
				Campo1 = "Field01",
				Campo2 = "Field02",
				Campo3 = "Field03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityHashDemo> table = new SQLTable<EntityHashDemo> { DB = db };
				table.Delete(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityHashDemo> tableDemo = new SQLTable<EntityHashDemo> { DB = dbDemo };
			bool result = tableDemo.Insert(entity);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			entity.Campo1 = string.Empty;
			entity.Campo2 = string.Empty;
			entity.Campo3 = string.Empty;
			dbDemo.Open();
			EntityHashDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();
			Assert.AreEqual("Key01", entityGet.Clave1);
			Assert.AreEqual("Key02", entityGet.Clave2);
			Assert.AreEqual("Field01", entityGet.Campo1);
			Assert.AreEqual("Field02", entityGet.Campo2);
			Assert.AreEqual("Field03", entityGet.Campo3);
			Assert.AreEqual(entityGet.Hash, entityGet.FieldsHashCode);
		}

		[Test]
		public void SQLtable_Read_ReadAll_CloseReader_01()
		{
			#region Prepara

			EntityDemo entity1 = new EntityDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "Field1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityDemo entity2 = new EntityDemo
			{
				Clave1 = "Key2Get01",
				Clave2 = "Key2Get02",
				Campo1 = "Field2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityDemo entity3 = new EntityDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "Field3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "Field3Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			List<EntityDemo> listaEntidades = new List<EntityDemo>();
			tableDemo.OpenReaderAll();
			EntityDemo ed = tableDemo.Read();
			while (ed != null)
			{
				listaEntidades.Add(ed);
				ed = tableDemo.Read();
			}
			tableDemo.CloseReader();
			dbDemo.Close();

			// Comprueba
			EntityDemo[] entidades = listaEntidades.ToArray();
			Assert.IsNotNull(entidades);
			Assert.AreEqual(3, entidades.Length);
			Assert.AreEqual("Key1Get01", entidades[0].Clave1);
			Assert.AreEqual("Key1Get02", entidades[0].Clave2);
			Assert.AreEqual("Field1Get01", entidades[0].Campo1);
			Assert.AreEqual("Field1Get02", entidades[0].Campo2);
			Assert.AreEqual("Field1Get03", entidades[0].Campo3);
			Assert.AreEqual("Key2Get01", entidades[1].Clave1);
			Assert.AreEqual("Key2Get02", entidades[1].Clave2);
			Assert.AreEqual("Field2Get01", entidades[1].Campo1);
			Assert.AreEqual("Field2Get02", entidades[1].Campo2);
			Assert.AreEqual("Field2Get03", entidades[1].Campo3);
			Assert.AreEqual("Key3Get01", entidades[2].Clave1);
			Assert.AreEqual("Key3Get02", entidades[2].Clave2);
			Assert.AreEqual("Field3Get01", entidades[2].Campo1);
			Assert.AreEqual("Field3Get02", entidades[2].Campo2);
			Assert.AreEqual("Field3Get03", entidades[2].Campo3);
		}

		[Test]
		public void SQLtable_Read_ReadWhere_CloseReader_01()
		{
			#region Prepara

			EntityDemo entity1 = new EntityDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "Field1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityDemo entity2 = new EntityDemo
			{
				Clave1 = "Key2Get01",
				Clave2 = "Key2Get02",
				Campo1 = "Field2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityDemo entity3 = new EntityDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "Field3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "FieldGet"
			};
			EntityDemo entity4 = new EntityDemo
			{
				Clave1 = "Key4Get01",
				Clave2 = "Key4Get02",
				Campo1 = "Field4Get01",
				Campo2 = "Field4Get02",
				Campo3 = "FieldGet"
			};
			EntityDemo entity5 = new EntityDemo
			{
				Clave1 = "Key5Get01",
				Clave2 = "Key5Get02",
				Campo1 = "Field5Get01",
				Campo2 = "Field5Get02",
				Campo3 = "FieldGet"
			};
			EntityDemo entity6 = new EntityDemo
			{
				Clave1 = "Key6Get01",
				Clave2 = "Key6Get02",
				Campo1 = "Field6Get01",
				Campo2 = "Field6Get02",
				Campo3 = "Field6Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
				table.Insert(entity4);
				table.Insert(entity5);
				table.Insert(entity6);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			List<EntityDemo> listaEntidades = new List<EntityDemo>();
			tableDemo.OpenReaderWhere("Campo3='FieldGet'");
			EntityDemo ed = tableDemo.Read();
			while (ed != null)
			{
				listaEntidades.Add(ed);
				ed = tableDemo.Read();
			}
			tableDemo.CloseReader();
			dbDemo.Close();

			// Comprueba
			EntityDemo[] entidades = listaEntidades.ToArray();
			Assert.IsNotNull(entidades);
			Assert.AreEqual(3, entidades.Length);
			Assert.AreEqual("Key3Get01", entidades[0].Clave1);
			Assert.AreEqual("Key3Get02", entidades[0].Clave2);
			Assert.AreEqual("Field3Get01", entidades[0].Campo1);
			Assert.AreEqual("Field3Get02", entidades[0].Campo2);
			Assert.AreEqual("FieldGet", entidades[0].Campo3);
			Assert.AreEqual("Key4Get01", entidades[1].Clave1);
			Assert.AreEqual("Key4Get02", entidades[1].Clave2);
			Assert.AreEqual("Field4Get01", entidades[1].Campo1);
			Assert.AreEqual("Field4Get02", entidades[1].Campo2);
			Assert.AreEqual("FieldGet", entidades[1].Campo3);
			Assert.AreEqual("Key5Get01", entidades[2].Clave1);
			Assert.AreEqual("Key5Get02", entidades[2].Clave2);
			Assert.AreEqual("Field5Get01", entidades[2].Campo1);
			Assert.AreEqual("Field5Get02", entidades[2].Campo2);
			Assert.AreEqual("FieldGet", entidades[2].Campo3);
		}

		[Test]
		public void SQLtable_Read_ReadWhereOrderBy_CloseReader_01()
		{
			#region Prepara

			EntityDemo entity1 = new EntityDemo
			{
				Clave1 = "Key1Get01",
				Clave2 = "Key1Get02",
				Campo1 = "Field1Get01",
				Campo2 = "Field1Get02",
				Campo3 = "Field1Get03"
			};
			EntityDemo entity2 = new EntityDemo
			{
				Clave1 = "Key2Get01",
				Clave2 = "Key2Get02",
				Campo1 = "Field2Get01",
				Campo2 = "Field2Get02",
				Campo3 = "Field2Get03"
			};
			EntityDemo entity3 = new EntityDemo
			{
				Clave1 = "Key3Get01",
				Clave2 = "Key3Get02",
				Campo1 = "CField3Get01",
				Campo2 = "Field3Get02",
				Campo3 = "FieldGet"
			};
			EntityDemo entity4 = new EntityDemo
			{
				Clave1 = "Key4Get01",
				Clave2 = "Key4Get02",
				Campo1 = "BField4Get01",
				Campo2 = "Field4Get02",
				Campo3 = "FieldGet"
			};
			EntityDemo entity5 = new EntityDemo
			{
				Clave1 = "Key5Get01",
				Clave2 = "Key5Get02",
				Campo1 = "AField5Get01",
				Campo2 = "Field5Get02",
				Campo3 = "FieldGet"
			};
			EntityDemo entity6 = new EntityDemo
			{
				Clave1 = "Key6Get01",
				Clave2 = "Key6Get02",
				Campo1 = "Field6Get01",
				Campo2 = "Field6Get02",
				Campo3 = "Field6Get03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.DeleteAll();
				table.Insert(entity1);
				table.Insert(entity2);
				table.Insert(entity3);
				table.Insert(entity4);
				table.Insert(entity5);
				table.Insert(entity6);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			List<EntityDemo> listaEntidades = new List<EntityDemo>();
			tableDemo.OpenReaderWhereOrderBy("Campo3='FieldGet'", "Campo1");
			EntityDemo ed = tableDemo.Read();
			while (ed != null)
			{
				listaEntidades.Add(ed);
				ed = tableDemo.Read();
			}
			tableDemo.CloseReader();
			dbDemo.Close();

			// Comprueba
			EntityDemo[] entidades = listaEntidades.ToArray();
			Assert.IsNotNull(entidades);
			Assert.AreEqual(3, entidades.Length);
			Assert.AreEqual("Key5Get01", entidades[0].Clave1);
			Assert.AreEqual("Key5Get02", entidades[0].Clave2);
			Assert.AreEqual("AField5Get01", entidades[0].Campo1);
			Assert.AreEqual("Field5Get02", entidades[0].Campo2);
			Assert.AreEqual("FieldGet", entidades[0].Campo3);
			Assert.AreEqual("Key4Get01", entidades[1].Clave1);
			Assert.AreEqual("Key4Get02", entidades[1].Clave2);
			Assert.AreEqual("BField4Get01", entidades[1].Campo1);
			Assert.AreEqual("Field4Get02", entidades[1].Campo2);
			Assert.AreEqual("FieldGet", entidades[1].Campo3);
			Assert.AreEqual("Key3Get01", entidades[2].Clave1);
			Assert.AreEqual("Key3Get02", entidades[2].Clave2);
			Assert.AreEqual("CField3Get01", entidades[2].Campo1);
			Assert.AreEqual("Field3Get02", entidades[2].Campo2);
			Assert.AreEqual("FieldGet", entidades[2].Campo3);
		}

		[Test]
		public void SQLTable_Update_TEntity_ret_bool()
		{
			#region Prepara

			EntityDemo entity = new EntityDemo
			{
				Clave1 = "KeyUpd01",
				Clave2 = "KeyUpd02",
				Campo1 = "FieldUpd01",
				Campo2 = "FieldUpd02",
				Campo3 = "FieldUpd03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				try
				{
					db.Execute("DROP TABLE [EntityDemo]");
				}
				catch (Exception)
				{
				}
				try
				{
					string comando = @"CREATE TABLE [EntityDemo] (
								[Clave1] NVARCHAR(100) NOT NULL,
								[Clave2] NVARCHAR(100) NOT NULL,
								[Campo1] NVARCHAR(100) NOT NULL,
								[Campo2] NVARCHAR(100) NOT NULL,
								[Campo3] NVARCHAR(100) NOT NULL,
							PRIMARY KEY ([Clave1],[Clave2])
							)";
					db.Execute(comando);
				}
				catch (Exception)
				{
				}
				db.Open();
				SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
				table.Delete(entity);
				table.Insert(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityDemo> tableDemo = new SQLTable<EntityDemo> { DB = dbDemo };
			EntityDemo entityUpd = tableDemo.Get(entity);
			entityUpd.Campo1 = "FieldUpdated01";
			entityUpd.Campo2 = "FieldUpdated02";
			entityUpd.Campo3 = "FieldUpdated03";
			bool result = tableDemo.Update(entityUpd);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			entity.Campo1 = string.Empty;
			entity.Campo2 = string.Empty;
			entity.Campo3 = string.Empty;
			dbDemo.Open();
			EntityDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();
			Assert.AreEqual("KeyUpd01", entityGet.Clave1);
			Assert.AreEqual("KeyUpd02", entityGet.Clave2);
			Assert.AreEqual("FieldUpdated01", entityGet.Campo1);
			Assert.AreEqual("FieldUpdated02", entityGet.Campo2);
			Assert.AreEqual("FieldUpdated03", entityGet.Campo3);
		}

		[Test]
		public void SQLTable_Update_TEntity_WithEncryptHash_ret_bool()
		{
			#region Prepara

			EntityEncryptHashDemo entity = new EntityEncryptHashDemo
			{
				Clave1 = "KeyUpd_EHDU_01",
				Clave2 = "KeyUpd_EHDU_02",
				Campo1 = "FieldUpd_EHDU_01",
				Campo2 = "FieldUpd_EHDU_02",
				Campo3 = "FieldUpd_EHDU_03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityEncryptHashDemo> table = new SQLTable<EntityEncryptHashDemo> { DB = db };
				table.Delete(entity);
				table.Insert(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityEncryptHashDemo> tableDemo = new SQLTable<EntityEncryptHashDemo> { DB = dbDemo };
			EntityEncryptHashDemo entityUpd = tableDemo.Get(entity);
			entityUpd.Campo1 = "FieldUpdated_EHDU_01";
			entityUpd.Campo2 = "FieldUpdated_EHDU_02";
			entityUpd.Campo3 = "FieldUpdated_EHDU_03";
			bool result = tableDemo.Update(entityUpd);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			entity.Campo1 = string.Empty;
			entity.Campo2 = string.Empty;
			entity.Campo3 = string.Empty;
			dbDemo.Open();
			EntityEncryptHashDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();
			Assert.AreEqual("KeyUpd_EHDU_01", entityGet.Clave1);
			Assert.AreEqual("KeyUpd_EHDU_02", entityGet.Clave2);
			Assert.AreEqual("FieldUpdated_EHDU_01", entityGet.Campo1);
			Assert.AreEqual("FieldUpdated_EHDU_02", entityGet.Campo2);
			Assert.AreEqual("FieldUpdated_EHDU_03", entityGet.Campo3);
			Assert.AreEqual(entityGet.Hash, entityGet.FieldsHashCode);
		}

		[Test]
		public void SQLTable_Update_TEntity_WithHash_ret_bool()
		{
			#region Prepara

			EntityHashDemo entity = new EntityHashDemo
			{
				Clave1 = "KeyUpd01",
				Clave2 = "KeyUpd02",
				Campo1 = "FieldUpd01",
				Campo2 = "FieldUpd02",
				Campo3 = "FieldUpd03"
			};
			DataBase db = null;
			try
			{
				db = new DataBase { ConnectionString = "TestDB" };
				db.Open();
				SQLTable<EntityHashDemo> table = new SQLTable<EntityHashDemo> { DB = db };
				table.Delete(entity);
				table.Insert(entity);
			}
			catch (Exception)
			{
			}
			finally
			{
				db.Close();
			}

			#endregion

			// Ejecuta
			DataBase dbDemo = new DataBase { ConnectionString = "TestDB" };
			dbDemo.Open();
			SQLTable<EntityHashDemo> tableDemo = new SQLTable<EntityHashDemo> { DB = dbDemo };
			EntityHashDemo entityUpd = tableDemo.Get(entity);
			entityUpd.Campo1 = "FieldUpdated01";
			entityUpd.Campo2 = "FieldUpdated02";
			entityUpd.Campo3 = "FieldUpdated03";
			bool result = tableDemo.Update(entityUpd);
			dbDemo.Close();

			// Comprueba
			Assert.AreEqual(true, result);
			entity.Campo1 = string.Empty;
			entity.Campo2 = string.Empty;
			entity.Campo3 = string.Empty;
			dbDemo.Open();
			EntityHashDemo entityGet = tableDemo.Get(entity);
			dbDemo.Close();
			Assert.AreEqual("KeyUpd01", entityGet.Clave1);
			Assert.AreEqual("KeyUpd02", entityGet.Clave2);
			Assert.AreEqual("FieldUpdated01", entityGet.Campo1);
			Assert.AreEqual("FieldUpdated02", entityGet.Campo2);
			Assert.AreEqual("FieldUpdated03", entityGet.Campo3);
			Assert.AreEqual(entityGet.Hash, entityGet.FieldsHashCode);
		}
	}
}

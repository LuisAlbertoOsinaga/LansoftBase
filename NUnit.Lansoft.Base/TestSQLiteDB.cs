/* -------------------------------------------------------------
	Archivo: TestSQLiteDB.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/mayo/21
----------------------------------------------------------------*/

using NUnit.Framework;
using System;
using Lansoft.Base;

namespace NUnit.Lansoft.Base
{
    [TestFixture]
    public class TestSQLiteDB
    {
        [Test]
        public void SQLiteDB_BeginTransaction_Commit_void_ret_void()
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
            IDataBase db = null;
            try
            {
                db = new SQLiteDB("TestDB");
                db.Open();
                SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
                table.DeleteAll();
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
            IDataBase dbTran = new SQLiteDB("TestDB");
            dbTran.Open();
            dbTran.BeginTransaction();
            SQLTable<EntityDemo> tableTran = new SQLTable<EntityDemo> { DB = dbTran };
            tableTran.Insert(entity1);
            tableTran.Insert(entity2);
            tableTran.Insert(entity3);
            dbTran.Commit();

            // Comprueba
            EntityDemo[] entityGets = tableTran.GetAll();
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
        public void SQLiteDB_BeginTransaction_RollBack_void_ret_void()
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
            IDataBase db = null;
            try
            {
                db = new SQLiteDB("TestDB");
                db.Open();
                SQLTable<EntityDemo> table = new SQLTable<EntityDemo> { DB = db };
                table.DeleteAll();
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
            IDataBase dbTran = new SQLiteDB("TestDB");
            dbTran.Open();
            SQLTable<EntityDemo> tableTran = new SQLTable<EntityDemo> { DB = dbTran };
            tableTran.Insert(entity1);
            tableTran.Insert(entity2);
            dbTran.BeginTransaction();
            tableTran.Insert(entity3);
            dbTran.RollBack();
            dbTran.Close();

            // Comprueba
            dbTran.Open();
            EntityDemo[] entityGets = tableTran.GetAll();
            dbTran.Close();
            Assert.IsNotNull(entityGets);
            Assert.AreEqual(2, entityGets.Length);
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
        }

        [Test]
        public void SQLiteDB_Close_void_ret_void()
        {
            // Prepara
            IDataBase db = new SQLiteDB();
            string closed = db.ConnectionState.ToString();
            db.Close();
            db.ConnectionString = "TestDataBase";
            db.Open();
            string opened = db.ConnectionState.ToString();

            // Ejecuta
            db.Close();

            // Comprueba
            db.Close();
            Assert.AreEqual("Closed", closed);
            Assert.AreEqual("Open", opened);
            Assert.AreEqual("Closed", db.ConnectionState.ToString());
        }
        
        [Test]
        public void SQLiteDB_Open_str_ConnStr_ret_void()
        {
            // Prepara
            IDataBase db = new SQLiteDB();

            // Ejecuta
            db.ConnectionString = "Data Source=Test.db";
            db.Open();

            // Comprueba
            Assert.AreEqual(DBConnectionState.Open, db.ConnectionState);
            db.Close();
        }

        [Test]
        public void SQLiteDB_Open_str_ConnStrName_ret_void()
        {
            // Prepara
            IDataBase db = new SQLiteDB();

            // Ejecuta
            db.ConnectionString = "TestDataBase";
            db.Open();

            // Comprueba
            Assert.AreEqual(DBConnectionState.Open, db.ConnectionState);
            db.Close();
        }

        [Test]
        public void SQLiteDB_Open_str_InvalidConnStr_throw_ex()
        {
            // Prepara
            IDataBase db = new SQLiteDB();

            try
            {
                // Ejecuta
                db.ConnectionString = "Test.db";
                db.Open();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Error en Connection.Open(). providerName = 'System.Data.SQLite', connectionString = 'Test.db'", ex.Message);
            }
        }

        [Test]
        public void SQLiteTable_DB_str_InvalidDataSource_throw_ex()
        {
            // Prepara
            IDataBase db = new SQLiteDB();

            try
            {
                // Ejecuta
                db.ConnectionString = "DataBase=Test.db";
                db.Open();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Error en Connection.Open(). providerName = 'System.Data.SQLite', connectionString = 'DataBase=Test.db'", ex.Message);
            }
        }
    }
}

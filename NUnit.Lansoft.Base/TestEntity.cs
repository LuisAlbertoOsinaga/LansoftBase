/* -------------------------------------------------------------
	Archivo: TestEntityDemo.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/30
----------------------------------------------------------------*/

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Lansoft.Base;

namespace NUnit.Lansoft.Base
{
	[TestFixture]
	public class TestEntity
	{
		[Test]
		public void Entity_Clone_IEntity_ret_void()
		{
			// Prepara
			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			EntityDemo entityClone = new EntityDemo();
			entity.Clone(entityClone);

			// Comprueba
			Assert.AreEqual("Key1", entityClone.Clave1);
			Assert.AreEqual("Key2", entityClone.Clave2);
			Assert.AreEqual("Field1", entityClone.Campo1);
			Assert.AreEqual("Field2", entityClone.Campo2);
			Assert.AreEqual("Field3", entityClone.Campo3);
		}

		[Test]
		public void Entity_Clone_void_ret_object()
		{
			// Prepara
			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			EntityDemo entityClone = (EntityDemo) entity.Clone();

			// Comprueba
			Assert.AreEqual(entity, entityClone);
		}

		[Test]
		public void Entity_Equals_IEntity_ret_bool()
		{
			// Prepara
			EntityDemo entity1 = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};
			IEntity entity2 = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			bool esIgual = entity1.Equals(entity2);

			// Comprueba
			Assert.IsFalse(object.ReferenceEquals(entity1, entity2));
			Assert.IsTrue(esIgual);
		}

		[Test]
		public void Entity_Equals_object_ret_bool()
		{
			// Prepara
			EntityDemo entity1 = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};
			object entity2 = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			bool esIgual = entity1.Equals(entity2);

			// Comprueba
			Assert.IsFalse(object.ReferenceEquals(entity1, entity2));
			Assert.IsTrue(esIgual);
		}

		[Test]
		public void Entity_FromXml_str_ret_void()
		{
			// Prepara
			string xml = "<EntityDemo><Clave1>A</Clave1><Clave2>B</Clave2><Campo1>C</Campo1><Campo2>D</Campo2><Campo3>E</Campo3></EntityDemo>";

			// Ejecuta
			EntityDemo entity = new EntityDemo();
			entity.FromXml(xml);

			// Comprueba
			Assert.AreEqual("A", entity.Clave1);
			Assert.AreEqual("B", entity.Clave2);
			Assert.AreEqual("C", entity.Campo1);
			Assert.AreEqual("D", entity.Campo2);
			Assert.AreEqual("E", entity.Campo3);
		}

		[Test]
		public void Entity_GetValue_str_EncryptVarios_ret_object()
		{
			// Prepara
			EntityEncryptHashVariosDemo entity = new EntityEncryptHashVariosDemo();
			entity.Clave = "key";
			entity.CampoInt = 17;
			entity.CampoDouble = 145.96;
			entity.CampoStr = "este es un texto corto";
			entity.CampoAByteCorto = new byte[] {1,2,3,4,5,6,7,8,9,0};
			List<byte> bytes = new List<byte>();
			for (int i = 0; i < 1000; i++)
				bytes.Add(Convert.ToByte(i % 256));
			entity.CampoAByteLargo = bytes.ToArray();

			// Ejecuta
			string clave = (string) entity.GetValue("Clave");
			string campoDouble = (string) entity.GetValue("CampoDouble");
			string campoInt = (string)entity.GetValue("CampoInt");
			string campoStr = (string)entity.GetValue("CampoStr");
			byte[] campoAByteCorto = (byte[])entity.GetValue("CampoAByteCorto");
			byte[] campoAByteLargo = (byte[])entity.GetValue("CampoAByteLargo");
			string hash = (string)entity.GetValue("Hash");

			// Comprueba
			Encryptor encriptor = new Encryptor();
			Assert.AreEqual("key", clave);
			Assert.AreEqual(encriptor.Encrypt("17"), campoInt);
			Assert.AreEqual(encriptor.Encrypt("145.96"), campoDouble);
			Assert.AreEqual(encriptor.Encrypt(entity.CampoStr), campoStr);

			// Assert.AreEqual para CampoAByteCorto
			byte[] bytesEncryptLength = new byte[4];
			Array.Copy(campoAByteCorto, bytesEncryptLength, bytesEncryptLength.Length);
			int encryptLength = Convierte.BytesToInt(bytesEncryptLength);
			byte[] bytesEncrypted = new byte[encryptLength];
			Array.Copy(campoAByteCorto, 4, bytesEncrypted, 0, bytesEncrypted.Length);
			Assert.AreEqual(encriptor.EncryptBytes(entity.CampoAByteCorto), bytesEncrypted);

			// Assert.AreEqual para CampoAByteLargo
			Array.Copy(campoAByteLargo, bytesEncryptLength, bytesEncryptLength.Length);
			encryptLength = Convierte.BytesToInt(bytesEncryptLength);
			bytesEncrypted = new byte[encryptLength];
			Array.Copy(campoAByteLargo, 4, bytesEncrypted, 0, bytesEncrypted.Length);
			byte[] bytesNoEncrypted = new byte[campoAByteLargo.Length - (4 + encryptLength)];
			Array.Copy(campoAByteLargo, 4 + encryptLength, bytesNoEncrypted, 0, bytesNoEncrypted.Length);
			byte[] bytesToEncrypt = new byte[100];
			Array.Copy(entity.CampoAByteLargo, bytesToEncrypt, 100);
			byte[] bytesNoEncryptedEsperados = new byte[bytesNoEncrypted.Length];
			Array.Copy(entity.CampoAByteLargo, 100, bytesNoEncryptedEsperados, 0, bytesNoEncryptedEsperados.Length);
			Assert.AreEqual(encriptor.EncryptBytes(bytesToEncrypt), bytesEncrypted);
			Assert.AreEqual(bytesNoEncryptedEsperados, bytesNoEncrypted);

			byte[] bytesToHashLargo = new byte[100];
			Array.Copy(campoAByteLargo, bytesToHashLargo, bytesToHashLargo.Length);
			string fields = string.Concat("key", encriptor.Encrypt("17"), encriptor.Encrypt("145.96"), encriptor.Encrypt(entity.CampoStr),
											Convert.ToBase64String(campoAByteCorto),
											Convert.ToBase64String(bytesToHashLargo));
			HashGenerator hg = new HashGenerator();
			string hashGenerado = hg.GetHashCode(fields);
			Assert.AreEqual(hashGenerado, hash);
		}

		[Test]
		public void Entity_GetValue_str_ret_object()
		{
			// Prepara
			EntityEncryptHashDemo entity = new EntityEncryptHashDemo();
			entity.Clave1 = "key01";
			entity.Clave2 = "key02";
			entity.Campo1 = "field01";
			entity.Campo2 = "field02";
			entity.Campo3 = "field03";

			// Ejecuta
			string clave1 = (string)entity.GetValue("Clave1");
			string clave2 = (string)entity.GetValue("Clave2");
			string campo1 = (string)entity.GetValue("Campo1");
			string campo2 = (string)entity.GetValue("Campo2");
			string campo3 = (string)entity.GetValue("Campo3");

			// Comprueba
			Encryptor encriptor = new Encryptor();
			Assert.AreEqual("key01", clave1);
			Assert.AreEqual("key02", clave2);
			Assert.AreEqual("field01", campo1);
			Assert.AreEqual(encriptor.Encrypt("field02"), campo2);
			Assert.AreEqual("field03", campo3);
		}

		[Test]
		public void Entity_Property_FieldHash_ret_str()
		{
			// Prepara
			EntityHashDemo entity = new EntityHashDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			string propHash = entity.FieldHash;

			// Comprueba
			Assert.AreEqual("Hash", propHash);
		}

		[Test]
		public void Entity_Property_FieldKeys_ret_Dictionary_str_object()
		{
			// Prepara
			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			List<string> dicProperties = entity.FieldKeys;

			// Comprueba
			Assert.IsTrue(dicProperties.IndexOf("Clave1") >= 0);
			Assert.IsTrue(dicProperties.IndexOf("Clave2") >= 0);
			Assert.IsFalse(dicProperties.IndexOf("Campo1") >= 0);
			Assert.IsFalse(dicProperties.IndexOf("Campo2") >= 0);
			Assert.IsFalse(dicProperties.IndexOf("Campo3") >= 0);
		}

		[Test]
		public void Entity_Property_FieldNoKeys_ret_Dictionary_str_object()
		{
			// Prepara
			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			List<string> dicProperties = entity.FieldNoKeys;

			// Comprueba
			Assert.IsFalse(dicProperties.IndexOf("Clave1") >= 0);
			Assert.IsFalse(dicProperties.IndexOf("Clave2") >= 0);
			Assert.IsTrue(dicProperties.IndexOf("Campo1") >= 0);
			Assert.IsTrue(dicProperties.IndexOf("Campo2") >= 0);
			Assert.IsTrue(dicProperties.IndexOf("Campo3") >= 0);
		}

		[Test]
		public void Entity_Property_Fields_ret_Dictionary_str_object()
		{
			// Prepara
			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			List<string> dicProperties = entity.Fields;

			// Comprueba
			Assert.IsTrue(dicProperties.IndexOf("Clave1") >= 0);
			Assert.IsTrue(dicProperties.IndexOf("Clave2") >= 0);
			Assert.IsTrue(dicProperties.IndexOf("Campo1") >= 0);
			Assert.IsTrue(dicProperties.IndexOf("Campo2") >= 0);
			Assert.IsTrue(dicProperties.IndexOf("Campo3") >= 0);
		}

		[Test]
		public void Entity_Property_FieldsHashCode_ret_str()
		{
			// Prepara
			EntityHashDemo entity = new EntityHashDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3",
				Hash = "Dummy"
			};

			// Ejecuta
			string hcode = entity.FieldsHashCode;

			// Comprueba
			string fields = string.Concat("Key1", "Key2", "Field1", "Field2", "Field3");
			HashGenerator hg = new HashGenerator();
			string hashGenerado = hg.GetHashCode(fields);
			Assert.AreEqual(hashGenerado, hcode);
		}

		[Test]
		public void Entity_SetValue_str_object_EncryptVarios_ret_void()
		{
			// Prepara
			EntityEncryptHashVariosDemo entity = new EntityEncryptHashVariosDemo();
			Encryptor encrip = new Encryptor();
			
			// campoAByteCorto
			byte[] campoAByteCorto = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
			byte[] cortoEncrypted = encrip.EncryptBytes(campoAByteCorto);
			byte[] cortoLenght = Convierte.IntToBytes(cortoEncrypted.Length);
			byte[] campoAByteCortoFuente = cortoLenght.Concat(cortoEncrypted).ToArray();
			
			// campoAByteLargo
			List<byte> bytes = new List<byte>();
			for (int i = 0; i < 1000; i++)
				bytes.Add(Convert.ToByte(i % 256));
			byte[] campoAByteLargo = bytes.ToArray();
			byte[] largoToEncrypt = new byte[100];
			Array.Copy(campoAByteLargo, largoToEncrypt, largoToEncrypt.Length);
			byte[] largoNoEncrypted = new byte[900];
			Array.Copy(campoAByteLargo, 100, largoNoEncrypted, 0, largoNoEncrypted.Length);
			byte[] largoEncrypted = encrip.EncryptBytes(largoToEncrypt);
			byte[] largoLenght = Convierte.IntToBytes(largoEncrypted.Length);
			byte[] campoAByteLargoFuente = largoLenght.Concat(largoEncrypted).Concat(largoNoEncrypted).ToArray();
			byte[] bytesToHashLargo = new byte[100];
			Array.Copy(campoAByteLargoFuente, bytesToHashLargo, bytesToHashLargo.Length);

			// Ejecuta
			Encryptor encriptor = new Encryptor();
			entity.SetValue("Clave", "key");
			entity.SetValue("CampoInt", encriptor.Encrypt("17"));
			entity.SetValue("CampoDouble", encriptor.Encrypt("145.96"));
			entity.SetValue("CampoStr", encriptor.Encrypt("este es un texto corto"));
			entity.SetValue("CampoAByteCorto", campoAByteCortoFuente);
			entity.SetValue("CampoAByteLargo", campoAByteLargoFuente);
			entity.SetValue("Hash", null);

			// Comprueba
			Assert.AreEqual("key", entity.Clave);
			Assert.AreEqual(17, entity.CampoInt);
			Assert.AreEqual(145.96, entity.CampoDouble);
			Assert.AreEqual("este es un texto corto", entity.CampoStr);
			Assert.AreEqual(campoAByteCorto, entity.CampoAByteCorto);
			Assert.AreEqual(campoAByteLargo, entity.CampoAByteLargo);
			string fields = string.Concat("key", encriptor.Encrypt("17"), encriptor.Encrypt("145.96"), encriptor.Encrypt(entity.CampoStr),
											Convert.ToBase64String(campoAByteCortoFuente),
											Convert.ToBase64String(bytesToHashLargo));
			HashGenerator hg = new HashGenerator();
			string hashGenerado = hg.GetHashCode(fields);
			Assert.AreEqual(hashGenerado, entity.Hash);
		}

		[Test]
		public void Entity_SetValue_str_object_ret_void()
		{
			// Prepara
			EntityEncryptHashDemo entity = new EntityEncryptHashDemo();

			// Ejecuta
			Encryptor encriptor = new Encryptor();
			entity.SetValue("Clave1", "key01");
			entity.SetValue("Clave2", "key02");
			entity.SetValue("Campo1", "field01");
			entity.SetValue("Campo2", encriptor.Encrypt("field02"));
			entity.SetValue("Campo3", "field03");

			// Comprueba
			Assert.AreEqual("key01", entity.Clave1);
			Assert.AreEqual("key02", entity.Clave2);
			Assert.AreEqual("field01", entity.Campo1);
			Assert.AreEqual("field02", entity.Campo2);
			Assert.AreEqual("field03", entity.Campo3);
		}

		[Test]
		public void Entity_ToString_ret_str()
		{
			// Prepara
			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			string xml = entity.ToString();

			// Comprueba
			XmlDocument xDoc = new XmlDocument();
			xDoc.LoadXml(xml);
			XmlNode nodo = xDoc.DocumentElement.FirstChild;
			Assert.AreEqual("Clave1", nodo.Name);
			Assert.AreEqual("Key1", nodo.InnerText);
			nodo = nodo.NextSibling;
			Assert.AreEqual("Clave2", nodo.Name);
			Assert.AreEqual("Key2", nodo.InnerText);
			nodo = nodo.NextSibling;
			Assert.AreEqual("Campo1", nodo.Name);
			Assert.AreEqual("Field1", nodo.InnerText);
			nodo = nodo.NextSibling;
			Assert.AreEqual("Campo2", nodo.Name);
			Assert.AreEqual("Field2", nodo.InnerText);
			nodo = nodo.NextSibling;
			Assert.AreEqual("Campo3", nodo.Name);
			Assert.AreEqual("Field3", nodo.InnerText);
		}
		
		[Test]
		public void Entity_ToXml_ret_str()
		{
			// Prepara
			EntityDemo entity = new EntityDemo
			{
				Clave1 = "Key1",
				Clave2 = "Key2",
				Campo1 = "Field1",
				Campo2 = "Field2",
				Campo3 = "Field3"
			};

			// Ejecuta
			string xml = entity.ToXml();

			// Comprueba
			XmlDocument xDoc = new XmlDocument();
			xDoc.LoadXml(xml);
			XmlNode nodo = xDoc.DocumentElement.FirstChild;
			Assert.AreEqual("Clave1", nodo.Name);
			Assert.AreEqual("Key1", nodo.InnerText);
			nodo = nodo.NextSibling;
			Assert.AreEqual("Clave2", nodo.Name);
			Assert.AreEqual("Key2", nodo.InnerText);
			nodo = nodo.NextSibling;
			Assert.AreEqual("Campo1", nodo.Name);
			Assert.AreEqual("Field1", nodo.InnerText);
			nodo = nodo.NextSibling;
			Assert.AreEqual("Campo2", nodo.Name);
			Assert.AreEqual("Field2", nodo.InnerText);
			nodo = nodo.NextSibling;
			Assert.AreEqual("Campo3", nodo.Name);
			Assert.AreEqual("Field3", nodo.InnerText);
		}
	}
}
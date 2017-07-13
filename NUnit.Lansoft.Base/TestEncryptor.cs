/* -------------------------------------------------------------
	Archivo: TestEncryptor.cs
	Modulo: Test.Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/30
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Lansoft.Base;

namespace NUnit.Lansoft.Base
{
	[TestFixture]
	public class TestEncryptor
	{
		[Test]
		public void Encryptor_Decrypt_str_ret_str()
		{
			// Prepara
			string word = "Texto plano a encriptar";
			Encryptor encryptor = new Encryptor();
			string wordEncrypted = encryptor.Encrypt(word);

			// Ejecuta
			Encryptor descryptor = new Encryptor();
			string wordDecrypted = descryptor.Decrypt(wordEncrypted);

			// Comprueba
			Assert.AreEqual(word, wordDecrypted);
		}

		[Test]
		public void Encryptor_DecryptBytes_ArrayByte_ret_ArrayByte()
		{
			// Prepara
			List<byte> bytes = new List<byte>();
			for (int i = 0; i < 10000; i++)
				bytes.Add(Convert.ToByte(i % 256));
			byte[] info = bytes.ToArray();
			Encryptor encryptor = new Encryptor();
			byte[] infoEncrypted = encryptor.EncryptBytes(info);

			// Ejecuta
			Encryptor descryptor = new Encryptor();
			byte[] infoDecrypted = descryptor.DecryptBytes(infoEncrypted);

			// Comprueba
			Assert.AreEqual(info, infoDecrypted);
		}

		[Test]
		public void Encryptor_Encrypt_str_ret_str()
		{
			// Prepara
			string word = "Gran inundacion a orillas del rio Elba. La nación se solidariza e intenta atenuar junta los daños estimados en 15.000 millones de euros.";

			// Ejecuta
			Encryptor encryptor = new Encryptor();
			string wordEncrypted = encryptor.Encrypt(word);

			// Comprueba
			Encryptor descryptor = new Encryptor();
			string wordDecrypted = descryptor.Decrypt(wordEncrypted);
			Assert.AreEqual(word, wordDecrypted);
		}
	
		[Test]
		public void Encryptor_EncryptBytes_ArrayByte_ret_ArrayByte()
		{
			// Prepara
			List<byte> bytes = new List<byte>();
			for (int i = 0; i < 10000; i++)
				bytes.Add(Convert.ToByte(i % 256));
			byte[] info = bytes.ToArray();

			// Ejecuta
			Encryptor encryptor = new Encryptor();
			byte[] infoEncrypted = encryptor.EncryptBytes(info);

			// Comprueba
			Encryptor descryptor = new Encryptor();
			byte[] infoDecrypted = descryptor.DecryptBytes(infoEncrypted);
			Assert.AreEqual(info, infoDecrypted);
		}
	}
}

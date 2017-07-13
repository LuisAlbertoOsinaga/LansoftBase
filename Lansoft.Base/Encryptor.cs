/* -------------------------------------------------------------
	Archivo: Encryptor.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/octubre/21
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lansoft.Base
{
	public class Encryptor
	{
		#region Constructores

		public Encryptor()
		{
			ChangeParams = true;
			Iters = 1;
			Pass = "PASSWORD_TEST_ABRETE_SESAMO";
			Salt = "SALT_VALUE_TEST_VALOR_SALADO";
			SymAlgoritmo = null;
		}

		#endregion

		#region Metodos Privados

		private int CalculateBytesBufferLength(int infoLength, int blockBytes)
		{
			return infoLength / blockBytes * blockBytes + blockBytes;
		}
		private int CalculateStringBufferLength(int infoLength, int blockBytes)
		{
			return (infoLength * 2) / blockBytes * blockBytes + blockBytes;
		}
		private byte[] ExecuteDecryption(CryptoStream cryptoStream, int blockBytes)
		{
			byte[] buffer = new byte[blockBytes];
			List<byte> bytesDesencrypted = new List<byte>();
			int bytesReaded = cryptoStream.Read(buffer, 0, blockBytes);
			while (bytesReaded > 0)
			{
				for (int i = 0; i < bytesReaded; i++)
					bytesDesencrypted.Add(buffer[i]);
				bytesReaded = cryptoStream.Read(buffer, 0, blockBytes);
			}
			return bytesDesencrypted.ToArray();
		}
		private int PrepareEncryptor(out SymmetricAlgorithm encryptor)
		{
			if (ChangeParams)
			{
				SymAlgoritmo = new RijndaelManaged();
				encryptor = SymAlgoritmo;
				byte[] bytesSalt = Encoding.Unicode.GetBytes(Salt);
				Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Pass, bytesSalt, Iters);
				encryptor.Key = deriveBytes.GetBytes(encryptor.KeySize / 8);
				int blockBytes = encryptor.BlockSize / 8;
				encryptor.IV = deriveBytes.GetBytes(blockBytes);
				ChangeParams = false;
				return blockBytes;
			}
			encryptor = SymAlgoritmo;
			return encryptor.BlockSize / 8;
		}

		#endregion

		#region Metodos Publicos

		public string Decrypt(string info)
		{
			// Prepara encriptador
			SymmetricAlgorithm encryptor;
			int blockBytes = PrepareEncryptor(out encryptor);

			// Stream de desencripcion
			byte[] bytes = Convert.FromBase64String(info);
			ICryptoTransform cryptoTransform = encryptor.CreateDecryptor();
			MemoryStream memoryStream = new MemoryStream(bytes);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);

			// Desencripta
			byte[] bytesDesencrypted = ExecuteDecryption(cryptoStream, blockBytes);

			// Cierra streams
			cryptoStream.Close();
			memoryStream.Close();

			return Encoding.Unicode.GetString(bytesDesencrypted);
		}
		public byte[] DecryptBytes(byte[] info)
		{
			// Prepara encriptador
			SymmetricAlgorithm encryptor;
			int blockBytes = PrepareEncryptor(out encryptor);

			// Stream de desencripcion
			ICryptoTransform cryptoTransform = encryptor.CreateDecryptor();
			MemoryStream memoryStream = new MemoryStream(info);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);

			// Desencripta
			byte[] bytesDesencrypted = ExecuteDecryption(cryptoStream, blockBytes);

			// Cierra streams
			cryptoStream.Close();
			memoryStream.Close();

			return bytesDesencrypted;
		}
		public string Encrypt(string info)
		{
			// Prepara encriptador
			SymmetricAlgorithm encryptor;
			int blockBytes = PrepareEncryptor(out encryptor);

			// Stream de encripcion
			ICryptoTransform cryptoTransform = encryptor.CreateEncryptor();
			int sizeBuffer = CalculateStringBufferLength(info.Length, blockBytes);
			byte[] buffer = new byte[sizeBuffer];
			MemoryStream memoryStream = new MemoryStream(buffer);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);

			// Encripta
			byte[] bytes = Encoding.Unicode.GetBytes(info);
			cryptoStream.Write(bytes, 0, bytes.Length);

			// Cierra streams
			cryptoStream.Close();
			memoryStream.Close();

			return Convert.ToBase64String(buffer);
		}
		public byte[] EncryptBytes(byte[] info)
		{
			// Prepara encriptador
			SymmetricAlgorithm encryptor;
			int blockSize = PrepareEncryptor(out encryptor);
			int sizeBuffer = CalculateBytesBufferLength(info.Length, blockSize);

			// Streams de encripcion
			ICryptoTransform cryptoTransform = encryptor.CreateEncryptor();
			byte[] buffer = new byte[sizeBuffer];
			MemoryStream memoryStream = new MemoryStream(buffer);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);

			// Encripta
			cryptoStream.Write(info, 0, info.Length);

			// Cierra streams
			cryptoStream.Close();
			memoryStream.Close();

			return buffer;
		}

		#endregion

		#region Properties

		private bool ChangeParams { get; set; }
		public int Iters 
		{
			get { return m_iters; }
			set 
			{
				if (m_iters != value)
				{
					m_iters = value;
					ChangeParams = true;
				}
			} 
		}
		public string Pass
		{
			get { return m_pass; }
			set 
			{
				if (m_pass != value)
				{
					m_pass = value;
					ChangeParams = true;
				}
			} 
		}
		public string Salt
		{
			get { return m_salt; }
			set 
			{
				if (m_salt != value)
				{
					m_salt = value;
					ChangeParams = true;
				}
			} 
		}
		private SymmetricAlgorithm SymAlgoritmo { get; set; }

		#endregion

		#region Campos

		private int m_iters;
		private string m_pass;
		private string m_salt;

		#endregion
	}
}
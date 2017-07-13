/* -------------------------------------------------------------
	Archivo: Entity.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/octubre/25
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Lansoft.Base
{
	public abstract class Entity : IEntity
	{
		#region Propiedades Estaticas

		[XmlIgnore]
		public static Encryptor Encriptador { get; set; }
		[XmlIgnore]
		public static HashGenerator Hasheador { get; set; }

		#endregion

		#region Constructor Estaticos

		static Entity()
		{
			Encriptador = new Encryptor();
			Hasheador = new HashGenerator();
		}

		#endregion

		#region IEntity

		#region Properties

		[XmlIgnore]
		public int EncryptIters { get; set; }
		[XmlIgnore]
		public string EncryptPass { get; set; }
		[XmlIgnore]
		public string EncryptSalt { get; set; }

		[XmlIgnore]
		public string FieldHash
		{
			get
			{
				Dictionary<string, object> dicHashes = Reflex.GetPropertiesNameAndValueWithAttributes(this, typeof(HashAttribute));
				if (dicHashes == null || dicHashes.Count == 0)
					return null;
				List<string> hashes = dicHashes.Keys.ToList<string>();
				return hashes[0];
			}
		}
		[XmlIgnore]
		public List<string> FieldKeys
		{
			get
			{
				Dictionary<string, object> dicKeys = Reflex.GetPropertiesNameAndValueWithAttributes(this, typeof(KeyAttribute));
				if (dicKeys == null || dicKeys.Count == 0)
					return null;
				return dicKeys.Keys.ToList<string>();
			}
		}
		[XmlIgnore]
		public List<string> FieldNoKeys
		{
			get
			{
				List<PropertyInfo> listPIFields = Reflex.GetPropertiesInfoWithAttributes(this, typeof(FieldAttribute));
				if (listPIFields == null || listPIFields.Count == 0)
					return null;

				List<PropertyInfo> listPIKeys = Reflex.GetPropertiesInfoWithAttributes(this, typeof(KeyAttribute));
				if (listPIKeys == null || listPIKeys.Count == 0)
					return Reflex.GetPropertiesNameAndValue(this, listPIFields).Keys.ToList<string>();

				List<PropertyInfo> listPINoKeys = listPIFields.Where(pif => !listPIKeys.Contains(pif)).ToList<PropertyInfo>();
				return Reflex.GetPropertiesNameAndValue(this, listPINoKeys).Keys.ToList<string>();
			}
		}
		[XmlIgnore]
		public List<string> Fields
		{
			get
			{
				Dictionary<string, object> dicFields = Reflex.GetPropertiesNameAndValueWithAttributes(this, typeof(FieldAttribute));
				if (dicFields == null || dicFields.Count == 0)
					return null;
				return dicFields.Keys.ToList<string>();
			}
		}
		[XmlIgnore]
		public string FieldsHashCode
		{
			get
			{
				if (FieldHash == null || Fields == null || Fields.Count == 0)
					return string.Empty;

				// Info a hashear
				StringBuilder sbInfo = new StringBuilder();
				Fields.ForEach
				(
					field => 
					{ 
						object value = GetValue(field);
						if (value != null)
						{
							switch (value.GetType().Name)
							{
								case "Byte[]":
									byte[] bytesToHash = (byte[])value;
									if (bytesToHash.Length > 100)
									{
										bytesToHash = new byte[100];
										Array.Copy((byte[])value, bytesToHash, bytesToHash.Length);
									}
									value = Convert.ToBase64String(bytesToHash);
									break;
								case "String":
									if(((string)value).Length > 100)
										value = ((string)value).Substring(0, 100);
									break;
								default:
									break;
							}
						}
						sbInfo.Append(value != null ? value.ToString() : string.Empty); 
					}
				);

				// Hashea
				if (HashIters != 0)
					Hasheador.Iters = HashIters;
				if (!string.IsNullOrEmpty(HashPass))
					Hasheador.Pass = HashPass;
				if (!string.IsNullOrEmpty(HashSalt))
					Hasheador.Salt = HashSalt;
				return Hasheador.GetHashCode(sbInfo.ToString());
			}
		}

		[XmlIgnore]
		public int HashIters { get; set; }
		[XmlIgnore]
		public string HashPass { get; set; }
		[XmlIgnore]
		public string HashSalt { get; set; }

		#endregion

		#region Methods

		public void Clone(IEntity entity)
		{
			if (entity.GetType().Name != this.GetType().Name)
				return;

			List<PropertyInfo> listPIs = Reflex.GetPropertiesInfoWithAttributes(this, typeof(FieldAttribute), typeof(HashAttribute));
			if (listPIs == null || listPIs.Count == 0)
				return;

			listPIs.ForEach(pi => pi.SetValue(entity, pi.GetValue(this, null), null));
		}
		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(obj, null))
				return false;

			if (object.ReferenceEquals(this, obj))
				return true;

			if (this.GetType() != obj.GetType())
				return false;

			return this.Equals(obj as IEntity);
		}
		public void FromXml(string xmlEntity)
		{
			((Entity) Xml.FromXml(xmlEntity, this.GetType())).Clone(this);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public object GetValue(string property)
		{
			// Si propiedad es Hash, coloca valor hash y lo devuelve
			if (property == FieldHash)
			{
				string hash = FieldsHashCode;
				Reflex.SetPropertyValue(this, property, hash);
				return hash;
			}

			object value = Reflex.GetPropertyValue(this, property);
			if (Reflex.PropertyHasAttribute(this, property, typeof(EncryptAttribute)))
			{
				if (EncryptIters != 0)
					Encriptador.Iters = EncryptIters;
				if (!string.IsNullOrEmpty(EncryptPass))
					Encriptador.Pass = EncryptPass;
				if (!string.IsNullOrEmpty(EncryptSalt))
					Encriptador.Salt = EncryptSalt;

				// obtiene valor encriptado de acuerdo al tipo de propiedad
				switch(value.GetType().Name)
				{
					case "Byte[]":
						byte[] bytesToEncrypt = (byte[])value;
						byte[] bytesNoEncrypt = null;
						if (bytesToEncrypt.Length > 100)
						{
							bytesToEncrypt = new byte[100];
							Array.Copy((byte[])value, bytesToEncrypt, bytesToEncrypt.Length);
							bytesNoEncrypt = new byte[((byte[])value).Length - 100];
							Array.Copy((byte[])value, 100, bytesNoEncrypt, 0, bytesNoEncrypt.Length);
						}
						byte[] valueEncrypted = Encriptador.EncryptBytes(bytesToEncrypt);
						byte[] bytesLength = Convierte.IntToBytes(valueEncrypted.Length);
						value = bytesLength.Concat(valueEncrypted).ToArray();
						if(bytesNoEncrypt != null)
							value = ((byte[])value).Concat(bytesNoEncrypt).ToArray();
						break;
					default:
						value = Encriptador.Encrypt(value.ToString());
						break;
				}
			}

			return value;
		}
		public void SetValue(string property, object value)
		{
			// Si propiedad es Hash, coloca valor hash
			if (property == FieldHash)
			{
				Reflex.SetPropertyValue(this, property, FieldsHashCode);
				return;
			}

			if (Reflex.PropertyHasAttribute(this, property, typeof(EncryptAttribute)))
			{
				if (EncryptIters != 0)
					Encriptador.Iters = EncryptIters;
				if (!string.IsNullOrEmpty(EncryptPass))
					Encriptador.Pass = EncryptPass;
				if (!string.IsNullOrEmpty(EncryptSalt))
					Encriptador.Salt = EncryptSalt;
				
				// obtiene valor desencriptado de acuerdo al tipo de propiedad
				switch (Reflex.GetPropertyTypeName(this, property))
				{
					case "Double":
						string doubleDecrypt = Encriptador.Decrypt(value.ToString());
						value = double.Parse(doubleDecrypt);
						break;
					case "Int32":
						string intDecrypt = Encriptador.Decrypt(value.ToString());
						value = int.Parse(intDecrypt);
						break;
					case "Byte[]":
						byte[] bytesLength = new byte[4];
						Array.Copy(((byte[])value), bytesLength, bytesLength.Length);
						int lengthEncrypted = Convierte.BytesToInt(bytesLength);
						byte[] bytesEncrypted = new byte[lengthEncrypted];
						Array.Copy((byte[])value, 4, bytesEncrypted, 0, bytesEncrypted.Length);
						byte[] bytesNoEncrypted = null;
						if (((byte[])value).Length > 4 + lengthEncrypted)
						{
							int lengthNoEncrypted = ((byte[])value).Length - (4 + lengthEncrypted);
							bytesNoEncrypted = new byte[lengthNoEncrypted];
							Array.Copy((byte[])value, 4 + lengthEncrypted, bytesNoEncrypted, 0, bytesNoEncrypted.Length);
						}
						byte[] bytesDecrypted = Encriptador.DecryptBytes(bytesEncrypted);
						value = bytesNoEncrypted == null ? bytesDecrypted : bytesDecrypted.Concat(bytesNoEncrypted).ToArray();
						break;
					default:
						value = Encriptador.Decrypt(value.ToString());
						break;
				}
			}

			Reflex.SetPropertyValue(this, property, value);
		}
		public override string ToString()
		{
			return Xml.ToXml(this);
		}
		public string ToXml()
		{
			return Xml.ToXml(this);
		}

		#endregion

		#region ICloneable

		public abstract object Clone();

		#endregion

		#region IEquatable<RegIngreso>

		public bool Equals(IEntity entityRight)
		{
			if (ToXml() != entityRight.ToXml())
				return false;

			return true;
		}

		#endregion
		
		#endregion
	}
}

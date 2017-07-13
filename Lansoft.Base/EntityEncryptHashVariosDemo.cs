/* -------------------------------------------------------------
	Archivo: EntityEncryptHashVariosDemo.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/29
----------------------------------------------------------------*/

using Lansoft.Base;

namespace Lansoft.Base
{
	public class EntityEncryptHashVariosDemo : Entity
	{
		#region Propiedades

		[Field]
		[Key]
		public string Clave { get; set; }
		[Field]
		[Encrypt]
		public int CampoInt { get; set; }
		[Field]
		[Encrypt]
		public double CampoDouble { get; set; }
		[Field]
		[Encrypt]
		public string CampoStr { get; set; }
		[Field]
		[Encrypt]
		public byte[] CampoAByteCorto { get; set; }
		[Field]
		[Encrypt]
		public byte[] CampoAByteLargo { get; set; }

		[Hash]
		public string Hash { get; set; }

		#endregion

		#region Metodos

		public override int GetHashCode()
		{
			return string.Concat(

				Clave.ToString(),
				CampoInt.ToString(),
				Hash.ToString()

			).GetHashCode();
		}

		#endregion

		#region ICloneable

		public override object Clone()
		{
			IEntity entityClone = new EntityEncryptHashVariosDemo();
			Clone(entityClone);
			return entityClone;
		}

		#endregion
	}
}
/* -------------------------------------------------------------
	Archivo: EntityHashDemo.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/10
----------------------------------------------------------------*/

using Lansoft.Base;

namespace Lansoft.Base
{
	public class EntityHashDemo : Entity
	{
		#region Propiedades

		[Field]
		[Key]
		public string Clave1 { get; set; }
		[Field]
		[Key]
		public string Clave2 { get; set; }
		[Field]
		public string Campo1 { get; set; }
		[Field]
		public string Campo2 { get; set; }
		[Field]
		public string Campo3 { get; set; }

		[Hash]
		public string Hash { get; set; }

		#endregion

		#region Metodos

		public override int GetHashCode()
		{
			return string.Concat(

				Clave1.ToString(),
				Clave2.ToString(),
				Campo1.ToString(),
				Campo2.ToString(),
				Campo3.ToString(),
				Hash.ToString()

			).GetHashCode();
		}

		#endregion

		#region ICloneable

		public override object Clone()
		{
			IEntity entityClone = new EntityHashDemo();
			Clone(entityClone);
			return entityClone;
		}

		#endregion
	}
}

/* -------------------------------------------------------------
	Archivo: IEntity.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/10
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace Lansoft.Base
{
	public interface IEntity : ICloneable, IEquatable<IEntity>
	{
		#region Properties

		int EncryptIters { get; set; }
		string EncryptPass { get; set; }
		string EncryptSalt { get; set; }

		string FieldHash { get; }
		List<string> FieldKeys { get; }
		List<string> FieldNoKeys { get; }
		List<string> Fields { get; }
		string FieldsHashCode { get; }

		int HashIters { get; set; }
		string HashPass { get; set; }
		string HashSalt { get; set; }

		#endregion

		#region Methods

		void Clone(IEntity entity);
		void FromXml(string xmlEntity);
		object GetValue(string property);
		void SetValue(string property, object value);
		string ToXml();

		#endregion
	}
}

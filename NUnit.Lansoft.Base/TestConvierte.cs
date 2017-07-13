/* -------------------------------------------------------------
	Archivo: TestConvierte.cs
	Modulo: Test.Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/30
----------------------------------------------------------------*/

using NUnit.Framework;
using Lansoft.Base;

namespace NUnit.Lansoft.Base
{
	[TestFixture]
	class TestConvierte
	{
		[Test]
		public void Convierte_BytesToInt_ArrayByte_ret_int()
		{
			// Prepara
			byte[] bytes = new byte[] { 0, 3, 183, 222 };

			// Ejecuta
			int entero = Convierte.BytesToInt(bytes);

			// Comprueba 
			Assert.AreEqual(243678, entero);
		}

		[Test]
		public void Convierte_IntToBytes_int_ret_ArrayByte()
		{
			// Prepara
			int entero = 243678;

			// Ejecuta
			byte[] bytes = Convierte.IntToBytes(entero);

			// Comprueba 
			byte[] bytesEsperados = new byte[] { 0, 3, 183, 222 };
			Assert.AreEqual(bytesEsperados, bytes);
		}
	}
}

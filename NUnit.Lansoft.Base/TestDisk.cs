/* -------------------------------------------------------------
	Archivo: TestDisk.cs
	Modulo: Test.Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/agosto/31
----------------------------------------------------------------*/

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

using Lansoft.Base;

namespace NUnit.Lansoft.Base
{
	[TestFixture]
	public class TestDisk
	{
		[Test]
		public void Disk_GetDrivesDiscosRemovibles_void_ret_ArrayStr()
		{
			// Prepara
			Disk.SimulaDrives = true; // En produccion = false
			
			// Ejecuta
			string[] discos = Disk.GetDrivesDiscosRemovibles();

			// Comprueba (Cambiar según equipo equipo)
			Assert.AreEqual(2, discos.Length);
			Assert.AreEqual(@"F:\", discos[0]);
			Assert.AreEqual(@"G:\", discos[1]);
		}
	}
}

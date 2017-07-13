/* -------------------------------------------------------------
	Archivo: Convierte.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/30
----------------------------------------------------------------*/

using System;

namespace Lansoft.Base
{
	public static class Convierte
	{
		public static int BytesToInt(byte[] bytes)
		{
			string strBytes = string.Empty;
			for (int i = 0; i < bytes.Length; i++)
				strBytes += string.Format("{0:X}", bytes[i]);
			return Convert.ToInt32(strBytes, 16);
		}
		public static byte[] IntToBytes(int entero)
		{
			string hexa = string.Format("{0:X}", entero).PadLeft(8, '0');
			byte[] bytes = new byte[4];
			for (int i = 0; i < bytes.Length; i++)
				bytes[i] = Convert.ToByte(Convert.ToInt32(hexa.Substring(i * 2, 2), 16));
			return bytes;
		}
	}
}

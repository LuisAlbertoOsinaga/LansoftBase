/* -------------------------------------------------------------
	Archivo: Xml.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/07
----------------------------------------------------------------*/

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Lansoft.Base
{
	public class Xml
	{
		public static object FromXml(string xmlObject, Type tipo)
		{
			XmlSerializer xSer = new XmlSerializer(tipo);
			StringReader sr = new StringReader(xmlObject);
			object obj = xSer.Deserialize(sr);
			sr.Close();
			return obj;
		}
		public static string NoDeclarationAndNoRootAttributes(string xml)
		{
			try
			{
				XmlDocument xDoc = new XmlDocument();
				xDoc.LoadXml(xml);
				if (xDoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
					xDoc.RemoveChild(xDoc.FirstChild);
				xDoc.FirstChild.Attributes.RemoveAll();

				return xDoc.OuterXml;
			}
			catch (ArgumentException ex)
			{
				throw new ArgumentException("Excepcion en Xml.NoDeclarationAndNoRootAttributes(). Argumento xml nulo.", ex);
			}
			catch (XmlException ex)
			{
				throw new XmlException("Excepcion en Xml.NoDeclarationAndNoRootAttributes(). El argumento xml no es un documento valido.", ex);
			}
		}
		public static string ToXml(object obj)
		{
			string xmlObject = null;
			StringBuilder sb = new StringBuilder();
			XmlSerializer xSer = new XmlSerializer(obj.GetType());
			StringWriter sw = new StringWriter(sb);
			xSer.Serialize(sw, obj);
			sw.Close();

			xmlObject = Xml.NoDeclarationAndNoRootAttributes(sb.ToString());
			return xmlObject;
		}
	}
}

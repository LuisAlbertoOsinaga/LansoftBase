/* -------------------------------------------------------------
	Archivo: Reflex.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/29
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lansoft.Base
{
	public class Reflex
	{
		public static List<PropertyInfo> GetPropertiesInfo(object objeto)
		{
			// Si objeto es nulo, regresa nulo
			if (objeto == null)
				return null;

			// Consigue Tipo de objeto
			Type objectType = objeto.GetType();

			//  Consigue todas las Properties info del objeto
			PropertyInfo[] pis = objectType.GetProperties();
			if (pis == null || pis.Length == 0)
				return null;

			return pis.ToList();
		}
		public static List<PropertyInfo> GetPropertiesInfoWithAttributes(object objeto, params Type[] attributeTypes)
		{
			// Si objeto es nulo o attributesTypes es nulo o de longitud cero, regresa nulo
			if (objeto == null || attributeTypes == null || attributeTypes.Length == 0)
				return null;
			
			// Consigue Tipo de objeto
			Type objectType = objeto.GetType();

			//  Consigue todas las Properties info del objeto
			PropertyInfo[] pis = objectType.GetProperties();
			if (pis == null || pis.Length == 0)
				return null;

			// Consigue lista de propiedades del objeto con atributos requeridos
			var listPIWithAttributes = new List<PropertyInfo>();
			foreach (var pi in pis)
			{
				foreach (var tipoAtributo in attributeTypes)
				{
					if (Attribute.GetCustomAttribute(pi, tipoAtributo) != null)
						listPIWithAttributes.Add(pi);
				}
			}
			if (listPIWithAttributes.Count == 0)
				return null;

			return listPIWithAttributes;
		}
		public static Dictionary<string, object> GetPropertiesNameAndValue(object objeto, List<PropertyInfo> propertiesInfo)
		{
			// Si objeto es nulo o propertiesInfo es nulo o de longitud cero, regresa nulo
			if (objeto == null || propertiesInfo == null || propertiesInfo.Count == 0)
				return null;

			// Arma Diccionario de nombres y valores de las propiedades con los atributos requeridos
			// (Key: Nombre de la propiedad, Value: Valor de la propiedad)
			Dictionary<string, object> dicProperties = new Dictionary<string, object>();
			propertiesInfo.ForEach(pi => dicProperties[pi.Name] = pi.GetValue(objeto, null));

			return dicProperties;
		}
		public static Dictionary<string, object> GetPropertiesNameAndValueWithAttributes(object objeto, params Type[] attributeTypes)
		{
			// Consigue properties info de las propiedades con los tipos atributos requeridos
			List<PropertyInfo> listPIs = GetPropertiesInfoWithAttributes(objeto, attributeTypes);
			if (listPIs == null || listPIs.Count == 0)
				return null;

			return GetPropertiesNameAndValue(objeto, listPIs);
		}
		public static string GetPropertyTypeName(object objeto, string propertyName)
		{
			// Si objeto o propertyName es nulo, lanza excepcion
			if (objeto == null)
				throw new ArgumentException("Error en argumento objeto. objeto nulo.");
			if (propertyName == null || propertyName == string.Empty)
				throw new ArgumentException("Error en argumento propertyName. propertyName nulo o vacio.");

			List<PropertyInfo> pis = GetPropertiesInfo(objeto);
			if (pis == null || pis.Count == 0)
				throw new ArgumentException("Error en argumento objeto. objeto no tiene propiedades para conseguir valores.");
			PropertyInfo pi = pis.Where(p => p.Name == propertyName).FirstOrDefault();
			if (pi == null)
				throw new ArgumentException(String.Format("Error en argumento propertyName. objeto no tiene propiedad '{0}'.", propertyName));
			return pi.PropertyType.Name;
		}
		public static object GetPropertyValue(object objeto, string propertyName)
		{
			// Si objeto o propertyName es nulo, lanza excepcion
			if (objeto == null)
				throw new ArgumentException("Error en argumento objeto. objeto nulo.");
			if (propertyName == null || propertyName == string.Empty)
				throw new ArgumentException("Error en argumento propertyName. propertyName nulo o vacio.");

			List<PropertyInfo> pis = GetPropertiesInfo(objeto);
			if (pis == null || pis.Count == 0)
				throw new ArgumentException("Error en argumento objeto. objeto no tiene propiedades para conseguir valores.");
			PropertyInfo pi = pis.Where(p => p.Name == propertyName).FirstOrDefault();
			if (pi == null)
				throw new ArgumentException(String.Format("Error en argumento propertyName. objeto no tiene propiedad '{0}'.", propertyName));
			return pi.GetValue(objeto, null);
		}
		public static bool PropertyHasAttribute(object objeto, string propertyName, Type attributeType)
		{
			Type objectType = objeto.GetType();
			PropertyInfo pi = objectType.GetProperty(propertyName);
			if(pi == null)
				return false;
			return Attribute.GetCustomAttribute(pi, attributeType) != null;
		}
		public static void SetPropertyValue(object objeto, string propertyName, object value)
		{
			// Si objeto o propertyName es nulo, lanza excepcion
			if(objeto == null)
				throw new ArgumentException("Error en argumento objeto. objeto nulo.");
			if (propertyName == null || propertyName == string.Empty)
				throw new ArgumentException("Error en argumento propertyName. propertyName nulo o vacio.");

			List<PropertyInfo> pis = GetPropertiesInfo(objeto);
			if (pis == null || pis.Count == 0)
				throw new ArgumentException("Error en argumento objeto. objeto no tiene propiedades para asignar valores.");
			PropertyInfo pi = pis.Where(p => p.Name == propertyName).FirstOrDefault();
			if(pi == null)
				throw new ArgumentException(String.Format("Error en argumento propertyName. objeto no tiene propiedad '{0}'.", propertyName));
			pi.SetValue(objeto, value, null);
		}
	}
}
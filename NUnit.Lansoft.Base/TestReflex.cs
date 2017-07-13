/* -------------------------------------------------------------
	Archivo: TestReflex.cs
	Modulo: Test.Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/29
----------------------------------------------------------------*/

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

using Lansoft.Base;

namespace NUnit.Lansoft.Base
{
	[TestFixture]
	public class TestReflex
	{
		[Test]
		public void Reflex_GetPropertiesInfo_object_null_ret_null()
		{
			// Prepara
			ClasePruebaPropiedadesFields objeto = null;

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfo(objeto);

			// Comprueba
			Assert.IsNull(pis);
		}

		[Test]
		public void Reflex_GetPropertiesInfo_object_ret_ListPI()
		{
			// Prepara
			ClasePruebaSinPropiedadesFields objeto = new ClasePruebaSinPropiedadesFields
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfo(objeto);

			// Comprueba
			List<string> nombresProp = pis.ConvertAll(pi => pi.Name);
			Assert.IsTrue(nombresProp.IndexOf("FieldBool") >= 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldDT") >= 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldInt") >= 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldStr") >= 0);
		}

		[Test]
		public void Reflex_GetPropertiesInfo_object_sinPropiedades_ret_null()
		{
			// Prepara
			ClasePruebaSinPropiedades objeto = new ClasePruebaSinPropiedades();

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfo(objeto);

			// Comprueba
			Assert.IsNull(pis);
		}

		[Test]
		public void Reflex_GetPropertiesInfoWithAttributes_object_ArrayType_ret_ListPI()
		{
			// Prepara
			ClasePruebaPropiedadesFields objeto = new ClasePruebaPropiedadesFields
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute));

			// Comprueba
			List<string> nombresProp = pis.ConvertAll(pi => pi.Name);
			Assert.IsTrue(nombresProp.IndexOf("FieldBool") >= 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldDT") >= 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldInt") >= 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldStr") >= 0);
		}

		[Test]
		public void Reflex_GetPropertiesInfoWithAttributes_object_null_ArrayType_ret_null()
		{
			// Prepara
			ClasePruebaPropiedadesFields objeto = null;

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute));

			// Comprueba
			Assert.IsNull(pis);
		}

		[Test]
		public void Reflex_GetPropertiesInfoWithAttributes_object_sinAtributos_ArrayType_ret_null()
		{
			// Prepara
			ClasePruebaSinAtributos objeto = new ClasePruebaSinAtributos
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute));

			// Comprueba
			Assert.IsNull(pis);
		}

		[Test]
		public void Reflex_GetPropertiesInfoWithAttributes_object_sinLosAtributos_ArrayType_ret_null()
		{
			// Prepara
			ClasePruebaPropiedadesFields objeto = new ClasePruebaPropiedadesFields
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(KeyAttribute));

			// Comprueba
			Assert.IsNull(pis);
		}

		[Test]
		public void Reflex_GetPropertiesInfoWithAttributes_object_sinPropiedades_ArrayType_ret_null()
		{
			// Prepara
			ClasePruebaSinPropiedades objeto = new ClasePruebaSinPropiedades
			{
				Field = null
			};

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute));

			// Comprueba
			Assert.IsNull(pis);
		}

		[Test]
		public void Reflex_GetPropertiesInfoWithAttributes_object_variosAtributos_ArrayType_ret_ListPI()
		{
			// Prepara
			ClasePruebaKeyField objeto = new ClasePruebaKeyField
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};

			// Ejecuta
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute), typeof(KeyAttribute));

			// Comprueba
			List<string> nombresProp = pis.ConvertAll(pi => pi.Name);
			Assert.IsTrue(nombresProp.IndexOf("FieldBool") >= 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldDT") < 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldInt") < 0);
			Assert.IsTrue(nombresProp.IndexOf("FieldStr") >= 0);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValue_object_ListPIs_ret_Dictionary_str_object()
		{
			// Prepara
			DateTime dtAhora = DateTime.Now;
			ClasePruebaPropiedadesFields objeto = new ClasePruebaPropiedadesFields
			{
				FieldBool = true,
				FieldDT = dtAhora,
				FieldInt = 115,
				FieldStr = "Hello World!"
			};
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute));

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValue(objeto, pis);

			// Comprueba
			Assert.AreEqual(true, (bool) dicProperties["FieldBool"]);
			Assert.AreEqual(dtAhora, (DateTime)dicProperties["FieldDT"]);
			Assert.AreEqual(115, (int)dicProperties["FieldInt"]);
			Assert.AreEqual("Hello World!", (string)dicProperties["FieldStr"]);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValue_object_null_ListPIs_ret_null()
		{
			// Prepara
			ClasePruebaPropiedadesFields nullObject = null;
			ClasePruebaPropiedadesFields objeto = new ClasePruebaPropiedadesFields
			{
				FieldBool = false,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute));

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValue(nullObject, pis);

			// Comprueba
			Assert.IsNull(dicProperties);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValue_object_sinAtributos_ListPIs_null_ret_null()
		{
			// Prepara
			ClasePruebaSinAtributos objeto = new ClasePruebaSinAtributos
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute));

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValue(objeto, pis);

			// Comprueba
			Assert.IsNull(dicProperties);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValue_object_sinLosAtributos_ListPIs_null_ret_null()
		{
			// Prepara
			ClasePruebaPropiedadesFields objeto = new ClasePruebaPropiedadesFields
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(KeyAttribute));

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValue(objeto, pis);

			// Comprueba
			Assert.IsNull(dicProperties);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValue_object_sinPropiedades_ListPIs_null_ret_null()
		{
			// Prepara
			ClasePruebaSinPropiedades objeto = new ClasePruebaSinPropiedades
			{
				Field = null
			};
			List<PropertyInfo> pis = Reflex.GetPropertiesInfoWithAttributes(objeto, typeof(FieldAttribute));

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValue(objeto, pis);

			// Comprueba
			Assert.IsNull(dicProperties);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValueWithAttributes_object_ArrayType_ret_Dictionary_str_object()
		{
			// Prepara
			DateTime dtAhora = DateTime.Now;
			ClasePruebaPropiedadesFields objeto = new ClasePruebaPropiedadesFields
			{
				FieldBool = true,
				FieldDT = dtAhora,
				FieldInt = 115,
				FieldStr = "Hello World!"
			};

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValueWithAttributes(objeto, typeof(FieldAttribute));

			// Comprueba
			Assert.AreEqual(true, (bool)dicProperties["FieldBool"]);
			Assert.AreEqual(dtAhora, (DateTime)dicProperties["FieldDT"]);
			Assert.AreEqual(115, (int)dicProperties["FieldInt"]);
			Assert.AreEqual("Hello World!", (string)dicProperties["FieldStr"]);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValueWithAttributes_object_null_ArrayType_ret_null()
		{
			// Prepara
			ClasePruebaPropiedadesFields objeto = null;

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValueWithAttributes(objeto, typeof(FieldAttribute));

			// Comprueba
			Assert.IsNull(dicProperties);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValueWithAttributes_object_sinAtributos_ArrayType_ret_null()
		{
			// Prepara
			ClasePruebaSinAtributos objeto = new ClasePruebaSinAtributos
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValueWithAttributes(objeto, typeof(FieldAttribute));

			// Comprueba
			Assert.IsNull(dicProperties);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValueWithAttributes_object_sinLosAtributos_ArrayType_ret_null()
		{
			// Prepara
			ClasePruebaPropiedadesFields objeto = new ClasePruebaPropiedadesFields
			{
				FieldBool = true,
				FieldDT = DateTime.MinValue,
				FieldInt = 0,
				FieldStr = string.Empty
			};

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValueWithAttributes(objeto, typeof(KeyAttribute));

			// Comprueba
			Assert.IsNull(dicProperties);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValueWithAttributes_object_sinPropiedades_ArrayType_ret_null()
		{
			// Prepara
			ClasePruebaSinPropiedades objeto = new ClasePruebaSinPropiedades
			{
				Field = null
			};

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValueWithAttributes(objeto, typeof(FieldAttribute));

			// Comprueba
			Assert.IsNull(dicProperties);
		}

		[Test]
		public void Reflex_GetPropertiesNameAndValueWithAttributes_object_variosAtributos_ArrayType_ret_Dictionary_str_object()
		{
			// Prepara
			DateTime dtAhora = DateTime.Now;
			ClasePruebaKeyField objeto = new ClasePruebaKeyField
			{
				FieldBool = true,
				FieldDT = dtAhora,
				FieldInt = 115,
				FieldStr = "Hello World!"
			};

			// Ejecuta
			Dictionary<string, object> dicProperties = Reflex.GetPropertiesNameAndValueWithAttributes(objeto, typeof(FieldAttribute), typeof(KeyAttribute));

			// Comprueba
			Assert.AreEqual(true, (bool)dicProperties["FieldBool"]);
			Assert.IsFalse(dicProperties.ContainsKey("FieldDT"));
			Assert.IsFalse(dicProperties.ContainsKey("FieldInt"));
			Assert.AreEqual("Hello World!", (string)dicProperties["FieldStr"]);
		}

        [Test]
        public void Reflex_GetPropertyTypeName_object_str_ret_str()
        {
            // Prepara
            DateTime ahora = DateTime.Now;
            ClasePruebaPropiedadesKeys objeto = new ClasePruebaPropiedadesKeys
            {
                FieldBool = true,
                FieldDT = ahora,
                FieldInt = 100,
                FieldStr = "Test"
            };

            // Ejecuta
            string tipoBool = Reflex.GetPropertyTypeName(objeto, "FieldBool");
            string tipoDT = Reflex.GetPropertyTypeName(objeto, "FieldDT");
            string tipoInt = Reflex.GetPropertyTypeName(objeto, "FieldInt");
            string tipoStr = Reflex.GetPropertyTypeName(objeto, "FieldStr");

            // Comprueba
            Assert.AreEqual("Boolean", tipoBool);
            Assert.AreEqual("DateTime", tipoDT);
            Assert.AreEqual("Int32", tipoInt);
            Assert.AreEqual("String", tipoStr);
        }

        [Test]
		public void Reflex_GetPropertyValue_object_null_str_throw_ex()
		{
			// Prepara
			ClasePruebaPropiedadesKeys objeto = null;

			try
			{
				// Ejecuta
				object value = Reflex.GetPropertyValue(objeto, "FieldInt");
				Assert.Fail();
			}
			catch (ArgumentException ex)
			{
				// Comprueba
				Assert.AreEqual("Error en argumento objeto. objeto nulo.", ex.Message);
			}
		}

		[Test]
		public void Reflex_GetPropertyValue_object_sinPropiedades_str_throw_ex()
		{
			// Prepara
			ClasePruebaSinPropiedades objeto = new ClasePruebaSinPropiedades();

			try
			{
				// Ejecuta
				object value = Reflex.GetPropertyValue(objeto, "FieldInt");
				Assert.Fail();
			}
			catch (ArgumentException ex)
			{
				// Comprueba
				Assert.AreEqual("Error en argumento objeto. objeto no tiene propiedades para conseguir valores.", ex.Message);
			}
		}

		[Test]
		public void Reflex_GetPropertyValue_object_str_null_throw_ex()
		{
			// Prepara
			DateTime ahora = DateTime.Now;
			ClasePruebaPropiedadesKeys objeto = new ClasePruebaPropiedadesKeys
			{
				FieldBool = true,
				FieldDT = ahora,
				FieldInt = 100,
				FieldStr = "Test"
			};

			try
			{
				// Ejecuta
				object value = Reflex.GetPropertyValue(objeto, "");
				Assert.Fail();
			}
			catch (ArgumentException ex)
			{
				// Comprueba
				Assert.AreEqual("Error en argumento propertyName. propertyName nulo o vacio.", ex.Message);
			}
		}

		[Test]
		public void Reflex_GetPropertyValue_object_str_objetoNoTinePropiedad_throw_ex()
		{
			// Prepara
			DateTime ahora = DateTime.Now;
			ClasePruebaPropiedadesKeys objeto = new ClasePruebaPropiedadesKeys
			{
				FieldBool = true,
				FieldDT = ahora,
				FieldInt = 100,
				FieldStr = "Test"
			};

			try
			{
				// Ejecuta
				object value = Reflex.GetPropertyValue(objeto, "PropertyNoExist");
				Assert.Fail();
			}
			catch (ArgumentException ex)
			{
				// Comprueba
				Assert.AreEqual("Error en argumento propertyName. objeto no tiene propiedad 'PropertyNoExist'.", ex.Message);
			}
		}

		[Test]
		public void Reflex_GetPropertyValue_object_str_ret_object()
		{
			// Prepara
			DateTime ahora = DateTime.Now;
			ClasePruebaPropiedadesKeys objeto = new ClasePruebaPropiedadesKeys
			{
				FieldBool = true,
				FieldDT = ahora,
				FieldInt = 100,
				FieldStr = "Test"
			};

			// Ejecuta
			int value = (int) Reflex.GetPropertyValue(objeto, "FieldInt");

			// Comprueba
			Assert.AreEqual(100, value);
		}

		[Test]
		public void Reflex_PropertyHasAttribute_object_str_Type_ret_bool()
		{
			// Prepara
			ClasePruebaKeyField objeto = new ClasePruebaKeyField();

			// Ejecuta
			bool hasAttributeKey = Reflex.PropertyHasAttribute(objeto, "FieldStr", typeof(KeyAttribute));
			bool hasAttributeField = Reflex.PropertyHasAttribute(objeto, "FieldStr", typeof(FieldAttribute));

			// Comprueba
			Assert.IsTrue(hasAttributeKey);
			Assert.IsFalse(hasAttributeField);
		}

		[Test]
		public void Reflex_SetPropertyValue_object_null_str_object_throw_ex()
		{
			// Prepara
			ClasePruebaPropiedadesKeys objeto = null;

			try
			{
				// Ejecuta
				Reflex.SetPropertyValue(objeto, "FieldInt", 111);
				Assert.Fail();
			}
			catch (ArgumentException ex)
			{
				// Comprueba
				Assert.AreEqual("Error en argumento objeto. objeto nulo.", ex.Message);
			}
		}

		[Test]
		public void Reflex_SetPropertyValue_object_sinPropiedades_str_object_throw_ex()
		{
			// Prepara
			ClasePruebaSinPropiedades objeto = new ClasePruebaSinPropiedades();

			try
			{
				// Ejecuta
				Reflex.SetPropertyValue(objeto, "FieldInt", 111);
				Assert.Fail();
			}
			catch (ArgumentException ex)
			{
				// Comprueba
				Assert.AreEqual("Error en argumento objeto. objeto no tiene propiedades para asignar valores.", ex.Message);
			}
		}

		[Test]
		public void Reflex_SetPropertyValue_object_str_null_object_throw_ex()
		{
			// Prepara
			DateTime ahora = DateTime.Now;
			ClasePruebaPropiedadesKeys objeto = new ClasePruebaPropiedadesKeys
			{
				FieldBool = true,
				FieldDT = ahora,
				FieldInt = 100,
				FieldStr = "Test"
			};

			try
			{
				// Ejecuta
				Reflex.SetPropertyValue(objeto, "", 111);
				Assert.Fail();
			}
			catch (ArgumentException ex)
			{
				// Comprueba
				Assert.AreEqual("Error en argumento propertyName. propertyName nulo o vacio.", ex.Message);
			}
		}

		[Test]
		public void Reflex_SetPropertyValue_object_str_object_ret_void()
		{
			// Prepara
			DateTime ahora = DateTime.Now;
			ClasePruebaPropiedadesKeys objeto = new ClasePruebaPropiedadesKeys
			{
				FieldBool = true,
				FieldDT = ahora,
				FieldInt = 100,
				FieldStr = "Test"
			};

			// Ejecuta
			Reflex.SetPropertyValue(objeto, "FieldInt", 111);

			// Comprueba
			Assert.AreEqual(111, objeto.FieldInt);
			Assert.AreEqual(true, objeto.FieldBool);
			Assert.AreEqual(ahora, objeto.FieldDT);
			Assert.AreEqual("Test", objeto.FieldStr);
		}

		[Test]
		public void Reflex_SetPropertyValue_object_str_objetoNoTinePropiedad_object_throw_ex()
		{
			// Prepara
			DateTime ahora = DateTime.Now;
			ClasePruebaPropiedadesKeys objeto = new ClasePruebaPropiedadesKeys
			{
				FieldBool = true,
				FieldDT = ahora,
				FieldInt = 100,
				FieldStr = "Test"
			};

			try
			{
				// Ejecuta
				Reflex.SetPropertyValue(objeto, "PropertyNoExist", 111);
				Assert.Fail();
			}
			catch (ArgumentException ex)
			{
				// Comprueba
				Assert.AreEqual("Error en argumento propertyName. objeto no tiene propiedad 'PropertyNoExist'.", ex.Message);
			}
		}
	}

	#region Clases de Soporte a las pruebas

	class ClasePruebaKeyField
	{
		[Key]
		public string FieldStr { get; set; }
		public int FieldInt { get; set; }
		public DateTime FieldDT { get; set; }
		[Field]
		public bool FieldBool { get; set; }
	}
	class ClasePruebaSinAtributos
	{
		public string FieldStr { get; set; }
		public int FieldInt { get; set; }
		public DateTime FieldDT { get; set; }
		public bool FieldBool { get; set; }
	}
	class ClasePruebaPropiedadesFields
	{
		[Field]
		public string FieldStr { get; set; }
		[Field]
		public int FieldInt { get; set; }
		[Field]
		public DateTime FieldDT { get; set; }
		[Field]
		public bool FieldBool { get; set; }
	}
	class ClasePruebaSinPropiedades
	{
		public object Field = null;
	}
	class ClasePruebaSinPropiedadesFields
	{
		[Key]
		public string FieldStr { get; set; }
		[Key]
		public int FieldInt { get; set; }
		public DateTime FieldDT { get; set; }
		public bool FieldBool { get; set; }
	}

	class ClasePruebaPropiedadesKeys
	{
		[Field, Key]
		public string FieldStr { get; set; }
		[Field, Key]
		public int FieldInt { get; set; }
		[Field]
		public DateTime FieldDT { get; set; }
		[Field]
		public bool FieldBool { get; set; }
	}
	class ClasePruebaSinPropiedadesKeys
	{
		[Field]
		public string FieldStr { get; set; }
		public int FieldInt { get; set; }
		public DateTime FieldDT { get; set; }
		[Field]
		public bool FieldBool { get; set; }
	}

	#endregion
}

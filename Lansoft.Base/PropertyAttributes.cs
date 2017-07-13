/* -------------------------------------------------------------
	Archivo: PropertyAttributes.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/junio/02
----------------------------------------------------------------*/

using System;

namespace Lansoft.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class EncryptAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FieldAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HashAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class KeyAttribute : Attribute
    {
    }
}

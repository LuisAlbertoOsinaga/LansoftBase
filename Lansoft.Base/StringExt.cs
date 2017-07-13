/* -------------------------------------------------------------
	Archivo: StringExt.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/02
----------------------------------------------------------------*/

namespace Lansoft.Base
{
    public static class StringExt
    {
        public static bool IsNullOrBlank(string texto)
        {
            if (texto != null)
                texto = texto.Trim();
            return string.IsNullOrEmpty(texto);
        }
    }
}

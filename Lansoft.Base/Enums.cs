/* -------------------------------------------------------------
	Archivo: Enums.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/mayo/07
----------------------------------------------------------------*/

namespace Lansoft.Base
{
    public enum DBConnectionState
    {
        Closed = 0,
        Open = 1,
        Connecting = 2,
        Executing = 4,
        Fetching = 8,
        Broken = 16
    }
}

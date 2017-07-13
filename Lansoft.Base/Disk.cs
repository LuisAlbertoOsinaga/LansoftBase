/* -------------------------------------------------------------
	Archivo: Disk.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/junio/02
----------------------------------------------------------------*/

using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Lansoft.Base
{
    public class Disk
    {
        #region Constructores

        static Disk()
        {
            SimulaDrives = false;
        }
        
        #endregion

        #region Propiedades

        public static bool SimulaDrives { get; set; }

        #endregion

        #region Metodos

        public static string[] GetDrivesDiscosRemovibles()
        {
            if (SimulaDrives)
                return new string[] { "F:\\", "G:\\" };

            return (from drive in DriveInfo.GetDrives()
                    where drive.DriveType == DriveType.Removable
                    select drive.Name).ToArray();
        }
        
        #endregion
    }
}

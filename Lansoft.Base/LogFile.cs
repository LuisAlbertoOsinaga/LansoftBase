/* -------------------------------------------------------------
	Archivo: LogFile.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/agosto/31
----------------------------------------------------------------*/

using System;
using System.IO;
using System.Text;

namespace Lansoft.Base
{

    public class LogFile
    {
        #region "Campos"


        private string m_archivo;
        #endregion

        #region "Propiedades"

        public string Archivo
        {
            get { return m_archivo; }
            set { m_archivo = value; }
        }

        #endregion

        #region "Constructores"

        public LogFile(string archivo)
        {
            this.Archivo = archivo;
        }

        #endregion

        #region "Metodos"

        public string Read(string codigo)
        {

            // Preapra linea codigo
            string lineaCodigo = "### " + codigo + " ###";

            // Busca linea codigo en archivo
            string[] lineas = File.ReadAllLines(Archivo);
            int i = 0;
            for (i = 0; i <= lineas.Length - 1; i++)
            {
                if (lineas[i] == lineaCodigo)
                {
                    break; // TODO: might not be correct. Was : Exit For
                }
            }

            // Si encuentra, devuelve mensaje
            if (i <= lineas.Length - 1)
            {
                int j = i + 1;
                StringBuilder mensaje = new StringBuilder();
                while (j < lineas.Length && !string.IsNullOrEmpty(lineas[j]))
                {
                    mensaje.AppendLine(lineas[j]);
                    j += 1;
                }
                return mensaje.ToString();
            }

            // Devuelve mensaje de errror
            return "*** ERROR ***";

        }
        public string Write(string mensaje)
        {

            // Arma codigo con fecha y Guid
            string fechaHora = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string subcodigo = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            string codigo = fechaHora + " " + subcodigo;

            // Prepara linea codigo
            string lineaCodigo = "### " + codigo + " ###";

            // Prepara lineas mensaje
            string lineasMensajes = lineaCodigo + Environment.NewLine + mensaje + Environment.NewLine;

            // Agrega a archivo log
            File.AppendAllText(Archivo, lineasMensajes);

            // Devuelve codigo
            return codigo;

        }
        public string WriteException(Exception ex)
        {
            StringBuilder mensaje = new StringBuilder();
            do
            {
                mensaje.AppendLine("*** Exception ***");
                mensaje.AppendLine(string.Format("Tipo: {0}", ex.GetType().FullName));
                mensaje.AppendLine(string.Format("Mensaje: {0}", ex.Message));
                mensaje.AppendLine(string.Format("Fuente: {0}", ex.Source));
                mensaje.AppendLine(string.Format("Método: {0}", ex.TargetSite.Name));
                mensaje.AppendLine(string.Format("StackTrace: {0}", ex.StackTrace));
                ex = ex.InnerException;
            } while (ex != null);
            return Write(mensaje.ToString());
        }

        #endregion
    }

}
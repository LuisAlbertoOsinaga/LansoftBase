/* -------------------------------------------------------------
	Archivo: TestLogFile.cs
	Modulo: Test.Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/Agosto/31
----------------------------------------------------------------*/

using NUnit.Framework;
using System;
using Lansoft.Base;

namespace NUnit.Lansoft.Base
{

    [TestFixture()]
    public class TestLogFile
    {

        [Test()]
        public void LogFile_Write_str_ret_str()
        {
            // Prepara
            LogFile logFile = new LogFile("logErrores.txt");

            // Ejecuta
            string mensajeIn = "Este es un mensaje de prueba" + Environment.NewLine + "Con una segunda linea" + Environment.NewLine;
            string codigo = logFile.Write(mensajeIn);

            // Comprueba
            string mensajeOut = logFile.Read(codigo);
            Assert.AreEqual(mensajeOut, mensajeIn);
        }

    }

}

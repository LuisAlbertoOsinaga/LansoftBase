/* -------------------------------------------------------------
	Archivo: HashGenerator.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/octubre/25
----------------------------------------------------------------*/

using System;
using System.Security.Cryptography;
using System.Text;

namespace Lansoft.Base
{
	public class HashGenerator
	{
		#region Constructors

		public HashGenerator()
		{
            ChangeParams = true;
            HashAlgoritmo = null;
            Iters = 1123;
			Pass = "PASSWORD_TEST_ABRETE_SESAMO";
			Salt = "SALT_VALUE_TEST_VALOR_SALADO";
		}

		#endregion

		#region Methods

		public string GetHashCode(string info)
		{
            // Prepara hasheador
            HMACSHA1 hasheador = PrepareHasher();

			// Hashea
            hasheador.ComputeHash(Encoding.ASCII.GetBytes(info));
            return Convert.ToBase64String(hasheador.Hash);
		}

		#endregion

        #region Properties

        private bool ChangeParams { get; set; }
        public int Iters
        {
            get { return m_iters; }
            set
            {
                if (m_iters != value)
                {
                    m_iters = value;
                    ChangeParams = true;
                }
            }
        }
        public string Pass
        {
            get { return m_pass; }
            set
            {
                if (m_pass != value)
                {
                    m_pass = value;
                    ChangeParams = true;
                }
            }
        }
        public string Salt
        {
            get { return m_salt; }
            set
            {
                if (m_salt != value)
                {
                    m_salt = value;
                    ChangeParams = true;
                }
            }
        }
        private HMACSHA1 HashAlgoritmo { get; set; }

        #endregion

        #region Metodos Privados

        private HMACSHA1 PrepareHasher()
        {
            if (ChangeParams)
            {
                byte[] bytesSalt = Encoding.ASCII.GetBytes(Salt);
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Pass, bytesSalt, Iters);
                byte[] key = deriveBytes.GetBytes(16);
                HashAlgoritmo = new HMACSHA1(key);
            }
            return HashAlgoritmo;
        }

        #endregion

        #region Campos

        private int m_iters;
        private string m_pass;
        private string m_salt;

        #endregion
    }
}

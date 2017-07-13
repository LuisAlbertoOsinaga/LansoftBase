/* -------------------------------------------------------------
	Archivo: EntityDemo2.cs
	Modulo: Lansoft.Base
	Autor: LAOS
	Actualizado: 2010/septiembre/15
----------------------------------------------------------------*/

using Lansoft.Base;

namespace Lansoft.Base
{
    public class EntityDemo2 : Entity
    {
        #region Propiedades

        [Field]
        [Key]
        public decimal Clave { get; set; }
        [Field]
        public string Campo { get; set; }
        [Field]
        public byte[] Imagen { get; set; }

        #endregion

        #region Metodos

        public override int GetHashCode()
        {
            return string.Concat(

                Clave.ToString(),
                Campo.ToString(),
                Imagen.ToString()

            ).GetHashCode();
        }

        #endregion

        #region ICloneable

        public override object Clone()
        {
            IEntity entityClone = new EntityDemo2();
            Clone(entityClone);
            return entityClone;
        }

        #endregion
    }
}

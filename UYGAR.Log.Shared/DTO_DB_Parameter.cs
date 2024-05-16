using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UYGAR.Log.Shared
{
    [Serializable]
    public  class DTO_DB_Parameter
    {
        #region Constructor

        public DTO_DB_Parameter() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType">Veriden Gelen Alan Tipini Alır.</param>
        /// <param name="parameterName">Sorgudan Gelen Parametre Adını Alır.</param>
        /// <param name="value">Sorgudan Gelen Değeri Alır.</param>
        public DTO_DB_Parameter(string dbType, string parameterName, object value)
        {
            DbType = dbType;
            ParameterName = parameterName;
            Value = value;
        }

        #endregion

        /// <summary>
        /// Veriden Gelen Alan Tipini Alır.
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// Sorgudan Gelen Parametre Adını Alır.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Sorgudan Gelen Değeri Alır.
        /// </summary>
        public object Value { get; set; }
    }
}

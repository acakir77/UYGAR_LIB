using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using UYGAR.Data.Attributes;

namespace UYGAR.Data.Base
{
    public class IDataReaderValues : IDisposable
    {
        public int GetInt(IDataReader reader, string collName)
        {
            int index = reader.GetOrdinal(collName);
            if (reader.IsDBNull(index))
                return 0;
            else
                return reader.GetInt32(index);
        }
        public int GetInt(IDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
                return 0;
            else
            {
                var valu = reader[index];
                return Convert.ToInt32(valu);}
               
        }
        public float GetFloat(IDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
                return 0;
            else
            {
                string floatstr = string.Format("{0}", reader[index]);
                return float.Parse(floatstr);
            }
        }
        public object GetValue(IDataReader reader, string collName)
        {
            int collindex = reader.GetOrdinal(collName);
            if (reader.IsDBNull(collindex))
                return null;
            else return reader.GetValue(collindex);

        }
        public object GetValue(IDataReader reader, int idxno, Type propertyType)
        {

            if (reader.IsDBNull(idxno))
                return null;
            else return Convert.ChangeType(reader.GetValue(idxno), propertyType);

        }
        public object GetValue(IDataReader reader, int idxno)
        {

            if (reader.IsDBNull(idxno))
                return null;
            else return reader.GetValue(idxno);

        }
        public string GetString(IDataReader reader, string collName)
        {
            int index = reader.GetOrdinal(collName);
            if (reader.IsDBNull(index))
                return string.Empty;
            else
                return string.Format("{0}", reader[index]);
        }
        public string GetString(IDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
                return string.Empty;
            else
                return reader.GetString(index);
        }
        public DateTime GetDatetime(IDataReader reader, string collName)
        {

            int index = reader.GetOrdinal(collName);
            if (reader.IsDBNull(index))
                return new DateTime(1901, 01, 01);
            else
                return reader.GetDateTime(index);
        }
        public DateTime GetDatetime(IDataReader reader, int index)
        {


            if (reader.IsDBNull(index))
                return new DateTime(1901, 01, 01);
            else
                return reader.GetDateTime(index);
        }
        public bool GetBoolen(IDataReader reader, string collName)
        {

            int index = reader.GetOrdinal(collName);
            if (reader.IsDBNull(index))
                return false;
            else
                return reader.GetBoolean(index);
        }
        public bool GetBoolen(IDataReader reader, int index)
        {


            if (reader.IsDBNull(index))
                return false;
            else
                return reader.GetBoolean(index);
        }
        public object GetEnum(IDataReader reader, string indexname, Type enumtype)
        {
            int indexNo = reader.GetOrdinal(indexname);
            if (reader.IsDBNull(indexNo))
                return Enum.Parse(enumtype, "0");
            else
                return Enum.Parse(enumtype, string.Format("{0}", reader.GetValue(indexNo)));
        }
        public object GetEnum(IDataReader reader, int indexNo, Type enumtype)
        {
            if (reader.IsDBNull(indexNo))
                return Enum.Parse(enumtype, "0");
            else
                return Enum.Parse(enumtype, string.Format("{0}", reader.GetValue(indexNo)));
        }
        public T GetEnum<T>(IDataReader reader, int indexNo)
        {

            if (reader.IsDBNull(indexNo))
                return (T)Enum.Parse(typeof(T), "0");
            else
                return (T)Enum.Parse(typeof(T), string.Format("{0}", reader.GetValue(indexNo)));
        }
        public T GetModel<T>(IDataReader reader, ref int indexname) where T : Model, new()
        {

            if (reader.IsDBNull(indexname))
            {
                PropertyInfo[] infos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                int k = indexname + 9;
                Array.ForEach(infos, info =>
                {
                    if ((ColumnAttribute.GetColumnAttribute(info) != null || AssociationAttribute.GetName(info) != null) && !info.PropertyType.Name.StartsWith("ModelCollection"))
                        k = k + 1;
                });
                indexname = k;
                return null;
            }

            T obej = new T();
            indexname = obej.PopulatereaderFirst(reader, indexname);
            return obej;

        }
        public T GetModelFirts<T>(IDataReader reader, int indexname) where T : Model, new()
        {
            if (reader.IsDBNull(indexname))
                return null;

            T obj = new T {OID = reader.GetInt32(indexname)};
            return obj;

        }
        public Model GetModelFirst(IDataReader reader, string indexname, Type modelType)
        {

            int indexNo = reader.GetOrdinal(string.Format("{0}", indexname));
            if (reader.IsDBNull(indexNo))
                return null;
            Model obj = (Model)Activator.CreateInstance(modelType);
            obj.OID = reader.GetInt32(indexNo);
            return obj;

        }
        public byte[] GetBytes(IDataReader reader, string collName)
        {
            int index = reader.GetOrdinal(collName);
            if (reader.IsDBNull(index))
                return new byte[0];
            else
                return (byte[])reader.GetValue(index);
        }
        public byte[] GetBytes(IDataReader reader, int index)
        {

            if (reader.IsDBNull(index))
                return new byte[0];
            else
                return (byte[])reader.GetValue(index);
        }
        public decimal GetDecimal(IDataReader reader, int index)
        {

            if (reader.IsDBNull(index))
                return 0;
            else
                return reader.GetDecimal(index);
        }
        public double GetDouble(IDataReader reader, int index)
        {

            if (reader.IsDBNull(index))
                return 0;
            else
                return reader.GetDouble(index);
        }
        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

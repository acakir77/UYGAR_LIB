using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;

using System.Web;
using UYGAR.Data.Attributes;
using UYGAR.Data.Configuration;
using UYGAR.Data.Connection;
using UYGAR.Data.Criterias;
using UYGAR.Log.Shared;

namespace UYGAR.Data.Base
{
    [Serializable]
    public class DbConnectionBase
    {
        public static string SequencerTablename = "SEQUENCER";
        public ApplicationConfig config;
        public string LastQuery { get; set; }

        protected DbConnectionBase()
        {


            if (HttpContext.Current != null)
            {

                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["TaxPayer"] != null)
                    {
                        TaxPayerDescendant.UserId = ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).UserId;
                        TaxPayerDescendant.Adi = ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).Adi;
                        TaxPayerDescendant.Soyadi = ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).Soyadi;
                        TaxPayerDescendant.IpAdres = ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).IpAdres;
                        TaxPayerDescendant.MacAdress = ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).MacAdress;
                        TaxPayerDescendant.ComputareName = ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).ComputareName;
                        TaxPayerDescendant.KullaniciAdi = ((TaxPayerWeb)HttpContext.Current.Session["TaxPayer"]).KullaniciAdi;
                      
                    }

                }


            }


        }
        public virtual string Shema => "dbo";
        public virtual string DbConnectionString => String.Empty;
        public virtual string DbName => String.Empty;
        public void SaveObject(Model _object)
        {

            if (_object == null)
                throw new Exception("Obje Boş Olmaz!!!");
            if (_object.OID == -1 || _object.OID.Equals(0))
                InsertObject(_object);
            else
                UpdateObject(_object);

        }


        public virtual string ConvertFieldName(string newName)
        {
            return newName;
        }
        public static string GetApplicationDirectory
        {
            get
            {
                var s = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                if (s != null)
                    return s.Replace(@"file:\", "");
                return "";
            }
        }
        public object GetPropertyInfoValue(PropertyInfo info, object _Object)
        {
            if (info.GetValue(_Object, null) != null)
            {
                if (!info.PropertyType.IsEnum)
                    return info.GetValue(_Object, null);
                else
                    return Convert.ToInt32(info.GetValue(_Object, null));
            }
            return DBNull.Value;
        }
        public virtual void InsertObject(Model _object)
        { }
        public virtual void InsertLog(DbActionType dbActionType, int RowVersion, int objectOid, string objectTableName, Type ObjectType, string query, DateTime time, List<DbParameter> parametrs)
        {

        }

        public virtual void UpdateObject(Model _object)
        { }
        public virtual void DeleteObject(Model _object)
        { }

        public virtual int ExecuteScalarInsert(string query, List<DbParameter> parameters)
        {

            int retval = -1;
            return retval;
        }

        public virtual object ExecuteScalar(string query)
        {
            return null;
        }
        public virtual int ExecuteScalar(string query, List<DbParameter> parametrs)
        {
            return 0;
        }
        public virtual int ExecuteScalarParams(string query, List<QueryParameters> dbParameters)
        {
            return 0;
        }
        public virtual DataTable ExecuteDataTable(string query)
        {
            return null;
        }
        public virtual DataTable ExecuteDataTable(string query, List<QueryParameters> dbParameters)
        { return null; }

        public virtual string db_old_query(Model _object)
        {
            return String.Empty;
        }

        public string TypeName(PropertyInfo fieldInfo)
        {
            return TypeName(fieldInfo.PropertyType, SizeAttribute.GetSize(fieldInfo));
        }

        public string TypeName(Type type, SizeAttribute sizeAttribute)
        {
            if (type == typeof(decimal))
                return $"decimal({sizeAttribute?.Precision ?? 18}, {sizeAttribute?.Scale ?? 8})";
            int size = sizeAttribute?.MaxSize ?? 1000;
            return GetTypeName(type, size);
        }

        public virtual string GetTypeName(Type type, int size)
        {
            return String.Empty;
        }

        public virtual string ToDbProviderString(Type targetType, object value)
        {
            return value.ToString();
        }
        public virtual string ToDbProviderString(Type targetType, object value, bool isdatetime)
        {
            return value.ToString();
        }
        #region LoadData
        public T LoadObject<T>(int oId) where T : Model, new()
        {
            if (oId != -1)
            {
                return LoadObject<T>(new CompareCriteria("OID", oId));
            }
            return null;
        }
        public virtual T LoadObject<T>(Criteria criteria) where T : Model, new()
        {

            return null;

        }
        public virtual T LoadObjectTemp<T>(Criteria criteria) where T : Model, new()
        {

            return null;

        }
        public virtual T LoadObject<T>(Criteria criteria, Order orders) where T : Model, new()
        {
            return null;
        }
        public ModelCollection<TBaseObject> LoadObjects<TBaseObject>() where TBaseObject : Model, new()
        {
            return LoadObjects<TBaseObject>(new Criteria());
        }
        public virtual ModelCollection<Model> LoadObjects(Type type, Criteria criteria)
        {
            return null;
        }
        public virtual ModelCollection<TBaseObject> LoadObjects<TBaseObject>(Criteria criteria) where TBaseObject : Model, new()
        {
            return null;






        }

        public virtual ModelCollection<TBaseObject> LoadObjects<TBaseObject>(Criteria criteria, Order orders) where TBaseObject : Model, new()
        {
            return null;
        }
        public virtual string GetSelectQuery(Type modeltype)
        {
            return String.Empty;
        }

        public virtual string PopulatequeryString(Type modelType, string tableName, int i)
        {
            return String.Empty;
        }
        public virtual string GetMaxValue(Type baseObjectType, string coloumName, Criteria criteria)
        {

            return String.Empty;

        }

        public virtual string GetMaxValue(Type baseObjectType, string coloumName)
        {
            return String.Empty;
        }
        public virtual string GetMinValue(Type baseObjectType, string coloumName)
        {

            return String.Empty;


        }
        public virtual decimal GetSumValue(Type baseObjectType, string coloumName, Criteria criteria)
        {
            return 0;
        }
        public virtual decimal GetSumValue(Type baseObjectType, string coloumName)
        {
            return 0;
        }
        public virtual string GetMinValue(Type baseObjectType, string coloumName, Criteria criteria)
        {
            return String.Empty;
        }
        public virtual List<T> LoadRaportBaseObjects<T>(string sql) where T : ModelRaporBase, new()
        {
            return null;
        }
        public virtual List<T> LoadRaportBaseObjects<T>(string sql, List<QueryParameters> dbParameters) where T : ModelRaporBase, new()
        {
            return null;
        }
        public virtual T LoadRaportBaseObje<T>(string sql) where T : ModelRaporBase, new()
        {
            return null;
        }

        public virtual DataSet LoadToDataSet(Type type, Criteria criter)
        {
            return null;
        }
        public virtual DataSet LoadToDataSet(string query)
        {
            return null;
        }
        public virtual DataTable LoadToDataTable(string query)
        {
            return null;
        }
        #endregion
        #region CreateTable
        public virtual void CreateSchema(Type objectType)
        {

        }

        public virtual void AddTable(string tableName, PropertyInfo[] propertyInfos, string description)
        {

        }

        public virtual bool IsTableAvailable(string tablename, string schemaname)
        {
            return false;
        }

        public virtual DataTable GetMetaData(string tablename)
        {
            return null;
        }

        public virtual void CreateTable(string tablename, string description)
        {

        }

        public bool IsFieldAvaible(DataTable metadataTable, string fieldName)
        {
            DataRow row = GetFieldInfoInMetadataTable(metadataTable, fieldName);
            return row != null;
        }
        private DataRow GetFieldInfoInMetadataTable(DataTable metadataTable, string fieldName)
        {
            return metadataTable.Rows.Cast<DataRow>().FirstOrDefault(row => (string)row["name"] == fieldName);
        }
        public bool IsFieldUpdateRequired(DataTable metadataTable, PropertyInfo propertyInfo)
        {
            string columnName = ColumnAttribute.GetColumnAttribute(propertyInfo) != null ? ColumnAttribute.GetName(propertyInfo) : AssociationAttribute.GetName(propertyInfo);
            if (propertyInfo == null)
                return false;
            if (propertyInfo.Name == columnName)
            {
                //m_fieldInfos[fName];

            }

            if (propertyInfo.GetType() == typeof(decimal))
                return true;

            DataRow row = GetFieldInfoInMetadataTable(metadataTable, columnName);

            string fieldType = row[1].ToString();
            int fieldSize = string.IsNullOrEmpty($"{row[2]}") ? 0 : Convert.ToInt32(row[2]);

            if (fieldType.Equals("varchar"))
                fieldType = $"varchar({fieldSize})";

            if (propertyInfo.GetType() == typeof(string) && fieldType != "text")
            {
                SizeAttribute attr = SizeAttribute.GetSize(propertyInfo);
                int size = 0;
                if (attr != null)
                    size = attr.MaxSize;

                if (fieldType == propertyInfo.GetType().Name && fieldSize == size)
                    return false;
            }
            else
                if (fieldType == TypeName(propertyInfo))
                return false;

            return false;
        }
        public virtual void UpdateField(string tableName, string schemaname, string fieldName, string type)
        { }
        public virtual void AddForeignKey(string tableName, PropertyInfo propertyInfo)
        { }
        public virtual void AddForeignKey(string fkeyName, string schemaName, string tableName, string masterFieldName, string detailTableName, string detailFieldName)
        { }

        public virtual string GetSequencerNumber(string keyname)
        {
            return string.Empty;

        }
        public virtual int GetSequencerNumber(string tablename, string schemaName)
        {
            return 0;

        }

        public virtual void AddField(string tableName, string fieldName, string type, bool canBeNull, string afterField)
        {
            InsertField(tableName, Shema, fieldName, type, canBeNull, afterField);
        }
        public virtual void InsertField(string tableName, string schemaname, string fieldName, string type, bool canBeNull, string afterField)
        { }

        public virtual void DelletAllIndex(string tablename, string schemaName)
        {

        }

        public virtual void AddIndex(string tableName, List<string> grupIndexList, bool isUnique)
        {

        }

        public virtual void AddEnumTable(string tableName, Type objectType, FieldInfo[] fieldInfos)
        {

        }
        #endregion

    }
}

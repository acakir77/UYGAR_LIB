using System;
using System.Reflection;
using System.Xml.Serialization;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;

namespace UYGAR.Data.Criterias
{

    /// <summary>
    /// Kar��la�t�rma kriteri. Bir �zelli�in alabilece�i de�er i�in "e�it", "e�itde�il", "k���k", "k���ke�it",
    /// "b�y�k", "b�y�ke�it", "i�inde" kar��la�t�rmalar�n� sa�lar.
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(CompareCriteria))]
    public class CompareCriteria : Criteria
    {
        #region Private Fields
        #endregion

        #region Public Properties

        /// <summary>
        /// Kritere konu olan �yenin ad�
        /// </summary>

        public string MemberName { get; set; }

        /// <summary>
        /// Kriter ko�ulu
        /// </summary>

        public CompareOperator CompareOperator { get; set; }

        /// <summary>
        /// Kriter de�eri
        /// </summary>

        public object Value { get; set; }
        #endregion

        #region Constructors
        public CompareCriteria()
        {

        }
        /// <summary>
        /// Kar��la�t�rma kriteri. Bir �zelli�in alabilece�i de�er i�in "e�it", "e�itde�il", "k���k", "k���ke�it",
        /// "b�y�k", "b�y�ke�it", "i�inde" kar��la�t�rmalar�n� sa�lar.
        /// </summary>
        /// <param name="memberName">�ye ad�</param>
        /// <param name="_operator">Operator</param>
        /// <param name="value">De�er</param>

        public CompareCriteria(string memberName, CompareOperator _operator, object value)
        {
            this.MemberName = memberName;
            this.CompareOperator = _operator;
            this.Value = value;
        }

        /// <summary>
        /// Kar��la�t�rma kriteri. Bir �zelli�in alabilece�i de�er i�in "e�it" kar��la�t�rmas�n� sa�lar.
        /// </summary>
        /// <param name="memberName">�ye ad�</param>
        /// <param name="value">De�er</param>
        public CompareCriteria(string memberName, object value)
            : this(memberName, CompareOperator.Equal, value)
        {
        }
        #endregion

        protected void OrganizeValue(ref PropertyInfo memberInfo, ref object _value)
        {

            _value = this.Value;
            if (_value.GetType() == typeof(Boolean))
                _value = Convert.ToInt32(_value);
            if (memberInfo.PropertyType.IsSubclassOf(typeof(Model)))
            {

                _value = ((Model)_value).OID;
            }
        }

        protected void ValidateParameters(PropertyInfo memberInfo, object _value)
        {
            if (memberInfo.PropertyType.IsValueType)
            {
                try
                {
                    Convert.ChangeType(_value, memberInfo.PropertyType);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Sql ko�ul par�ac��� �retir.
        /// </summary>
        /// <param name="info">Ko�ulun uygulanaca�� BaseObject'in Type'�</param>
        /// <param name="type"></param>
        /// <param name="connection"></param>
        /// <returns>Sql ko�ul par�ac���</returns>
        public override string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            PropertyInfo member = type.GetProperty(this.MemberName);
            System.Diagnostics.Debug.Assert(member != null,
                string.Format("Member '{0}' not found in class '{1}'", this.MemberName, type.Name));

            object _value = null;
            string criteria = string.Empty;
            OrganizeValue(ref member, ref _value);

            ValidateParameters(member, _value);
            string tableName = TableAttribute.GetName(type);
            string fieldName = "";

            if (member.PropertyType.IsSubclassOf(typeof(Model)))
                fieldName = string.Format("{0}.{1}_ID", tableName, this.MemberName);
            else
                fieldName = string.Format("{0}.{1}", tableName, this.MemberName);
            fieldName = connection.ConvertFieldName(fieldName);


            switch (this.CompareOperator)
            {
                case CompareOperator.Equal:
                    criteria = string.Format(" {0} = {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    //if (member.PropertyType == typeof(string))
                    //    criteria = string.Format("{0}= {1}", fieldName, DbConnection.ToDbProviderString(member.PropertyType, _value));
                    //else
                    //    criteria = string.Format(" {0} = {1}", fieldName, DbConnection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.Big:
                    criteria = string.Format(" {0} > {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.Little:
                    criteria = string.Format(" {0} < {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.BigEqual:
                    criteria = string.Format(" {0} >= {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.LittleEqual:
                    criteria = string.Format(" {0} <= {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.Like:
                    System.Diagnostics.Trace.Assert(member.PropertyType.Equals(typeof(string)),
                        "Like compares only accept to string type");
                    criteria = string.Format(" {0} LIKE {2}%{1}%{2}", fieldName, _value, "'");
                    break;
                case CompareOperator.StartWith:
                    System.Diagnostics.Trace.Assert(member.PropertyType.Equals(typeof(string)),
                        "Like compares only accept to string type");
                    criteria = string.Format("{0} LIKE {2}{1}%{2}", fieldName, _value, "'");
                    break;
                case CompareOperator.EndWith:
                    System.Diagnostics.Trace.Assert(member.PropertyType.Equals(typeof(string)),
                        "Like compares only accept to string type");
                    criteria = string.Format("{0} LIKE {2}%{1}{2}", fieldName, _value, "'");
                    break;
                case CompareOperator.NotEqual:
                    criteria = string.Format(" {0} <> {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.BigSmallCharSensitive:
                    criteria = string.Format(" {0} = {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.DateEqual:
                    criteria = string.Format("CAST({0} AS DATE) = {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.DateTimeEqual:
                    criteria = string.Format("CAST({0} AS DATETIME) = {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value, true));
                    break;
                case CompareOperator.DateBigEqual:
                    criteria = string.Format(" CAST({0} AS DATE)  >= {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.DateTimeBigEqual:
                    criteria = string.Format(" CAST({0} AS DATETIME)  >= {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value, true));
                    break;
                case CompareOperator.DateLittleEqual:
                    criteria = string.Format(" CAST({0} AS DATE)  <= {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.DateTimeLittleEqual:
                    criteria = string.Format(" CAST({0} AS DATETIME)  <= {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value, true));
                    break;
                case CompareOperator.DateBig:
                    criteria = string.Format(" CAST({0} AS DATE)  > {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value));
                    break;
                case CompareOperator.DateTimeBig:
                    criteria = string.Format(" CAST({0} AS DATETIME)  > {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value, true));
                    break;
                case CompareOperator.DateLittle:
                    criteria = string.Format(" CAST({0} AS DATE)   > {1}", fieldName, connection.ToDbProviderString(member.PropertyType, _value, true));
                    break;
            }

            return criteria;
        }
    }
}

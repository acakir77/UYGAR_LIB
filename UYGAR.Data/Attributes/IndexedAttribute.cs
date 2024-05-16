using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IndexedAttribute : Attribute
    {
        private int groupNumber = 0;
        private PropertyInfo m_property;
        private bool _IsUnique = false;

        public bool IsUnique
        {
            get { return _IsUnique; }
            set { _IsUnique = value; }
        }
        public PropertyInfo Property
        {
            get { return m_property; }
            set { m_property = value; }
        }

        public int GroupNumber
        {
            get { return groupNumber; }
            set { groupNumber = value; }
        }
        


        /// <summary>
        /// Herhangi bir sýnýf kayýdýn alan / özelliklerinden bir ya da birkaçýnýn
        /// sistemde aynýsýnýn bulunmamasý istendiðinde kullanýlýr.
        /// 
        /// Bkz. HowTo-Using_IndexedAttribute.cs
        /// </summary>
        public IndexedAttribute()
        {
            this.GroupNumber = 0;
        }

        /// <summary>
        /// Herhangi bir sýnýf kayýdýn alan / özelliklerinden bir ya da birkaçýnýn
        /// sistemde aynýsýnýn bulunmamasý istendiðinde kullanýlýr.
        /// 
        /// Bkz. HowTo-Using_IndexedAttribute.cs
        /// </summary>
        /// <param name="groupNumber">Eðer ayný anda iki alanýn karþýlaþtýrýmlasý isteniyor ise bu 
        /// property'lere verilen grup numarasý.
        ///Bkz. HowTo-Using_IndexedAttribute.cs</param>
        public IndexedAttribute(int groupNumber)
        {
            this.groupNumber = groupNumber;
        }

        #region Static Methods
        #region Create Criteria
        public static IndexedAttribute[] GetIndexedAttributes(MemberInfo member)
        {
            object[] attributes_ = member.GetCustomAttributes(typeof(IndexedAttribute), true);
            IndexedAttribute[] attributes = new IndexedAttribute[attributes_.Length];
            attributes_.CopyTo(attributes, 0);

            return attributes;
        }
        public static Hashtable GetIndexedAttributes(Type Types)
        {
            Hashtable _HasTable = new Hashtable();
            PropertyInfo[] attributes_ = Types.GetProperties();
            foreach (PropertyInfo Member in attributes_)
            {
                object[] attributes = Member.GetCustomAttributes(typeof(IndexedAttribute), false);

                foreach (IndexedAttribute item in attributes)
                {
                    item.Property = Member;
                    LocateUniqueRecordAttributeInGroup(_HasTable, item);
                }
            }
            return _HasTable;
        }
        protected static void LocateUniqueRecordAttributeInGroup(Hashtable hashTable, IndexedAttribute attribute)
        {
            List<IndexedAttribute> list = hashTable[attribute.GroupNumber] as List<IndexedAttribute>;
            if (list == null)
                list = new List<IndexedAttribute>();

            list.Add(attribute);
            hashTable[attribute.GroupNumber] = list;
        }

        protected static Hashtable GroupUniqueRecordAttributes(List<IndexedAttribute> attributeList)
        {
            Hashtable groups = new Hashtable();
            List<IndexedAttribute> ungroupedFields = new List<IndexedAttribute>();

            foreach (IndexedAttribute attribute in attributeList)
            {
                if (attribute.GroupNumber != 0)
                    LocateUniqueRecordAttributeInGroup(groups, attribute);
                else
                    ungroupedFields.Add(attribute);
            }
            groups[0] = ungroupedFields;
            return groups;
        }



        /// <summary>
        /// Verilen sýnýfýn Unique atrribute'lerini gruplar.
        /// 
        /// Gruplama bir Hashtable içerisinde yapýlýr,
        /// Hashtable'ýn anahtarý int, deðeri List[IndexedAttribute] cinsindendir.
        /// Anahtar, grup numarasýný içerir.
        /// </summary>
        /// <param name="type">PersistentObject tipi</param>
        /// <returns></returns>
        public static Hashtable CollectAndGroupUniqueRecordAttributes(System.Type type)
        {
            List<IndexedAttribute> attributeList = new List<IndexedAttribute>();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                IndexedAttribute[] propertyAttributes = IndexedAttribute.GetIndexedAttributes(property);
                foreach (IndexedAttribute attr in propertyAttributes)
                {
                    attr.Property = property;
                    attributeList.Add(attr);
                }
            }

            Hashtable hashTable = null;
            if (attributeList.Count > 0)
                hashTable = GroupUniqueRecordAttributes(attributeList);

            return hashTable;
        }



        public static List<IndexedAttribute> GetLit(Type _Typs)
        {

            Hashtable _Table = GetIndexedAttributes(_Typs);
            List<IndexedAttribute> _Atribute = new List<IndexedAttribute>();
            foreach (int item in _Table.Keys)
            {
                foreach (IndexedAttribute indexx in _Table[item] as List<IndexedAttribute>)
                {
                    _Atribute.Add(indexx);
                }
            }

            return _Atribute;
        }

        /// <summary>
        /// Verilen PersistentObject için eþsizlik kontrolü kriterlerini türetir
        /// </summary>
        /// <param name="type"></param>
        /// <param name="_object"></param>
        /// <returns></returns>

        #endregion

        #region User Fiendly Criteria String




        #endregion
        #endregion
    }
}

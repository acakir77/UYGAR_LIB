using System;
using System.Reflection;
using UYGAR.Data.Attributes;
using UYGAR.Data.Base;

namespace UYGAR.Data.Criterias
{
    [Serializable]
    public class InDetailCriteria : Criteria
    {

        public Criteria Criteria1 { get; set; }

        public string AssociationName { get; set; }
        public string AssociationType { get; set; }
        public string PorpertyNameInAssociation { get; set; }
        public Type AsType { get; set; }
        public InDetailCriteria()
        {

        }

        public InDetailCriteria(string associationName, Criteria criteria)
        {
            this.AssociationName = associationName;
            this.Criteria1 = criteria;
        }
        public InDetailCriteria(string _AssociationType, string _PorpertyNameInAssociation, Criteria criteria)
        {
            this.AssociationType = _AssociationType;
            this.PorpertyNameInAssociation = _PorpertyNameInAssociation;
            this.Criteria1 = criteria;
        }


        //public override string PopulateSQLCriteria(System.Type type)
        //{
        //    PropertyInfo pInfo = AssociationAttribute.GetPropertyInfo(type, this.AssociationName);
        //    AssociationAttribute attr = AssociationAttribute.GetAssociationAttribute(pInfo);
        //    string sql = string.Format("{4}.OID in (select {0} from {1}.{2} where {3})", string.Format("{0}_ID", pInfo.Name), "dbo", TableAttribute.GetName(attr.ObjectType), this.Criteria1.PopulateSQLCriteria(attr.ObjectType), TableAttribute.GetName(type));
        //    return sql;
        //}
        public override string PopulateSQLCriteria(System.Type type, DbConnectionBase connection)
        {
            GetAssType();
            System.Type _assType = AsType;
            PropertyInfo pInfo = _assType.GetProperty(this.PorpertyNameInAssociation);
            //AssociationAttribute.GetPropertyInfo(type, this.AssociationName);
            //    AssociationAttribute attr = AssociationAttribute.GetAssociationAttribute(pInfo);

            string sql = string.Format("{4}.OID in (select {0} from {1}.{2} where {3})", string.Format("{0}_ID", pInfo.Name), "dbo", TableAttribute.GetName(_assType), this.Criteria1.PopulateSQLCriteria(_assType,connection), TableAttribute.GetName(type));
            return sql;
        }
        public void GetAssType()
        {

            Type[] _types = GetInheritedTypes();
            Array.ForEach(_types, cretaingtype =>
            {
                if (cretaingtype.Name.Equals(AssociationType))
                    this.AsType = cretaingtype;
            });

        }
        public Type[] GetInheritedTypes()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assemblies = currentDomain.GetAssemblies();

            Type[] types = new Type[0];

            foreach (Assembly assembly in assemblies)
                try
                {
                    Type[] assemblyTypes = assembly.GetTypes();
                    foreach (Type atype in assemblyTypes)
                    {
                        if (atype.IsSubclassOf(typeof(Model)) && !atype.IsAbstract)
                        {
                            Array.Resize(ref types, types.Length + 1);
                            types[types.Length - 1] = atype;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
            return types;
        }
    }
}

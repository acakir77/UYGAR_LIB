using System;

namespace UYGAR.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Db_ActionAttribute : Attribute
    {

        public bool IsSelect
        { get; set; }

        public bool IsInsert
        { get; set; }

        public bool IsUpdate
        { get; set; }

        public bool IsDelete
        { get; set; }

        public Db_ActionAttribute(bool Select, bool Insert, bool Update, bool Delete)
        {
            IsSelect = Select;
            IsInsert = Insert;
            IsUpdate = Update;
            IsDelete = Delete;
        }

        public static bool IsSelectRequired(Type objectType)
        {
            Db_ActionAttribute[] attributes =
                           objectType.GetCustomAttributes(typeof(Db_ActionAttribute), true) as Db_ActionAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Db_ActionAttribute] Attribute yalnızca 1 tane olabilir. {0}", objectType));
            if (attributes.Length == 0)
                return false;
            return attributes[0].IsSelect;
        }


        public static bool IsInsertRequired(Type objectType)
        {
            Db_ActionAttribute[] attributes =
                           objectType.GetCustomAttributes(typeof(Db_ActionAttribute), true) as Db_ActionAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Db_ActionAttribute] Attribute yalnızca 1 tane olabilir. {0}", objectType));
            if (attributes.Length == 0)
                return false;
            return attributes[0].IsInsert;
        }

        public static bool IsUpdateRequired(Type objectType)
        {
            Db_ActionAttribute[] attributes =
                           objectType.GetCustomAttributes(typeof(Db_ActionAttribute), true) as Db_ActionAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Db_ActionAttribute] Attribute yalnızca 1 tane olabilir. {0}", objectType));
            if (attributes.Length == 0)
                return false;
            return attributes[0].IsUpdate;
        }

        public static bool IsDeleteRequired(Type objectType)
        {
            Db_ActionAttribute[] attributes =
                           objectType.GetCustomAttributes(typeof(Db_ActionAttribute), true) as Db_ActionAttribute[];

            if (attributes.Length > 1)
                throw new InvalidOperationException(string.Format("[Db_ActionAttribute] Attribute yalnızca 1 tane olabilir. {0}", objectType));
            if (attributes.Length == 0)
                return false;
            return attributes[0].IsDelete;
        }
    }
}

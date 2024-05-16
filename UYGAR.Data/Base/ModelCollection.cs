using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;

using System.Web;

namespace UYGAR.Data.Base
{
    [Serializable]
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class ModelCollection<TBaseObject> : List<TBaseObject>, ICollection<TBaseObject>, IEnumerable<TBaseObject>, IListSource, IDisposable
    {
        public enum SortOrder { Ascending, Descending };
        #region Constructor

        public ModelCollection()
        {

        }



        #endregion

        #region Methods
        public IList<T> ToIList<T>() where T : TBaseObject
        {
            return (from object ct in this select (T)ct).ToList();
        }
   
        public TBaseObject GetItem()
        {
            TBaseObject item = default(TBaseObject);

            if (this.Count != 0)
            {
                item = this[0];
                this.RemoveAt(0);
            }
            return item;
        }
 
        #endregion




        #region IEnumerable Methods
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {

            return this.GetEnumerator();
        }
        public ModelCollection<TBaseObject> FinAllModel(Predicate<TBaseObject> match)
        {
            ModelCollection<TBaseObject> returned = new ModelCollection<TBaseObject>();
            returned.AddRange(this.FindAll(match));
            return returned;
        }
      
        #endregion



        #region Sorting







        #endregion
        #region IListSource Members

        bool IListSource.ContainsListCollection => false;

        public bool Kayitvar => !Count.Equals(0);

        IList IListSource.GetList()
        {
            return this;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        #endregion




    }
}

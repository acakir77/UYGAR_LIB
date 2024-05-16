using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UYGAR.Data.Base
{
    [Serializable]
    public class Order : List<String>
    {
        private bool _isDesc = false;

        public bool IsDesc
        {
            get { return _isDesc; }
            set { _isDesc = value; }
        }


    }
}

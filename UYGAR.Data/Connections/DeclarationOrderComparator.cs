using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UYGAR.Data.Connection
{
    public class DeclarationOrderComparator : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            PropertyInfo first = x as PropertyInfo;
            PropertyInfo second = y as PropertyInfo;
            if (first.MetadataToken < second.MetadataToken)
                return -1;
            else if (first.MetadataToken > second.MetadataToken)
                return 1;

            return 0;
        }
    }
}

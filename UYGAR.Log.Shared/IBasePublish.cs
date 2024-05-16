using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UYGAR.Log.Shared
{
    public interface IBasePublish
    {
        bool PublishData(ILOGParametre parametre);
    }
}

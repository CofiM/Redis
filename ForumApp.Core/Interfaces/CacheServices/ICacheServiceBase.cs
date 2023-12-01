using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces.CacheServices
{
    public interface ICacheServiceBase
    {
        void BuildOrRefreshCache();
    }
}

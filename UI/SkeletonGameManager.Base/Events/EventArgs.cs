using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameManager.Base
{
    public class ProviderUpdatedEventArgs : EventArgs
    {
        public int Status { get; set; }
    }
}

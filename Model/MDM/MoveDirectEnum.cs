using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    public enum MoveDirectEnum : int
    {
        Forward  = 0,
        Backoff = 1,
        Reverse=2,
        None =-1,
    }
}

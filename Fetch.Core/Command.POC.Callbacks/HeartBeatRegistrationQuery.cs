using System;
using System.Threading.Tasks;

namespace CommandPOCCallbacks
{
    public class HeartBeatRegistrationQuery
    {
        public Func<object, Task<object>> Heart { get; set; }
    }
}
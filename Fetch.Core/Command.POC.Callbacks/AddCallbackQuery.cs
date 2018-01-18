using System;
using System.Threading.Tasks;

namespace CommandPOCCallbacks
{
    public class AddCallbackQuery
    {
        public int A { get; set; }
        public int B { get; set; }
        public Func<object, Task<object>> Add { get; set; }
    }
}
using System;
using System.Threading.Tasks;

namespace Programs.Models
{
    public class CallbackQuery
    {
        public Func<object, Task<object>> CallbackFunc { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLib
{
    public class MyTraceListener:TraceListener
    {
        public event EventHandler<string> evt;
        public MyTraceListener()
        {

        }
        public override void Write(string message)
        {
           
        }
        public override void WriteLine(string message)
        {
            evt?.Invoke(null, message);
        }

    }
}

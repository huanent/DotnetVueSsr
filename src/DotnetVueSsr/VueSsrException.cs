using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetVueSsr
{
    public class VueSsrException : Exception
    {
        public VueSsrException()
        {
        }

        public VueSsrException(string message) : base(message)
        {
        }
    }

    public class NotComponentException : VueSsrException
    {
        public NotComponentException(string code):base(@$"The code is not a vue component:
{code}
")
        {

        }
    }
}

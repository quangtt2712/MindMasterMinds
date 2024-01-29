using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Utility.Exceptions
{
    public class InvalidOldPasswordException : Exception
    {
        public InvalidOldPasswordException(string message) : base(message)
        {
        }
    }
}

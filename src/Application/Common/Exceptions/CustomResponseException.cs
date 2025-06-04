using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class CustomResponseException: Exception
    {
        public CustomResponseException()
        {
            
        }
        public CustomResponseException(string message) : base(message)
        {

        }
    }
}

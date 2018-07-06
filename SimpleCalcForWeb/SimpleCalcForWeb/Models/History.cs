using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCalcForWeb.Models
{
    public class History
    {
        public string Expression { get; set; }
        public string Message { get; set; }
        public string HostName { get; set; }

        public History(string expression, string message, string hostName)
        {
            Expression = expression;
            Message = message;
            HostName = hostName;
        }
    }
}

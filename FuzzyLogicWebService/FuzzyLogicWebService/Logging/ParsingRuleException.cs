using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FuzzyLogicWebService.Logging
{
    public class ParsingRuleException :  Exception
    {
        public ParsingRuleException()
            : base() { }

        public ParsingRuleException(string message)
            : base(message) { }

        /*public ParsingRuleException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public ParsingRuleException(string message, Exception innerException)
            : base(message, innerException) { }

        public ParsingRuleException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }*/

    }
}
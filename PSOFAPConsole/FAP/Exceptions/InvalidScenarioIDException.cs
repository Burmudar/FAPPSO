using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.FAP.Exceptions
{
    public class InvalidScenarioIDException : Exception
    {
        public InvalidScenarioIDException() : base() { }

        public InvalidScenarioIDException(string message) : base(message) { }

        public InvalidScenarioIDException(string message, Exception inner) : base(message) { }
    }
}

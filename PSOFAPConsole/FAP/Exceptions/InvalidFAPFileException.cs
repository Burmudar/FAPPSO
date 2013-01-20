using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAPConsole.FAP.Exceptions
{
    public class InvalidFAPFileException : Exception
    {
        
        public InvalidFAPFileException() : base()
        {
        }

        public InvalidFAPFileException(string message) : base(message)
        {
        }

        public InvalidFAPFileException(string message, Exception inner)
        {
        }
    }
}

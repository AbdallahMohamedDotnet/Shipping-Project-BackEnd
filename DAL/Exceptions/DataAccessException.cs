using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public sealed class DataAccessException : Exception
    {
        // Fix: Add a return type (void) to the method
        public  DataAccessException(Exception ex, string custommessage, ILogger logger)
        {
            logger.LogError($"main exception {ex.Message} developer custom exception {custommessage}");
        }
    }
}

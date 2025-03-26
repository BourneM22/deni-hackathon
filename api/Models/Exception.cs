using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException() { }

        public EmailNotFoundException(string message) : base(message) { }

        public EmailNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class PasswordNotMatchException : Exception
    {
        public PasswordNotMatchException() { }

        public PasswordNotMatchException(string message) : base(message) { }

        public PasswordNotMatchException(string message, Exception inner) : base(message, inner) { }
    }
}
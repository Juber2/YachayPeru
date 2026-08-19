using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public string Code { get; }
        public string[]? Errors { get; }

        public ValidationException(string code,  string[]? details = null)
        {
            Code = code;
            Errors = details;
        }
    }
}

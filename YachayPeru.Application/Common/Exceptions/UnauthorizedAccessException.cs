using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Application.Common.Exceptions
{
    public class UnauthorizedAccessException : Exception
    {
        public string Code { get; }
        public object? Errors { get; }

        public UnauthorizedAccessException(string code, string message, object? details = null)
            : base(message)
        {
            Code = code;
            Errors = details;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Application.Common.Exceptions
{
    public sealed class BusinessException:Exception
    {
        public string Code { get; }
        public object? Errors { get; }

        public BusinessException(string code, string message, object? details = null)
            : base(message)
        {
            Code = code;
            Errors = details;
        }
    }
}

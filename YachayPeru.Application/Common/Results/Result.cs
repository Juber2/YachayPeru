using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Application.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public bool IsFailure => !IsSuccess;
        public string? Code { get; protected set; }
        public string? Message { get; protected set; }
        public object? Metadata { get; protected set; }

        protected Result() { }

        public static Result Success() => new Result
        {
            IsSuccess = true
        };

        public static Result Failure(string message, string? code = null) => new Result
        {
            IsSuccess = false,
            Code = code,
            Message = message
        };
    }

    public sealed class Result<T> : Result
    {
        public T? Value { get; private set; }

        public static Result<T> Success(T value) => new Result<T>
        {
            IsSuccess = true,
            Value = value
        };

        public static new Result<T> Failure(string message, string? code = null) => new Result<T>
        {
            IsSuccess = false,
            Code = code,
            Message = message
        };

        public static Result<T> Failure(string message, string code, object metadata) => new Result<T>
        {
            IsSuccess = false,
            Code = code,
            Message = message,
            Metadata = metadata
        };
    }
}

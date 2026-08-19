using System.Diagnostics;
using System.Text.Json.Serialization;

namespace YachayPeru.API.Contracts.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Code { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Errors { get; init; }

        public static ApiResponse<T> Ok(T data, string message = "")
            => new()
            {
                Success = true,
                Message = message,
                Data = data
            };

        public static ApiResponse<T> Fail(string message)
            => new()
            {
                Success = false,
                Message = message,
                Data = default
            };
        public static ApiResponse<T> Error(string message = "", string? code = null, string[]? errors = null)
            => new()
            {
                Success = false,
                Code = code,
                Message = message,
                Errors = errors ?? Array.Empty<string>()
            };

    }
}

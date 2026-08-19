namespace YachayPeru.Application.Common.Exceptions
{
    public sealed class UnauthorizedException : Exception
    {
        public string Code { get; }

        public UnauthorizedException(string code, string message)
            : base(message)
        {
            Code = code;
        }
    }
}

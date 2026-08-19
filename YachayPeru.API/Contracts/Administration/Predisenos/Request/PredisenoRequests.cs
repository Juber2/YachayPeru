namespace YachayPeru.API.Contracts.Administration.Predisenos.Request
{
    public class UpsertPredisenoRequest
    {
        public string Title { get; set; } = default!;
        public string TreeJson { get; set; } = default!;
    }
}

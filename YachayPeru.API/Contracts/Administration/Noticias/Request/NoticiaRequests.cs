namespace YachayPeru.API.Contracts.Administration.Noticias.Request
{
    public class UpsertNoticiaRequest
    {
        public string Title { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string Body { get; set; } = default!;
        public bool IsActive { get; set; } = true;
    }
}

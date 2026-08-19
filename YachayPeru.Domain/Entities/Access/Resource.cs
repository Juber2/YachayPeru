namespace YachayPeru.Domain.Entities.Access
{
    public class Resource
    {
        public int    Id    { get; set; }
        public string Code  { get; set; } = string.Empty;
        public string Name  { get; set; } = string.Empty;
        public string Scope { get; set; } = "platform";
    }
}

namespace YachayPeru.Domain.Entities.Common
{
    public class MasterCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? ParentCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

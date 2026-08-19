namespace YachayPeru.Application.Features.Resources.Queries.GetResourcesByScope
{
    public record PermissionItem(int Id, string Code);

    public record ResourcePermissionItem(
        string ResourceCode,
        string ResourceName,
        IReadOnlyList<PermissionItem> Permissions);
}

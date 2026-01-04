namespace Env0.Core;

public sealed class MaintenanceBatch
{
    public string Id { get; set; } = string.Empty;
    public int Count { get; set; }
    public string Source { get; set; } = string.Empty;
    public int CreatedTick { get; set; }
    public string? Category { get; set; }
    public string? Note { get; set; }
    public bool Submitted { get; set; }
}

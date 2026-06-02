public record FileDto
{
    public string FullName { get; init; } = string.Empty;
    public uint Version { get; init; }
}
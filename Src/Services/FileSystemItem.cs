public class FileSystemItem
{
    public FileSystemType Type { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Size { get; set; }
    public uint Version { get; set; } = 1;
    public override string ToString()
    {
        return FullName;
    }
    public override bool Equals(object? obj)
    {
        if (obj is FileSystemItem item)
        {
            return Type == item.Type && FullName == item.FullName;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, FullName);
    }
}


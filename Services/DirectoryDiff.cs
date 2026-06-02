public class DirectoryDiff
{
    public FileDto[]? NewFiles { get; set; }
    public FileDto[]? DeletedFiles { get; set; }
    public FileDto[]? ModifiedFiles { get; set; }
    public string[]? NewSubdirectories { get; set; }
    public string[]? DeletedSubdirectories { get; set; }

}

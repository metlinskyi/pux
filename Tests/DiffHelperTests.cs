namespace Tests;

using static Assert;

public class DiffHelperTests
{
    [Test]
    public void GetModifiedFileSystemItem()
    {
        List<FileSystemItem> prev = new List<FileSystemItem>()
        {
            new FileSystemItem
            {
                Type = FileSystemType.File,
                FullName = "file1.txt",
                Size = 100,
                Version = 2
            },
            new FileSystemItem
            {
                Type = FileSystemType.Directory,
                FullName = "subdir",
                Size = 0
            }
        };
        List<FileSystemItem> current = new List<FileSystemItem>()
        {
            new FileSystemItem
            {
                Type = FileSystemType.File,
                FullName = "file1.txt",
                Size = 150,
                Version = 1
            },
            new FileSystemItem
            {
                Type = FileSystemType.Directory,
                FullName = "subdir",
                Size = 0,
            }
        };

        var modifiedItems = DiffHelper.GetModifiedFileSystemItem(prev, current);

        That(modifiedItems, Is.Not.Null);
        That(modifiedItems.Count(), Is.EqualTo(1));
        That(modifiedItems.First().Version, Is.EqualTo(3));
    }
}

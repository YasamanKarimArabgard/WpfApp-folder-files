

using System.IO;

namespace WpfApp_folder_files.Services;

public class ExplorerServices
{
    public static List<Models.ExplorerDriveItem> GetDrives()
    {
        return DriveInfo.GetDrives().Select(c => new Models.ExplorerDriveItem()
        {
            Name = c.Name,
        }).ToList();
    }

    public static List<Models.ExplorerFolderItems> GetFolders(string path)
    {
        return Directory.GetDirectories(path).Select(c => new Models.ExplorerFolderItems()
        {
            Name = Path.GetFileName(c)
        }).ToList();
    }

    public static List<Models.ExplorerFileItem> GetFiles(string path)
    {
        return Directory.GetFiles(path).Select(c => new Models.ExplorerFileItem()
        {
            Name = Path.GetFileName(c)
        }).ToList();
    }
};

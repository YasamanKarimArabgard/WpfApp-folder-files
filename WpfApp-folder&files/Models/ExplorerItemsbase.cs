using System.IO;
using System.Windows.Xps.Packaging;

namespace WpfApp_folder_files.Models;
public abstract class ExplorerItemsbase
{
    public string Name { get; set; }

    public abstract string ThumbNail { get; }
}

public class ExplorerBackItem : ExplorerItemsbase
{
    public override string ThumbNail => "";
}

public class ExplorerFolderItems : ExplorerItemsbase
{
    public override string ThumbNail => "Images/folder.png";
}

public class ExplorerFileItem : ExplorerItemsbase
{
    public override string ThumbNail
    {
        get
        {
            var fileExtention = Path.GetExtension(Name)?.ToLower().Trim();

            if (fileExtention == null)
                return "Images/file.png";

            switch (fileExtention) {
                case ".txt":
                    return "Images/textFile.png";
                default:
                    return "Images/file.png";
            }
        }
    }
}

public class ExplorerDriveItem : ExplorerItemsbase
{
    public override string ThumbNail => "Images/drive.png";
}


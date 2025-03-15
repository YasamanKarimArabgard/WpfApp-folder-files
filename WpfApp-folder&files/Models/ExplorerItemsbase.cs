using System.IO;
using System.Windows.Xps.Packaging;

namespace WpfApp_folder_files.Models;
public abstract class ExplorerItemsbase
{
    public string Name { get; set; }

    public abstract string ThumbNail { get; }

    public virtual string CreateDate {

        get { return string.Empty; }
    
    }

    public virtual string FileSize
    {

        get { return string.Empty; }

    }
}

public class ExplorerBackItem : ExplorerItemsbase
{
    public override string ThumbNail => "";
}

public class ExplorerFolderItems : ExplorerItemsbase
{
    public DirectoryInfo Info { get; set; }
    public override string ThumbNail => "Images/folder.png";

    public override string CreateDate
    {
        get
        {
            return Info.CreationTime.ToString("G");
        }
    }

}

public class ExplorerFileItem : ExplorerItemsbase
{
    public FileInfo Info { get; set; }
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

    public override string CreateDate
    {
        get
        {
            return Info.CreationTime.ToString("G");
        }
    }

    public override string FileSize
    {
        get
        {
            if (Info.Length <= 1024)
            {
                return "1 KB";
            }
            else
            {
                return "1 MB";
            }
        }
    }
}

public class ExplorerDriveItem : ExplorerItemsbase
{
    public override string ThumbNail => "Images/drive.png";
}


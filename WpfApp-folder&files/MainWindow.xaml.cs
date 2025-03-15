using System.Collections.ObjectModel;
using System.Windows;
using WpfApp_folder_files.Models;
using System.IO;
using System.Windows.Data;

namespace WpfApp_folder_files
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ExplorerItemsbase> items = new ObservableCollection<ExplorerItemsbase>();
        public string? currentPath { get; set; } = null;
        public string? currentName = null;

        public string PathToCopy { get; set; }
        public string PathToCut { get; set; }

        public bool PathToCopyOrPasteIsFolder { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            loadCurrentPath();
            ItemsListVeiw.ItemsSource = items;

        }

        private void loadCurrentPath()
        {
            items.Clear();
            if (!string.IsNullOrEmpty(currentPath))
            {
                items.Add(new Models.ExplorerBackItem()
                {
                    Name = ".."
                });
            }

            if (string.IsNullOrEmpty(currentPath))
            {
                foreach (var drive in Services.ExplorerServices.GetDrives())
                {
                    items.Add(drive);
                };

            }
            else
            {
                var folders = Services.ExplorerServices.GetFolders(currentPath).OrderBy(x => x.Name);
                foreach (var folder in folders)
                {
                    items.Add(folder);
                }

                var files = Services.ExplorerServices.GetFiles(currentPath).OrderBy(s => s.Name);
                foreach (var file in files)
                {
                    items.Add(file);
                }
            }
        }

        private void ItemsListVeiw_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = ItemsListVeiw.SelectedItem as ExplorerItemsbase;

            if (selectedItem is ExplorerBackItem && currentPath.Equals(Path.GetPathRoot(currentPath),
                StringComparison.InvariantCultureIgnoreCase))
            {
                currentPath = null;
            }
            else if (selectedItem is ExplorerFileItem FileItem) {

                var extention = Path.GetExtension(FileItem.Name)?.Trim().ToLower();
                
                if(extention == ".txt")
                {
                    var editorWindow = new veiws.EditorWindow();
                    editorWindow.filePath = Path.Combine(currentPath, FileItem.Name);
                    editorWindow.ShowDialog();
                }
            } else {
                if (string.IsNullOrEmpty(currentPath))
                {
                    currentPath = selectedItem.Name;
                }
                else
                {
                    currentPath = Path.GetFullPath(Path.Combine(currentPath, selectedItem.Name));
                }
            }
            loadCurrentPath();
        }

        private void NewFolderBtn_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(currentPath))
            {

                MessageBox.Show("you can not create folder here.");
                return;
            }

            var getNameWindowName = new veiws.GetItemNameWindow();
            getNameWindowName.ShowDialog();


            if (getNameWindowName.DialogResult ?? false)
            {
                var directoryPath = Path.Combine(currentPath, getNameWindowName.NameTextBox.Text);

                if (Directory.Exists(directoryPath))
                {
                    MessageBox.Show($"Directory {getNameWindowName.NameTextBox.Text} is already exist!");
                    return;
                }

                Directory.CreateDirectory(directoryPath);

                loadCurrentPath();
            }
        }
        private void NewFileBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentPath))
            {

                MessageBox.Show("You can not create file here.");
                return;
            }

            var getNameWindowName = new veiws.GetItemNameWindow();
            getNameWindowName.ShowDialog();


            if (getNameWindowName.DialogResult ?? false)
            {
                var FilePath = Path.Combine(currentPath, getNameWindowName.NameTextBox.Text);

                if (File.Exists(FilePath))
                {
                    MessageBox.Show($"File {getNameWindowName.NameTextBox.Text} is already exist!");
                    return;
                }

                using var stream = File.Create(FilePath);

                loadCurrentPath();
            }
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {

            var selectedItem = ItemsListVeiw.SelectedItem as ExplorerItemsbase;

            if (selectedItem == null)
                return;

            var getNameWindow = new veiws.GetItemNameWindow()
            {
                CurrentName = selectedItem.Name
            };

            getNameWindow.ShowDialog();

            if (getNameWindow.DialogResult ?? false) {

                var oldPath = Path.Combine(currentPath, selectedItem.Name);
                var newPath = Path.Combine(currentPath, getNameWindow.NameTextBox.Text);

                if (selectedItem is ExplorerFileItem)
                {
                    File.Move(oldPath, newPath);
                }else if(selectedItem is ExplorerFolderItems)
                {
                    Directory.Move(oldPath, newPath);
                }

                loadCurrentPath();
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemsListVeiw.SelectedItem as ExplorerItemsbase;

            var result = MessageBox.Show("Are sure to delete?", "Delete file", MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                var path = Path.Combine(currentPath, selectedItem.Name);

                if (selectedItem is ExplorerFolderItems) {
                    Directory.Delete(path, true);
                }
                if(selectedItem is ExplorerFileItem) {
                     File.Delete(path);
                }
            }
            loadCurrentPath();
        }

        private void CutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemsListVeiw.SelectedItem as ExplorerItemsbase;

            if (selectedItem == null)
                return;

            var path = Path.Combine(currentPath, selectedItem.Name);

            PathToCut = path;

            if(selectedItem is ExplorerFolderItems)
            {
                PathToCopyOrPasteIsFolder = true;
            }

        }

        private void PasteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PathToCut))
            {
                var newPath = Path.Combine(currentPath, Path.GetFileName(PathToCut));

                if (PathToCopyOrPasteIsFolder)
                {
                    Directory.Move(PathToCut, newPath);
                }else
                {
                    File.Move(PathToCut, newPath)
                }
            }else if (!string.IsNullOrEmpty(PathToCopy))
            {

            }

            loadCurrentPath();
        }
    }
}
using System.Collections.ObjectModel;
using System.Windows;
using WpfApp_folder_files.Models;
using System.IO;
using System.Reflection.Metadata;

namespace WpfApp_folder_files
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ExplorerItemsbase> items = new ObservableCollection<ExplorerItemsbase>();
        public string? currentPath { get; set; } = null;
        public string? currentName = null;
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

            var getNameWindowName = new veiws.GetItemName();
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

            var getNameWindowName = new veiws.GetItemName();
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
            var getNameWindow = new veiws.GetItemName();
            {
                currentName = selectedItem.Name;
            };

            getNameWindow.ShowDialog();

            if (getNameWindow.DialogResult ?? false) {

                var oldPath = Path.Combine(currentPath, selectedItem.Name);
                var newPath = Path.Combine(currentPath, getNameWindow.NameTextBox.Text);

                if (selectedItem is Models.ExplorerFileItem)
                {
                    File.Move(newPath, oldPath);
                }else if(selectedItem is ExplorerFolderItems)
                {
                    Directory.Move(newPath, newPath);
                }

                loadCurrentPath();
            }
        }
    }
}
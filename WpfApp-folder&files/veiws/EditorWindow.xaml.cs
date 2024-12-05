
using System.Windows;
using System.IO;

namespace WpfApp_folder_files.veiws
{
    public partial class EditorWindow : Window
    {

        public string filePath { get; set; }
        public EditorWindow()
        {
            InitializeComponent();
        }

        private void EditSaveBtn_Click(object sender, RoutedEventArgs e)
        {

            File.WriteAllText(filePath, EditorTextBox.Text);
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fileContent = File.ReadAllText(filePath);

            EditorTextBox.Text = fileContent;
        }

        private void EditorCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

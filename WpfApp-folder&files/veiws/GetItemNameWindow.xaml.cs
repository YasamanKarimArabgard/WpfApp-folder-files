using System.Runtime.InteropServices;
using System.Windows;

namespace WpfApp_folder_files.veiws
{
    /// <summary>
    /// Interaction logic for GetItemName.xaml
    /// </summary>
    public partial class GetItemNameWindow : Window
    {
        public string? CurrentName { get; set; } = null;
        public GetItemNameWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(NameTextBox.Text?.Trim()))
            {
                MessageBox.Show("You Must Eenter a Name!");
            }

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentName))
            {
                NameTextBox.Text = CurrentName;
            }
        }
    }
}

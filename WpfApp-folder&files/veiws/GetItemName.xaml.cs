using System.Runtime.InteropServices;
using System.Windows;

namespace WpfApp_folder_files.veiws
{
    /// <summary>
    /// Interaction logic for GetItemName.xaml
    /// </summary>
    public partial class GetItemName : Window
    {
        public string? currentItemName = null;
        public GetItemName()
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

        private void GetItemName_Loaded(object sender, RoutedEventArgs e)
        {
            //NameTextBox.Text = currentItemName.nam000000
        }
    }
}

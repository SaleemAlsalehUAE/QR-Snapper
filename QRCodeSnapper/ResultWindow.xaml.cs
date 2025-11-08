using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace QRCodeSnapper
{
    public partial class ResultWindow : Window
    {
        public ResultWindow(string decodedUrl)
        {
            InitializeComponent();
            decodedLink.Inlines.Add(decodedUrl);
            decodedLink.NavigateUri = new System.Uri(decodedUrl);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Use Process.Start to open the URL in the default browser.
            // It's important to use this for security reasons.
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
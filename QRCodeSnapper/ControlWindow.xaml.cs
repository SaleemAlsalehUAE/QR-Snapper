using System.Windows;

namespace QRCodeSnapper
{
    public partial class ControlWindow : Window
    {
        public ControlWindow()
        {
            InitializeComponent();
        }

        private void NewSnip_Click(object sender, RoutedEventArgs e)
        {
            // Hide this control window before showing the overlay
            this.Hide();

            // Give Windows a moment to hide the window before we start the snip
            System.Threading.Thread.Sleep(200);

            // Create and show our overlay window
            var overlay = new OverlayWindow();
            overlay.ShowDialog(); // This waits until the overlay is closed

            // After the overlay is closed, show this control window again
            this.Show();
            this.Activate(); // Bring it to the front
        }


        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // This line allows the window to be dragged from its content area
            this.DragMove();
        }
    }
}
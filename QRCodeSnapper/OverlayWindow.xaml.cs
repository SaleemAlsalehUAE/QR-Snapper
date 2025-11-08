using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QRCodeSnapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();
        }

        private bool isDragging = false;
        private Point startPoint;

        private void Canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Start dragging
            isDragging = true;
            startPoint = e.GetPosition(this);

            // Position the rectangle at the start point
            System.Windows.Controls.Canvas.SetLeft(selectionRectangle, startPoint.X);
            System.Windows.Controls.Canvas.SetTop(selectionRectangle, startPoint.Y);

            // Reset the rectangle's size and make it visible
            selectionRectangle.Width = 0;
            selectionRectangle.Height = 0;
            selectionRectangle.Visibility = Visibility.Visible;
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // If we are dragging, update the rectangle's size and position
            if (isDragging)
            {
                Point currentPoint = e.GetPosition(this);

                // Calculate the rectangle's dimensions.
                // Math.Min and Math.Max handle dragging in any direction (up-left, down-right, etc.)
                double left = Math.Min(startPoint.X, currentPoint.X);
                double top = Math.Min(startPoint.Y, currentPoint.Y);
                double width = Math.Abs(startPoint.X - currentPoint.X);
                double height = Math.Abs(startPoint.Y - currentPoint.Y);

                // Update the rectangle's position and size
                System.Windows.Controls.Canvas.SetLeft(selectionRectangle, left);
                System.Windows.Controls.Canvas.SetTop(selectionRectangle, top);
                selectionRectangle.Width = width;
                selectionRectangle.Height = height;
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isDragging = false;

            // First, hide the overlay so it doesn't appear in our screenshot
            this.Visibility = Visibility.Collapsed;

            // Give the screen a moment to redraw itself without our overlay
            // This is a small hack to ensure we don't capture our own semi-transparent window
            System.Threading.Thread.Sleep(50);

            try
            {
                // Define the selected area as a WPF Rect
                var selectedRect = new Rect(
                    System.Windows.Controls.Canvas.GetLeft(selectionRectangle),
                    System.Windows.Controls.Canvas.GetTop(selectionRectangle),
                    selectionRectangle.Width,
                    selectionRectangle.Height
                );

                // If the user just clicked without dragging, the width/height will be 0.
                // In that case, we do nothing and just close.
                if (selectedRect.Width > 0 && selectedRect.Height > 0)
                {
                    // Use our new service to do the work
                    var readerService = new QRCodeReaderService();
                    string decodedText = readerService.DecodeQrCodeFromScreen(selectedRect);

                    if (!string.IsNullOrEmpty(decodedText))
                    {
                        // SUCCESS! Copy to clipboard and show a message box.
                        Clipboard.SetText(decodedText);
                        var resultWindow = new ResultWindow(decodedText);
                        resultWindow.ShowDialog();
                    }
                    else
                    {
                        // FAILURE. Show an error message.
                        MessageBox.Show("Could not find a QR code in the selected area.", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Close the application whether it succeeded or failed.
                this.Close();
            }
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }
        }


    }
}
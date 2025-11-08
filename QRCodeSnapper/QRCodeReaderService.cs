using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using ZXing;
using ZXing.Windows.Compatibility;

namespace QRCodeSnapper
{
    public class QRCodeReaderService
    {
        public string DecodeQrCodeFromScreen(Rect selectionArea)
        {
            // Convert WPF's Rect to a System.Drawing.Rectangle, accounting for screen scaling (DPI)
            var drawingRect = ConvertRectToDrawingRectangle(selectionArea);

            // Capture the specified area of the screen into a Bitmap
            using (Bitmap capturedBitmap = new Bitmap(drawingRect.Width, drawingRect.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics graphics = Graphics.FromImage(capturedBitmap))
                {
                    graphics.CopyFromScreen(
                        drawingRect.Left,
                        drawingRect.Top,
                        0,
                        0,
                        drawingRect.Size,
                        CopyPixelOperation.SourceCopy);
                }

                // DEBUGGING: Save the captured image to a file to inspect it.
                // Make sure the C:\temp folder exists!
                //capturedBitmap.Save(@"C:\temp\debug_capture.png", ImageFormat.Png);

                // Now use the decoding logic from our Phase 1 console app
                var reader = new BarcodeReader();
                var result = reader.Decode(capturedBitmap);

                return result?.Text; // Return the decoded text, or null if nothing was found
            }
        }

        private Rectangle ConvertRectToDrawingRectangle(Rect wpfRect)
        {
            // Get the screen's DPI scaling factor
            PresentationSource source = PresentationSource.FromVisual(Application.Current.MainWindow);
            double dpiX = source.CompositionTarget.TransformToDevice.M11;
            double dpiY = source.CompositionTarget.TransformToDevice.M22;

            // Convert WPF device-independent units to physical pixels
            return new Rectangle(
                (int)(wpfRect.Left * dpiX),
                (int)(wpfRect.Top * dpiY),
                (int)(wpfRect.Width * dpiX),
                (int)(wpfRect.Height * dpiY)
            );
        }
    }
}
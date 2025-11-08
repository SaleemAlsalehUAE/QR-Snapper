using System.Drawing;
using ZXing;
using ZXing.Windows.Compatibility; // This line will now work!

// IMPORTANT: Replace this path with the ACTUAL path to YOUR QR code image file.
string filePath = @"C:\temp\qrcode.png";

Console.WriteLine($"Attempting to decode QR code from: {filePath}");

try
{
    // This simple constructor now works because of the compatibility package.
    var reader = new BarcodeReader();

    // Load the image file into a bitmap
    using (var qrCodeBitmap = (Bitmap)Image.FromFile(filePath))
    {
        // Detect and decode the barcode from the bitmap
        var result = reader.Decode(qrCodeBitmap);

        // Check if a result was found
        if (result != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SUCCESS! QR Code decoded.");
            Console.WriteLine($"--> Decoded Text: {result.Text}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FAILURE: No QR code could be found in the image.");
            Console.ResetColor();
        }
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"An error occurred: {ex.Message}");
    Console.ResetColor();
}

Console.WriteLine("\nPress any key to exit.");
Console.ReadKey();
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WakEncyclopedie.Utility {
    public static class Tools {
        /// <summary>
        /// Create an BitmapImage from a byte array
        /// <para>Source : https://stackoverflow.com/a/9564425 </para>
        /// </summary>
        /// <param name="imageData">Array of byte of our image</param>
        /// <returns>BitmapImage</returns>
        public static BitmapImage LoadImage(byte[] imageData) {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData)) {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Return true if it's not a number</returns>
        public static bool OnlyAllowNumbersForText(TextCompositionEventArgs e) {
            // Allow only numbers
            Regex regex = new Regex("^[0-9]*$");
            return !regex.IsMatch(e.Text);
        }



        public static void ClearUselessZeroInText(TextBox tbx) {
            while (tbx.Text.Length > 1 && tbx.Text[0] == '0') {
                // Remove the first char
                tbx.Text = tbx.Text.Substring(1);
                // Reposition the cursor at the end of the text
                tbx.SelectionStart = tbx.Text.Length;
                tbx.SelectionLength = 0;
            }
        }

        public static void ClearUselessZeroInText(ComboBox cbx) {
            while (cbx.Text.Length > 1 && cbx.Text[0] == '0') {
                // Remove the first char
                cbx.Text = cbx.Text.Substring(1);
            }
        }

        public static string CorrectMinAndMaxValueForText(string txt) {
            if (string.IsNullOrEmpty(txt)) {
                txt = GlobalConstants.MIN_LEVEL.ToString();
            }
            if (Convert.ToInt32(txt) > GlobalConstants.MAX_LEVEL) {
                txt = GlobalConstants.MAX_LEVEL.ToString();
            }
            return txt;
        }
        
        public static void DisableCutAndPastCommands(object sender, ExecutedRoutedEventArgs e) {
            if (e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste) {
                e.Handled = true;
            }
        }
    }
}

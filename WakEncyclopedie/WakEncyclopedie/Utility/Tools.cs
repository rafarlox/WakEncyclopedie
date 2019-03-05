using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        /// <summary>
        /// Permute two element of a list
        /// </summary>
        /// <typeparam name="T">The type of our list</typeparam>
        /// <param name="list">The list were we invert the elements</param>
        /// <param name="indexA">The index of the first element to permute</param>
        /// <param name="indexB">The index of the second element to permute</param>
        /// <returns>Return the list with the element permuted</returns>
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB) {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        /// <summary>
        /// Find all visual children of a controls who have the specified type
        /// <para>Source : https://stackoverflow.com/a/978352 </para>
        /// </summary>
        /// <typeparam name="T">The type of the controls searched</typeparam>
        /// <param name="depObj">The control in which we will search</param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject {
            if (depObj != null) {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T) {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child)) {
                        yield return childOfChild;
                    }
                }
            }
        }

        /*/// <summary>
        /// Fill an array with the same value
        /// </summary>
        /// <typeparam name="T">Type of the array</typeparam>
        /// <param name="arr">The array that we want to fill</param>
        /// <param name="value">The value used to fill the array</param>
        public static T[] Populate<T>(this T[] arr, T value) {
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = value;
            }
            return arr;
        }*/ // Doesn't seeams to work, the values are the same for all element but when we edit one element it's change all elements

    }
}

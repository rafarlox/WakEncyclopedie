using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WakEncyclopedie
{
    /// <summary>
    /// Logique d'interaction pour UcStatsItems.xaml
    /// </summary>
    public partial class UcStatsItems : UserControl
    {
        public EnchantedItem ActualItem { get; set; }
        public UcStatsItems()
        {
            InitializeComponent();
        }

        private void LblItemName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0) {
                System.Diagnostics.Process.Start(ActualItem.Url);
            }
        }

        private void LblItemName_MouseEnter(object sender, MouseEventArgs e) {
            LblItemName.TextDecorations = TextDecorations.Underline;
        }

        private void LblItemName_MouseLeave(object sender, MouseEventArgs e) {
            LblItemName.TextDecorations = null;
        }
    }
}

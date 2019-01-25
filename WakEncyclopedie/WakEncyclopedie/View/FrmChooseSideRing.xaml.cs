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
using System.Windows.Shapes;

namespace WakEncyclopedie.View
{
    /// <summary>
    /// Logique d'interaction pour FrmChooseSideRing.xaml
    /// </summary>
    public partial class FrmChooseSideRing : Window
    {
        private MainWindow _mainWindow;
        /// <summary>
        /// Form that represent a messagebox that decide where equip the ring
        /// </summary>
        /// <param name="mainWindow">The form where we insert the ring</param>
        public FrmChooseSideRing(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            _mainWindow.InsertRingToLeft = null;
            Close();
        }
        private void BtnRingLeft_Click(object sender, RoutedEventArgs e) {
            _mainWindow.InsertRingToLeft = true;
            Close();
        }
        private void BtnRingRight_Click(object sender, RoutedEventArgs e) {
            _mainWindow.InsertRingToLeft = false;
            Close();
        }

    }
}

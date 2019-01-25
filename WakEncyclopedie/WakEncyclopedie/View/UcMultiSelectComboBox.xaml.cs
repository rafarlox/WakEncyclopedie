using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WakEncyclopedie.DAO;

namespace WakEncyclopedie.View
{
    /// <summary>
    /// Logique d'interaction pour UcMultiSelectComboBox.xaml
    /// </summary>
    public partial class UcMultiSelectComboBox : UserControl
    {
        public List<Element> ListElements { get; set; }

        public UcMultiSelectComboBox()
        {
            InitializeComponent(); // Could be usefull later : https://www.codeproject.com/Articles/563862/Multi-Select-ComboBox-in-WPF
            ListElements = new List<Element>();
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(UcMultiSelectComboBox));
            dpd.AddValueChanged(UcMultiSelectCombo, ElementsUpdated);
        }
        
        /// <summary>
        /// Fill the list of element when the source is updated
        /// </summary>
        private void ElementsUpdated(object sender, EventArgs e)
        {
            if (UcMultiSelectCombo.ItemsSource != null)
            {
                foreach (Element element in UcMultiSelectCombo.ItemsSource)
                {
                    ListElements.Add(element);
                }
            }
        }

        /// <summary>
        /// Cancel the selection of an new Element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxElement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ComboBox)sender).SelectedIndex = -1;
        }

        /// <summary>
        /// Set the element to selected when we click on the checkbox
        /// </summary>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            Element selectedElement = (Element)cbx.DataContext;
            ListElements.Find(x => x.Id == selectedElement.Id).IsSelected = (bool)cbx.IsChecked;
        }
    }

}

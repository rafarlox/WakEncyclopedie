using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WakEncyclopedie.DAO;

namespace WakEncyclopedie {
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// Inspired from : https://www.codeproject.com/Articles/563862/Multi-Select-ComboBox-in-WPF
    /// </summary>
    public partial class MultiSelectComboBox {
        private const string All = "Tout sélectionner";
        /// <summary>
        /// List who contains the element of the ItemsSource and with a custom element to select everything (All)
        /// </summary>
        private List<Element> ListElementsWithAll { get; set; }
        private bool IsMouseWheeling = false;

        public MultiSelectComboBox() {
            InitializeComponent();
            ListElementsWithAll = new List<Element>();
            // Add an MouseWheelEvent for the Combobox
            AddHandler(ComboBox.MouseWheelEvent, new MouseWheelEventHandler(MultiSelectCombox_MouseWheel), true);
        }

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty =
             DependencyProperty.Register("ItemsSource", typeof(IQueryable<Element>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public IQueryable<Element> ItemsSource {
            get { return (IQueryable<Element>)GetValue(ItemsSourceProperty); }
            set {
                SetValue(ItemsSourceProperty, value);
                ClosePopUp(); // Automatically close the popup when updating
            }
        }

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
        #endregion

        #region Events
        /// <summary>
        /// Update the elements in the control when the ItemsSource property is modified
        /// </summary>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.LoadElementsToControl();
        }

        /// <summary>
        /// (De)Select the element choosed when the user click on a checkbox
        /// </summary>
        private void CheckBox_Click(object sender, RoutedEventArgs e) {
            CheckBox clickedBox = (CheckBox)sender;

            if (clickedBox.Content.ToString() == All) {
                if (clickedBox.IsChecked.Value) {
                    SelectAllElement();
                } else {
                    DeselectAllElement();
                }

            } else {
                VerifyIfAllElementsAreSelected();
            }
            SetText();
        }

        /// <summary>
        /// (De)Select the element choosed when the user select an element of the ComboBox
        /// </summary>
        private void MultiSelectCombox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (MultiSelectCombox.SelectedIndex != -1 && IsMouseWheeling == false) {
                // (De)select the selected element
                Element elemSelected = (Element)MultiSelectCombox.SelectedItem;
                elemSelected.IsSelected = !elemSelected.IsSelected;

                // If it's was the "All Element" then we (de)select the others elements consequently
                if (elemSelected.Name == All) {
                    if (elemSelected.IsSelected) {
                        SelectAllElement();
                    } else {
                        DeselectAllElement();
                    }
                }
                // Update the control
                VerifyIfAllElementsAreSelected();
                SetText();
                // Set the tag to true so the pop up stay open
                MultiSelectCombox.Tag = true;
                // Set the selected index to -1 so the user can reselect the same item for inversing its state
                MultiSelectCombox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Disable the selection_change of the ComboBox when we're wheeling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiSelectCombox_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            IsMouseWheeling = true;
        }

        /// <summary>
        /// Enable the selection_change of the ComboBox after our wheel
        /// </summary>
        private void MultiSelectCombox_MouseWheel(object sender, MouseWheelEventArgs e) {
            IsMouseWheeling = false;
            MultiSelectCombox.SelectedIndex = -1; // Set the index to -1 because it's modified when we wheel
        }
        #endregion
        #region Methods
        /// <summary>
        /// Get the list of elements without the "All element"
        /// </summary>
        /// <returns></returns>
        public List<Element> GetListElements() {
            List<Element> listElements = new List<Element>();
            foreach (Element elem in ListElementsWithAll) {
                if (elem.Name != All) {
                    listElements.Add(elem);
                }
            }
            return listElements;
        }

        private void SelectAllElement() {
            foreach (Element elem in ListElementsWithAll) {
                elem.IsSelected = true;
            }
        }

        private void DeselectAllElement() {
            foreach (Element elem in ListElementsWithAll) {
                elem.IsSelected = false;
            }
        }

        /// <summary>
        /// Load each elements from the ItemsSource to the ComboBox
        /// </summary>
        private void LoadElementsToControl() {
            ListElementsWithAll.Clear();
            if (this.ItemsSource.ToList().Count > 0)
                ListElementsWithAll.Add(new Element(All)); // Add an element to select everything

            foreach (Element elem in this.ItemsSource) {
                ListElementsWithAll.Add(elem);
            }
            MultiSelectCombox.ItemsSource = ListElementsWithAll; // Load the list to the ComboBox
        }

        /// <summary>
        /// Update the text of the ComboBox according to the selected elements
        /// </summary>
        private void SetText() {
            // Format the displayed text
            StringBuilder displayText = new StringBuilder();
            foreach (Element element in ListElementsWithAll) {
                if (element.IsSelected == true && element.Name == All) {
                    displayText = new StringBuilder();
                    displayText.Append(All);
                    break;
                } else if (element.IsSelected == true && element.Name != All) {
                    displayText.Append(element.Name);
                    displayText.Append(',');
                }
            }
            // Remove the potential last comma
            this.Text = displayText.ToString().TrimEnd(new char[] { ',' });
            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(this.Text)) {
                this.Text = this.DefaultText;
            }
        }

        /// <summary>
        /// Automatically select the "All element" according to whether the other elements are selected
        /// </summary>
        private void VerifyIfAllElementsAreSelected() {
            int _selectedCount = 0;
            foreach (Element elem in ListElementsWithAll) {
                if (elem.IsSelected && elem.Name != All)
                    _selectedCount++;
            }
            if (_selectedCount == ListElementsWithAll.Count - 1)
                ListElementsWithAll.FirstOrDefault(i => i.Name == All).IsSelected = true;
            else
                ListElementsWithAll.FirstOrDefault(i => i.Name == All).IsSelected = false;
        }

        /// <summary>
        /// Force the closure of the pop up by editing the tag of the combobox
        /// </summary>
        public void ClosePopUp() {
            MultiSelectCombox.Tag = false;
        }
        #endregion

    }
}

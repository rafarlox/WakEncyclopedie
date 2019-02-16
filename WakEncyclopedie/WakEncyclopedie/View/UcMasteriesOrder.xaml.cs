using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WakEncyclopedie.Utility;
using static WakEncyclopedie.GlobalConstants;

namespace WakEncyclopedie.View {
    /// <summary>
    /// Interaction logic for UcMasteriesOrder.xaml
    /// </summary>
    public partial class UcMasteriesOrder : UserControl {
        public List<Elementary> MasteriesOrder { get; set; }

        public UcMasteriesOrder() {
            InitializeComponent();
            MasteriesOrder = new List<Elementary>() {
                Elementary.Fire,
                Elementary.Water,
                Elementary.Earth,
                Elementary.Air,
            };
        }

        private void ButtonMoveUp_Click(object sender, RoutedEventArgs e) {
            Button btn = (Button)sender;
            int btnIndex = GetButtonIndex(btn);
            SwapMasteries(btnIndex);
        }

        private void ButtonMoveDown_Click(object sender, RoutedEventArgs e) {
            Button btn = (Button)sender;
            int btnIndex = GetButtonIndex(btn);
            SwapMasteries(btnIndex, false);

        }

        private int GetButtonIndex(Button btn) {
            StackPanel sp = (StackPanel)btn.Parent;
            return LbxMasteriesOrder.Items.IndexOf(sp.Parent);
        }

        /// <summary>
        /// Swap the masteries in the ListBox and in the public List
        /// </summary>
        /// <param name="masteryIndex">Index in the ListBox of the selected mastery</param>
        /// <param name="swapUp">If true we try to swap the selected mastery with the previous mastery, else we swap the next mastery</param>
        private void SwapMasteries(int masteryIndex, bool swapUp = true) {
            // Get the index of the second mastery
            int otherMasteryIndex = (swapUp) ? masteryIndex - 1 : masteryIndex + 1;

            if (otherMasteryIndex >= 0 && otherMasteryIndex < LbxMasteriesOrder.Items.Count) {
                // Swap masteries in the Listbox
                var itemToMove = LbxMasteriesOrder.Items[masteryIndex];
                LbxMasteriesOrder.Items.RemoveAt(masteryIndex);
                LbxMasteriesOrder.Items.Insert(otherMasteryIndex, itemToMove);
                // Swap the list of masteries
                MasteriesOrder.Swap(masteryIndex, otherMasteryIndex);
            }
        }
    }
}

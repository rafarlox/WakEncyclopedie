using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WakEncyclopedie.BO;
using WakEncyclopedie.DAO;
using WakEncyclopedie.Utility;
using static WakEncyclopedie.GlobalConstants;

namespace WakEncyclopedie.View {
    /// <summary>
    /// Interaction logic for UcRunesManager.xaml
    /// </summary>
    public partial class UcRunesManager : UserControl {
        private const string TOGGLE_BUTTON_STYLE = "ToggleButtonStyle";
        private const string ATTACK_RUNES = "AttackRunes";
        private const string DEFENSE_RUNES = "DefenseRunes";
        private const string SUPPORT_RUNES = "SupportRunes";
        private const string ACTIVATED_RUNES = "runes d'activés";

        private Build _actualBuild;
        public Build ActualBuild {
            get => _actualBuild;
            set {
                _actualBuild = value;
                InitializeView();
            }
        }
        private RunesBuild BRunes { get => ActualBuild.BRunes; }

        public UcRunesManager() {
            InitializeComponent();
        }

        public void InitializeView() {
            CreateRunesSlot(ATTACK_RUNES, BRunes.AttackRunes, Rune.AttackRunesDictionary, GrdAttack);
            CreateRunesSlot(DEFENSE_RUNES, BRunes.DefenseRunes, Rune.DefenseRunesDictionary, GrdDefense);
            CreateRunesSlot(SUPPORT_RUNES, BRunes.SupportRunes, Rune.SupportRunesDictionary, GrdSupport);
            UpdateView();
        }

        public void UpdateView() {
            LblTotalRunesAttack.Content = string.Format("{0}/{1} {2}", ActualBuild.BRunes.GetCountEnabledRunes(ActualBuild.BRunes.AttackRunes),  ActualBuild.BRunes.AttackRunes.Count(), ACTIVATED_RUNES);
            LblTotalRunesDefense.Content = string.Format("{0}/{1} {2}", ActualBuild.BRunes.GetCountEnabledRunes(ActualBuild.BRunes.DefenseRunes), ActualBuild.BRunes.DefenseRunes.Count(), ACTIVATED_RUNES);
            LblTotalRunesSupport.Content = string.Format("{0}/{1} {2}", ActualBuild.BRunes.GetCountEnabledRunes(ActualBuild.BRunes.SupportRunes), ActualBuild.BRunes.SupportRunes.Count(), ACTIVATED_RUNES);
        }

        private void CreateRunesSlot(string runesType, Rune[] runesArray, object runesSource, Grid runesGrid) {
            int leftMargin = 10;
            int topMargin = 10;

            for (int i = 0; i < runesArray.Count(); i++) {
                ToggleButton toggle = new ToggleButton() {
                    Style = (Style)FindResource(TOGGLE_BUTTON_STYLE),
                    Margin = new Thickness(leftMargin, topMargin, 0, 0),
                    DataContext = runesSource,
                    Tag = i,
                    Name = runesType,
                };

                Grid grid = new Grid();
                grid.Children.Add(toggle);
                runesGrid.Children.Add(grid);

                leftMargin += 35;
            }
        }

        private void CbxRunes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cbx = (ComboBox)sender; // Can be the combobox of level or rune
            Grid gridOfPopup = (Grid)cbx.Parent;
            Popup popupParent = (Popup)gridOfPopup.Parent;
            Grid gridOfToggleBtn = (Grid)popupParent.Parent;
            ToggleButton toggleBtn = (ToggleButton)gridOfToggleBtn.TemplatedParent;
            ComboBox cbxLevel = (ComboBox)gridOfPopup.FindName("CbxRuneLevel");
            ComboBox cbxRune = (ComboBox)gridOfPopup.FindName("CbxRunes");

            if (cbxLevel != null && cbxRune != null) {
                int runeLevel = Convert.ToInt32(((ComboBoxItem)cbxLevel.SelectedItem).Content);
                int runeIndex = (int)toggleBtn.Tag;
                string runeSlot = toggleBtn.Name;
                KeyValuePair<KeyValuePair<RuneType, string>, string> runeSelected = (KeyValuePair<KeyValuePair<RuneType, string>, string>)cbxRune.SelectedItem;
                RuneType runeType = runeSelected.Key.Key;

                switch (runeSlot) {
                    case ATTACK_RUNES:
                        ActualBuild.BRunes.AttackRunes[runeIndex].EquipRune(runeType);
                        ActualBuild.BRunes.AttackRunes[runeIndex].Level = runeLevel;
                        break;
                    case DEFENSE_RUNES:
                        ActualBuild.BRunes.DefenseRunes[runeIndex].EquipRune(runeType);
                        ActualBuild.BRunes.DefenseRunes[runeIndex].Level = runeLevel;
                        break;
                    case SUPPORT_RUNES:
                        ActualBuild.BRunes.SupportRunes[runeIndex].EquipRune(runeType);
                        ActualBuild.BRunes.SupportRunes[runeIndex].Level = runeLevel;
                        break;
                    default:
                        Console.WriteLine("Unknown rune stot");
                        break;
                }
                ActualBuild.BStats.CalculateBuildStats();
                UpdateView();
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e) {
            ToggleButton toggleSender = (ToggleButton)sender;
            foreach (ToggleButton toggle in Tools.FindVisualChildren<ToggleButton>(GrdBackground)) {
                if (toggle != toggleSender) {
                    toggle.IsChecked = false;
                }
            }
        }
    }
}

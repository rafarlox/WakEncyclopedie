using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WakEncyclopedie.DAO;
using WakEncyclopedie.Utility;

namespace WakEncyclopedie.View {
    /// <summary>
    /// Logique d'interaction pour UcBuildStats.xaml
    /// </summary>
    public partial class UcBuildStats : UserControl
    {
        public Build UcBuild { get; set; }
        private BuildStats BStats { get => UcBuild.BStats; }
        private UcSkillsManagement UcSkillsManager { get => UcBuild.BindedUcSkillsManagement; }
        private UcRunesManager UcRunesManager { get => UcBuild.BindedUcRunesManager; }

        public UcBuildStats()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            // Fill the combobox for nation bonus
            CbbxNationBonus.Items.Clear();
            CbbxNationBonus.Items.Add(0);
            foreach (int i in BStats.NationsLevelsForBonus) {
                CbbxNationBonus.Items.Add(i);
            }
            // Fill the combobox of levels
            CbxLevel.Items.Clear();
            foreach (int lvl in GlobalConstants.MODULE_LEVELS) {
                CbxLevel.Items.Add(lvl);
            }
            UpdateView();
        }

        public void UpdateView() {
            // Build Level
            CbxLevel.Text = UcBuild.LevelBuild.ToString();
            // Guild bonus status
            CbxGuildBonus.IsChecked = BStats.GuildBonusActived;
            // Nation bonus level
            CbbxNationBonus.SelectedValue = BStats.NationBonusLevel;
            // Major stats
            LblHp.Content = BStats.CalculateTotalHp();
            LblAp.Content = BStats.ActionPoint;
            LblMp.Content = BStats.MovementPoint;
            LblWp.Content = BStats.WakfuPoint;
            LblArmor.Content = BStats.CalculateArmor();
            LblBarrier.Content = BStats.Barrier;
            // Masteries and resistances
            LblFireMastery.Content = BStats.FireMastery;
            LblFireResistance.Content = String.Format("{0}% ({1})", BStats.GetReductionOfResistance(0), BStats.FireResistance);
            LblWaterMastery.Content = BStats.WaterMastery;
            LblWaterResistance.Content = String.Format("{0}% ({1})", BStats.GetReductionOfResistance(1), BStats.WaterResistance);
            LblEarthMastery.Content = BStats.EarthMastery;
            LblEarthResistance.Content = String.Format("{0}% ({1})", BStats.GetReductionOfResistance(2), BStats.EarthResistance);
            LblAirMastery.Content = BStats.AirMastery;
            LblAirResistance.Content = String.Format("{0}% ({1})", BStats.GetReductionOfResistance(3), BStats.AirResistance);
            // Primary stats
            LblDammageInflincted.Content = BStats.DammageInfligedInPercent + "%";
            LblHealthPerformed.Content = BStats.HealthPerformedInPercent + "%";
            LblCriticalChance.Content = BStats.CriticalHits + "%";
            LblBlock.Content = BStats.Block + "%";
            LblInitiative.Content = BStats.Initiative;
            LblRange.Content = BStats.Range;
            LblDodge.Content = BStats.Dodge;
            LblLock.Content = BStats.Lock;
            LblWisdom.Content = BStats.Wisdom;
            LblProspecting.Content = BStats.Prospecting;
            LblControl.Content = BStats.Control;
            LblKitSkill.Content = BStats.KitSkill;
            LblWill.Content = BStats.Will;
            // Secondary stats
            LblCritMastery.Content = BStats.CriticalMastery;
            LblCritResistance.Content = BStats.CriticalResistance;
            LblRearMastery.Content = BStats.RearMastery;
            LblRearResistance.Content = BStats.RearResistance;
            LblMeleeMastery.Content = BStats.MeleeMastery;
            LblDistanceMastery.Content = BStats.DistanceMastery;
            LblSingleTargetMastery.Content = BStats.SingleTargetMastery;
            LblAreaMastery.Content = BStats.AreaMastery;
            LblHealthMastery.Content = BStats.HealingMastery;
            LblBerserkMastery.Content = BStats.BerserkMastery;
            // Skills
            UcSkillsManager.UpdateView();
            // Runes
            UcRunesManager.UpdateView();
        }


        /// <summary>
        /// Only allow numbers for the level of the build
        /// </summary>
        private void CbxLevel_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            e.Handled = Tools.OnlyAllowNumbersForText(e);
        }

        /// <summary>
        /// Correct the min and max value of the textbox of levels when the textchange
        /// </summary>
        private void CbxLevel_TextChanged(object sender, TextChangedEventArgs e) {
            CbxLevel.Text = Tools.CorrectMinAndMaxValueForText(CbxLevel.Text);
        }

        /// <summary>
        /// Clear the useless zero when the combobox lost the focus
        /// </summary>
        private void CbxLevel_LostFocus(object sender, System.Windows.RoutedEventArgs e) {
            Tools.ClearUselessZeroInText(CbxLevel);
            if (Convert.ToInt32(CbxLevel.Text) < GlobalConstants.MIN_LEVEL) {
                CbxLevel.Text = GlobalConstants.MIN_LEVEL.ToString();
            }
            UcBuild.LevelBuild = Convert.ToInt32(CbxLevel.Text);
            UpdateView();
        }

        private void CbxLevel_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) {
            Tools.DisableCutAndPastCommands(sender, e);
        }

        private void CbxLevel_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                CbxLevel.Text = CbxLevel.SelectedValue.ToString();
            } catch (Exception) {
                // The level is set manually
            }
            UcBuild.LevelBuild = Convert.ToInt32(CbxLevel.Text);
            UpdateView();
        }

        private void CbxGuildBonus_CheckChanged(object sender, System.Windows.RoutedEventArgs e) {
            if (CbxGuildBonus.IsChecked == true) {
                BStats.GuildBonusActived = true;
            } else {
                BStats.GuildBonusActived = false;
            }
            UpdateView();
        }

        private void CbbxNationBonus_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (CbbxNationBonus.SelectedIndex != -1) {
                BStats.NationBonusLevel = Convert.ToInt32(e.AddedItems[0]);
                UpdateView();
            }
        }

        private void CbxLevel_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) {
            e.Handled = true;
        }

        private void CbbxNationBonus_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) {
            e.Handled = true;
        }
    }

    public class IsLessThanConverter : IValueConverter {
        public static readonly IValueConverter Instance = new IsLessThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            // remove % symbol
            string v = value.ToString();
            if (v.IndexOf('%') != -1) {
                v = v.Remove(v.IndexOf('%'));
            }
            // convert values
            int intValue = System.Convert.ToInt32(v);
            int compareToValue = System.Convert.ToInt32(parameter);

            return intValue < compareToValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}

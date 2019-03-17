using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WakEncyclopedie.DAO;
using WakEncyclopedie.Utility;

namespace WakEncyclopedie.View {
    /// <summary>
    /// Interaction logic for UcCustomBuildStats.xaml
    /// </summary>
    public partial class UcCustomBuildStats : UserControl {
        public Build UcBuild { get; set; }
        private BuildStats BStats { get => UcBuild.BStats; }

        public UcCustomBuildStats() {
            InitializeComponent();
        }

        /// <summary>
        /// Filter the textbox before any text input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tbx_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            TextBox tbx = (TextBox)sender;
            // If the user insert '-' when the textbox is empty, then we do nothing
            if (e.Text == "-" && tbx.Text.Length == 0) {
                return;
            }
            // Only Allow numbers and the minus sign (-) when it's the first char on the textbox
            if ((e.Text != "-" || tbx.CaretIndex != 0 || tbx.Text[0] == '-') && Tools.OnlyAllowNumbersForText(e)) {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Clear the useless zero in the textbox
        /// </summary>
        private void Tbx_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox tbx = (TextBox)sender;
            Tools.ClearUselessZeroInText(tbx);
        }

        /// <summary>
        /// Correct the textbox if necessary and update the stats
        /// </summary>
        private void Tbx_LostFocus(object sender, RoutedEventArgs e) {
            TextBox tbx = (TextBox)sender;
            if (String.IsNullOrEmpty(tbx.Text) || tbx.Text == "-") {
                tbx.Text = "0";
            }
            BStats.LoadCustomStatsList(CreateCustomStatsList());
        }

        private List<Stat> CreateCustomStatsList() {
            List<Stat> customStatsList = new List<Stat>() {
                new Stat(GlobalConstants.HEALTH_POINTS_ID, Convert.ToInt32(TbxHp.Text)),
                new Stat(GlobalConstants.ACTION_POINT_ID, Convert.ToInt32(TbxAp.Text)),
                new Stat(GlobalConstants.MOVEMENT_POINT_ID, Convert.ToInt32(TbxMp.Text)),
                new Stat(GlobalConstants.WAKFU_POINT_ID, Convert.ToInt32(TbxWp.Text)),
                new Stat(GlobalConstants.FIRE_MASTERY_ID, Convert.ToInt32(TbxFireMastery.Text)),
                new Stat(GlobalConstants.WATER_MASTERY_ID, Convert.ToInt32(TbxWaterMastery.Text)),
                new Stat(GlobalConstants.EARTH_MASTERY_ID, Convert.ToInt32(TbxEarthMastery.Text)),
                new Stat(GlobalConstants.AIR_MASTERY_ID, Convert.ToInt32(TbxAirMastery.Text)),
                new Stat(GlobalConstants.MELEE_MASTERY_ID, Convert.ToInt32(TbxMeleeMastery.Text)),
                new Stat(GlobalConstants.DISTANCE_MASTERY_ID, Convert.ToInt32(TbxDistanceMastery.Text)),
                new Stat(GlobalConstants.SINGLE_TARGET_MASTERY_ID, Convert.ToInt32(TbxSingleTargetMastery.Text)),
                new Stat(GlobalConstants.AREA_MASTERY_ID, Convert.ToInt32(TbxAreaMastery.Text)),
                new Stat(GlobalConstants.CRITICAL_MASTERY_ID, Convert.ToInt32(TbxCritMastery.Text)),
                new Stat(GlobalConstants.REAR_MASTERY_ID, Convert.ToInt32(TbxRearMastery.Text)),
                new Stat(GlobalConstants.HEALING_MASTERY_ID, Convert.ToInt32(TbxHealthMastery.Text)),
                new Stat(GlobalConstants.BERSERK_MASTERY_ID, Convert.ToInt32(TbxBerserkMastery.Text)),
                new Stat(GlobalConstants.FIRE_RESISTANCE_ID, Convert.ToInt32(TbxFireResistance.Text)),
                new Stat(GlobalConstants.WATER_RESISTANCE_ID, Convert.ToInt32(TbxWaterResistance.Text)),
                new Stat(GlobalConstants.EARTH_RESISTANCE_ID, Convert.ToInt32(TbxEarthResistance.Text)),
                new Stat(GlobalConstants.AIR_RESISTANCE_ID, Convert.ToInt32(TbxAirResistance.Text)),
                new Stat(GlobalConstants.CRITICAL_RESISTANCE_ID, Convert.ToInt32(TbxCritResistance.Text)),
                new Stat(GlobalConstants.REAR_RESISTANCE_ID, Convert.ToInt32(TbxRearResistance.Text)),
                new Stat(GlobalConstants.CRITICAL_HITS_ID, Convert.ToInt32(TbxCriticalChance.Text)),
                new Stat(GlobalConstants.BLOCK_ID, Convert.ToInt32(TbxBlock.Text)),
                new Stat(GlobalConstants.INITIATIVE_ID, Convert.ToInt32(TbxInitiative.Text)),
                new Stat(GlobalConstants.RANGE_ID, Convert.ToInt32(TbxRange.Text)),
                new Stat(GlobalConstants.DODGE_ID, Convert.ToInt32(TbxDodge.Text)),
                new Stat(GlobalConstants.LOCK_ID, Convert.ToInt32(TbxLock.Text)),
                new Stat(GlobalConstants.WISDOM_ID, Convert.ToInt32(TbxWisdom.Text)),
                new Stat(GlobalConstants.PROSPECTING_ID, Convert.ToInt32(TbxProspecting.Text)),
                new Stat(GlobalConstants.CONTROL_ID, Convert.ToInt32(TbxControl.Text)),
                new Stat(GlobalConstants.KIT_SKILL_ID, Convert.ToInt32(TbxKitSkill.Text)),
                new Stat(GlobalConstants.WILL_ID, Convert.ToInt32(TbxWill.Text)),
                new Stat(GlobalConstants.DAMMAGE_INFLIGED_ID, Convert.ToInt32(TbxDammageInflincted.Text)),
                new Stat(GlobalConstants.HEALTH_PERFORMED_ID, Convert.ToInt32(TbxHealthPerformed.Text)),
            };
            return customStatsList;
        }

        /// <summary>
        /// Disable the cut and past command for avoiding error
        /// </summary>
        private void Tbx_PreviewExecuted(object sender, ExecutedRoutedEventArgs e) {
            Tools.DisableCutAndPastCommands(sender, e);
        }

        /// <summary>
        /// Disable the space
        /// </summary>
        private void Tbx_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Space) {
                e.Handled = true;
            }
        }
    }
}

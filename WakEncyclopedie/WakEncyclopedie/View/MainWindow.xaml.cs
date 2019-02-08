using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WakEncyclopedie.DAO;
using WakEncyclopedie.Utility;
using WakEncyclopedie.View;

namespace WakEncyclopedie {
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private EncyclopediaDB EncycloDB { get; set; }
        private Build ActualBuild { get; set; }
        public bool? InsertRingToLeft = null;
        private bool IsScrollAlreadyAtTheEnd = false;
        private double PreviousScrollVerticalOffset { get; set; }
        private bool ChangeVerticalOffset = false;
        private int ActualSortedColumnIndex { get; set; }
        private ListSortDirection ActualSortDirection { get; set; }

        public MainWindow() {
            InitializeComponent();
            EncycloDB = new EncyclopediaDB();
            ActualBuild = new Build(GlobalConstants.MAX_LEVEL, UcActualEquipements, UcActualSkillsManager);
        }

        #region Datagrid events
        /// <summary>
        /// Load the differents data to the form
        /// </summary>
        private void FrmMain_Loaded(object sender, RoutedEventArgs e) {
            try {
                // Load the actual build to the UserControls
                UcActualBuildStats.UcBuild = ActualBuild;
                UcActualCustomBuildStats.UcBuild = ActualBuild;
                UcActualSkillsManager.ActualSkills = ActualBuild.BSkill;

                // Load data in datagrid and combobox
                ItemsDataGrid.ItemsSource = EncycloDB.GetAllItemsWithImg();

                MscbxRarity.UcMultiSelectCombo.ItemsSource = EncycloDB.GetAllRarities();
                MscbxType.UcMultiSelectCombo.ItemsSource = EncycloDB.GetAllTypes();
                MscbxStats.UcMultiSelectCombo.ItemsSource = EncycloDB.GetAllStats();

                // Create events
                CreateBuildImagesEvents();
                // Create an event that will trigger when the data of the datagrid are updated
                CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(ItemsDataGrid.Items);
                ((INotifyCollectionChanged)myCollectionView).CollectionChanged += ItemsDataGrid_CollectionChanged;
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Verify the number of elements in the datagridview after an update
        /// <para>If we don't have enough items in the datagrid, then we disable the "Next button"</para>
        /// </summary>
        private void ItemsDataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (ItemsDataGrid.Items.Count < EncyclopediaDB.ITEMS_TO_LOAD) {
                BtnShowNextItems.IsEnabled = false;
            } else {
                BtnShowNextItems.IsEnabled = true;
            }
        }

        /// <summary>
        /// Show next items of the datagrid
        /// </summary>
        private void BtnShowNextItems_Click(object sender, RoutedEventArgs e) {
            ItemsDataGrid.ItemsSource = EncycloDB.GoToNextItems();
        }

        /// <summary>
        /// Show previous items of the datagrid
        /// </summary>
        private void BtnShowPreviousItems_Click(object sender, RoutedEventArgs e) {
            ItemsDataGrid.ItemsSource = EncycloDB.GoToPreviousItems();
        }

        /// <summary>
        /// Show the detailed stats of the item selected
        /// </summary>
        private void ItemsDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e) {
            if (ItemsDataGrid.SelectedItem != null) {
                EnchantedItem item = new EnchantedItem((Item)ItemsDataGrid.SelectedItem);
                // Show the stat of the item selected in the user control
                LoadItemsStatsToContent(item);
            }
        }

        /// <summary>
        /// Create a modal form for the selected item when we do a right click
        /// </summary>
        private void ItemsDataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e) {
            if (ItemsDataGrid.SelectedItem != null) {
                // Create a modal form who contains the stats of the item selected
                EnchantedItem item = new EnchantedItem((Item)ItemsDataGrid.SelectedItem);
                CreateModalOfItemStats(item);
            }
        }

        private void ItemsDataGrid_MouseDown(object sender, MouseButtonEventArgs e) {
            ItemsDataGrid_SelectedCellsChanged(sender, null);
        }
        #endregion Datagrid events

        #region Search events
        /// <summary>
        /// Cancel the selection of an new Item
        /// </summary>
        private void CbxItemsSearch_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ((ComboBox)sender).SelectedIndex = -1;
        }

        /// <summary>
        /// Filter the input of search by lvl
        /// </summary>
        private void TbxItemLvlSearch_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = Tools.OnlyAllowNumbersForText(e);
        }

        private void TbxItemLvlSearch_PreviewExecuted(object sender, ExecutedRoutedEventArgs e) {
            Tools.DisableCutAndPastCommands(sender, e);
        }

        /// <summary>
        /// Clear the useless zero in the textbox of levels
        /// </summary>
        private void TbxItemLvlSearch_TextChanged(object sender, TextChangedEventArgs e) {
            TextBox tbx = (TextBox)sender;
            Tools.ClearUselessZeroInText(tbx);
        }

        /// <summary>
        /// Correct the min and max value of the textbox of levels when it lost the focus
        /// </summary>
        private void TbxItemLvlSearch_LostFocus(object sender, RoutedEventArgs e) {
            TextBox tbx = (TextBox)sender;
            tbx.Text = Tools.CorrectMinAndMaxValueForText(tbx.Text);
        }

        /// <summary>
        /// Search the items by differents filter
        /// </summary>
        private void BtnSearchItems_Click(object sender, RoutedEventArgs e) {
            // Force the correction of the levels before searching the items
            TbxItemLvlSearch_LostFocus(TbxItemLvlMinSearch, e);
            TbxItemLvlSearch_LostFocus(TbxItemLvlMaxSearch, e);

            int minLvl = Convert.ToInt32(TbxItemLvlMinSearch.Text);
            int maxLvl = Convert.ToInt32(TbxItemLvlMaxSearch.Text);
            string nameItem = TbxNameSearch.Text.Trim();

            int[] idRarities = GetIntArrayOfSelectedElements(MscbxRarity.ListElements);
            int[] idTypes = GetIntArrayOfSelectedElements(MscbxType.ListElements);
            int[] idStats = GetIntArrayOfSelectedElements(MscbxStats.ListElements);

            ItemsDataGrid.ItemsSource = EncycloDB.SearchItems(minLvl, maxLvl, nameItem, idRarities, idTypes, idStats);
            ActualSortedColumnIndex = 0; // Reset the sort
            if (ItemsDataGrid.Items.Count > 0) {
                ItemsDataGrid.ScrollIntoView(ItemsDataGrid.Items[0]);
            }
        }
        #endregion Search events

        #region Methods
        private void CreateBuildImagesEvents() {
            UcActualEquipements.ImgAccessory.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgAmulet.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgBelt.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgBoots.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgBreastplate.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgCloak.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgEpaulettes.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgHandLeft.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgHandRight.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgHelmet.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgMount.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgPet.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgRingLeft.MouseDown += Image_MouseDown;
            UcActualEquipements.ImgRingRight.MouseDown += Image_MouseDown;
        }

        /// <summary>
        /// Event that is triggered when we click on an image of the equipement
        /// </summary>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e) {
            Image img = (Image)sender;
            EnchantedItem item = new EnchantedItem();
            if (img.Tag != null) {
                item = ActualBuild.GetEquipedItemById((int)img.Tag);

                if (e.LeftButton == MouseButtonState.Pressed) {
                    // Left click, we load the item stats to the user control of stats
                    LoadItemsStatsToContent(item, true);
                } else if (e.RightButton == MouseButtonState.Pressed) {
                    // Right click, we load the item stats to a new modal of stats
                    CreateModalOfItemStats(item);
                }
            }
        }

        /// <summary>
        /// // Show the stat of the item selected in the user control
        /// </summary>
        /// <param name="item">The item we want to show</param>
        private void LoadItemsStatsToContent(EnchantedItem item, bool itemCanBeRemoved = false) {
            UcStatsItems uc = new UcStatsItems();
            LoadItemToUserControlStats(item, uc, itemCanBeRemoved);
            CCtrlItemStats.Content = uc;
        }

        /// <summary>
        /// // Create a modal form who contains the stats of the item selected
        /// </summary>
        /// <param name="item">The item we want to show</param>
        /// <param name="itemCanBeEquiped">If true, then the button for equiped the item will be showed. Else it will be the button for removing the item that will be showed</param>
        private void CreateModalOfItemStats(EnchantedItem item) {
            ModalItemStats modal = new ModalItemStats();
            UcStatsItems uc = new UcStatsItems();
            LoadItemToUserControlStats(item, uc);
            modal.CCtrlItemStats.Content = uc;
            modal.Show();
            // We have to bring the modal form in front after displaying it
            modal.Topmost = true;
        }

        /// <summary>
        /// Load the item selected with formated stats to the user control for stats
        /// </summary>
        private void LoadItemToUserControlStats(EnchantedItem item, UcStatsItems uc, bool itemCanBeRemoved = false) {
            uc.ActualItem = item;
            uc.LblItemName.Text = item.Name;
            uc.LblItemLvl.Content = "Niv." + item.Level;
            uc.LblItemRarity.Content = item.RarityName;
            uc.LblItemType.Content = item.Type;
            uc.ImgItemPicture.Source = Tools.LoadImage(item.Image);
            uc.ImgItemRarity.Source = Tools.LoadImage(item.RarityImage);
            uc.BtnEquip.Click += EquipItemToBuild;
            if (itemCanBeRemoved) {
                uc.BtnEquip.Content = "Modifier";
                uc.BtnRemove.Visibility = Visibility.Visible;
                uc.BtnRemove.Click += RemoveItemToBuild;
            }

            // Customize the items of the listbox for each stats in the item loaded
            foreach (Stat stat in item.StatList) {
                if (stat.Type == "Coup Critique" || stat.Type == "Parade") {
                    stat.Type = "% " + stat.Type.Trim();
                } else {
                    // When we add the space to the stat we have to trim it first for avoiding multiple space
                    stat.Type = " " + stat.Type.Trim();
                }

                // Create stackpanel for the stats
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                TextBlock tb = new TextBlock();
                tb.Text = stat.Value + stat.Type;
                sp.Children.Add(tb);
                uc.LbxItemStats.Items.Add(sp);

                // If the stat correspond to multiplte elements of masteries or resistances,
                // then we must create an new StackPanel with checkbox for each elements
                if (GlobalConstants.IDS_ELEM_ARRAY.Contains(stat.Id)) {
                    sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    // Save the actual item to the tag for recovering his properties later
                    sp.Tag = item;

                    // Define the name of the StackPanel with a string for masteries or resistances
                    // This information will be needed later
                    bool? elemForMasteries = GlobalConstants.IdForMasteriesOrResistances(stat.Id);
                    if (elemForMasteries == true) {
                        sp.Name = GlobalConstants.MASTERIES_STRING;
                    } else if (elemForMasteries == false) {
                        sp.Name = GlobalConstants.RESISTANCES_STRING;
                    } else {
                        Console.WriteLine("Unknown id of masteries or resistances for the tag of the stackpanel");
                    }
                    // Add the checkbox to the stackpanel
                    sp.Children.Add(CreateCheckboxElement(GlobalConstants.FIRE_IMAGE_PATH, sp));
                    sp.Children.Add(CreateCheckboxElement(GlobalConstants.WATER_IMAGE_PATH, sp));
                    sp.Children.Add(CreateCheckboxElement(GlobalConstants.EARTH_IMAGE_PATH, sp));
                    sp.Children.Add(CreateCheckboxElement(GlobalConstants.AIR_IMAGE_PATH, sp));
                    uc.LbxItemStats.Items.Add(sp);
                }
            }
        }

        /// <summary>
        /// Create a CheckBox corresponding to an element for a specifical StackPanel
        /// </summary>
        /// <param name="imageSourcePath">Path that correspond to an element</param>
        /// <param name="sp">The StackPanel parent of the CheckBox</param>
        /// <returns></returns>
        private CheckBox CreateCheckboxElement(string imageSourcePath, StackPanel sp) {
            ImageSourceConverter converter = new ImageSourceConverter();
            CheckBox cbx = new CheckBox();
            // Recover the item stocked in the StackPanel tag
            EnchantedItem item = (EnchantedItem)sp.Tag;

            // Create the image for the checkbox
            Image img = new Image();
            img.Source = (ImageSource)converter.ConvertFromString(imageSourcePath);
            cbx.Content = img;

            // Create the event click for the checkbox and save the differents informations that needed
            // Saving the stackpanel to the tag of the checkbox allows to save the two objects in one properties
            cbx.Tag = sp;
            cbx.Name = GlobalConstants.GetElemStringFromImagePath(imageSourcePath);
            cbx.Click += CbxItemElement_Click;

            // Check automatically the checkbox if the masteries or resistances are already selected
            switch (imageSourcePath) {
                case GlobalConstants.FIRE_IMAGE_PATH:
                    if (sp.Name == GlobalConstants.MASTERIES_STRING && item.FireMastery > 0) {
                        cbx.IsChecked = true;
                    }
                    if (sp.Name == GlobalConstants.RESISTANCES_STRING && item.FireResistance > 0) {
                        cbx.IsChecked = true;
                    }
                    break;
                case GlobalConstants.WATER_IMAGE_PATH:
                    if (sp.Name == GlobalConstants.MASTERIES_STRING && item.WaterMastery > 0) {
                        cbx.IsChecked = true;
                    }
                    if (sp.Name == GlobalConstants.RESISTANCES_STRING && item.WaterResistance > 0) {
                        cbx.IsChecked = true;
                    }
                    break;
                case GlobalConstants.EARTH_IMAGE_PATH:
                    if (sp.Name == GlobalConstants.MASTERIES_STRING && item.EarthMastery > 0) {
                        cbx.IsChecked = true;
                    }
                    if (sp.Name == GlobalConstants.RESISTANCES_STRING && item.EarthResistance > 0) {
                        cbx.IsChecked = true;
                    }
                    break;
                case GlobalConstants.AIR_IMAGE_PATH:
                    if (sp.Name == GlobalConstants.MASTERIES_STRING && item.AirMastery > 0) {
                        cbx.IsChecked = true;
                    }
                    if (sp.Name == GlobalConstants.RESISTANCES_STRING && item.AirResistance > 0) {
                        cbx.IsChecked = true;
                    }
                    break;
                default:
                    break;
            }
            return cbx;
        }

        /// <summary>
        /// Event triggered when a checkbox of elements is clicked
        /// </summary>
        private void CbxItemElement_Click(object sender, RoutedEventArgs e) {
            // The checkbox tag contains his parent stackpanel
            // That stackpanel contains the others checkbox of elements and the item corresponding
            CheckBox cbx = (CheckBox)sender;
            StackPanel sp = (StackPanel)cbx.Tag;
            EnchantedItem item = (EnchantedItem)sp.Tag;

            // Count and verify the elements selected 
            bool masteries = true;
            bool fire = false;
            bool water = false;
            bool earth = false;
            bool air = false;
            int count = 0;
            foreach (CheckBox c in sp.Children) {
                if (c.IsChecked == true) {
                    count++;
                    switch (c.Name) {
                        case GlobalConstants.FIRE_STRING:
                            fire = true;
                            break;
                        case GlobalConstants.WATER_STRING:
                            water = true;
                            break;
                        case GlobalConstants.EARTH_STRING:
                            earth = true;
                            break;
                        case GlobalConstants.AIR_STRING:
                            air = true;
                            break;
                        default:
                            Console.WriteLine("Unknown name for checkbox of element");
                            break;
                    }
                }
            }
            // Uncheck the checkbox if too many elements have been selected
            if (sp.Name == GlobalConstants.MASTERIES_STRING) {
                masteries = true;
                if (count > item.MasteriesElementsRequired) {
                    cbx.IsChecked = false;
                    return;
                }
            } else if (sp.Name == GlobalConstants.RESISTANCES_STRING) {
                masteries = false;
                if (count > item.ResistancesElementsRequired) {
                    cbx.IsChecked = false;
                    return;
                }
            } else {
                Console.WriteLine("Unknown name for the stackpanel");
                return; // We don't want to transmute the items if the name is unknown
            }
            // Finally transmute the elements of the item, even if the total of elements selected is insufficient
            item.TransmutateItemElements(masteries, fire, water, earth, air);
        }

        /// <summary>
        /// Try to equip the selected item to the build
        /// </summary>
        private void EquipItemToBuild(object sender, RoutedEventArgs e) {
            // To access to the selected item we have to get the UcStatsItems that triggered this event
            Button btn = (Button)sender;
            Grid grid = (Grid)btn.Parent;
            UcStatsItems ucStatsParent = (UcStatsItems)grid.Parent;

            if (ucStatsParent.ActualItem.ConditionsRespected) {
                ucStatsParent.LblInfo.Content = String.Empty;

                // If it's not a ring, then we can try to equip it directly
                if (ucStatsParent.ActualItem.IdType != Build.ID_RING) {
                    if (!ActualBuild.EquipItem(ucStatsParent.ActualItem)) {
                        // TODO: show error message when the item can't be equiped
                    }
                } else {
                    // else we have to determine if the user want to equip the ring to the left or right hand
                    FrmChooseSideRing frmChooseSideRing = new FrmChooseSideRing(this);
                    frmChooseSideRing.ShowDialog();
                    if (InsertRingToLeft != null) {
                        if (InsertRingToLeft == true) {
                            ActualBuild.EquipRingLeft(ucStatsParent.ActualItem);
                        } else {
                            ActualBuild.EquipRingRight(ucStatsParent.ActualItem);
                        }
                    }
                }
                UcActualBuildStats.UpdateView();
            } else {
                ucStatsParent.LblInfo.Content = "Pas assez d'éléments sélectionnées.";
            }
        }

        private void RemoveItemToBuild(object sender, RoutedEventArgs e) {
            // To access to the selected item we have to get the UcStatsItems that triggered this event
            Button btn = (Button)sender;
            Grid grid = (Grid)btn.Parent;
            UcStatsItems ucStatsParent = (UcStatsItems)grid.Parent;

            ActualBuild.RemoveItem(ucStatsParent.ActualItem);
            UcActualBuildStats.UpdateView();
        }


        private void BtnRemoveAllItems_Click(object sender, RoutedEventArgs e) {
            // TODO: Add messagebox of confirmation
            ActualBuild.RemoveAllItem();
            UcActualBuildStats.UpdateView();
        }

        private int[] GetIntArrayOfSelectedElements(List<Element> elementList) {
            List<int> elementedSelected = new List<int>();
            foreach (Element elem in elementList) {
                if (elem.IsSelected) {
                    elementedSelected.Add(elem.Id);
                }
            }
            return elementedSelected.ToArray();
        }
        #endregion Methods

        private void UpdateDatabase() {
            EncyclopediaUpdater updater = new EncyclopediaUpdater(this);
            updater.StartUpdateDB();
        }

        public void ShowUpdateStatus(DateTime start, DateTime end, string url) {
            TimeSpan duration = end - start;
            string timeTotal = String.Format("{0}:{1}:{2}", duration.Hours, duration.Minutes, duration.Seconds);
            MessageBox.Show(String.Format("L'update a commencé à {0} et s'est terminé à {1} pour l'url {2}\nDurée total : {3}", start.ToString(), end.ToString(), url, timeTotal));
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e) {
            FrmAbout frm = new FrmAbout();
            frm.ShowDialog();
        }

        /// <summary>
        /// Remove the Overflow Toogle Button of the Toolbar
        /// Source : https://stackoverflow.com/a/1051264
        /// </summary>
        private void ToolBar_Loaded(object sender, RoutedEventArgs e) {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null) {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null) {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void MenuReset_Click(object sender, RoutedEventArgs e) {
            ActualBuild.ResetBuild();
            UcActualBuildStats.UpdateView();
        }

        /// <summary>
        /// Lazy loading of items
        /// </summary>
        private void ItemsDataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e) {
            ScrollViewer scrollViewer = (ScrollViewer)e.OriginalSource;
            if (ChangeVerticalOffset) {
                scrollViewer.ScrollToVerticalOffset(PreviousScrollVerticalOffset);
                ChangeVerticalOffset = false;
            } else {
                PreviousScrollVerticalOffset = e.VerticalOffset;
                // Trigger when the scrollbar is at the end
                if (e.VerticalOffset == scrollViewer.ScrollableHeight && e.VerticalOffset != 0 && !IsScrollAlreadyAtTheEnd) {
                    ItemsDataGrid.ItemsSource = EncycloDB.LoadNewItems();
                    if (ActualSortedColumnIndex != 0) {
                        ItemsDataGrid.Columns[ActualSortedColumnIndex].SortDirection = ActualSortDirection;
                    }
                    IsScrollAlreadyAtTheEnd = true;
                    ChangeVerticalOffset = true;
                } else {
                    IsScrollAlreadyAtTheEnd = false;
                }
            }

        }

        /// <summary>
        /// Manage the sort of the items
        /// </summary>
        private void ItemsDataGrid_Sorting(object sender, DataGridSortingEventArgs e) {
            if (e.Column.SortDirection == null || e.Column.SortDirection == ListSortDirection.Descending) {
                // Sort by asc
                ItemsDataGrid.ItemsSource = EncycloDB.SortPreviousSearch(e.Column.SortMemberPath, ListSortDirection.Ascending);
                ItemsDataGrid.Columns[e.Column.DisplayIndex].SortDirection = ListSortDirection.Descending; // Inversed sort because the event will automatically inverse it again
                ActualSortDirection = ListSortDirection.Ascending;
            } else {
                // Sort by desc
                ItemsDataGrid.ItemsSource = EncycloDB.SortPreviousSearch(e.Column.SortMemberPath, ListSortDirection.Descending);
                ItemsDataGrid.Columns[e.Column.DisplayIndex].SortDirection = ListSortDirection.Ascending; // Inversed sort because the event will automatically inverse it again
                ActualSortDirection = ListSortDirection.Descending;
            }
            ActualSortedColumnIndex = e.Column.DisplayIndex;

            // Scroll to top after the sorting
            // source: https://social.msdn.microsoft.com/Forums/en-US/fdccbf75-bd1c-41c6-b209-71d6537603df/datagrid-event-after-columns-sorting
            Dispatcher.BeginInvoke((Action)delegate () {
                if (ItemsDataGrid.Items.Count > 0) {
                    ItemsDataGrid.ScrollIntoView(ItemsDataGrid.Items[0]);
                }
            }, null);
        }
    }
}

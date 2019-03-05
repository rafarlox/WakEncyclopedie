using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WakEncyclopedie.BO;

namespace WakEncyclopedie.View {
    /// <summary>
    /// Interaction logic for UcSkillsManagement.xaml
    /// </summary>
    public partial class UcSkillsManagement : UserControl {
        private Skill _actualSkills;
        private const int TOP_START_TEXT_BLOCK = 30;
        private const int TOP_INCREMENT_TEXT_BLOCK = 30;
        private const int LEFT_START_LABEL = 360;
        private const int LEFT_ADD_BUTTON = LEFT_START_LABEL + 20;
        private const int LEFT_REMOVE_BUTTON = LEFT_START_LABEL - 30;
        private const string INTELLIGENCE_STRING = "intelligence";
        private const string STRENGTH_STRING = "strength";
        private const string AGILITY_STRING = "agility";
        private const string LUCK_STRING = "luck";
        private const string MAJOR_STRING = "major";
        private const int DEFAULT_POINTS = 1;
        private const int MULTIPLE_POINTS = 10;
        private const int MAX_POINTS = 50;

        private bool AddingPoints = true;

        public Skill ActualSkills {
            get {
                return _actualSkills;
            } 
            set {
                _actualSkills = value;
                UpdateView();
            }
        }

        public UcSkillsManagement() {
            InitializeComponent();
            GrdIntelligence.Tag = INTELLIGENCE_STRING;
            GrdStrength.Tag = STRENGTH_STRING;
            GrdAgility.Tag = AGILITY_STRING;
            GrdLuck.Tag = LUCK_STRING;
            GrdMajor.Tag = MAJOR_STRING;
        }

        public void UpdateView() {
            LblIntelligenceRemainingPoints.Content = ActualSkills.IntelligencePointsRemaining;
            LblStrengthRemainingPoints.Content = ActualSkills.StrengthPointsRemaining;
            LblAgilityRemainingPoints.Content = ActualSkills.AgilityPointsRemaining;
            LblLuckRemainingPoints.Content = ActualSkills.LuckPointsRemaining;
            LblMajorRemainingPoints.Content = ActualSkills.MajorPointsRemaining;

            CreateSkillsStats();
        }

        private void CreateSkillsStats() {
            Dictionary<List<SkillsStat>, Grid> dictListWithGrid = new Dictionary<List<SkillsStat>, Grid>() {
                { ActualSkills.Intelligence, GrdIntelligence },
                { ActualSkills.Strength, GrdStrength },
                { ActualSkills.Agility, GrdAgility },
                { ActualSkills.Luck, GrdLuck },
                { ActualSkills.Major, GrdMajor },
            };

            foreach (KeyValuePair<List<SkillsStat>, Grid> listSS in dictListWithGrid) {
                listSS.Value.Children.Clear();
                int top = TOP_START_TEXT_BLOCK;
                foreach (SkillsStat skill in listSS.Key) {
                    TextBlock tb = CreateTextBlock(skill, top);
                    Label lb = CreateLabel(skill, top);
                    Button btnAdd = CreateAddButton(skill, top, listSS.Value.Tag.ToString());
                    Button btnRemove = CreateRemoveButton(skill, top, listSS.Value.Tag.ToString());
                    listSS.Value.Children.Add(tb);
                    listSS.Value.Children.Add(lb);
                    listSS.Value.Children.Add(btnAdd);
                    listSS.Value.Children.Add(btnRemove);
                    top += TOP_INCREMENT_TEXT_BLOCK;
                }
            }
        }

        private TextBlock CreateTextBlock(SkillsStat skill, int top) {
            TextBlock tb = new TextBlock() {
                Text = skill.Type,
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Color.FromRgb(130, 142, 142)),
                Width = 404,
                Height = 25,
                Margin = new Thickness(10, top, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(5, 5, 0, 0),
            };
            return tb;
        }

        private Label CreateLabel(SkillsStat skill, int top) {
            Label lb = new Label() {
                Content = skill.AssignedPoints,
                Margin = new Thickness(LEFT_START_LABEL, top, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Foreground = new SolidColorBrush(Colors.White),
            };
            if (skill.AssignedPoints == skill.MaxAssignedPoints) {
                lb.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            return lb;
        }

        private Button CreateButton(int left, int top, string content, string gridTag, SkillsStat skill) {
            Button btn = new Button() {
                Content = content,
                Margin = new Thickness(left, top, 0, 0),
                Height = 25,
                Width = 25,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Name = gridTag,
                Tag = skill,
            };
            return btn;
        }
        
        private Button CreateAddButton(SkillsStat skill, int top, string gridTag) {
            Button btn = CreateButton(LEFT_ADD_BUTTON, top, "+", gridTag, skill);
            btn.Click += BtnAddPoints_Click;
            return btn;
        }

        private Button CreateRemoveButton(SkillsStat skill, int top, string gridTag) {
            Button btn = CreateButton(LEFT_REMOVE_BUTTON, top, "-", gridTag, skill);
            btn.Click += BtnRemovePoints_Click;
            return btn;
        }

        private void BtnAddPoints_Click(object sender, RoutedEventArgs e) {
            AddingPoints = true;
            AssignPoints(sender, DeterminePointsToAssign());
        }

        private void BtnRemovePoints_Click(object sender, RoutedEventArgs e) {
            AddingPoints = false;
            AssignPoints(sender, DeterminePointsToAssign());
        }

        private int DeterminePointsToAssign() {
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0 && (Keyboard.Modifiers & ModifierKeys.Shift) > 0) {
                return MAX_POINTS;
            } else if ((Keyboard.Modifiers & ModifierKeys.Shift) > 0) {
                return MULTIPLE_POINTS;
            } else {
                return DEFAULT_POINTS;
            }
        }


        private void AssignPoints(object sender, int pointsToAssign) {
            Button btn = (Button)sender;
            SkillsStat skill = (SkillsStat)btn.Tag;

            if (!AddingPoints) {
                pointsToAssign = -pointsToAssign;
            }

            switch (btn.Name) {
                case INTELLIGENCE_STRING:
                    ActualSkills.AssignPointsInIntelligence(skill.Id, pointsToAssign);
                    break;
                case STRENGTH_STRING:
                    ActualSkills.AssignPointsInStrength(skill.Id, pointsToAssign);
                    break;
                case AGILITY_STRING:
                    ActualSkills.AssignPointsInAgility(skill.Id, pointsToAssign);
                    break;
                case LUCK_STRING:
                    ActualSkills.AssignPointsInLuck(skill.Id, pointsToAssign);
                    break;
                case MAJOR_STRING:
                    ActualSkills.AssignPointsInMajor(skill.Id, pointsToAssign);
                    break;
                default:
                    Console.WriteLine("Unknown button name");
                    break;
            }
            UpdateView();
        }

        private void BtnResetAllStats_Click(object sender, RoutedEventArgs e) {
            ActualSkills.ResetSkills();
            UpdateView();
        }
    }
}

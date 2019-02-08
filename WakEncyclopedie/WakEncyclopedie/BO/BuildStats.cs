using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WakEncyclopedie.BO;
using WakEncyclopedie.DAO;

namespace WakEncyclopedie {
    public class BuildStats {
        private Build Build { get; set; }

        public List<Stat> CustomStatsList { get; private set; }

        #region base_stats
        private const int BASE_HP = 50;
        private const int BASE_AP = 6;
        private const int BASE_MP = 3;
        private const int BASE_WP = 6;
        private const int BASE_CC = 3;
        private const int BASE_CONTROL = 1;
        #endregion base_stats
        #region stats_per_levels
        private const int HP_PER_LEVEL = 10;
        private const int LEVEL_FOR_MASTERIES_BONUS_1 = 40;
        private const int LEVEL_FOR_MASTERIES_BONUS_2 = 60;
        private const int LEVEL_FOR_MASTERIES_BONUS_3 = 80;
        private const int LEVEL_FOR_MASTERIES_BONUS_4 = 100;
        private const int LEVEL_FOR_MASTERIES_BONUS_5 = 120;
        private const int BONUS_VALUE_BY_LEVEL = 20;
        #endregion stats_per_levels
        #region aptitudes
        private int _healthPoints;
        private int _armor;
        private int _actionPoint;
        private int _movementPoint;
        private int _wakfuPoint;
        private int _fireMastery;
        private int _waterMastery;
        private int _earthMastery;
        private int _airMastery;
        private int _meleeMastery;
        private int _distanceMastery;
        private int _singleTargetMastery;
        private int _areaMastery;
        private int _criticalMastery;
        private int _rearMastery;
        private int _healingMastery;
        private int _berserkMastery;
        private int _fireResistance;
        private int _waterResistance;
        private int _earthResistance;
        private int _airResistance;
        private int _criticalResistance;
        private int _rearResistance;
        private int _dammageInfligedInPercent;
        private int _healthPerformedInPercent;
        private int _criticalHits;
        private int _block;
        private int _initiative;
        private int _range;
        private int _dodge;
        private int _lock;
        private int _wisdom;
        private int _prospecting;
        private int _control;
        private int _kitSkill;
        private int _will;

        public int HealthPoints { get => _healthPoints; set => _healthPoints = value; }
        public int Armor { get => _armor; set => _armor = value; }
        public int ActionPoint { get => _actionPoint; set => _actionPoint = value; }
        public int MovementPoint { get => _movementPoint; set => _movementPoint = value; }
        public int WakfuPoint { get => _wakfuPoint; set => _wakfuPoint = value; }
        public int FireMastery { get => _fireMastery; set => _fireMastery = value; }
        public int WaterMastery { get => _waterMastery; set => _waterMastery = value; }
        public int EarthMastery { get => _earthMastery; set => _earthMastery = value; }
        public int AirMastery { get => _airMastery; set => _airMastery = value; }
        public int MeleeMastery { get => _meleeMastery; set => _meleeMastery = value; }
        public int DistanceMastery { get => _distanceMastery; set => _distanceMastery = value; }
        public int SingleTargetMastery { get => _singleTargetMastery; set => _singleTargetMastery = value; }
        public int AreaMastery { get => _areaMastery; set => _areaMastery = value; }
        public int CriticalMastery { get => _criticalMastery; set => _criticalMastery = value; }
        public int RearMastery { get => _rearMastery; set => _rearMastery = value; }
        public int HealingMastery { get => _healingMastery; set => _healingMastery = value; }
        public int BerserkMastery { get => _berserkMastery; set => _berserkMastery = value; }
        public int FireResistance { get => _fireResistance; set => _fireResistance = value; }
        public int WaterResistance { get => _waterResistance; set => _waterResistance = value; }
        public int EarthResistance { get => _earthResistance; set => _earthResistance = value; }
        public int AirResistance { get => _airResistance; set => _airResistance = value; }
        public int CriticalResistance { get => _criticalResistance; set => _criticalResistance = value; }
        public int RearResistance { get => _rearResistance; set => _rearResistance = value; }
        public int DammageInfligedInPercent { get => _dammageInfligedInPercent; set => _dammageInfligedInPercent = value; }
        public int HealthPerformedInPercent { get => _healthPerformedInPercent; set => _healthPerformedInPercent = value; }
        public int CriticalHits { get => _criticalHits; set => _criticalHits = value; }
        public int Block { get => _block; set => _block = value; }
        public int Initiative { get => _initiative; set => _initiative = value; }
        public int Range { get => _range; set => _range = value; }
        public int Dodge { get => _dodge; set => _dodge = value; }
        public int Lock { get => _lock; set => _lock = value; }
        public int Wisdom { get => _wisdom; set => _wisdom = value; }
        public int Prospecting { get => _prospecting; set => _prospecting = value; }
        public int Control { get => _control; set => _control = value; }
        public int KitSkill { get => _kitSkill; set => _kitSkill = value; }
        public int Will { get => _will; set => _will = value; }
        public int HealthInPercent { get; set; }
        public int Barrier { get; set; }
        public int HealthReceivedInPercent { get; set; }
        public int HealthPointsInArmorInPercent { get; set; }
        #endregion aptitudes
        #region guild_bonus
        private bool _guildBonusActived = false;
        public bool GuildBonusActived {
            get => _guildBonusActived;
            set {
                if (_guildBonusActived != value) {
                    _guildBonusActived = value;
                    CalculateBuildStats();
                }
            }
        }
        public List<Stat> GuildBonusStats { get; private set; }
        #endregion
        #region nation_bonus
        private int _nationBonusLevel = 0;
        public int NationBonusLevel {
            get => _nationBonusLevel;
            set {
                if (_nationBonusLevel != value) {
                    _nationBonusLevel = value;
                    CalculateBuildStats();
                }
            }
        }

        private const int NATION_LEVEL_FOR_BONUS_1 = 20;
        private const int NATION_LEVEL_FOR_BONUS_2 = 30;
        private const int NATION_LEVEL_FOR_BONUS_3 = 40;

        private const int MP_VALUE_FOR_MAJOR_SKILL = 1;
        private const int RANGE_VALUE_FOR_MAJOR_SKILL = 1;
        private const int CONTROL_VALUE_FOR_MAJOR_SKILL = 2;

        public readonly List<int> NationsLevelsForBonus = new List<int>() { NATION_LEVEL_FOR_BONUS_1, NATION_LEVEL_FOR_BONUS_2, NATION_LEVEL_FOR_BONUS_3 };

        public List<Stat> NationBonusStats { get; private set; }
        #endregion

        public BuildStats() {

        }

        public BuildStats(Build build) {
            GuildBonusStats = CreateGuildBonusStats();
            NationBonusStats = new List<Stat>();
            CustomStatsList = new List<Stat>();
            Build = build;
            CalculateBuildStats();
        }

        private List<Stat> CreateGuildBonusStats() {
            return new List<Stat>() {
                new Stat(GlobalConstants.HEALTH_POINTS_ID, GlobalConstants.HEALTH_STRING, 65),
                new Stat(GlobalConstants.INITIATIVE_ID, "Initiative", 10),
                new Stat(GlobalConstants.DODGE_ID, "Esquive", 20),
                new Stat(GlobalConstants.LOCK_ID, "Tacle", 20),
                new Stat(GlobalConstants.RESISTANCE_ALL_ID, "Résistance", 20),
                new Stat(GlobalConstants.WISDOM_ID, GlobalConstants.WISDOM_STRING, 10),
                new Stat(GlobalConstants.PROSPECTING_ID, GlobalConstants.PROSPECTING_STRING, 10),
                new Stat(GlobalConstants.KIT_SKILL_ID, "Art du Barda", 3),
                new Stat(GlobalConstants.DAMMAGE_INFLIGED_ID, GlobalConstants.DAMMAGE_INFLIGED_STRING, 8),
                new Stat(GlobalConstants.HEALTH_PERFORMED_ID, GlobalConstants.HEALTH_PERFORMED_STRING, 8),
            };
        }

        public void LoadCustomStatsList(List<Stat> customStats) {
            CustomStatsList = customStats;
            CalculateBuildStats();
        }

        private void CalculateStatsPerLevel() {
            HealthPoints = BASE_HP + HP_PER_LEVEL * Build.LevelBuild;
            if (Build.LevelBuild >= LEVEL_FOR_MASTERIES_BONUS_1)
                AddMasteriesAll(BONUS_VALUE_BY_LEVEL);
            if (Build.LevelBuild >= LEVEL_FOR_MASTERIES_BONUS_2)
                AddMasteriesAll(BONUS_VALUE_BY_LEVEL);
            if (Build.LevelBuild >= LEVEL_FOR_MASTERIES_BONUS_3)
                AddMasteriesAll(BONUS_VALUE_BY_LEVEL);
            if (Build.LevelBuild >= LEVEL_FOR_MASTERIES_BONUS_4)
                AddMasteriesAll(BONUS_VALUE_BY_LEVEL);
            if (Build.LevelBuild >= LEVEL_FOR_MASTERIES_BONUS_5)
                AddMasteriesAll(BONUS_VALUE_BY_LEVEL);
        }

        private void CalculteSkillsStats() {
            foreach (List<SkillsStat> listSS in Build.BSkill.GetAllListOfSkillsStat()) {
                foreach (SkillsStat stat in listSS) {
                    AddStatToBuild(stat.Id, stat.GetTotalValue());
                }
            }
        }

        private void CalculateGuildBonusStats() {
            if (GuildBonusActived) {
                foreach (Stat stat in GuildBonusStats) {
                    AddStatToBuild(stat.Id, stat.Value);
                }
            }
        }

        private void CalculateNationBonusStats() {
            Stat health = new Stat(GlobalConstants.HEALTH_POINTS_ID, GlobalConstants.HEALTH_STRING, NationBonusLevel);
            Stat dammage_infliged = new Stat(GlobalConstants.DAMMAGE_INFLIGED_ID, GlobalConstants.DAMMAGE_INFLIGED_STRING, 0);
            Stat wisdom = new Stat(GlobalConstants.WISDOM_ID, GlobalConstants.WISDOM_STRING, 0);
            Stat prospecting = new Stat(GlobalConstants.PROSPECTING_ID, GlobalConstants.PROSPECTING_STRING, 0);

            if (NationBonusLevel >= NATION_LEVEL_FOR_BONUS_1) {
                dammage_infliged.Value = 5;
            }
            if (NationBonusLevel >= NATION_LEVEL_FOR_BONUS_2) {
                wisdom.Value = 10;
            }
            if (NationBonusLevel >= NATION_LEVEL_FOR_BONUS_3) {
                prospecting.Value = 10;
            }
            NationBonusStats = new List<Stat>() {
                health,
                dammage_infliged,
                wisdom,
                prospecting
            };
            foreach (Stat stat in NationBonusStats) {
                AddStatToBuild(stat.Id, stat.Value);
            }
        }

        public void CalculateBuildStats() {
            ResetStatsOfBuild();
            CalculateStatsPerLevel();
            CalculteSkillsStats();
            CalculateGuildBonusStats();
            CalculateNationBonusStats();
            List<EnchantedItem> items = Build.GetBuildItems();
            foreach (EnchantedItem item in items) {
                foreach (Stat stat in item.StatList) {
                    AddStatToBuild(stat.Id, stat.Value);
                }
                AddItemMasteriesAndElement(item);
            }
            foreach (Stat stat in CustomStatsList) {
                AddStatToBuild(stat.Id, stat.Value);
            }
        }

        private void AddStatToBuild(int idStat, int valueStat) {
            if (valueStat != 0) {
                switch (idStat) {
                    case GlobalConstants.HEALTH_POINTS_ID:
                        HealthPoints += valueStat;
                        break;
                    case GlobalConstants.ACTION_POINT_ID:
                        ActionPoint += valueStat;
                        break;
                    case GlobalConstants.MOVEMENT_POINT_ID:
                        MovementPoint += valueStat;
                        break;
                    case GlobalConstants.WAKFU_POINT_ID:
                        WakfuPoint += valueStat;
                        break;
                    case GlobalConstants.MASTERY_ALL_ID:
                        AddMasteriesAll(valueStat);
                        break;
                    case GlobalConstants.FIRE_MASTERY_ID:
                        FireMastery += valueStat;
                        break;
                    case GlobalConstants.WATER_MASTERY_ID:
                        WaterMastery += valueStat;
                        break;
                    case GlobalConstants.EARTH_MASTERY_ID:
                        EarthMastery += valueStat;
                        break;
                    case GlobalConstants.AIR_MASTERY_ID:
                        AirMastery += valueStat;
                        break;
                    case GlobalConstants.MELEE_MASTERY_ID:
                        MeleeMastery += valueStat;
                        break;
                    case GlobalConstants.DISTANCE_MASTERY_ID:
                        DistanceMastery += valueStat;
                        break;
                    case GlobalConstants.SINGLE_TARGET_MASTERY_ID:
                        SingleTargetMastery += valueStat;
                        break;
                    case GlobalConstants.AREA_MASTERY_ID:
                        AreaMastery += valueStat;
                        break;
                    case GlobalConstants.CRITICAL_MASTERY_ID:
                        CriticalMastery += valueStat;
                        break;
                    case GlobalConstants.REAR_MASTERY_ID:
                        RearMastery += valueStat;
                        break;
                    case GlobalConstants.HEALING_MASTERY_ID:
                        HealingMastery += valueStat;
                        break;
                    case GlobalConstants.BERSERK_MASTERY_ID:
                        BerserkMastery += valueStat;
                        break;
                    case GlobalConstants.RESISTANCE_ALL_ID:
                    case GlobalConstants.RES_ALL_FOR_MAJOR_ID:
                        AddResistancesAll(valueStat);
                        break;
                    case GlobalConstants.FIRE_RESISTANCE_ID:
                        FireResistance += valueStat;
                        break;
                    case GlobalConstants.WATER_RESISTANCE_ID:
                        WaterResistance += valueStat;
                        break;
                    case GlobalConstants.EARTH_RESISTANCE_ID:
                        EarthResistance += valueStat;
                        break;
                    case GlobalConstants.AIR_RESISTANCE_ID:
                        AirResistance += valueStat;
                        break;
                    case GlobalConstants.CRITICAL_RESISTANCE_ID:
                        CriticalResistance += valueStat;
                        break;
                    case GlobalConstants.REAR_RESISTANCE_ID:
                        RearResistance += valueStat;
                        break;
                    case GlobalConstants.CRITICAL_HITS_ID:
                        CriticalHits += valueStat;
                        break;
                    case GlobalConstants.BLOCK_ID:
                        Block += valueStat;
                        break;
                    case GlobalConstants.INITIATIVE_ID:
                        Initiative += valueStat;
                        break;
                    case GlobalConstants.RANGE_ID:
                        Range += valueStat;
                        break;
                    case GlobalConstants.DODGE_ID:
                        Dodge += valueStat;
                        break;
                    case GlobalConstants.LOCK_ID:
                        Lock += valueStat;
                        break;
                    case GlobalConstants.WISDOM_ID:
                        Wisdom += valueStat;
                        break;
                    case GlobalConstants.PROSPECTING_ID:
                        Prospecting += valueStat;
                        break;
                    case GlobalConstants.CONTROL_ID:
                        Control += valueStat;
                        break;
                    case GlobalConstants.KIT_SKILL_ID:
                        KitSkill += valueStat;
                        break;
                    case GlobalConstants.WILL_ID:
                        Will += valueStat;
                        break;
                    case GlobalConstants.HEALTH_POINTS_IN_PERCENT_ID:
                        HealthInPercent += valueStat;
                        break;
                    case GlobalConstants.BARRIER_ID:
                        Barrier += valueStat;
                        break;
                    case GlobalConstants.HEALTH_RECEIVED_IN_PERCENT_ID:
                        HealthReceivedInPercent += valueStat;
                        break;
                    case GlobalConstants.HEALTH_POINTS_IN_ARMOR_IN_PERCENT_ID:
                        HealthPointsInArmorInPercent += valueStat;
                        break;
                    case GlobalConstants.LOCK_AND_DODGE_ID:
                        Lock += valueStat;
                        Dodge += valueStat;
                        break;
                    case GlobalConstants.MP_AND_DAMMAGE_ID:
                        AddMasteriesAll(valueStat);
                        if (valueStat != 0) {
                            MovementPoint += MP_VALUE_FOR_MAJOR_SKILL;
                        }
                        break;
                    case GlobalConstants.RANGE_AND_DAMMAGE_ID:
                        AddMasteriesAll(valueStat);
                        if (valueStat != 0) {
                            Range += RANGE_VALUE_FOR_MAJOR_SKILL;
                        }
                        break;
                    case GlobalConstants.CONTROL_AND_DAMMAGE_ID:
                        AddMasteriesAll(valueStat);
                        if (valueStat != 0) {
                            Control += CONTROL_VALUE_FOR_MAJOR_SKILL;
                        }
                        break;
                    case GlobalConstants.DAMMAGE_INFLIGED_ID:
                        DammageInfligedInPercent += valueStat;
                        break;
                    case GlobalConstants.HEALTH_PERFORMED_ID:
                        HealthPerformedInPercent += valueStat;
                        break;
                    default:
                        Console.WriteLine(String.Format("Unknown id stat {0} with value {1}", idStat, valueStat));
                        break;
                }
            }
        }

        /// <summary>
        /// Add the masteries and resistances elementary of the item to the build stats
        /// </summary>
        private void AddItemMasteriesAndElement(EnchantedItem item) {
            FireMastery += item.FireMastery;
            FireResistance += item.FireResistance;
            WaterMastery += item.WaterMastery;
            WaterResistance += item.WaterResistance;
            EarthMastery += item.EarthMastery;
            EarthResistance += item.EarthResistance;
            AirMastery += item.AirMastery;
            AirResistance += item.AirResistance;
        }

        private void AddMasteriesAll(int value) {
            FireMastery += value;
            WaterMastery += value;
            EarthMastery += value;
            AirMastery += value;
        }

        private void AddResistancesAll(int value) {
            FireResistance += value;
            WaterResistance += value;
            EarthResistance += value;
            AirResistance += value;
        }

        public void ResetBuildStats() {
            NationBonusLevel = 0;
            GuildBonusActived = false;
            ResetStatsOfBuild();
        }

        private void ResetStatsOfBuild() {
            Armor = 0;
            ActionPoint = BASE_AP;
            MovementPoint = BASE_MP;
            WakfuPoint = BASE_WP;
            FireMastery = 0;
            WaterMastery = 0;
            EarthMastery = 0;
            AirMastery = 0;
            MeleeMastery = 0;
            DistanceMastery = 0;
            SingleTargetMastery = 0;
            AreaMastery = 0;
            CriticalMastery = 0;
            RearMastery = 0;
            HealingMastery = 0;
            BerserkMastery = 0;
            FireResistance = 0;
            WaterResistance = 0;
            EarthResistance = 0;
            AirResistance = 0;
            CriticalResistance = 0;
            RearResistance = 0;
            CriticalHits = BASE_CC;
            Block = 0;
            Initiative = 0;
            Range = 0;
            Dodge = 0;
            Lock = 0;
            Wisdom = 0;
            Prospecting = 0;
            Control = BASE_CONTROL;
            KitSkill = 0;
            Will = 0;
            HealthInPercent = 0;
            Barrier = 0;
            HealthReceivedInPercent = 0;
            HealthPointsInArmorInPercent = 0;
            DammageInfligedInPercent = 0;
            HealthPerformedInPercent = 0;
        }


        /// <summary>
        /// Get the percentage of reduction of resistances for an element
        /// </summary>
        /// <param name="codeElement">Indicate wich element choose (fire = 0, water = 1, earth = 2, air = 3)</param>
        /// <returns>Return the percentage of reduction</returns>
        public double GetReductionOfResistance(int codeElement) {
            switch (codeElement) {
                case 0:
                    return CalculateReductionOfResistances(FireResistance);
                case 1:
                    return CalculateReductionOfResistances(WaterResistance);
                case 2:
                    return CalculateReductionOfResistances(EarthResistance);
                case 3:
                    return CalculateReductionOfResistances(AirResistance);
                default:
                    return 0;
            }
        }

        private double CalculateReductionOfResistances(double resistanceValue) {
            // Formula : Rounded down( 1 - 0.8 ^ (resistanceValue/100) ) * 100
            double resistanceInPercent = resistanceValue / 100.0;
            double reductionInPercent = 1 - Math.Pow(0.8, resistanceInPercent);
            double reductionFinal = Math.Floor(reductionInPercent * 100);
            return reductionFinal;
        }

        public int CalculateTotalHp() {
            return Convert.ToInt32(HealthPoints + HealthPoints * (HealthInPercent / 100.0));
        }

        public double CalculateArmor() {
            Armor = Convert.ToInt32(HealthPoints * (HealthPointsInArmorInPercent / 100.0));
            return Armor;
        }
    }
}

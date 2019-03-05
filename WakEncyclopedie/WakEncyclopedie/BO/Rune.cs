using System;
using System.Collections.Generic;
using static WakEncyclopedie.GlobalConstants;

namespace WakEncyclopedie.BO {
    public class Rune {
        private const int MIN_LEVEL = 1;
        private const int MAX_LEVEL = 10;
        private const int DEFAULT_LEVEL = 10;
        private const int ADVANCED_RUNE_LEVEL = 150;

        private const string NAME_RUNE_NULL = "Emplacement vide";
        private const string NAME_RUNE_MASTERY = "Rune de Maîtrise Élémentaire";
        private const string NAME_RUNE_RESISTANCE = "Rune de Résistance Élémentaire";
        private const string NAME_RUNE_HEALTH = "Rune de Points de Vie";
        private const string NAME_RUNE_DODGE = "Rune d'Esquive";
        private const string NAME_RUNE_LOCK = "Rune de Tacle";
        private const string NAME_RUNE_INITATIVE = "Rune d'Initative";
        private const string NAME_RUNE_MELEE = "Rune Fabuleuse de Mêlée";
        private const string NAME_RUNE_DISTANCE = "Rune Fabuleuse de Distance";
        private const string NAME_RUNE_SINGLE_TARGET = "Rune Fabuleuse de Précision";
        private const string NAME_RUNE_AREA = "Rune Fabuleuse de Destruction";
        private const string NAME_RUNE_CRIT_MASTERY = "Rune Fabuleuse d'Audace";
        private const string NAME_RUNE_REAR = "Rune Fabuleuse de Fourberie";
        private const string NAME_RUNE_HEALTH_MASTERY = "Rune Fabuleuse d'Altruisme";
        private const string NAME_RUNE_BERSERK = "Rune Fabuleuse de Fureur";
        private const string NAME_RUNE_CRIT_CHANCE = "Rune Fabuleuse d'Influence";
        private const string NAME_RUNE_BLOCK = "Rune Fabuleuse de Robustesse";
        private const string NAME_RUNE_HEALTH_DEF = "Rune Fabuleuse de Vie";
        private const string NAME_RUNE_WILL = "Rune Fabuleuse de Détermination";
        private const string NAME_RUNE_RANGE = "Rune Fabuleuse d'Acuité";
        private const string NAME_RUNE_MP = "Rune Fabuleuse de Célérité";
        private const string NAME_RUNE_AP = "Rune Fabuleuse de Vivacité";

        private const string IMAGE_PATH_RUNE_MASTERY = "pack://application:,,,/Images/Rune_Mastery_All.png";
        private const string IMAGE_PATH_RUNE_RESISTANCE = "pack://application:,,,/Images/Rune_Resistance_All.png";
        private const string IMAGE_PATH_RUNE_HEALTH = "pack://application:,,,/Images/Rune_Health.png";
        private const string IMAGE_PATH_RUNE_DODGE = "pack://application:,,,/Images/Rune_Dodge.png";
        private const string IMAGE_PATH_RUNE_LOCK = "pack://application:,,,/Images/Rune_Lock.png";
        private const string IMAGE_PATH_RUNE_INITIATIVE = "pack://application:,,,/Images/Rune_Initiative.png";
        private const string IMAGE_PATH_RUNE_MELEE = "pack://application:,,,/Images/Rune_Melee.png";
        private const string IMAGE_PATH_RUNE_DISTANCE = "pack://application:,,,/Images/Rune_Distance.png";
        private const string IMAGE_PATH_RUNE_SINGLE_TARGET = "pack://application:,,,/Images/Rune_ST.png";
        private const string IMAGE_PATH_RUNE_AREA = "pack://application:,,,/Images/Rune_Area.png";
        private const string IMAGE_PATH_RUNE_CRIT_MASTERY = "pack://application:,,,/Images/Rune_Crit_Mastery.png";
        private const string IMAGE_PATH_RUNE_REAR = "pack://application:,,,/Images/Rune_Rear.png";
        private const string IMAGE_PATH_RUNE_HEALTH_MASTERY = "pack://application:,,,/Images/Rune_Health_Mastery.png";
        private const string IMAGE_PATH_RUNE_BERSERK = "pack://application:,,,/Images/Rune_Berserk.png";
        private const string IMAGE_PATH_RUNE_CRIT_CHANCE = "pack://application:,,,/Images/Rune_CC.png";
        private const string IMAGE_PATH_RUNE_BLOCK = "pack://application:,,,/Images/Rune_Block.png";
        private const string IMAGE_PATH_RUNE_HEALTH_DEF = "pack://application:,,,/Images/Rune_Health_Def.png";
        private const string IMAGE_PATH_RUNE_WILL = "pack://application:,,,/Images/Rune_Will.png";
        private const string IMAGE_PATH_RUNE_RANGE = "pack://application:,,,/Images/Rune_Range.png";
        private const string IMAGE_PATH_RUNE_MP = "pack://application:,,,/Images/Rune_MP.png";
        private const string IMAGE_PATH_RUNE_AP = "pack://application:,,,/Images/Rune_AP.png";


        public static Dictionary<KeyValuePair<RuneType, string>, string> AttackRunesDictionary = new Dictionary<KeyValuePair<RuneType, string>, string>() {
            { new KeyValuePair<RuneType, string>(0, string.Empty), NAME_RUNE_NULL },
            { new KeyValuePair<RuneType, string>(RuneType.Mastery, IMAGE_PATH_RUNE_MASTERY), NAME_RUNE_MASTERY },
            { new KeyValuePair<RuneType, string>(RuneType.Melee, IMAGE_PATH_RUNE_MELEE), NAME_RUNE_MELEE },
            { new KeyValuePair<RuneType, string>(RuneType.Distance, IMAGE_PATH_RUNE_DISTANCE), NAME_RUNE_DISTANCE },
            { new KeyValuePair<RuneType, string>(RuneType.SingleTarget, IMAGE_PATH_RUNE_SINGLE_TARGET), NAME_RUNE_SINGLE_TARGET },
            { new KeyValuePair<RuneType, string>(RuneType.Area, IMAGE_PATH_RUNE_AREA), NAME_RUNE_AREA },
            { new KeyValuePair<RuneType, string>(RuneType.CritMastery, IMAGE_PATH_RUNE_CRIT_MASTERY), NAME_RUNE_CRIT_MASTERY },
            { new KeyValuePair<RuneType, string>(RuneType.Rear, IMAGE_PATH_RUNE_REAR), NAME_RUNE_REAR },
            { new KeyValuePair<RuneType, string>(RuneType.HealthMastery, IMAGE_PATH_RUNE_HEALTH_MASTERY), NAME_RUNE_HEALTH_MASTERY },
            { new KeyValuePair<RuneType, string>(RuneType.Berserk, IMAGE_PATH_RUNE_BERSERK), NAME_RUNE_BERSERK },
            { new KeyValuePair<RuneType, string>(RuneType.Range, IMAGE_PATH_RUNE_RANGE), NAME_RUNE_RANGE },
            { new KeyValuePair<RuneType, string>(RuneType.Mp, IMAGE_PATH_RUNE_MP), NAME_RUNE_MP },
            { new KeyValuePair<RuneType, string>(RuneType.Ap, IMAGE_PATH_RUNE_AP), NAME_RUNE_AP },
        };

        public static Dictionary<KeyValuePair<RuneType, string>, string> DefenseRunesDictionary = new Dictionary<KeyValuePair<RuneType, string>, string>() {
            { new KeyValuePair<RuneType, string>(0, string.Empty), NAME_RUNE_NULL },
            { new KeyValuePair<RuneType, string>(RuneType.Resistance, IMAGE_PATH_RUNE_RESISTANCE), NAME_RUNE_RESISTANCE },
            { new KeyValuePair<RuneType, string>(RuneType.Block, IMAGE_PATH_RUNE_BLOCK), NAME_RUNE_BLOCK },
            { new KeyValuePair<RuneType, string>(RuneType.HealthDef, IMAGE_PATH_RUNE_HEALTH_DEF), NAME_RUNE_HEALTH_DEF },
            { new KeyValuePair<RuneType, string>(RuneType.Range, IMAGE_PATH_RUNE_RANGE), NAME_RUNE_RANGE },
            { new KeyValuePair<RuneType, string>(RuneType.Mp, IMAGE_PATH_RUNE_MP), NAME_RUNE_MP },
            { new KeyValuePair<RuneType, string>(RuneType.Ap, IMAGE_PATH_RUNE_AP), NAME_RUNE_AP },
        };

        public static Dictionary<KeyValuePair<RuneType, string>, string> SupportRunesDictionary = new Dictionary<KeyValuePair<RuneType, string>, string>() {
            { new KeyValuePair<RuneType, string>(0, string.Empty), NAME_RUNE_NULL },
            { new KeyValuePair<RuneType, string>(RuneType.Health, IMAGE_PATH_RUNE_HEALTH), NAME_RUNE_HEALTH },
            { new KeyValuePair<RuneType, string>(RuneType.Dodge, IMAGE_PATH_RUNE_DODGE), NAME_RUNE_DODGE },
            { new KeyValuePair<RuneType, string>(RuneType.Lock, IMAGE_PATH_RUNE_LOCK), NAME_RUNE_LOCK },
            { new KeyValuePair<RuneType, string>(RuneType.Initiative, IMAGE_PATH_RUNE_INITIATIVE), NAME_RUNE_INITATIVE },
            { new KeyValuePair<RuneType, string>(RuneType.Will, IMAGE_PATH_RUNE_WILL), NAME_RUNE_WILL },
            { new KeyValuePair<RuneType, string>(RuneType.Range, IMAGE_PATH_RUNE_RANGE), NAME_RUNE_RANGE },
            { new KeyValuePair<RuneType, string>(RuneType.Mp, IMAGE_PATH_RUNE_MP), NAME_RUNE_MP },
            { new KeyValuePair<RuneType, string>(RuneType.Ap, IMAGE_PATH_RUNE_AP), NAME_RUNE_AP },
        };

        /// <summary>
        /// Bonus for mastery and resistance runes
        /// </summary>
        private const int BONUS_PER_LEVEL_STANDARD_RUNES = 1;
        /// <summary>
        /// Bonus for dodge, lock, initiative runes
        /// </summary>
        private const int BONUS_PER_LEVEL_SUPPORT_RUNES = 2;
        /// <summary>
        /// Bonus for melee, distance, single target, area, critical and rear masteries runes
        /// </summary>
        private const int BONUS_PER_LEVEL_ADVANCED_RUNES = 2;
        /// <summary>
        /// Bonus for health and berserk masteries runes
        /// </summary>
        private const int BONUS_PER_LEVEL_SPECIFIC_RUNES = 3;
        /// <summary>
        /// Bonus for crit chance, block, range, mp and ap runes
        /// </summary>
        private const int BONUS_PER_LEVEL_EXQUISITE_RUNES = 1;
        private const int BONUS_PER_LEVEL_HEALTH = 4;
        private const int BONUS_PER_LEVEL_HEALTH_DEF = 6;
        private const double BONUS_PER_LEVEL_WILL = 0.5;

        private RuneType _type;
        private int _level;
        private int IdStatBonus { get; set; }
        private int BonusPerLevel { get; set; }

        public RuneSlot Slot { get; private set; }
        public string Name { get; private set; }
        public string ImagePath { get; private set; }
        public bool Enabled { get; set; }
        public bool ExquisiteEnabled { get; set; }
        public RuneType Type {
            get => _type;
            private set {
                _type = value;
                InitRuneFromType();
            }
        }
        public int Level {
            get => _level;
            set {
                if (value < MIN_LEVEL)
                    value = MIN_LEVEL;
                else if (value > MAX_LEVEL)
                    value = MAX_LEVEL;
                _level = value;
            }
        }

        public Rune(RuneSlot slot, bool enabled = false, bool exquisiteEnabled = false) {
            Slot = slot;
            Enabled = enabled;
            ExquisiteEnabled = exquisiteEnabled;
            Level = DEFAULT_LEVEL;
        }

        public bool EquipRune(RuneType type) {
            return VerifyRuneType(type);
        }

        private bool VerifyRuneType(RuneType type) {
            bool typeOk = false;
            if (type == RuneType.Null) {
                typeOk = true;
            }
            switch (Slot) {
                case RuneSlot.Attack:
                    if (type == RuneType.Mastery || type == RuneType.Melee || type == RuneType.Distance || type == RuneType.SingleTarget ||
                        type == RuneType.Area || type == RuneType.CritMastery || type == RuneType.Rear || type == RuneType.HealthMastery ||
                        type == RuneType.Berserk || type == RuneType.CritChance || type == RuneType.Range || type == RuneType.Mp || type == RuneType.Ap) {
                        typeOk = true;
                    }
                    break;
                case RuneSlot.Defense:
                    if (type == RuneType.Resistance || type == RuneType.Block || type == RuneType.HealthDef || 
                        type == RuneType.Range || type == RuneType.Mp || type == RuneType.Ap) {
                        typeOk = true;
                    }
                    break;
                case RuneSlot.Support:
                    if (type == RuneType.Health || type == RuneType.Dodge || type == RuneType.Lock || type == RuneType.Initiative ||
                        type == RuneType.Will || type == RuneType.Range || type == RuneType.Mp || type == RuneType.Ap) {
                        typeOk = true;
                    }
                    break;
                case RuneSlot.Relic:
                    // TODO
                    break;
                case RuneSlot.Epic:
                    // TODO
                    break;
                default:
                    Console.WriteLine("Unknown Rune slot");
                    break;
            }
            if (typeOk) {
                Type = type;
                return true;
            } else {
                return false;
            }
        }

        private void InitRuneFromType() {
            switch (Type) {
                case RuneType.Null:
                    SetProperties(NAME_RUNE_NULL, string.Empty, 0, 0);
                    break;
                case RuneType.Mastery:
                    SetProperties(NAME_RUNE_MASTERY, IMAGE_PATH_RUNE_MASTERY, GlobalConstants.MASTERY_ALL_ID, BONUS_PER_LEVEL_STANDARD_RUNES);
                    break;
                case RuneType.Resistance:
                    SetProperties(NAME_RUNE_RESISTANCE, IMAGE_PATH_RUNE_RESISTANCE, GlobalConstants.RESISTANCE_ALL_ID, BONUS_PER_LEVEL_STANDARD_RUNES);
                    break;
                case RuneType.Health:
                    SetProperties(NAME_RUNE_HEALTH, IMAGE_PATH_RUNE_HEALTH, GlobalConstants.HEALTH_POINTS_ID, BONUS_PER_LEVEL_HEALTH);
                    break;
                case RuneType.Dodge:
                    SetProperties(NAME_RUNE_DODGE, IMAGE_PATH_RUNE_DODGE, GlobalConstants.DODGE_ID, BONUS_PER_LEVEL_SUPPORT_RUNES);
                    break;
                case RuneType.Lock:
                    SetProperties(NAME_RUNE_LOCK, IMAGE_PATH_RUNE_LOCK, GlobalConstants.LOCK_ID, BONUS_PER_LEVEL_SUPPORT_RUNES);
                    break;
                case RuneType.Initiative:
                    SetProperties(NAME_RUNE_INITATIVE, IMAGE_PATH_RUNE_INITIATIVE, GlobalConstants.INITIATIVE_ID, BONUS_PER_LEVEL_SUPPORT_RUNES);
                    break;
                case RuneType.Melee:
                    SetProperties(NAME_RUNE_MELEE, IMAGE_PATH_RUNE_MELEE, GlobalConstants.MELEE_MASTERY_ID, BONUS_PER_LEVEL_ADVANCED_RUNES);
                    break;
                case RuneType.Distance:
                    SetProperties(NAME_RUNE_DISTANCE, IMAGE_PATH_RUNE_DISTANCE, GlobalConstants.DISTANCE_MASTERY_ID, BONUS_PER_LEVEL_ADVANCED_RUNES);
                    break;
                case RuneType.SingleTarget:
                    SetProperties(NAME_RUNE_SINGLE_TARGET, IMAGE_PATH_RUNE_SINGLE_TARGET, GlobalConstants.SINGLE_TARGET_MASTERY_ID, BONUS_PER_LEVEL_ADVANCED_RUNES);
                    break;
                case RuneType.Area:
                    SetProperties(NAME_RUNE_AREA, IMAGE_PATH_RUNE_AREA, GlobalConstants.AREA_MASTERY_ID, BONUS_PER_LEVEL_ADVANCED_RUNES);
                    break;
                case RuneType.CritMastery:
                    SetProperties(NAME_RUNE_CRIT_MASTERY, IMAGE_PATH_RUNE_CRIT_MASTERY, GlobalConstants.CRITICAL_MASTERY_ID, BONUS_PER_LEVEL_ADVANCED_RUNES);
                    break;
                case RuneType.Rear:
                    SetProperties(NAME_RUNE_REAR, IMAGE_PATH_RUNE_REAR, GlobalConstants.REAR_MASTERY_ID, BONUS_PER_LEVEL_ADVANCED_RUNES);
                    break;
                case RuneType.HealthMastery:
                    SetProperties(NAME_RUNE_HEALTH_MASTERY, IMAGE_PATH_RUNE_HEALTH_MASTERY, GlobalConstants.HEALING_MASTERY_ID, BONUS_PER_LEVEL_SPECIFIC_RUNES);
                    break;
                case RuneType.Berserk:
                    SetProperties(NAME_RUNE_BERSERK, IMAGE_PATH_RUNE_BERSERK, GlobalConstants.BERSERK_MASTERY_ID, BONUS_PER_LEVEL_SPECIFIC_RUNES);
                    break;
                case RuneType.HealthDef:
                    SetProperties(NAME_RUNE_HEALTH_DEF, IMAGE_PATH_RUNE_HEALTH_DEF, GlobalConstants.HEALTH_POINTS_ID, BONUS_PER_LEVEL_SPECIFIC_RUNES);
                    break;
                case RuneType.CritChance:
                    SetProperties(NAME_RUNE_CRIT_CHANCE, IMAGE_PATH_RUNE_CRIT_CHANCE, GlobalConstants.CRITICAL_HITS_ID, BONUS_PER_LEVEL_EXQUISITE_RUNES);
                    break;
                case RuneType.Block:
                    SetProperties(NAME_RUNE_BLOCK, IMAGE_PATH_RUNE_BLOCK, GlobalConstants.BLOCK_ID, BONUS_PER_LEVEL_EXQUISITE_RUNES);
                    break;
                case RuneType.Range:
                    SetProperties(NAME_RUNE_RANGE, IMAGE_PATH_RUNE_RANGE, GlobalConstants.RANGE_ID, BONUS_PER_LEVEL_EXQUISITE_RUNES);
                    break;
                case RuneType.Mp:
                    SetProperties(NAME_RUNE_MP, IMAGE_PATH_RUNE_MP, GlobalConstants.MOVEMENT_POINT_ID, BONUS_PER_LEVEL_EXQUISITE_RUNES);
                    break;
                case RuneType.Ap:
                    SetProperties(NAME_RUNE_AP, IMAGE_PATH_RUNE_AP, GlobalConstants.ACTION_POINT_ID, BONUS_PER_LEVEL_EXQUISITE_RUNES);
                    break;
                default:
                    Console.WriteLine("Unknown Rune type");
                    break;
            }
        }

        private void SetProperties(string name, string imagePath, int idStatBonus, int bonusPerLevel) {
            Name = name;
            ImagePath = imagePath;
            IdStatBonus = idStatBonus;
            BonusPerLevel = bonusPerLevel;
        }

        public int GetTotalBonus() {
            return BonusPerLevel * Level;
        }
    }

}

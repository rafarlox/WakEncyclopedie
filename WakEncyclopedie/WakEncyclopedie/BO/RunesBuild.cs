using System.Collections.Generic;
using WakEncyclopedie.DAO;
using WakEncyclopedie.Utility;
using static WakEncyclopedie.GlobalConstants;

namespace WakEncyclopedie.BO {
    public class RunesBuild {
        private const int TOTAL_MAX_RUNES_ATTACK = 10;
        private const int TOTAL_MAX_RUNES_DEFENSE = 11;
        private const int TOTAL_MAX_RUNES_SUPPORT = 5;
        private const int FIRST_LEVEL_RUNES = 1; // breastplate, cloak and boots
        private const int SECOND_LEVEL_RUNES = 40; // weapons (one hand = 1 rune, two hands = 2 runes), helmet, dagger and shield
        private const int THIRD_LEVEL_RUNES = 80; // all items except weapons, dagger and shield
        private const int FINAL_LEVEL_RUNES = 120; // all items except dagger and shield
        private const int CRIT_BONUS_LEVEL = 10;
        private const int BLOCK_BONUS_LEVEL = 10;
        private const int RANGE_BONUS_LEVEL = 60;
        private const int MP_BONUS_LEVEL = 90;
        private const int AP_BONUS_LEVEL = 140;
        public const int EXQUISITE_RUNE_LEVEL = 150;

        private int _critChanceLevel;
        private int _blockLevel;
        private int _rangeLevel;
        private int _mpLevel;
        private int _apLevel;

        public Build ActualBuild { get; set; }

        public int CritChanceBonus { get; private set; }
        public int BlockBonus { get; private set; }
        public int RangeBonus { get; private set; }
        public int MpBonus { get; private set; }
        public int ApBonus { get; private set; }

        public int CritChanceLevel {
            get => _critChanceLevel;
            private set {
                _critChanceLevel = value;
                if (_critChanceLevel >= CRIT_BONUS_LEVEL) {
                    CritChanceBonus = _critChanceLevel / CRIT_BONUS_LEVEL;
                } else {
                    CritChanceBonus = 0;
                }
            }
        }
        public int BlockLevel {
            get => _blockLevel;
            private set {
                _blockLevel = value;
                if (_blockLevel >= BLOCK_BONUS_LEVEL) {
                    BlockBonus = _blockLevel / CRIT_BONUS_LEVEL;
                } else {
                    BlockBonus = 0;
                }
            }
        }
        public int RangeLevel {
            get => _rangeLevel;
            private set {
                _rangeLevel = value;
                if (_rangeLevel >= RANGE_BONUS_LEVEL) {
                    RangeBonus = 1;
                } else {
                    RangeBonus = 0;
                }
            }
        }
        public int MpLevel {
            get => _mpLevel;
            private set {
                _mpLevel = value;
                if (_mpLevel >= MP_BONUS_LEVEL) {
                    MpBonus = 1;
                } else {
                    MpBonus = 0;
                }
            }
        }
        public int ApLevel {
            get => _apLevel;
            private set {
                _apLevel = value;
                if (_apLevel >= AP_BONUS_LEVEL) {
                    ApBonus = 1;
                } else {
                    ApBonus = 0;
                }
            }
        }

        public Rune[] AttackRunes { get; private set; }
        public Rune[] DefenseRunes { get; private set; }
        public Rune[] SupportRunes { get; private set; }
        public Rune RelicRune { get; set; }
        public Rune EpicRune { get; set; }

        private List<int> IdTypeRunesAttack { get; } = new List<int>() {
            Build.ID_ONE_HANDED_WEAPON,
            Build.ID_TWO_HANDED_WEAPON,
            Build.ID_HELMET,
            Build.ID_RING,
        };
        private List<int> IdTypeRunesDefense { get; } = new List<int>() {
            Build.ID_BREASTPLATE,
            Build.ID_CLOACK,
            Build.ID_EPAULETTES,
            Build.ID_AMULET,
        };

        private List<int> IdTypeRunesSupport { get; } = new List<int>() {
            Build.ID_BOOTS,
            Build.ID_BELT,
        };

        public RunesBuild() {

        }

        public RunesBuild(Build actualBuild) {
            ActualBuild = actualBuild;
            InitializeRunes();
        }

        private void InitializeRunes() {
            AttackRunes = new Rune[TOTAL_MAX_RUNES_ATTACK];
            for (int i = 0; i < AttackRunes.Length; i++) {
                AttackRunes[i] = new Rune(RuneSlot.Attack, false);
            }
            DefenseRunes = new Rune[TOTAL_MAX_RUNES_DEFENSE];
            for (int i = 0; i < DefenseRunes.Length; i++) {
                DefenseRunes[i] = new Rune(RuneSlot.Defense, false);
            }
            SupportRunes = new Rune[TOTAL_MAX_RUNES_SUPPORT];
            for (int i = 0; i < SupportRunes.Length; i++) {
                SupportRunes[i] = new Rune(RuneSlot.Support, false);
            }
            RelicRune = new Rune(RuneSlot.Relic, false);
            EpicRune = new Rune(RuneSlot.Epic, false);
            VerifyRunesAvailability();
        }

        /// <summary>
        /// Enable or disable the runes according to the items equiped
        /// </summary>
        public void VerifyRunesAvailability() {
            bool relicFound = false;
            bool epicFound = false;
            int totalRunesAttack = 0;
            int totalRunesDefense = 0;
            int totalRunesSupport = 0;
            int nbExquisiteRunesAttackAvailable = 0;
            int nbExquisiteRunesDefenseAvailable = 0;
            int nbExquisiteRunesSupportAvailable = 0;

            // Disable all runes before looking the items
            DisableAllRunesInArray(AttackRunes);
            DisableAllRunesInArray(DefenseRunes);
            DisableAllRunesInArray(SupportRunes);

            foreach (EnchantedItem item in ActualBuild.GetBuildItems()) {
                // Verify if the item is an relic or epic
                if (item.IdRarity == Build.RARITY_ID_RELIC) {
                    relicFound = true;
                } else if (item.IdRarity == Build.RARITY_ID_EPIC) {
                    epicFound = true;
                }

                int runesToAdd = CountRunesEmplacement(item.IdType, item.Level);
                // Verify the type of runes who correspond to the item by it's id type and count the number of runes enabled
                if (IdTypeRunesAttack.IndexOf(item.IdType) != -1 || item.IsDaggerOrShield() == true) {
                    totalRunesAttack += runesToAdd;
                    if (item.Level >= EXQUISITE_RUNE_LEVEL) {
                        nbExquisiteRunesAttackAvailable += runesToAdd;
                    }
                } else if (IdTypeRunesDefense.IndexOf(item.IdType) != -1 || item.IsDaggerOrShield() == false) {
                    totalRunesDefense += runesToAdd;
                    if (item.Level >= EXQUISITE_RUNE_LEVEL) {
                        nbExquisiteRunesDefenseAvailable += runesToAdd;
                    }
                } else if (IdTypeRunesSupport.IndexOf(item.IdType) != -1) {
                    totalRunesSupport += runesToAdd;
                    if (item.Level >= EXQUISITE_RUNE_LEVEL) {
                        nbExquisiteRunesSupportAvailable += runesToAdd;
                    }
                }
            }
            // Enable the runes
            if (relicFound)
                RelicRune.Enabled = true;
            if (epicFound)
                EpicRune.Enabled = true;

            EnableRunesInArray(AttackRunes, totalRunesAttack);
            EnableRunesInArray(DefenseRunes, totalRunesDefense);
            EnableRunesInArray(SupportRunes, totalRunesSupport);

            EnableExquisiteRunesInArray(AttackRunes, nbExquisiteRunesAttackAvailable);
            EnableExquisiteRunesInArray(DefenseRunes, nbExquisiteRunesDefenseAvailable);
            EnableExquisiteRunesInArray(SupportRunes, nbExquisiteRunesSupportAvailable);

            // Calculate the total of special bonus after activating the runes
            CountSpecialBonus();
        }

        private void EnableRunesInArray(Rune[] runes, int runesToEnable) {
            for (int i = 0; i < runesToEnable; i++) {
                runes[i].Enabled = true;
            }
        }
        private void EnableExquisiteRunesInArray(Rune[] runes, int runesToEnable) {
            for (int i = 0; i < runesToEnable; i++) {
                runes[i].ExquisiteEnabled = true;
            }
        }

        private void DisableAllRunesInArray(Rune[] runes) {
            foreach (Rune rune in runes) {
                rune.Enabled = false;
            }
        }

        public int CountRunesEmplacement(int idType, int level) {
            int runesCount = 0;
            switch (idType) {
                // Two runes max, start at level 40
                case Build.ID_ONE_HANDED_WEAPON:
                    if (level >= FINAL_LEVEL_RUNES) {
                        runesCount += 2;
                    } else if (level >= SECOND_LEVEL_RUNES) {
                        runesCount += 1;
                    }
                    break;
                // Threee runes max, start at level 40
                case Build.ID_TWO_HANDED_WEAPON:
                    if (level >= FINAL_LEVEL_RUNES) {
                        runesCount += 3;
                    } else if (level >= SECOND_LEVEL_RUNES) {
                        runesCount += 2;
                    }
                    break;
                // Three runes max, start at level 1 (level 40 for helmets)
                case Build.ID_BOOTS:
                case Build.ID_BREASTPLATE:
                case Build.ID_CLOACK:
                case Build.ID_HELMET:
                    if (level >= FINAL_LEVEL_RUNES) {
                        runesCount += 3;
                    } else if (level >= THIRD_LEVEL_RUNES) {
                        runesCount += 2;
                    } else if ((idType == Build.ID_HELMET && level >= SECOND_LEVEL_RUNES) ||
                               (idType != Build.ID_HELMET && level >= FIRST_LEVEL_RUNES)) {
                        runesCount += 1;
                    }
                    break;
                // Two runes max, start at level 80
                case Build.ID_AMULET:
                case Build.ID_BELT:
                case Build.ID_EPAULETTES:
                case Build.ID_RING:
                    if (level >= FINAL_LEVEL_RUNES) {
                        runesCount += 2;
                    } else if (level >= THIRD_LEVEL_RUNES) {
                        runesCount += 1;
                    }
                    break;
                // One rune max
                case Build.ID_SECOND_HAND:
                    if (level >= SECOND_LEVEL_RUNES) {
                        runesCount += 1;
                    }
                    break;
                default:
                    break;
            }
            return runesCount;
        }

        private void CountSpecialBonus() {
            // Reset the bonus
            CritChanceBonus = 0;
            BlockBonus = 0;
            RangeBonus = 0;
            MpBonus = 0;
            ApBonus = 0;
            // Calculate the total of level for each bonus
            CalculateSpecialLevelInArray(GetAllRunes());
        }

        private void CalculateSpecialLevelInArray(List<Rune> runes) {
            foreach (Rune rune in runes) {
                if (rune.Enabled && rune.ExquisiteEnabled) {
                    switch (rune.Type) {
                        case RuneType.CritChance:
                            CritChanceLevel += rune.GetTotalBonus();
                            break;
                        case RuneType.Block:
                            BlockLevel += rune.GetTotalBonus();
                            break;
                        case RuneType.Range:
                            RangeLevel += rune.GetTotalBonus();
                            break;
                        case RuneType.Mp:
                            MpLevel += rune.GetTotalBonus();
                            break;
                        case RuneType.Ap:
                            ApLevel += rune.GetTotalBonus();
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        public bool IsExquisiteRune(Rune rune) {
            if (rune.Type == RuneType.Ap || rune.Type == RuneType.Mp || rune.Type == RuneType.Range ||
                rune.Type == RuneType.CritChance || rune.Type == RuneType.Block || rune.Type == RuneType.Melee ||
                rune.Type == RuneType.Distance || rune.Type == RuneType.SingleTarget || rune.Type == RuneType.Area ||
                rune.Type == RuneType.CritMastery || rune.Type == RuneType.Rear || rune.Type == RuneType.HealthMastery ||
                rune.Type == RuneType.Berserk || rune.Type == RuneType.HealthDef) {
                return true;
            } else {
                return false;
            }
        }

        public bool IsSpecialRune(Rune rune) {
            if (rune.Type == RuneType.Ap || rune.Type == RuneType.Mp || rune.Type == RuneType.Range ||
                rune.Type == RuneType.CritChance || rune.Type == RuneType.Block) {
                return true;
            } else {
                return false;
            }
        }

        public List<Rune> GetAllRunes() {
            List<Rune> runesList = new List<Rune>();
            runesList.AddRange(AttackRunes);
            runesList.AddRange(DefenseRunes);
            runesList.AddRange(SupportRunes);
            runesList.Add(RelicRune);
            runesList.Add(EpicRune);
            return runesList;
        }

        public int GetCountEnabledRunes(Rune[] runes) {
            int count = 0;
            foreach (Rune rune in runes) {
                if (rune.Enabled == true) {
                    count++;
                }
            }
            return count;
        }
    }
}

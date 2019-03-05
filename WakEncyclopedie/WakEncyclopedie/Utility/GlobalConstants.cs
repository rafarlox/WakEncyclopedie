using System;

namespace WakEncyclopedie {
    public static class GlobalConstants {
        public enum Elementary { Fire, Water, Earth, Air }
        public enum RuneSlot { Attack, Defense, Support, Relic, Epic }
        public enum RuneType {
            Null = 0,
            Mastery = MASTERY_ALL_ID,
            Resistance = RESISTANCE_ALL_ID,
            Health = HEALTH_POINTS_ID,
            Dodge = DODGE_ID,
            Lock = LOCK_ID,
            Initiative = INITIATIVE_ID,
            Melee = MELEE_MASTERY_ID,
            Distance = DISTANCE_MASTERY_ID,
            SingleTarget = SINGLE_TARGET_MASTERY_ID,
            Area = AREA_MASTERY_ID,
            CritMastery = CRITICAL_MASTERY_ID,
            Rear = REAR_MASTERY_ID,
            HealthMastery = HEALING_MASTERY_ID,
            Berserk = BERSERK_MASTERY_ID,
            CritChance = CRITICAL_HITS_ID,
            Block = BLOCK_ID,
            HealthDef = HEALTH_POINTS_FOR_DEF_RUNES_ID,
            Will = WILL_ID,
            Range = RANGE_ID,
            Mp = MOVEMENT_POINT_ID,
            Ap = ACTION_POINT_ID
        }
        public const int MIN_LEVEL = 0;
        public const int MAX_LEVEL = 200;
        public static readonly int[] MODULE_LEVELS = new int[13] { 20, 35, 50, 65, 80, 95, 110, 125, 140, 155, 170, 185, 200 };
        #region image_path
        // Path for type of items
        public const string HELMET_IMAGE_PATH = "pack://application:,,,/Images/Helmet_bw.png";
        public const string CLOAK_IMAGE_PATH = "pack://application:,,,/Images/Cloak_bw.png";
        public const string AMULET_IMAGE_PATH = "pack://application:,,,/Images/Amulet_bw.png";
        public const string EPAULETTES_IMAGE_PATH = "pack://application:,,,/Images/Epaulettes_bw.png";
        public const string BREASTPLATE_IMAGE_PATH = "pack://application:,,,/Images/Breastplate_bw.png";
        public const string BELT_IMAGE_PATH = "pack://application:,,,/Images/Belt_bw.png";
        public const string BOOTS_IMAGE_PATH = "pack://application:,,,/Images/Boots_bw.png";
        public const string RING_IMAGE_PATH = "pack://application:,,,/Images/Ring_bw.png";
        public const string ONE_HANDED_WEAPON_IMAGE_PATH = "pack://application:,,,/Images/OneHandedWeapon_bw.png";
        public const string SECOND_HAND_WEAPON_IMAGE_PATH = "pack://application:,,,/Images/SecondHand_bw.png";
        public const string PET_IMAGE_PATH = "pack://application:,,,/Images/Pet.png";
        public const string MOUNT_IMAGE_PATH = "pack://application:,,,/Images/Mount_bw.png";
        public const string EMBLEM_IMAGE_PATH = "pack://application:,,,/Images/Emblem_bw.png";
        // Path for element
        public const string FIRE_IMAGE_PATH = "pack://application:,,,/Images/Fire.png";
        public const string WATER_IMAGE_PATH = "pack://application:,,,/Images/Water.png";
        public const string EARTH_IMAGE_PATH = "pack://application:,,,/Images/Earth.png";
        public const string AIR_IMAGE_PATH = "pack://application:,,,/Images/Air.png";
        #endregion image_path
        #region elems_string
        public const string MASTERIES_STRING = "masteries";
        public const string RESISTANCES_STRING = "resistances";
        public const string FIRE_STRING = "fire";
        public const string WATER_STRING = "water";
        public const string EARTH_STRING = "earth";
        public const string AIR_STRING = "air";
        #endregion elems_string
        #region elems_id
        public const int ID_MASTERIES_3_ELEM = 6;
        public const int ID_MASTERIES_2_ELEM = 7;
        public const int ID_MASTERIES_1_ELEM = 8;
        public const int ID_RESISTANCES_3_ELEM = 22;
        public const int ID_RESISTANCES_2_ELEM = 23;
        public const int ID_RESISTANCES_1_ELEM = 24;
        public static readonly int[] IDS_ELEM_ARRAY = new int[6] { ID_MASTERIES_3_ELEM, ID_MASTERIES_2_ELEM, ID_MASTERIES_1_ELEM, ID_RESISTANCES_3_ELEM, ID_RESISTANCES_2_ELEM, ID_RESISTANCES_1_ELEM };
        #endregion elems_id
        #region stats_id
        public const int HEALTH_POINTS_ID = 1;
        public const int ACTION_POINT_ID = 2;
        public const int MOVEMENT_POINT_ID = 3;
        public const int WAKFU_POINT_ID = 4;
        public const int MASTERY_ALL_ID = 5;
        public const int FIRE_MASTERY_ID = 9;
        public const int WATER_MASTERY_ID = 10;
        public const int EARTH_MASTERY_ID = 11;
        public const int AIR_MASTERY_ID = 12;
        public const int MELEE_MASTERY_ID = 13;
        public const int DISTANCE_MASTERY_ID = 14;
        public const int SINGLE_TARGET_MASTERY_ID = 15;
        public const int AREA_MASTERY_ID = 16;
        public const int CRITICAL_MASTERY_ID = 17;
        public const int REAR_MASTERY_ID = 18;
        public const int HEALING_MASTERY_ID = 19;
        public const int BERSERK_MASTERY_ID = 20;
        public const int RESISTANCE_ALL_ID = 21;
        public const int FIRE_RESISTANCE_ID = 25;
        public const int WATER_RESISTANCE_ID = 26;
        public const int EARTH_RESISTANCE_ID = 27;
        public const int AIR_RESISTANCE_ID = 28;
        public const int CRITICAL_RESISTANCE_ID = 29;
        public const int REAR_RESISTANCE_ID = 30;
        public const int CRITICAL_HITS_ID = 31;
        public const int BLOCK_ID = 32;
        public const int INITIATIVE_ID = 33;
        public const int RANGE_ID = 34;
        public const int DODGE_ID = 35;
        public const int LOCK_ID = 36;
        public const int WISDOM_ID = 37;
        public const int PROSPECTING_ID = 38;
        public const int CONTROL_ID = 39;
        public const int KIT_SKILL_ID = 40;
        public const int WILL_ID = 41;
        // const below are for skills stats, the id are negative for avoid conflict with id from database
        // intelligence
        public const int HEALTH_POINTS_IN_PERCENT_ID = -2;
        public const int BARRIER_ID = -3;
        public const int HEALTH_RECEIVED_IN_PERCENT_ID = -4;
        public const int HEALTH_POINTS_IN_ARMOR_IN_PERCENT_ID = -5;
        // Agility
        public const int LOCK_AND_DODGE_ID = -6;
        // Major
        public const int MP_AND_DAMMAGE_ID = -7;
        public const int RANGE_AND_DAMMAGE_ID = -8;
        public const int CONTROL_AND_DAMMAGE_ID = -9;
        public const int HEALTH_PERFORMED_ID = -10;
        public const int DAMMAGE_INFLIGED_ID = -11;
        public const int RES_ALL_FOR_MAJOR_ID = -12;
        public const int HEALTH_POINTS_FOR_DEF_RUNES_ID = -13;
        #endregion stats_id
        #region stats_string
        public const string HEALTH_STRING = "Points de vie";
        public const string WISDOM_STRING = "Sagesse";
        public const string PROSPECTING_STRING = "Prospection";
        public const string DAMMAGE_INFLIGED_STRING = "% Dommages infligés";
        public const string HEALTH_PERFORMED_STRING = "Soins réalisés";
        #endregion stats_string
        #region methods
        public static bool IsIdOfMastery(int idStat) {
            const int idMasteryStart = 5;
            const int idMasteryEnd = 20;
            if (idStat >= idMasteryStart && idStat <= idMasteryEnd) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Look if the id received correspond to the id of masteries or resistances
        /// </summary>
        /// <param name="id">Id of a stat</param>
        /// <returns>Return true if it's an id for masteries, return false if it's an id for resistances, else return null</returns>
        public static bool? IdForMasteriesOrResistances(int id) {
            if (id == ID_MASTERIES_3_ELEM || id == ID_MASTERIES_2_ELEM || id == ID_MASTERIES_1_ELEM) {
                return true;
            } else if (id == ID_RESISTANCES_3_ELEM || id == ID_RESISTANCES_2_ELEM || id == ID_RESISTANCES_1_ELEM) {
                return false;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Get the string that correspond to the element of the image source path
        /// </summary>
        /// <param name="imageSourcePath">Path that correspond to an element</param>
        /// <returns>Return the element corresponding, else return an empty string.</returns>
        public static string GetElemStringFromImagePath(string imageSourcePath) {
            switch (imageSourcePath) {
                case FIRE_IMAGE_PATH:
                    return FIRE_STRING;
                case WATER_IMAGE_PATH:
                    return WATER_STRING;
                case EARTH_IMAGE_PATH:
                    return EARTH_STRING;
                case AIR_IMAGE_PATH:
                    return AIR_STRING;
                default:
                    return String.Empty;
            }
        }

        public static int GetNbOfElementById(int idStat) {
            switch (idStat) {
                case ID_MASTERIES_3_ELEM:
                case ID_RESISTANCES_3_ELEM:
                    return 3;
                case ID_MASTERIES_2_ELEM:
                case ID_RESISTANCES_2_ELEM:
                    return 2;
                case ID_MASTERIES_1_ELEM:
                case ID_RESISTANCES_1_ELEM:
                    return 1;
                default:
                    return 0;
            }
        }

        public static Elementary? GetElementaryByImagePath(string imgPath) {
            switch (imgPath) {
                case FIRE_IMAGE_PATH:
                    return Elementary.Fire;
                case WATER_IMAGE_PATH:
                    return Elementary.Water;
                case EARTH_IMAGE_PATH:
                    return Elementary.Earth;
                case AIR_IMAGE_PATH:
                    return Elementary.Air;
                default:
                    return null;
            }
        }
        #endregion methods
    }
}

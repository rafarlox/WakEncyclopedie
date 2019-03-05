using System;
using System.Linq;

namespace WakEncyclopedie {
    public class EnchantedItem : Item {
        private int ElemMasteriesValue { get; set; }
        private int ElemResistancesValue { get; set; }

        public int MasteriesElementsRequired { get; private set; }
        public int ResistancesElementsRequired { get; private set; }

        public int FireMastery { get; private set; }
        public int FireResistance { get; private set; }
        public int WaterMastery { get; private set; }
        public int WaterResistance { get; private set; }
        public int EarthMastery { get; private set; }
        public int EarthResistance { get; private set; }
        public int AirMastery { get; private set; }
        public int AirResistance { get; private set; }

        public bool ConditionsRespected { get; private set; }

        public int KitSkill { get; private set; }

        public EnchantedItem() : this(new Item()) {

        }

        public EnchantedItem(Item item) {
            Id = item.Id;
            Name = item.Name;
            Level = item.Level;
            Image = item.Image;
            Url = item.Url;
            IdType = item.IdType;
            Type = item.Type;
            TypeImage = item.TypeImage;
            IdRarity = item.IdRarity;
            RarityName = item.RarityName;
            RarityImage = item.RarityImage;
            StatList = item.StatList;

            MasteriesElementsRequired = 0;
            ResistancesElementsRequired = 0;

            CalculateMaxElements();
            VerifyAllConditions();
        }

        /// <summary>
        /// Calculate the total of element required for the masteries and resistances of the item
        /// </summary>
        private void CalculateMaxElements() {
            // Search through the stats the id that correspond to an id of masteries or resistances
            foreach (Stat stat in StatList) {
                if (GlobalConstants.IDS_ELEM_ARRAY.Contains(stat.Id)) {
                    switch (stat.Id) {
                        case GlobalConstants.ID_MASTERIES_3_ELEM:
                            MasteriesElementsRequired = 3;
                            ElemMasteriesValue = stat.Value;
                            break;
                        case GlobalConstants.ID_MASTERIES_2_ELEM:
                            MasteriesElementsRequired = 2;
                            ElemMasteriesValue = stat.Value;
                            break;
                        case GlobalConstants.ID_MASTERIES_1_ELEM:
                            MasteriesElementsRequired = 1;
                            ElemMasteriesValue = stat.Value;
                            break;
                        case GlobalConstants.ID_RESISTANCES_3_ELEM:
                            ResistancesElementsRequired = 3;
                            ElemResistancesValue = stat.Value;
                            break;
                        case GlobalConstants.ID_RESISTANCES_2_ELEM:
                            ResistancesElementsRequired = 2;
                            ElemResistancesValue = stat.Value;
                            break;
                        case GlobalConstants.ID_RESISTANCES_1_ELEM:
                            ResistancesElementsRequired = 1;
                            ElemResistancesValue = stat.Value;
                            break;
                        default:
                            Console.WriteLine("Incoherence between the const array and the const id in GlobalConstants class.");
                            break;
                    }
                }

                // Save the value of kit skill in a properties for easier access
                if (stat.Id == GlobalConstants.KIT_SKILL_ID) {
                    KitSkill = stat.Value;
                }
            }
        }

        /// <summary>
        /// Verify all conditions of the item for being equiped
        /// </summary>
        /// <returns>True if the conditions are respected, else return false</returns>
        private bool VerifyAllConditions() {
            // Verify the numbers of masteries and resistances selected
            int countMasteries = 0;
            int countResistances = 0;

            if (FireMastery != 0)
                countMasteries++;
            if (WaterMastery != 0)
                countMasteries++;
            if (EarthMastery != 0)
                countMasteries++;
            if (AirMastery != 0)
                countMasteries++;

            if (FireResistance != 0)
                countResistances++;
            if (WaterResistance != 0)
                countResistances++;
            if (EarthResistance != 0)
                countResistances++;
            if (AirResistance != 0)
                countResistances++;

            if (countMasteries == MasteriesElementsRequired && countResistances == ResistancesElementsRequired) {
                ConditionsRespected = true;
                return true;
            } else {
                ConditionsRespected = false;
                return false;
            }
        }
        
        /// <summary>
        /// Transmute the elements of masteries or resistances of the item.
        /// </summary>
        /// <param name="masteries">If true we will transmute the masteries, else the resistances.</param>
        /// <param name="fire">If true the fire element has been selected for the transmutation</param>
        /// <param name="water">If true the water element has been selected for the transmutation</param>
        /// <param name="earth">If true the earth element has been selected for the transmutation</param>
        /// <param name="air">If true the air element has been selected for the transmutation</param>
        public void TransmutateItemElements(bool masteries, bool fire, bool water, bool earth, bool air) {
            if (masteries) {
                // Transmute masteries
                if (fire)
                    FireMastery = ElemMasteriesValue;
                else
                    FireMastery = 0;
                if (water)
                    WaterMastery = ElemMasteriesValue;
                else
                    WaterMastery = 0;
                if (earth)
                    EarthMastery = ElemMasteriesValue;
                else
                    EarthMastery = 0;
                if (air)
                    AirMastery = ElemMasteriesValue;
                else
                    AirMastery = 0;
            } else {
                // Transmute resistances
                if (fire)
                    FireResistance = ElemResistancesValue;
                else
                    FireResistance = 0;
                if (water)
                    WaterResistance = ElemResistancesValue;
                else
                    WaterResistance = 0;
                if (earth)
                    EarthResistance = ElemResistancesValue;
                else
                    EarthResistance = 0;
                if (air)
                    AirResistance = ElemResistancesValue;
                else
                    AirResistance = 0;
            }
            // Verify the conditions after transmuting 
            ConditionsRespected = VerifyAllConditions();
        }

        /// <summary>
        /// Verify if the item is a dagger or a shield.
        /// <para>Return true if it's a dagger, false if it's a shield and null if it is neither one nor the other</para>
        /// </summary>
        /// <returns>Return true if it's a dagger, false if it's a shield and null if it is neither one nor the other</returns>
        public bool? IsDaggerOrShield() {
            if (IdType == DAO.Build.ID_SECOND_HAND) {
                foreach (Stat stat in StatList) {
                    if (GlobalConstants.IsIdOfMastery(stat.Id)) {
                        return true;
                    }
                }
                return false;
            }
            return null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WakEncyclopedie.BO;
using WakEncyclopedie.Utility;
using WakEncyclopedie.View;

namespace WakEncyclopedie.DAO {
    public class Build {
        #region constants
        private const int ID_HELMET = 1;
        private const int ID_CLOACK = 2;
        private const int ID_AMULET = 3;
        private const int ID_EPAULETTES = 4;
        private const int ID_BREASTPLATE = 5;
        private const int ID_BELT = 6;
        private const int ID_BOOTS = 7;
        public const int ID_RING = 8;
        private const int ID_TWO_HANDED_WEAPON = 9;
        private const int ID_ONE_HANDED_WEAPON = 10;
        private const int ID_SECOND_HAND = 11;
        private const int ID_EMBLEM = 12;
        private const int ID_PET = 13;
        private const int ID_MOUNT = 14;
        private const int ID_TORCHE = 15;
        private const int ID_TOOLS = 16;
        private const int RARITY_ID_RELIC = 5;
        private const int RARITY_ID_EPIC = 7;
        #endregion constants
        
        private int _levelBuild;

        private ImageSourceConverter Converter { get; set; }
        private UcEquipements BindedUcEquipements { get; set; }
        public UcSkillsManagement BindedUcSkillsManagement { get; private set; }
        public BuildStats BStats { get; private set; } = new BuildStats();
        public Skill BSkill { get; private set; } = new Skill();

        public int LevelBuild {
            get => _levelBuild;
            set {
                if (value < GlobalConstants.MIN_LEVEL) {
                    value = GlobalConstants.MIN_LEVEL;
                } else if (value > GlobalConstants.MAX_LEVEL) {
                    value = GlobalConstants.MAX_LEVEL;
                }
                if (_levelBuild != value) {
                    _levelBuild = value;
                    RemoveItemsWithTooHighLevel();
                    BSkill.ResetSkills();
                    BStats.CalculateBuildStats();
                }
            }
        }
        public EnchantedItem Helmet { get; private set; } = new EnchantedItem();
        public EnchantedItem Cloak { get; private set; } = new EnchantedItem();
        public EnchantedItem Amulet { get; private set; } = new EnchantedItem();
        public EnchantedItem Epaulettes { get; private set; } = new EnchantedItem();
        public EnchantedItem Breastplate { get; private set; } = new EnchantedItem();
        public EnchantedItem Belt { get; private set; } = new EnchantedItem();
        public EnchantedItem RingLeft { get; private set; } = new EnchantedItem();
        public EnchantedItem RingRight { get; private set; } = new EnchantedItem();
        public EnchantedItem Boots { get; private set; } = new EnchantedItem();
        public EnchantedItem TwoHandedWeapon { get; private set; } = new EnchantedItem();
        public EnchantedItem OneHandedWeapon { get; private set; } = new EnchantedItem();
        public EnchantedItem SecondHand { get; private set; } = new EnchantedItem();
        public EnchantedItem Accessory { get; private set; } = new EnchantedItem();
        public EnchantedItem Pet { get; private set; } = new EnchantedItem();
        public EnchantedItem Mount { get; private set; } = new EnchantedItem();

        /// <summary>
        /// Create a new build
        /// </summary>
        /// <param name="levelBuild">The level of the build</param>
        public Build(int levelBuild) {
            Converter = new ImageSourceConverter();
            BindedUcEquipements = new UcEquipements();
            BStats = new BuildStats(this);
            BSkill = new Skill(this);
            LevelBuild = levelBuild;
        }

        /// <summary>
        /// Create a new build who is binded to a UcEquipements
        /// </summary>
        /// <param name="levelBuild">The level of the build</param>
        /// <param name="bindedUcEquipements">UcEquipements that will be refresh when the build is updated</param>
        public Build(int levelBuild, UcEquipements bindedUcEquipements, UcSkillsManagement bindedUcSkillsManagement) : this(levelBuild) {
            BindedUcEquipements = bindedUcEquipements;
            BindedUcSkillsManagement = bindedUcSkillsManagement;
            BStats.CalculateBuildStats();
        }

        /// <summary>
        /// Get a list of the actual items of the build
        /// </summary>
        /// <returns></returns>
        public List<EnchantedItem> GetBuildItems() {
            List<EnchantedItem> items = new List<EnchantedItem> {
                Helmet,
                Cloak,
                Amulet,
                Epaulettes,
                Breastplate,
                Belt,
                RingLeft,
                RingRight,
                Boots,
                TwoHandedWeapon,
                OneHandedWeapon,
                SecondHand,
                Accessory,
                Pet,
                Mount
            };
            return items;
        }

        /// <summary>
        /// Search the item in the build that correspond to the id
        /// </summary>
        /// <param name="id">Id of the equiped item</param>
        /// <returns>The equiped item that correspond to the id</returns>
        public EnchantedItem GetEquipedItemById(int id) {
            return GetBuildItems().Find(x => x.Id == id);
        }

        /// <summary>
        /// Verify that the build don't have more than one epic and don't have more than one relic
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Return true if the item can be equiped, else return false</returns>
        private bool VerifyBuildConditionsForRarity(EnchantedItem item) {
            List<EnchantedItem> items = GetBuildItems();
            if (item.IdRarity == RARITY_ID_RELIC || item.IdRarity == RARITY_ID_EPIC) {
                // We can't equip the item if that will engender more thant one epic or one relic
                if (items.Any(x => x.IdRarity == item.IdRarity) && !items.Any(x => x.Id == item.Id)) {
                    return false;
                }
            }
            return true;
        }

        public void ResetBuild() {
            RemoveAllItem();
            BStats.ResetBuildStats();
            LevelBuild = GlobalConstants.MAX_LEVEL;
        }

        #region Equip methods
        /// <summary>
        /// Equip an item to the build
        /// </summary>
        /// <param name="item">The item to equip</param>
        /// <returns>Return true if the item has been correctly equiped, else return false</returns>
        public bool EquipItem(EnchantedItem item) {
            switch (item.IdType) {
                case ID_HELMET:
                    return EquipHelmet(item);
                case ID_CLOACK:
                    return EquipCloak(item);
                case ID_AMULET:
                    return EquipAmulet(item);
                case ID_EPAULETTES:
                    return EquipEpaulettes(item);
                case ID_BREASTPLATE:
                    return EquipBreastplate(item);
                case ID_BELT:
                    return EquipBelt(item);
                case ID_BOOTS:
                    return EquipBoots(item);
                case ID_TWO_HANDED_WEAPON:
                    return EquipTwoHandedWeapon(item);
                case ID_ONE_HANDED_WEAPON:
                    return EquipOneHandedWeapon(item);
                case ID_SECOND_HAND:
                    return EquipSecondHand(item);
                case ID_PET:
                    return EquipPet(item);
                case ID_MOUNT:
                    return EquipMount(item);
                case ID_EMBLEM:
                case ID_TORCHE:
                case ID_TOOLS:
                    return EquipAccessory(item);
                default:
                    Console.WriteLine("Unknown id type for the method EquipItem in Build");
                    return false;
            }
        }
        public void RemoveAllItem() {
            RemoveHelmet();
            RemoveCloak();
            RemoveAmulet();
            RemoveEpaulettes();
            RemoveBreastplate();
            RemoveBelt();
            RemoveBoots();
            RemoveRingLeft();
            RemoveRingRight();
            RemoveTwoHandedWeapon();
            RemoveOneHandedWeapon();
            RemoveSecondHand();
            RemovePet();
            RemoveMount();
            RemoveAccessory();
        }

        private void RemoveItemsWithTooHighLevel() {
            List<EnchantedItem> items = GetBuildItems();
            foreach (EnchantedItem item in items) {
                if (item.Level > LevelBuild) {
                    RemoveItem(item);
                }
            }
        }


        /// <summary>
        /// Remove an item equiped
        /// </summary>
        /// <param name="item">The item that we want to remove</param>
        public void RemoveItem(EnchantedItem item) {
            // remove the item that correspond to the type id
            switch (item.IdType) {
                case ID_HELMET:
                    RemoveHelmet();
                    break;
                case ID_CLOACK:
                    RemoveCloak();
                    break;
                case ID_AMULET:
                    RemoveAmulet();
                    break;
                case ID_EPAULETTES:
                    RemoveEpaulettes();
                    break;
                case ID_BREASTPLATE:
                    RemoveBreastplate();
                    break;
                case ID_BELT:
                    RemoveBelt();
                    break;
                case ID_BOOTS:
                    RemoveBoots();
                    break;
                case ID_TWO_HANDED_WEAPON:
                    RemoveTwoHandedWeapon();
                    break;
                case ID_ONE_HANDED_WEAPON:
                    RemoveOneHandedWeapon();
                    break;
                case ID_SECOND_HAND:
                    RemoveSecondHand();
                    break;
                case ID_PET:
                    RemovePet();
                    break;
                case ID_MOUNT:
                    RemoveMount();
                    break;
                case ID_EMBLEM:
                case ID_TORCHE:
                case ID_TOOLS:
                    RemoveAccessory();
                    break;
                default:
                    // If the type id is "unknown", then it's probably a ring. So we compare the id of the actuals ring to known wich ring remove
                    if (item.Id == RingLeft.Id) {
                        RemoveRingLeft();
                    } else if (item.Id == RingRight.Id) {
                        RemoveRingRight();
                    }
                    break;
            }
            RemoveItemsWithTooHighLevel();
        }

        private bool EquipHelmet(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_HELMET && VerifyBuildConditionsForRarity(item)) {
                Helmet = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgHelmet.Source = Tools.LoadImage(Helmet.Image);
                    BindedUcEquipements.ImgHelmet.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }

        private bool EquipCloak(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_CLOACK && VerifyBuildConditionsForRarity(item)) {
                Cloak = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgCloak.Source = Tools.LoadImage(Cloak.Image);
                    BindedUcEquipements.ImgCloak.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }

        private bool EquipAmulet(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_AMULET && VerifyBuildConditionsForRarity(item)) {
                Amulet = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgAmulet.Source = Tools.LoadImage(Amulet.Image);
                    BindedUcEquipements.ImgAmulet.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipEpaulettes(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_EPAULETTES && VerifyBuildConditionsForRarity(item)) {
                Epaulettes = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgEpaulettes.Source = Tools.LoadImage(Epaulettes.Image);
                    BindedUcEquipements.ImgEpaulettes.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipBreastplate(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_BREASTPLATE && VerifyBuildConditionsForRarity(item)) {
                Breastplate = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgBreastplate.Source = Tools.LoadImage(Breastplate.Image);
                    BindedUcEquipements.ImgBreastplate.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipBelt(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_BELT && VerifyBuildConditionsForRarity(item)) {
                Belt = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgBelt.Source = Tools.LoadImage(Belt.Image);
                    BindedUcEquipements.ImgBelt.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        public bool EquipRingLeft(EnchantedItem item) {
            // In more than the levels and type contraints, the ring must be unique in the build
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_RING && RingRight.Id != item.Id && VerifyBuildConditionsForRarity(item)) {
                RingLeft = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgRingLeft.Source = Tools.LoadImage(RingLeft.Image);
                    BindedUcEquipements.ImgRingLeft.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        public bool EquipRingRight(EnchantedItem item) {
            // In more than the levels and type contraints, the ring must be unique in the build
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_RING && RingLeft.Id != item.Id && VerifyBuildConditionsForRarity(item)) {
                RingRight = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgRingRight.Source = Tools.LoadImage(RingRight.Image);
                    BindedUcEquipements.ImgRingRight.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipBoots(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_BOOTS && VerifyBuildConditionsForRarity(item)) {
                Boots = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgBoots.Source = Tools.LoadImage(Boots.Image);
                    BindedUcEquipements.ImgBoots.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipTwoHandedWeapon(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_TWO_HANDED_WEAPON && VerifyBuildConditionsForRarity(item)) {
                TwoHandedWeapon = item;
                // we can't have an one handed and/or second hand weapons with a two handed weapon
                OneHandedWeapon = new EnchantedItem();
                SecondHand = new EnchantedItem();
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgHandLeft.Source = Tools.LoadImage(TwoHandedWeapon.Image);
                    BindedUcEquipements.ImgHandRight.Source = Tools.LoadImage(TwoHandedWeapon.Image);
                    BindedUcEquipements.ImgHandLeft.Tag = item.Id;
                    BindedUcEquipements.ImgHandRight.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipOneHandedWeapon(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_ONE_HANDED_WEAPON && VerifyBuildConditionsForRarity(item)) {
                OneHandedWeapon = item;
                // we can't have a two handed weapon with a one handed weapon
                TwoHandedWeapon = new EnchantedItem();
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgHandRight.Source = Tools.LoadImage(OneHandedWeapon.Image);
                    BindedUcEquipements.ImgHandRight.Tag = item.Id;
                    // if the second hand is not equiped then we have to remove the image
                    if (SecondHand.Id == 0) {
                        Uri uri = new Uri(GlobalConstants.SECOND_HAND_WEAPON_IMAGE_PATH);
                        BitmapImage bitmapImage = new BitmapImage(uri);
                        BindedUcEquipements.ImgHandLeft.Source = bitmapImage;
                        BindedUcEquipements.ImgHandLeft.Tag = null;
                    }
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipSecondHand(EnchantedItem item) {
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill && item.IdType == ID_SECOND_HAND && VerifyBuildConditionsForRarity(item)) {
                SecondHand = item;
                // we can't have a two handed weapon with a second hand weapon
                TwoHandedWeapon = new EnchantedItem();
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgHandLeft.Source = Tools.LoadImage(SecondHand.Image);
                    BindedUcEquipements.ImgHandLeft.Tag = item.Id;
                    // if the right hand is not equiped then we have to remove the image
                    if (OneHandedWeapon.Id == 0) {
                        Uri uri = new Uri(GlobalConstants.ONE_HANDED_WEAPON_IMAGE_PATH);
                        BitmapImage bitmapImage = new BitmapImage(uri);
                        BindedUcEquipements.ImgHandRight.Source = bitmapImage;
                        BindedUcEquipements.ImgHandRight.Tag = null;
                    }
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipAccessory(EnchantedItem item) {
            // An accessory can be an emblem, a tool or a torche
            if (item.Level <= LevelBuild + BStats.KitSkill + item.KitSkill
                && ((item.IdType == ID_EMBLEM) || (item.IdType == ID_TOOLS) || (item.IdType == ID_TORCHE))
                && VerifyBuildConditionsForRarity(item)) {
                Accessory = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgAccessory.Source = Tools.LoadImage(Accessory.Image);
                    BindedUcEquipements.ImgAccessory.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipPet(EnchantedItem item) {
            // Pets don't have level constraints
            if (item.IdType == ID_PET && VerifyBuildConditionsForRarity(item)) {
                Pet = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgPet.Source = Tools.LoadImage(Pet.Image);
                    BindedUcEquipements.ImgPet.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }
        private bool EquipMount(EnchantedItem item) {
            // Mounts don't have level constraints
            if (item.IdType == ID_MOUNT && VerifyBuildConditionsForRarity(item)) {
                Mount = item;
                if (BindedUcEquipements != null) {
                    BindedUcEquipements.ImgMount.Source = Tools.LoadImage(Mount.Image);
                    BindedUcEquipements.ImgMount.Tag = item.Id;
                }
                BStats.CalculateBuildStats();
                return true;
            } else {
                return false;
            }
        }

        private void RemoveHelmet() {
            Helmet = new EnchantedItem();
            BindedUcEquipements.ImgHelmet.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.HELMET_IMAGE_PATH);
            BindedUcEquipements.ImgHelmet.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveCloak() {
            Cloak = new EnchantedItem();
            BindedUcEquipements.ImgCloak.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.CLOAK_IMAGE_PATH);
            BindedUcEquipements.ImgCloak.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveAmulet() {
            Amulet = new EnchantedItem();
            BindedUcEquipements.ImgAmulet.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.AMULET_IMAGE_PATH);
            BindedUcEquipements.ImgAmulet.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveEpaulettes() {
            Epaulettes = new EnchantedItem();
            BindedUcEquipements.ImgEpaulettes.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.EPAULETTES_IMAGE_PATH);
            BindedUcEquipements.ImgEpaulettes.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveBreastplate() {
            Breastplate = new EnchantedItem();
            BindedUcEquipements.ImgBreastplate.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.BREASTPLATE_IMAGE_PATH);
            BindedUcEquipements.ImgBreastplate.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveBelt() {
            Belt = new EnchantedItem();
            BindedUcEquipements.ImgBelt.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.BELT_IMAGE_PATH);
            BindedUcEquipements.ImgBelt.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveBoots() {
            Boots = new EnchantedItem();
            BindedUcEquipements.ImgBoots.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.BOOTS_IMAGE_PATH);
            BindedUcEquipements.ImgBoots.Tag = null;
            BStats.CalculateBuildStats();
        }
        
        private void RemoveRingLeft() {
            RingLeft = new EnchantedItem();
            BindedUcEquipements.ImgRingLeft.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.RING_IMAGE_PATH);
            BindedUcEquipements.ImgRingLeft.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveRingRight() {
            RingRight = new EnchantedItem();
            BindedUcEquipements.ImgRingRight.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.RING_IMAGE_PATH);
            BindedUcEquipements.ImgRingRight.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveTwoHandedWeapon() {
            TwoHandedWeapon = new EnchantedItem();
            BindedUcEquipements.ImgHandLeft.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.SECOND_HAND_WEAPON_IMAGE_PATH);
            BindedUcEquipements.ImgHandLeft.Tag = null;
            BindedUcEquipements.ImgHandRight.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.ONE_HANDED_WEAPON_IMAGE_PATH);
            BindedUcEquipements.ImgHandRight.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveOneHandedWeapon() {
            OneHandedWeapon = new EnchantedItem();
            BindedUcEquipements.ImgHandRight.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.ONE_HANDED_WEAPON_IMAGE_PATH);
            BindedUcEquipements.ImgHandRight.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveSecondHand() {
            SecondHand = new EnchantedItem();
            BindedUcEquipements.ImgHandLeft.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.SECOND_HAND_WEAPON_IMAGE_PATH);
            BindedUcEquipements.ImgHandLeft.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemovePet() {
            Pet = new EnchantedItem();
            BindedUcEquipements.ImgPet.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.PET_IMAGE_PATH);
            BindedUcEquipements.ImgPet.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveMount() {
            Mount = new EnchantedItem();
            BindedUcEquipements.ImgMount.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.MOUNT_IMAGE_PATH);
            BindedUcEquipements.ImgMount.Tag = null;
            BStats.CalculateBuildStats();
        }

        private void RemoveAccessory() {
            Accessory = new EnchantedItem();
            BindedUcEquipements.ImgAccessory.Source = (ImageSource)Converter.ConvertFromString(GlobalConstants.EMBLEM_IMAGE_PATH);
            BindedUcEquipements.ImgAccessory.Tag = null;
            BStats.CalculateBuildStats();
        }
        #endregion Equip methods

    }
}

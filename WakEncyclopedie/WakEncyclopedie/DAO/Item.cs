using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WakEncyclopedie {
    public class Item {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public byte[] Image { get; set; }
        public string Url { get; set; }
        public int IdType { get; set; }
        public string Type { get; set; }
        public byte[] TypeImage { get; set; }
        public int IdRarity { get; set; }
        public string RarityName { get; set; }
        public byte[] RarityImage { get; set; }
        public List<Stat> StatList { get; set; }

        public Item() {
            StatList = new List<Stat>();
        }

        public Item(int id)
        {
            Id = id;
        }

        public Item(int id, string name, int lvl, byte[] itemImage, string url, int idType, string type, byte[] typeImage, int idRarity,  string rarity, byte[] rarityImage, List<Stat> statList) {
            Id = id;
            Name = name;
            Level = lvl;
            Image = itemImage;
            Url = url;
            IdType = IdType;
            Type = type;
            TypeImage = typeImage;
            IdRarity = IdRarity;
            RarityName = rarity;
            RarityImage = rarityImage;
            StatList = statList;
        }
    }
}

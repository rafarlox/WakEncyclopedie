using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakEncyclopedie.DAO
{
    public class Element
    {
        private int _id;
        private string _name;
        private byte[] _img;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public byte[] Img { get => _img; set => _img = value; }
        public bool IsSelected { get; set; }

        public Element()
        {

        }

        public Element(int id, string name, byte[] img)
        {
            Id = id;
            Name = name;
            Img = img;
            IsSelected = false;
        }


    }
}

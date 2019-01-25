using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakEncyclopedie {
    public class Stat {
        private int _id;
        private string _type;
        private int _value;

        public int Id { get => _id; set => _id = value; }
        public string Type { get => _type; set => _type = value; }
        public int Value { get => _value; set => _value = value; }

        public Stat() {

        }

        public Stat(string type, int value) {
            Type = type;
            Value = value;
        }

        public Stat(int id, int value) {
            Id = id;
            Value = value;
        }

        public Stat(int id, string type, int value) {
            Id = id;
            Type = type;
            Value = value;
        }
    }
}

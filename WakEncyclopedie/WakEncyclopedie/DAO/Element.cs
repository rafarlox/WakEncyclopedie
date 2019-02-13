using System.ComponentModel;

namespace WakEncyclopedie.DAO {
    public class Element : INotifyPropertyChanged {
        private int _id;
        private string _name;
        private byte[] _img;
        private bool _isSelected;

        public int Id {
            get {
                return _id;
            }
            set {
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public string Name {
            get {
                return _name;
            }
            set {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }
        public bool IsSelected {
            get {
                return _isSelected;
            }
            set {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }
        public byte[] Img {
            get {
                return _img;
            }
            set {
                _img = value;
                NotifyPropertyChanged("Img");
            }
        }

        public Element() {

        }

        public Element(string name) {
            Name = name;
        }

        public Element(int id, string name, byte[] img) {
            Id = id;
            Name = name;
            Img = img;
            IsSelected = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

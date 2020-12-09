using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlSample
{
    [Serializable]
    public class Person
    { 

        public string MName { get; set; }
        public string Job { get; set; }
        public string Sex { get; set; }
        public string Tel { get; set; }
        public bool TF { get; set; }
        //private bool mTF;
        //public bool TF 
        //{ 
        //    get{ return mTF; }
        //    set {
        //        mTF = value; 
        //        OnPropertyChanged("TF");
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void OnPropertyChanged(string propertyName)
        //{
        //    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}

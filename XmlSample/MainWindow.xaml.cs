using MLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XmlSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public string xmlPath = MFile.path4;
        public string xmlName = "person.xml";
        private Person mperson;
        public Person person
        {
            get
            {
                return mperson;
            }
            set
            {
                mperson = value;
                OnPropertyChanged("person");
            }
        }
        IXml xml = new IXml();
        public MainWindow()
        {
            DataContext =this;
            InitializeComponent();
            XmlLoad();
            Refresh(0);

        }
        public void XmlLoad()
        {
            try
            {
                person = (Person)xml.deserialize_from_xml(xmlPath, xmlName, typeof(Person));
  
            }
            catch (Exception)
            {
                person = new Person() { MName = "张三",Job="厨师",Sex="男", TF=true};
                xml.serialize_to_xml(xmlPath, xmlName, person);
            }
           
        }
        public void Refresh(int cmd)
        {
            if (cmd == 0)
            {
                person=(Person)xml.deserialize_from_xml(xmlPath, xmlName, typeof(Person));
                txName.Text = person.MName;
                txJob.Text = person.Job;
                txSex.Text = person.Sex;
                txTel.Text = person.Tel;
                txTF.Text = person.TF.ToString();

            }
            else
            {
                person.MName = txName.Text;
                person.Job = txJob.Text;
                person.Sex = txSex.Text;
                person.Tel = txTel.Text;
                person.TF = Convert.ToBoolean(txTF.Text);
                xml.serialize_to_xml(xmlPath, xmlName, person);

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Refresh(1);
            Refresh(0);
        }
    }


}

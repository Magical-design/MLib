using MLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.MemoryMappedFiles;
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


namespace IniSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            IniLoad();
            Refresh(0);
        }

        private string mName = "";
        public string MName
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
                Ini.WriteIniData("员工", "姓名", mName, iniPath);
            }
        }

        private string mJob = "";
        public string Job
        {
            get
            {
                return mJob;
            }
            set
            {
                mJob = value;
                Ini.WriteIniData("员工", "岗位", mJob, iniPath);
            }
        }

        private string mSex = "";
        public string Sex
        {
            get
            {
                return mSex;
            }
            set
            {
                mSex = value;
                Ini.WriteIniData("员工", "性别", mSex, iniPath);
            }
        }

        private string mTel = "";
        public string Tel
        {
            get
            {
                return mTel;
            }
            set
            {
                mTel = value;
                Ini.WriteIniData("员工", "电话", mTel, iniPath);
            }
        }

        private bool mTF = false ;
        public bool TF
        {
            get
            {
                return mTF;
            }
            set
            {
                mTF = value;
                Ini.WriteIniData("员工", "是否", mTF.ToString(), iniPath);
                OnPropertyChanged("TF");
            }
        }

        public string iniPath = MFile.path4 + "Parameter.ini";
        public void IniLoad()
        {
            Ini.IniFileCreate(iniPath);
            mName = Ini.ReadIniData("员工", "姓名", "张三", iniPath);
            mJob = Ini.ReadIniData("员工", "岗位", "123", iniPath);
            mSex = Ini.ReadIniData("员工", "性别", "男", iniPath);
            mTel = Ini.ReadIniData("员工", "电话", "12345678", iniPath);
            TF= Convert.ToBoolean(Ini.ReadIniData("员工", "是否", "True", iniPath));
        }

        public  void  Refresh(int cmd)
        {
            if (cmd==0)
            {
                txName.Text = MName;
                txJob.Text = Job;
                txSex.Text = Sex;
                txTel.Text = Tel;
                txTF.Text = TF.ToString();

            }
            else
            {
                MName = txName.Text;
                Job = txJob.Text;
                Sex = txSex.Text;
                Tel = txTel.Text;
                TF = Convert.ToBoolean(txTF.Text);
            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {

            Refresh(1);
            Refresh(0);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

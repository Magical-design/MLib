using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static OfficeOpenXml.ExcelErrorValue;

namespace MLib.View.IOCtr
{
    /// <summary>
    /// IOCtr.xaml 的交互逻辑
    /// </summary>
    public partial class IOCtr : UserControl
    {
        string template = "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType=\"Button\"> "
                + "<Border Name = \"border\" BorderThickness=\"{TemplateBinding BorderThickness}\" BorderBrush=\"{TemplateBinding BorderBrush}\" Background=\"{TemplateBinding Background}\"> "
                + "<ContentPresenter HorizontalAlignment = \"Center\" VerticalAlignment=\"Center\" /> "
                + "</Border>"
                + "<ControlTemplate.Triggers> "
                + "<Trigger Property = \"IsMouseOver\" Value=\"True\">"
                + "<Setter Property = \"Opacity\" Value=\"0.9\" />"
                + "<Setter Property = \"BorderThickness\" Value=\"1.5\" />"
                + "<Setter Property = \"BorderBrush\" Value=\"Blue\" />"
                + "</Trigger>"
                + "</ControlTemplate.Triggers>"
                + "</ControlTemplate>";
        //public Border bordSet=new Border() { BorderThickness =new Thickness(0.1), BorderBrush = Brushes.Black };
        //public Border bordRst = new Border() { BorderThickness = new Thickness(0.1), BorderBrush = Brushes.Black };
        public TextBox tbTitle = new TextBox() { MinWidth = 50, Height = 20, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true };
        public Button btY = new Button() { MinWidth = 50, Height = 20};
        public TextBox tbSet = new TextBox() { MinWidth = 50, Height = 20, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true };
        public TextBox tbRst = new TextBox() { MinWidth = 50, Height = 20, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true };
        public string yAddress = "";
        public string YAddress
        {
            get { return yAddress; }
            set
            {
                yAddress = value;
            }
        }

        private string setSensor = "";
        public string SetSensor
        {
            get { return setSensor; }
            set
            {
                setSensor = value;
            }
        }
        private string rstSensor = "";
        public string RstSensor
        {
            get { return rstSensor; }
            set
            {
                rstSensor = value;
            }
        }
        private string yOFFTitle = "";
        public string YOFFTitle
        {
            get { return yOFFTitle; }
            set
            {
                yOFFTitle = value;
            }
        }
        private string yONTitle = "";
        public string YONTitle
        {
            get { return yONTitle; }
            set
            {
                yONTitle = value;
            }
        }
        private bool yStatus = false;
        public bool YStatus
        {
            get { return yStatus; }
            set
            {
                yStatus = value;
                YStatusChanged(value);
            }
        }
        private void YStatusChanged(bool val)
        {
            if (val)
                btY.Background = Brushes.Green;
            else
                btY.Background = Brushes.Red;

            if (SetSensor != "" || RstSensor != "")
            {
                btY.Content = YAddress;
            }
            else if (val && YONTitle != "")
                btY.Content = YONTitle + "_" + YAddress;

            else
                btY.Content = YOFFTitle + "_" + YAddress;
        }
        private bool setSensorStatus = false;
        public bool SetSensorStatus
        {
            get { return setSensorStatus; }
            set
            {
                setSensorStatus = value;
                SetSensorStatusChanged(value);
            }
        }
        private void SetSensorStatusChanged(bool val)
        {
            if (val)
                tbSet.Background = Brushes.Green;
            else
                tbSet.Background = Brushes.Red;
            string s = "";
            if (YOFFTitle.Contains("气缸"))
                s = "_S";
            else if(YOFFTitle.Contains("真空"))
                s = "_负压";
            if (YAddress == "")
                tbSet.Text = YOFFTitle + "_" + SetSensor+s;
            else
                tbSet.Text = SetSensor+s;
            
        }
        private bool rstSensorStatus = false;
        public bool RstSensorStatus
        {
            get { return rstSensorStatus; }
            set
            {
                rstSensorStatus = value;
                RstSensorStatusChanged(value);
            }
        }
        private void RstSensorStatusChanged(bool val)
        {
            if (val)
                tbRst.Background = Brushes.Green;
            else
                tbRst.Background = Brushes.Red;
            string s = "";
            if (YOFFTitle.Contains("气缸"))
                s = "_R";
            tbRst.Text = RstSensor+s;
        }
        public IOCtr()
        {
            InitializeComponent();
            btY.Template = (ControlTemplate)XamlReader.Parse(template);
            btY.Click += (s, e) =>
            {
                //YStatus = !YStatus;
            };

            //bordSet.Child = tbSet;
            //bordRst.Child = tbRst;
        }

        public void PropertyChanged(string myAddress, string msetSensor, string mrstSensor, string myOFFTitle, string myONTitle)
        {
            if (YAddress != myAddress || SetSensor != msetSensor || RstSensor != mrstSensor || YOFFTitle != myOFFTitle || YONTitle != myONTitle)
            {
                YAddress = myAddress;
                SetSensor = msetSensor;
                RstSensor = mrstSensor;
                YOFFTitle = myOFFTitle;
                YONTitle = myONTitle;
                gdTitle.Children.Clear();
                ugdContent1.Children.Clear();
                ugdContent1.Columns = 0;
                if (YAddress != "")
                {
                    if (SetSensor != "" || RstSensor != "")
                    {
                        gdTitle.Children.Add(tbTitle);
                        tbTitle.Text = YOFFTitle;
                    }
                    
                    ugdContent1.Children.Add(btY);
                    ugdContent1.Columns++;
                    if (SetSensor != "")
                    {
                        ugdContent1.Children.Add(tbSet);
                        ugdContent1.Columns++;
                    }
                    if (RstSensor != "")
                    {
                        ugdContent1.Children.Add(tbRst);
                        ugdContent1.Columns++;
                    }
                }
                else
                {
                    if (SetSensor != "")
                    {
                        ugdContent1.Children.Add(tbSet);
                        ugdContent1.Columns++;
                    }
                }    
                YStatusChanged(YStatus);
                SetSensorStatusChanged(SetSensorStatus);
                RstSensorStatusChanged(RstSensorStatus);
            }
        }
    }
}

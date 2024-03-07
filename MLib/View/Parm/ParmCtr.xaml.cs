using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
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
using System.Windows.Markup;

namespace MLib.View.Parm
{

    /// <summary>
    /// ParmCtr.xaml 的交互逻辑
    /// </summary>
    public partial class ParmCtr : UserControl
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
        public TextBox tbTitle = new TextBox() { MinWidth = 50, MinHeight = 20, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true };
        public TextBox lb1 = new TextBox() { MinWidth = 20, MinHeight = 20, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true };
        public TextBox lb2 = new TextBox() { MinWidth = 20, MinHeight = 20, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center, IsReadOnly = true };
        public TextBox tbParm1 = new TextBox() { MinWidth = 50, MinHeight = 20, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        public TextBox tbParm2 = new TextBox() { MinWidth = 50, MinHeight = 20, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        public Button btM = new Button() { MinWidth = 50, MinHeight = 20 };
        private string add1 = "";
        public string CurText1 = "";
        public string CurText2 = "";
        public bool lck1 = false;
        public bool lck2 = false;
        public string Add1
        { 
            get { return add1; } 
            set { add1 = value; }
        }
        private string add2 = "";
        public string Add2
        {
            get { return add2; }
            set { add2 = value; }
        }

        
        public ParmCtr()
        {
            InitializeComponent();
            btM.Template = (ControlTemplate)XamlReader.Parse(template);
            tbParm1.Style = Resources["txbInErrorTip"] as Style;
            tbParm2.Style = Resources["txbInErrorTip"] as Style;
        }



        public string Scale1="";
        public string Scale2 = "";
        private string mOFFTitle = "";
        public string MOFFTitle
        {
            get { return mOFFTitle; }
            set
            {
                mOFFTitle = value;
            }
        }
        private string mONTitle = "";
        public string MONTitle
        {
            get { return mONTitle; }
            set
            {
                mONTitle = value;
            }
        }
        private bool mStatus = false;
        public bool MStatus
        {
            get { return mStatus; }
            set
            {
                mStatus = value;
                MStatusChanged(value);
            }
        }
        private void MStatusChanged(bool val)
        {
            if (val)
                btM.Background = Brushes.Green;
            else
                btM.Background = Brushes.Red;

           if (val && MONTitle != "")
                btM.Content = MONTitle ;

            else
                btM.Content = MOFFTitle;
        }
        
        public void PropertyChanged(string mTitle, string mAdd1, string mAdd2)
        {
            if (MOFFTitle!= mTitle || Add1!=mAdd1 || Add2!=mAdd2)
            {
                MOFFTitle=mTitle;
                Add1 = mAdd1;
                Add2 = mAdd2;
                gdTitle.Children.Clear();
                ugdContent1.Children.Clear();
                ugdContent1.Columns = 0;
                lb1.Text = "";
                lb2.Text = "";
                if (Add2 != "")
                {
                    if (Add2.Substring(0, 1) == "M")
                        return;
                    if (MOFFTitle != "")
                    {
                        gdTitle.Children.Add(tbTitle);
                        tbTitle.Text = MOFFTitle;
                    }
                    if(MOFFTitle.Contains("气缸"))
                    {
                        lb1.Text = "置位";
                        lb2.Text = "复位";
                    }
                    if (MOFFTitle.Contains("真空"))
                    {
                        lb1.Text = "吸";
                        lb2.Text = "吹";
                    }
                    if(lb1.Text!="")
                    {
                        ugdContent1.Children.Add(lb1);
                        ugdContent1.Columns++;
                    }
                       
                    if(Add1!="")
                    {
                        ugdContent1.Children.Add(tbParm1);
                        ugdContent1.Columns++;
                    }
                        
                    if (lb1.Text != "")
                    {
                        ugdContent1.Children.Add(lb2);
                        ugdContent1.Columns++;
                    }
                        
                    if (Add2 != "")
                    { 
                        ugdContent1.Children.Add(tbParm2);
                        ugdContent1.Columns++;
                    }
                        
                }
                else
                {
                    if (Add1.Substring(0, 1) == "M" || Add1.Substring(0, 1) == "Y" || Add1.Substring(1).Contains ("."))
                    {
                        ugdContent1.Children.Add(btM);
                        ugdContent1.Columns++;
                        MStatusChanged(MStatus) ;
                    }
                    else
                    {
                        if(MOFFTitle!="")
                        {
                            lb1.Text = MOFFTitle;
                            ugdContent1.Children.Add(lb1);
                            ugdContent1.Columns++;
                        }
                        if (Add1 != "")
                        {
                            ugdContent1.Children.Add(tbParm1);
                            ugdContent1.Columns++;
                        }
                    }

                        
                }

            }
        }
        public void PropertyChanged(string mTitle, string mAdd1, string mAdd2,bool isReadonly)
        {
            PropertyChanged(mTitle, mAdd1, mAdd2);
            if (isReadonly)
            {
                tbParm1.IsReadOnly = true;
                tbParm2.IsReadOnly = true;
                btM.IsEnabled = false;
            }
                

        }
    }
}

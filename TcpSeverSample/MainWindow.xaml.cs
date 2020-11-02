using System;
using System.Collections.Generic;
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
using MLib.NET;

namespace TcpSeverSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!Opened)
                tcpSever.Start();
            else
                tcpSever.Stop();
        }

        public ITcpSever tcpSever=null;
        private bool mOpened = false;
        public bool Opened
        {
            get
            {
                return mOpened;
            }
            set
            {
                if (mOpened != value)
                {
                    if (value)
                        btConnect.Content = "关闭";
                    else
                        btConnect.Content = "打开";
                }
                mOpened = value;
                if (mOpened)
                {
                    OpStu.Fill = new SolidColorBrush(Colors.Green);
                    //btSend.IsEnabled = true;
                }


                else
                {
                    OpStu.Fill = new SolidColorBrush(Colors.Red);
                    //btSend.IsEnabled = false;


                }


            }
        }
        private bool mConnect = false;
        public bool Connect
        {
            get
            {
                return mConnect;
            }
            set
            {

                mConnect = value;
                if (mConnect)
                {
                    CntStu.Fill = new SolidColorBrush(Colors.Green);
                    btSend.IsEnabled = true;
                }


                else
                {
                    CntStu.Fill = new SolidColorBrush(Colors.Red);
                    btSend.IsEnabled = false;


                }


            }
        }
        public void Init()
        {
            Opened= false;
            Connect = false;
            txPort.Text = "3000";
            tcpSever = new ITcpSever("127.0.0.1", int.Parse(txPort.Text));
            tcpSever.OpenStatusChanged += RefreshStu;
            tcpSever.ConnectStatusChange += RefreshConnect;
            tcpSever.AutoReceiveContent += Receive;
        }

        public void RefreshStu (object obj,bool e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Opened = e;
            }));
        }

        public void RefreshConnect(object obj, bool e)
        {
            Dispatcher.Invoke(new Action(() => { Connect = e; }));

        }
        public void Receive(object obj, string s)
        {
            Dispatcher.Invoke(new Action(() => {
                txRecContent.Text += s;

            }));

        }

        private void btSend_Click(object sender, RoutedEventArgs e)
        {
            tcpSever.Send(txSendContent.Text);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {

                tcpSever.Close();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }
    }
}

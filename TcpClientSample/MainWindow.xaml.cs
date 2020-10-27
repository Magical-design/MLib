using MLib.NET;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace TcpClientSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public ITcpClient iTcpClient =new ITcpClient();
        private bool mConnect = false;
        public bool Connect
        {
            get
            {
                return mConnect;
            }
            set
            {
                if(mConnect!=value)
                {
                    if (value)
                        btConnect.Content = "断开";
                    else
                        btConnect.Content = "连接";
                }
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

        public void RefreshConnect(object obj ,bool e)
        {
            Dispatcher.Invoke(new Action(() => { Connect = e; }));
           
        }
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            txIP.Text = "127.0.0.1";
            txPort.Text = "3000";
            iTcpClient.AutoConnect = true;
            Connect = false;
            iTcpClient.ConnectStatusChange += RefreshConnect;
            iTcpClient.AutoReceiveContent +=Receive;


        }

        private async void btConnect_ClickAsync(object sender, RoutedEventArgs e)
        {
            
            
            try
            {
                if (!Connect)
                {
                    iTcpClient.ip = txIP.Text;
                    iTcpClient.port = Int32.Parse(txPort.Text);
                    await iTcpClient._Connect();

                }
                else
                    iTcpClient.Close();


            }
            catch (System.Exception ex)
            {
                
                Console.WriteLine(ex);
            }
            
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void btSend_Click(object sender, RoutedEventArgs e)
        {
            iTcpClient.Send(txSendContent.Text);

            //stream.Close();
        }

        public  void Receive(object obj,string s)
        {
            Dispatcher.Invoke(new Action(() => {
                txRecContent.Text += s;

            }));

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {

                iTcpClient.Close();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

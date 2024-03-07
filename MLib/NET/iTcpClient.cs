using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MLib.NET
{
    public class ITcpClient
    {
        public string ip = "127.0.0.1";
        public int port = 0;
        private TcpClient tcpClient=null;
        public bool AutoConnect = false;
        public event EventHandler<bool> ConnectStatusChange;
        public event EventHandler<string> AutoReceiveContent;
        private bool connect = false;
        private bool connecting = false;
        private bool sendAndRecLock = false;
        private bool autoReceiving = false;
        private bool close = false;
        //private bool autoReceivingEn = false;
        private bool autoReceived = false;
        private bool autoReceiveEn = false;

        public bool AutoReceiveEn
        {
            get
            {
                return autoReceiveEn;
            }
            set
            {

                autoReceiveEn = value;
                if(autoReceiveEn)
                    AutoReceive();

            }
        }
        public bool Connect
        {
            get
            {
                return connect;
            }
            set
            {
                if (connect != value && value)
                    ;
                if (connect != value)
                {
                    if(ConnectStatusChange != null)
                    ConnectStatusChange(null, value);
                }
                connect = value;

            }
        }
        public ITcpClient()
        {
            tcpClient = new TcpClient();
            //AutoReceive();
        }
        public ITcpClient(string mip, int mport)
        {
            ip = mip;
            port = mport;
            tcpClient = new TcpClient(ip, port);
            //AutoReceive();
        }

        private object lck=new object();
        private int count = 0;
        private void CheckCnt()
        {
            
            try
            {

                if (!tcpClient.Client.Connected)
                    Trace.WriteLine("Connect=false");
                // Dispatcher.CurrentDispatcher.Invoke(new Action(() => {    
                bool c= !((tcpClient.Client.Poll(1000, SelectMode.SelectRead) && (tcpClient.Client.Available == 0)) || !tcpClient.Client.Connected);
                if (c)
                {
                    count = 0;
                    Connect = c;
                }
                else if(count<10)
                    count++;
                else
                    Connect = c;
                //Trace.WriteLine("1" + tcpClient.Client.Poll(1000, SelectMode.SelectRead));
                //Trace.WriteLine("2" + (tcpClient.Client.Available == 0));
                //Trace.WriteLine("3" + !tcpClient.Client.Connected);

           // }));
            }
            catch (Exception ex)
            {
                Connect = false;
                Trace.WriteLine(ex);
                tcpClient.Close();
                tcpClient = new TcpClient();
            }

        }

        public void Send(string s)
        {
            if (Connect)
            {
                try
                {
                    Byte[] sedData = System.Text.Encoding.ASCII.GetBytes(s);
                    NetworkStream stream = tcpClient.GetStream();
                    stream.Write(sedData, 0, sedData.Length);
                }
                catch (Exception ex)
                {

                    Trace.WriteLine(ex);
                }
            }


        }

        public async Task SendAsy(string s)
        {
            await Task.Run(() =>
            {

                Send(s);
            });

        }

        public async Task<string> SendAndRecAsy(string s)
        {
            sendAndRecLock = true;
            await Task.Run(()=>{ while (autoReceiving) { Thread.Sleep(20); } ; });               
            await SendAsy(s);
            string rs = await RecAsync();
            sendAndRecLock = false;
            return rs;

        }



        public async Task<string> RecAsync()
        {
            try
            {

                if (Connect)
                {
                    byte[] recData = new Byte[256];
                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    NetworkStream stream = tcpClient.GetStream();
                    stream.ReadTimeout = 200;
                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = await stream.ReadAsync(recData, 0, recData.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(recData, 0, bytes);
                    return responseData;

                }
                else
                    return "未连接";

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return ex.ToString();
            }
        }
        public string Rec()
        {
            try
            {

                if (Connect)
                {
                    byte[] recData = new Byte[256];
                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    NetworkStream stream = tcpClient.GetStream();
                    stream.ReadTimeout = 200;
                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes =  stream.Read(recData, 0, recData.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(recData, 0, bytes);
                    return responseData;
                }
                else
                    return "未连接";

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return ex.ToString();
            }
        }
        private async void AutoReceive()
        {
            await Task.Run(() => { while (autoReceived || !close) Thread.Sleep(20); });
            autoReceived = true;
            await Task.Run(() =>
            {
                while (AutoReceiveEn)
                {
                    try
                    {
                        CheckCnt();
                        if (!sendAndRecLock && Connect)
                        {
                            if(tcpClient.Available<=0)
                            {
                                Thread.Sleep(30);
                                continue;
                            }
                            autoReceiving = true;
                            byte[] recData = new Byte[256];
                            // String to store the response ASCII representation.
                            String responseData = String.Empty;

                            NetworkStream stream = tcpClient.GetStream();
                            stream.ReadTimeout = 10000;
                            // Read the first batch of the TcpServer response bytes.
                            Int32 bytes = stream.Read(recData, 0, recData.Length);
                            //responseData = System.Text.Encoding.ASCII.GetString(recData, 0, bytes);
                            responseData = System.Text.Encoding.Default.GetString(recData, 0, bytes);
                            autoReceiving = false;
                            AutoReceiveContent?.Invoke(null, responseData);
                        }
                    }
                    catch (Exception ex)
                    {
                        autoReceiving = false;
                        Thread.Sleep(200);
                        Trace.WriteLine(ex);
                    }
                    

                }


            });
            autoReceiving = false;
            autoReceived = false;
        }
        public async  void _Connect()
        {
            await Task.Run(() => { while (close) Thread.Sleep(20) ; });
            if (connecting)
                return;
            else
            {
                await Task.Run(() =>
                {

                    do
                    {
                        CheckCnt(); 
                        if (!Connect)
                        {
                            
                            try
                            {
                                connecting = true;
                                tcpClient.Connect(ip, port);
                                CheckCnt();
                            }
                            catch (Exception ex)
                            {
                                tcpClient.Close();
                                
                                tcpClient = new TcpClient();
                                Trace.WriteLine(ex);
                            }
                        }
                        if(!Connect)
                            Thread.Sleep(1500);
                        else
                            Thread.Sleep(200);
                    } while (AutoConnect && !close);
                    connecting = false;
                });
            }
        }

        public void Close()
        {
            try
            {
                tcpClient.Close();
                tcpClient = new TcpClient();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            try
            {
                Connect = false;
                close = true;
                while (connecting) Thread.Sleep(50); 
                //while (autoReceivingEn) Thread.Sleep(50); 
                Connect = false;
                close = false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
        public async void CloseAsy()
        {
            await Task.Run(() =>{
                Close();
            });
        }

    }
}

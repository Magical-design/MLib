using System;
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
        public TcpClient tcpClient;
        public bool AutoConnect = false;
        public event EventHandler<bool> ConnectStatusChange;
        public event EventHandler<string> AutoReceiveContent;
        private bool connect = false;
        private bool connecting = false;
        private bool sendAndRecLock = false;
        private bool autoReceiving = false;

        
        public bool Connect
        {
            get
            {
                return connect;
            }
            set
            {
                if (connect != value && ConnectStatusChange!=null)
                {
                    if(!value)
                    {
                        try
                        {
                            tcpClient.Close();
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);

                        }
                        tcpClient = new TcpClient();
                    }
                    ConnectStatusChange(null, value);
                }
                    
  
            connect = value;

            }
        }
        public ITcpClient()
        {
            tcpClient = new TcpClient();
            AutoReceive();
        }
        public ITcpClient(string mip, int mport)
        {
            ip = mip;
            port = mport;
            tcpClient = new TcpClient(ip, port);
            AutoReceive();
        }

        public object lck=new object();
        public void CheckCnt()
        {
            
            try
            {

           // Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                Connect = !((tcpClient.Client.Poll(1000, SelectMode.SelectRead) && (tcpClient.Client.Available == 0)) || !tcpClient.Client.Connected);
           // }));


            }
            catch (Exception ex)
            {
                Connect = false;
                Console.WriteLine(ex);

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

                    Console.WriteLine(ex);
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
            await Task.Run(()=>{ while (autoReceiving) { Thread.Sleep(50); } ; });               
            await SendAsy(s);
            string rs = await RecAsync();
            sendAndRecLock = false;
            return rs;

        }



        private async Task<string> RecAsync()
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

                return ex.ToString();
            }
        }

        public async void AutoReceive()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try

                    {
                        CheckCnt();
                        if (!sendAndRecLock && Connect)
                        {
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
                        Console.WriteLine(ex);
                    }
                    

                }


            });

        }
        public async  Task _Connect()
        {
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
                                Console.WriteLine(ex);
                            }

                        }
                        if (!Connect)
                            Thread.Sleep(1500);
                        else
                            Thread.Sleep(200);

                    } while (AutoConnect);
                    connecting = false;


                });

            }


        }

        public void Close()
        {

            try
            {
                Connect = false;

            }
            catch (Exception ex)
            {
                tcpClient.Close();
                tcpClient = new TcpClient();
                Console.WriteLine(ex);
            }
        }

    }
}

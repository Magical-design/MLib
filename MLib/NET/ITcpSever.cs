using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MLib.NET
{
    public class ITcpSever
    {
        public string ip = "127.0.0.1";
        public int port = 0;

        public TcpClient tcpClient;
        public TcpListener tcpListener;
        public event EventHandler<bool> ConnectStatusChange;
        public event EventHandler<bool> OpenStatusChanged;
        public event EventHandler<string> AutoReceiveContent;
        private bool connect = false;
        private bool opened = false;
        public bool Opened
        {
            get
            {
                return opened;
            }
            set
            {
                if (opened != value)
                {
                    OpenStatusChanged?.Invoke(null, value);
                }
                
                opened = value;
                if (!opened)
                    Connect = false;
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
                if (connect != value && ConnectStatusChange != null)
                {
                    ConnectStatusChange(null, value);
                }
                connect = value;
            }
        }

        public ITcpSever(string mip, int mport)
        {
            ip = mip;
            port = mport;
            tcpListener = new TcpListener(IPAddress.Parse(ip), port);
            AutoReceive();
        }

        public void Start()
        {

            try
            {

                tcpListener.Start();
                Opened = true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        public void Stop()
        {
            try
            {
                Opened = false;

                tcpListener.Stop();
                try
                {
                    tcpClient.Close();

                }
                catch (Exception)
                {

               
                }
                tcpClient = null;



            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }
        public void CheckCnt()
        {
            if(Opened)
            {
                if(tcpClient!=null)
                    Connect = !((tcpClient.Client.Poll(1000, SelectMode.SelectRead) && (tcpClient.Client.Available == 0)) || !tcpClient.Client.Connected);

                if (!Connect && tcpListener.Pending())
                {
                        tcpClient = tcpListener.AcceptTcpClient();
                        Connect = tcpClient.Connected;

                }
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
        public async void AutoReceive()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try

                    {
                        CheckCnt();
                        if (Connect && Opened)
                        {
                            byte[] recData = new Byte[256];
                            // String to store the response ASCII representation.
                            String responseData = String.Empty;

                            NetworkStream stream = tcpClient.GetStream();
                            stream.ReadTimeout = 10000;
                            // Read the first batch of the TcpServer response bytes.
                            Int32 bytes = stream.Read(recData, 0, recData.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(recData, 0, bytes);
                            AutoReceiveContent?.Invoke(null, responseData);
                        }

                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(200);
                        Console.WriteLine(ex);
                    }


                }


            });

        }

        public void Close()
        {

            try
            {
                Stop();
                tcpListener = null;


            }
            catch (Exception ex )
            {
                Console.WriteLine(ex);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MLib.NET
{
    public class IUdpClient
    {
        public string localIP = "127.0.0.1";
        public string remoteIP = "127.0.0.1";
        public int localPort = 0;
        public int remotePort = 0;
        public event EventHandler<bool> ConnectStatusChange;
        public event EventHandler<bool> OpenStatusChanged;
        public event EventHandler<string> AutoReceiveContent;
        private bool connect = false;
        private bool opened = false;
        private UdpClient udpClient;
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



        public IUdpClient()
        {
            AutoReceive();
        }

        public void Start()
        {
            try
            {
                IPEndPoint LocalIpEndPoint = new IPEndPoint(IPAddress.Parse(localIP), localPort);
                udpClient = new UdpClient(LocalIpEndPoint);
                //udpClient.Connect();
                Opened = true;
            }
            catch (Exception ex)
            {
                Opened = false;
                Console.WriteLine(ex);
            }
            
        }

        public void Stop()
        {

            try
            {
                Opened = false;
                udpClient.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        private async void AutoReceive()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try

                    {
                        if (Opened)
                        {
                            udpClient.Client.ReceiveTimeout = 10000;
                            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);

                            string returnData = Encoding.ASCII.GetString(receiveBytes);
                            AutoReceiveContent?.Invoke(null, returnData);
                        }
                        else
                            Thread.Sleep(200);

                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(200);
                        Console.WriteLine(ex);
                    }


                }


            });

        }

        public void Send(string s)
        {
            try
            {
                if(Opened)
                {
                        Byte[] sendBytes = Encoding.ASCII.GetBytes(s);

                        udpClient.Send(sendBytes, sendBytes.Length,remoteIP,remotePort);
                    
                        Connect = true;
                }
            }
            catch (Exception ex)
            {
                Connect = false;
                Console.WriteLine(ex);
            }

        }

        public async Task SendAsync(string s)
        {
            try
            {
                if (Opened)
                {
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(s);

                    await udpClient.SendAsync(sendBytes, sendBytes.Length, remoteIP, remotePort);

                    Connect = true;
                }
            }
            catch (Exception ex)
            {
                Connect = false;
                Console.WriteLine(ex);
            }

        }
    }
}

using IMAS.Core.Parser.VMF.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace VMF_Viewer
{
    public class Multicast
    {
        public IPAddress ipLocal;
        public IPAddress mCastAddress;
        public int Port;
        public bool isRunning = false;
        public bool DEBUG = true;
        public Thread mCastThread;
        public static Multicast instance;
        public Controller.Mode multicastType;

        public Multicast(string ipLocal, string mCastAddress, string Port, Controller.Mode multicastType)
        {
            try
            {
                this.ipLocal = IPAddress.Parse(ipLocal);
                this.mCastAddress = IPAddress.Parse(mCastAddress);
                this.Port = int.Parse(Port);
                this.multicastType = multicastType;
                if (instance == null)
                {
                    instance = this;
                }
            }
            catch (Exception e) { Console.instance?.Write(e.Message); }        
        }

        public void StartMulticast()
        {
            this.isRunning = true;
            if (mCastThread == null)
            {
                if (this.multicastType == Controller.Mode.Reciever)
                {
                    mCastThread = new Thread(MulticastRecieverThread);
                    mCastThread.Start();
                } 
            }            
        }

        public void StopMulticast()
        {
            this.isRunning = false;
        }

        public void SendVMFMessage(byte[] message)
        {
            using (var udpClient = new UdpClient(AddressFamily.InterNetwork))
            {
                var address = this.mCastAddress;
                var ipEndPoint = new IPEndPoint(address, this.Port);
                udpClient.JoinMulticastGroup(address);
                udpClient.Send(message, message.Length, ipEndPoint);
                udpClient.Close();
            }
        }

        private void MulticastRecieverThread()
        {            
            Console.instance?.Write("Interface: Bound [{0}] Port [{1}]", this.ipLocal.ToString(), this.Port.ToString());
            Socket mcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mcSocket.Bind(new IPEndPoint(this.ipLocal, this.Port));   //interface to bind to

            //join multicast group
            Console.instance?.UIWorker.RunWorkerAsync(String.Format("Multicast Join: Group [{0}]", this.mCastAddress.ToString()));            
            mcSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(this.mCastAddress, ipLocal));

            IPEndPoint iep = new IPEndPoint(ipLocal, 0);
            EndPoint ep = (EndPoint)iep;
            byte[] Buffer = new byte[65536];

            mcSocket.ReceiveTimeout = 10000;            

            //Console.instance?.UIWorker.RunWorkerAsync("Listening for packets...");
            while (this.isRunning)
            {
                try
                {
                    int rc = mcSocket.ReceiveFrom(Buffer, ref ep);
                    VMF.instance.ParseVMF(Buffer);
                    /*
                    MilStd6017 parser = new MilStd6017(Buffer);                    
                    List<object> resultList = parser.Parse();

                    if (resultList != null && resultList.Count > 0)
                    {
                        //DEBUG: Log Complete Message
                        if (this.DEBUG)
                        {
                            Console.instance?.UIWorker.RunWorkerAsync(String.Format("(" + mcSocket.ProtocolType + ") " + ipLocal.ToString() + ":" + Port.ToString(),
                                ep.ToString(),
                                this.GetType().Name,
                                VMFParserHelper.Dump(resultList),
                                DateTimeOffset.Now.ToString()));
                        }


                        //read header messages to determine message types contained
                        //foreach (MilStd47001_Message m in parser.GetHeaderData().Messages)
                        //{
                        //    Console.WriteLine("FAD: " + m.FunctionAreaDesignator.ToString() + " MsgNum: " + m.MessageNumber.ToString());
                        //    if (m.MessageStandardVersion == 5 || m.MessageStandardVersion == 6 || m.MessageStandardVersion == 7)
                        //    {
                        //        if (m.FunctionAreaDesignator == 7)
                        //        {
                        //            Console.WriteLine();
                        //        }
                        //    }
                        //}

                        foreach (object o in resultList)
                        {
                            if (o is K0501Data)
                            {
                                //Console.instance?.Write(String.Format("Received VMF K0501: URN [{0}]", ((K0501Data)o).Urn.ToString()));
                                ProcessK0501((K0501Data)o, parser.GetHeaderData());
                            }
                            if (o is K0701Data)
                            {
                                //Console.instance?.Write(String.Format("Received VMF K0701: URN [{0}], DEST_URN [{@1}]", ((K0701Data)o).Urn.ToString(), parser.GetHeaderData().RecipientAddress));
                                //ProcessK0701((K0701Data)o, parser.GetHeaderData());
                            }
                        }
                    }    */                
                }
                catch (Exception e)
                {
                    //Console.instance?.UIWorker.RunWorkerAsync(e.Message);
                }
            }

            mcSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership,
            new MulticastOption(this.mCastAddress, ipLocal));
            Console.instance?.UIWorker.RunWorkerAsync("Dropped MulticastMembership");
            mcSocket.Close();
            mCastThread = null;
        }

        private void ProcessK0501(K0501Data msg, MilStd47001Data hdr)
        {            
            var Latitude = msg.UnitLatitude.ToString();    //n281_402_1
            var Longitude = msg.UnitLongitude.ToString();  //n282_402_1
            var URN = msg.Urn.ToString();                  //n4004_12_1
            var ReceivedTime = DateTime.UtcNow;

            Console.instance?.Write(String.Format("Sending Position Report. URN [{0}] LAT [{1}] LNG [{2}]", URN, Latitude, Longitude));
        }
    }

    public static class VMFParserHelper
    {
        public static string Dump<T>(this T x)
        {
            return JsonConvert.SerializeObject(x, Formatting.Indented);
        }

        public static T Encode<T>(string x)
        {
            return JsonConvert.DeserializeObject<T>(x);
        }
    }
}

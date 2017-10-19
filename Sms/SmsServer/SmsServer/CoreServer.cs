using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;
using System.Drawing;

namespace SmsServer
{
    public class CoreServer
    {
        ComandiServer ObjCmndSrv;
        public List<TcpClient> ListaClient = new List<TcpClient>();
        public int lastClient = 0;

        public CoreServer()
        {
            ObjCmndSrv = new ComandiServer(this);
            AscoltoServer();
        }

        public void AscoltoServer()
        {
            Console.Clear();
            Console.Title = "Socket Multi Thread - Server - Numeri";
            Console.WriteAscii("Sms Server", Color.SkyBlue);
            Console.WriteLine("Inserire l'indirizzo ip e porta su cui mettersi in ascolto: ");
            string[] ip = Console.ReadLine().Split(':');
            TcpListener objTcpListener = new TcpListener(IPAddress.Parse(ip[0]), Convert.ToInt16(ip[1]));
            objTcpListener.Start();

            Thread thrdCmndSrvr = new Thread(ObjCmndSrv.ListenerCommands);
            thrdCmndSrvr.Start();
            Console.Clear();
            Console.WriteLine("Server avviato[" + ip[0] + ":" + ip[1] + "]", Color.Yellow);
            while (true)
            {
                TcpClient objTcpClient = objTcpListener.AcceptTcpClient();
                if (ObjCmndSrv.LetturaIpBloccati(objTcpClient.Client.RemoteEndPoint.ToString()))
                {
                    InvioUnClient(objTcpClient, "Non puoi accedere al server in quanto presente nella blacklist!");
                    objTcpClient.Close();
                }
                else
                {
                    ListaClient.Add(objTcpClient);
                    //ListaClient.OrderBy(client => client.Client.RemoteEndPoint.ToString().ToList());
                    Console.WriteLine(DateTime.Now + " Client: " + objTcpClient.Client.RemoteEndPoint.ToString() + " connesso.", Color.LightGreen);

                    Thread objThread = new Thread(GestioneClient);
                    objThread.Start(objTcpClient);
                    if (lastClient != ListaClient.Count)
                    {
                        lastClient = ListaClient.Count;
                        InvioListaClientConnessi();
                    }
                }
            }
        }

        public void GestioneClient(object _listaClient)
        {
            TcpClient objTcpClient = (TcpClient)_listaClient;
            Byte[] datiRicevuti = new Byte[256];
            int datiInArrivo = -1;
            while ((datiInArrivo != 0) && (objTcpClient.Connected))
            {
                datiInArrivo = 0;
                NetworkStream objNetworkStream = objTcpClient.GetStream();

                try
                {
                    datiRicevuti = new Byte[256];
                    datiInArrivo = objNetworkStream.Read(datiRicevuti, 0, datiRicevuti.Length);
                    Elabora(Encoding.ASCII.GetString(datiRicevuti).NoZero());
                }
                catch (Exception ecc) { }
                finally
                {
                    try
                    {
                        if (datiInArrivo == 0)
                        {
                            Console.WriteLine(DateTime.Now + " " + objTcpClient.Client.RemoteEndPoint.ToString() + " si è disconnesso.", Color.IndianRed);
                            ListaClient.Remove(objTcpClient);
                            InvioTuttiClient("!/rc" + objTcpClient.Client.RemoteEndPoint.ToString());//Comando per rimuovere 1 client
                        }
                    }
                    catch (Exception ecc) { }
                }
            }
        }
        //!/lc ottiene i client, !/cp chat privata con 1 client
        public void Elabora(string _testo)
        {
            int endIp;

            if (_testo.Length > 2)
            {
                if ((_testo[0] == '!') && (_testo[1] == '/'))
                {
                    if ((_testo[2] == 'l') && (_testo[3] == 'c'))//InviaTuttiIclientConnessi
                    {
                        //InvioUnClient(ListaClient.Last(), _testo);
                        InvioTuttiClient(_testo);
                    }
                    if ((_testo[2] == 'c') && (_testo[3] == 'p'))//InstauraUnaChatPrivataCon1Client
                    {
                        endIp = _testo.IndexOf('|');
                        string ipDestinatario = _testo.Substring(4, endIp - 4);
                        int endIpMittente = _testo.IndexOf('@');
                        string ipMittente = _testo.Substring(endIp + 1, endIpMittente - (endIp + 1));
                        string testoSenzaComandi = _testo.Substring(endIpMittente + 1, _testo.Length - (endIpMittente + 1));
                        InvioUnClient(CercaClient(ipDestinatario), "!/cp" + ipDestinatario + "|" + ipMittente + "@" + DateTime.Now.ToString("hh:mm:ss") + ": " + testoSenzaComandi);
                        InvioUnClient(CercaClient(ipMittente), "!/cp" + ipMittente + "|" + ipDestinatario + "@" + DateTime.Now.ToString("hh:mm:ss") + ": " + testoSenzaComandi);
                        Console.WriteLine(DateTime.Now + " Chat Privata tra " + ipMittente + " e " + ipDestinatario + " mex: " + testoSenzaComandi);
                    }
                }
                else
                {
                    endIp = _testo.IndexOf(':', _testo.IndexOf(':') + 1);
                    InvioTuttiClient(DateTime.Now.ToString("hh:mm:ss") + " " + _testo);
                    Console.WriteLine(DateTime.Now + " Chat pubblica " + _testo);
                }
            }
            else
            {
                endIp = _testo.IndexOf(':', _testo.IndexOf(':') + 1);
                InvioTuttiClient(DateTime.Now.ToString("hh:mm:ss") + " " + _testo);
                Console.WriteLine(DateTime.Now + " Chat pubblica " + _testo);
            }
        }

        public TcpClient CercaClient(string _ipDestinatario)
        {
            return ListaClient.Find(clnt => clnt.Client.RemoteEndPoint.ToString() == _ipDestinatario);
        }

        public void InvioListaClientConnessi()
        {
            try
            {
                Byte[] datiInviati = new Byte[256];
                string appo = "!/lc";
                for (int i = 0; i < ListaClient.Count; i++)
                {
                    if (i != ListaClient.Count - 1)
                        appo += ListaClient[i].Client.RemoteEndPoint.ToString() + "|";
                    else
                        appo += ListaClient[i].Client.RemoteEndPoint.ToString();
                }
                datiInviati = Encoding.ASCII.GetBytes(appo);
                foreach (TcpClient client in ListaClient)
                {
                    NetworkStream objNetworkStream = client.GetStream();
                    objNetworkStream.Write(datiInviati, 0, datiInviati.Length);
                }
            }
            catch (Exception ecc) { }
        }

        public void InvioUnClient(TcpClient _objTcpClient, string _testo)
        {
            try
            {
                Byte[] datiInviati = new Byte[256];
                datiInviati = Encoding.ASCII.GetBytes(_testo);

                foreach (TcpClient client in ListaClient)
                {
                    if (client == _objTcpClient)
                    {
                        NetworkStream objNetworkStream = client.GetStream();
                        objNetworkStream.Write(datiInviati, 0, datiInviati.Length);
                    }
                }
            }
            catch (Exception ecc) { }
        }

        public void InvioTuttiTranneUnClient(TcpClient _objTcpClient, string _testo)
        {
            try
            {
                Byte[] datiInviati = new Byte[256];
                datiInviati = Encoding.ASCII.GetBytes(_testo);
                foreach (TcpClient client in ListaClient)
                {
                    if (client != _objTcpClient)
                    {
                        NetworkStream objNetworkStream = client.GetStream();
                        objNetworkStream.Write(datiInviati, 0, datiInviati.Length);
                    }
                }
            }
            catch (Exception ecc) { }
        }

        public void InvioTuttiClient(string _testo)
        {
            try
            {
                Byte[] datiInviati = new Byte[256];
                datiInviati = Encoding.ASCII.GetBytes(_testo);
                foreach (TcpClient client in ListaClient)
                {
                    NetworkStream objNetworkStream = client.GetStream();
                    objNetworkStream.Write(datiInviati, 0, datiInviati.Length);
                }
            }
            catch (Exception ecc) { }
        }
    }
}

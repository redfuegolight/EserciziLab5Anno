using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using Console = Colorful.Console;
using System.Drawing;

namespace SmsServer
{
    public class ComandiServer
    {
        private static string percorsoLog = AppDomain.CurrentDomain.BaseDirectory + "Blacklist.txt";
        CoreServer ObjCoreServer;

        public ComandiServer(CoreServer _coreSrv)
        {
            ObjCoreServer = _coreSrv;
        }

        public void ControllaClientConnesso(string _ip)
        {
            int posClient = ObjCoreServer.ListaClient.FindIndex(x => x.Client.RemoteEndPoint.ToString() == _ip);
            if(posClient != -1)
            {
                TcpClient clientDaBannare = ObjCoreServer.ListaClient[posClient];
                ObjCoreServer.InvioUnClient(clientDaBannare, "Sei stato inserito nella blacklist!");
                ObjCoreServer.InvioTuttiClient("!/rc" + clientDaBannare.Client.RemoteEndPoint.ToString());
                clientDaBannare.Close();
                ObjCoreServer.ListaClient.RemoveAt(posClient);
                Console.WriteLine("Live ban di " + _ip + " effettuato con successo");
            }
        }

        public void ListenerCommands()
        {
            string comando = "";

            while (true)
            {
                comando = Console.ReadLine();

                if (comando.Contains("!/"))
                {
                    string[] comandoEcontenuti = comando.Split('|');
                    switch (comandoEcontenuti[0])
                    {
                        case "!/ban":
                            InsertIpIntoBlackList(comandoEcontenuti[1]);
                            ControllaClientConnesso(comandoEcontenuti[1]);
                            break;
                        case "!/unban":
                            RemoveBlackListedIp(comandoEcontenuti[1]);
                            break;
                        case "!/listaClient":
                            ListaClientConnessi();
                            break;
                    }
                    comando = "";
                }
            }
        }

        public void ListaClientConnessi()
        {
            Console.WriteAscii("Client connessi", Color.SteelBlue);
            foreach (TcpClient clnt in ObjCoreServer.ListaClient)
            {
                Console.WriteLine(clnt.Client.RemoteEndPoint.ToString());
            }
            Console.WriteAscii("--------------", Color.SteelBlue);
        }

        public void RemoveBlackListedIp(string _ipDaRimuovere)
        {
            if (!File.Exists("Blacklist.txt"))
                File.Create("Blacklist.txt");

            string tempFile = Path.GetTempFileName();
            var linesToKeep = File.ReadLines(percorsoLog).Where(l => l != _ipDaRimuovere);

            File.WriteAllLines(tempFile, linesToKeep);
            File.Delete(percorsoLog);
            File.Move(tempFile, percorsoLog);

            Console.WriteLine(_ipDaRimuovere + " rimosso dalla blacklist!");
        }

        public void InsertIpIntoBlackList(string _ipDaBloccare)
        {
            if (!File.Exists("Blacklist.txt"))
                File.Create("Blacklist.txt");

            TextWriter file = new StreamWriter(percorsoLog, true);
            file.WriteLine(_ipDaBloccare);
            file.Close();
            Console.WriteLine(_ipDaBloccare + " aggiunto alla blacklist!");
        }

        public bool LetturaIpBloccati(string _ipDaControllare)
        {
            if (!File.Exists("Blacklist.txt"))
                File.Create("Blacklist.txt");

            string line;
            StreamReader file = new StreamReader(percorsoLog);
            while ((line = file.ReadLine()) != null)
            {
                if (line == _ipDaControllare)
                {
                    Console.WriteLine(_ipDaControllare + " è nella blacklist!");
                    return true;
                }
            }
            file.Close();

            return false;
        }

    }
}

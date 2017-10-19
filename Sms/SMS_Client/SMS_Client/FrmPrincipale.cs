using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SMS_Client
{
    public partial class FrmPrincipale : Form
    {//Creare file log
        TcpClient ObjTcpClient;
        public FrmPrincipale()
        {
            InitializeComponent();
        }
        void Connessione()
        {
            try
            {
                ObjTcpClient = new TcpClient("localhost", 13);
                this.Text = "Connesso come: " + ObjTcpClient.Client.LocalEndPoint.ToString();
                MessageBox.Show("Connesso come: " + ObjTcpClient.Client.LocalEndPoint.ToString(), "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnConnect.Enabled = false;
                btnSendMex.Enabled = true;
                do
                {
                    if (ObjTcpClient.Connected)
                    {
                        Thread objThread = new Thread(Leggi)
                        {
                            IsBackground = true
                        };
                        objThread.Start();
                    }
                } while (!ObjTcpClient.Connected);
            }
            catch (Exception ecc) { MessageBox.Show("Impossibile connettersi"); }
        }

        public void Leggi()
        {
            int datiArrivo = -1;
            while ((ObjTcpClient.Connected) && (datiArrivo != 0))
            {
                Byte[] datiRicevuti = new Byte[256];
                datiArrivo = 0;
                NetworkStream objNetworkStream = ObjTcpClient.GetStream();
                try
                {
                    datiArrivo = objNetworkStream.Read(datiRicevuti, 0, datiRicevuti.Length);
                    if (datiArrivo != 0)
                    {
                        string letteraRicevuta = Encoding.ASCII.GetString(datiRicevuti).NoZero();
                        if ((letteraRicevuta[0] == '!') && (letteraRicevuta[1] == '/'))
                        {
                            if ((letteraRicevuta[2] == 'l') && (letteraRicevuta[3] == 'c'))
                            {
                                string client = letteraRicevuta.Substring(4, letteraRicevuta.Length - 4);
                                AddClient(client);
                            }
                            if ((letteraRicevuta[2] == 'r') && (letteraRicevuta[3] == 'c'))//Rimuove1Client dalla lista di quelli connessi
                            {
                                string ipDestinatario = letteraRicevuta.Substring(4, letteraRicevuta.Length - 4);
                                RemoveConnectedClient(ipDestinatario);
                            }
                            if ((letteraRicevuta[2] == 'a') && (letteraRicevuta[3] == 'c'))//Aggiunge1Client alla lista di quelli connessi
                            {
                                string ipDestinatario = letteraRicevuta.Substring(4, letteraRicevuta.Length - 4);
                                AddConnectedClient(ipDestinatario);
                            }
                            if ((letteraRicevuta[2] == 'c') && (letteraRicevuta[3] == 'p'))
                            {
                                int endIp = letteraRicevuta.IndexOf('|');
                                string ipDestinatario = letteraRicevuta.Substring(4, endIp - 4);
                                int endIpMittente = letteraRicevuta.IndexOf('@');
                                string ipMittente = letteraRicevuta.Substring(endIp + 1, endIpMittente - (endIp + 1));
                                string testoSenzaComandi = letteraRicevuta.Substring(endIpMittente + 1, letteraRicevuta.Length - (endIpMittente + 1));
                                CreateTabPage(ipMittente);
                                AddlineTabPages(ipMittente + "|" + testoSenzaComandi);
                            }
                        }
                        else
                        {
                            Addline(letteraRicevuta);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Attenzione", "Il server ha chiuso la connessione!");
                        btnConnect.Enabled = true;
                        btnSendMex.Enabled = false;
                    }
                }
                catch (Exception ecc)
                {                
                    //btnConnect.Enabled = true;
                    //btnSendMex.Enabled = false;
                }
            }
        }

        public void AddConnectedClient(string _ipClient)
        {
            lstClient.Invoke(new Action<string>(AggiungiClientListBox), _ipClient);
        }

        public void AggiungiClientListBox(string _ipClient)
        {
            lstClient.Items.Add(_ipClient);
        }

        public void RemoveConnectedClient(string _ipClient)
        {
            lstClient.Invoke(new Action<string>(RimuoviClientListBox), _ipClient);
        }
        public void RimuoviClientListBox(string _ipClient)
        {
            bool rimosso = false; int i = 0;
            do
            {
                if(_ipClient == lstClient.Items[i].ToString())
                {
                    lstClient.Items.RemoveAt(i);
                    rimosso = true;
                }
                i++;
            } while (!rimosso);
        }

        public void CreateTabPage(string _ip)
        {
            tbCntrlListaChat.Invoke(new Action<string>(CreazioneTab), _ip);
        }

        public void CreazioneTab(string _ipConv)
        {
            bool tabPageEsistente = false;
            for (int i = 0; i < tbCntrlListaChat.TabPages.Count; i++)
            {
                if (_ipConv == tbCntrlListaChat.TabPages[i].Text)
                {
                    tabPageEsistente = true;
                }
            }

            if (!tabPageEsistente)
            {
                RichTextBox rchTxtChtPrv = new RichTextBox()
                {
                    Dock = DockStyle.Fill
                };
                TabPage tbPgChtPrv = new TabPage(_ipConv)
                {
                    Name = _ipConv
                };
                tbPgChtPrv.Controls.Add(rchTxtChtPrv);
                tbCntrlListaChat.Controls.Add(tbPgChtPrv);
            }
        }

        public void AddlineTabPages(string _testoConIp)
        {
            tbCntrlListaChat.Invoke(new Action<string>(RigaTbPges), _testoConIp);
        }

        public void RigaTbPges(string _testoConIp)
        {
            int endIp = _testoConIp.IndexOf('|');
            string ipDestinatario = _testoConIp.Substring(0, endIp);
            string testo = _testoConIp.Substring(endIp + 1, _testoConIp.Length - (endIp + 1));
            for (int i = 0; i < tbCntrlListaChat.TabPages.Count; i++)
            {
                if (ipDestinatario == tbCntrlListaChat.TabPages[i].Text)
                {
                    RichTextBox rchTxtBx = (RichTextBox)tbCntrlListaChat.TabPages[i].Controls[0];
                    rchTxtBx.Text += testo + Environment.NewLine;
                }
            }
        }

        public void Addline(string _testo)
        {
            rchTxtMex.Invoke(new Action<string>(Riga), _testo);
        }

        public void Riga(string _testo)
        {
            rchTxtMex.Text += _testo + Environment.NewLine;
        }

        public void AddClient(string _client)
        {
            lstClient.Invoke(new Action<string>(AggiungiHost), _client);
        }

        public void AggiungiHost(string _client)
        {
            string[] clients = _client.Split('|');
            lstClient.Items.Clear();
            for (int i = 0; i < clients.Length; i++)
            {
                if (clients[i] != ObjTcpClient.Client.LocalEndPoint.ToString())
                {
                    lstClient.Items.Add(clients[i]);
                }
            }
        }

        public void Scrivi()
        {
            try
            {
                Byte[] datiDainviare = new Byte[256];
                if (tbCntrlListaChat.SelectedTab.Name != "tbPgGlbl")
                {
                    datiDainviare = Encoding.ASCII.GetBytes("!/cp" + tbCntrlListaChat.SelectedTab.Name + "|" + ObjTcpClient.Client.LocalEndPoint + "@" + ObjTcpClient.Client.LocalEndPoint + ": " + txtBx_MexInvio.Text);
                }
                else
                {
                    if ((txtBx_MexInvio.Text[0] != '!') && (txtBx_MexInvio.Text[1] != '/'))
                    {
                        datiDainviare = Encoding.ASCII.GetBytes(ObjTcpClient.Client.LocalEndPoint.ToString() + ": " + txtBx_MexInvio.Text);
                    }
                    else
                    {
                        datiDainviare = Encoding.ASCII.GetBytes(txtBx_MexInvio.Text);
                    }
                }
                NetworkStream objNetworkStream = ObjTcpClient.GetStream();
                objNetworkStream.Write(datiDainviare, 0, datiDainviare.Length);
                txtBx_MexInvio.Text = "";
            }
            catch (Exception ecc) {MessageBox.Show(ecc.ToString()); }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            Connessione();
        }

        private void BtnSendMex_Click(object sender, EventArgs e)
        {
            Scrivi();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LstClient_MouseDoubleClick(object sender, MouseEventArgs e)
        {          
            CreateTabPage(lstClient.SelectedItem.ToString());
            tbCntrlListaChat.SelectTab(lstClient.SelectedItem.ToString());
            txtBx_MexInvio.Focus();
            txtBx_MexInvio.SelectionStart = txtBx_MexInvio.Text.Length;
        }
    }
}

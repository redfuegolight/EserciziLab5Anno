namespace SMS_Client
{
    partial class FrmPrincipale
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipale));
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtBx_MexInvio = new System.Windows.Forms.TextBox();
            this.lstClient = new System.Windows.Forms.ListBox();
            this.rchTxtMex = new System.Windows.Forms.RichTextBox();
            this.btnSendMex = new System.Windows.Forms.Button();
            this.tbCntrlListaChat = new System.Windows.Forms.TabControl();
            this.tbPgGlbl = new System.Windows.Forms.TabPage();
            this.tbCntrlListaChat.SuspendLayout();
            this.tbPgGlbl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(340, 258);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(77, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connetti";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // txtBx_MexInvio
            // 
            this.txtBx_MexInvio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBx_MexInvio.Location = new System.Drawing.Point(1, 258);
            this.txtBx_MexInvio.Name = "txtBx_MexInvio";
            this.txtBx_MexInvio.Size = new System.Drawing.Size(252, 20);
            this.txtBx_MexInvio.TabIndex = 1;
            // 
            // lstClient
            // 
            this.lstClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstClient.HorizontalScrollbar = true;
            this.lstClient.Location = new System.Drawing.Point(297, 1);
            this.lstClient.Name = "lstClient";
            this.lstClient.Size = new System.Drawing.Size(120, 251);
            this.lstClient.TabIndex = 2;
            this.lstClient.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LstClient_MouseDoubleClick);
            // 
            // rchTxtMex
            // 
            this.rchTxtMex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rchTxtMex.Location = new System.Drawing.Point(3, 3);
            this.rchTxtMex.Name = "rchTxtMex";
            this.rchTxtMex.Size = new System.Drawing.Size(277, 220);
            this.rchTxtMex.TabIndex = 3;
            this.rchTxtMex.Text = "";
            // 
            // btnSendMex
            // 
            this.btnSendMex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendMex.Enabled = false;
            this.btnSendMex.Location = new System.Drawing.Point(259, 258);
            this.btnSendMex.Name = "btnSendMex";
            this.btnSendMex.Size = new System.Drawing.Size(75, 23);
            this.btnSendMex.TabIndex = 4;
            this.btnSendMex.Text = "Invia";
            this.btnSendMex.UseVisualStyleBackColor = true;
            this.btnSendMex.Click += new System.EventHandler(this.BtnSendMex_Click);
            // 
            // tbCntrlListaChat
            // 
            this.tbCntrlListaChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCntrlListaChat.Controls.Add(this.tbPgGlbl);
            this.tbCntrlListaChat.Location = new System.Drawing.Point(0, 0);
            this.tbCntrlListaChat.Name = "tbCntrlListaChat";
            this.tbCntrlListaChat.SelectedIndex = 0;
            this.tbCntrlListaChat.Size = new System.Drawing.Size(291, 252);
            this.tbCntrlListaChat.TabIndex = 5;
            // 
            // tbPgGlbl
            // 
            this.tbPgGlbl.Controls.Add(this.rchTxtMex);
            this.tbPgGlbl.Location = new System.Drawing.Point(4, 22);
            this.tbPgGlbl.Name = "tbPgGlbl";
            this.tbPgGlbl.Padding = new System.Windows.Forms.Padding(3);
            this.tbPgGlbl.Size = new System.Drawing.Size(283, 226);
            this.tbPgGlbl.TabIndex = 0;
            this.tbPgGlbl.Text = "Chat";
            this.tbPgGlbl.UseVisualStyleBackColor = true;
            // 
            // FrmPrincipale
            // 
            this.AcceptButton = this.btnSendMex;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(422, 284);
            this.Controls.Add(this.tbCntrlListaChat);
            this.Controls.Add(this.btnSendMex);
            this.Controls.Add(this.lstClient);
            this.Controls.Add(this.txtBx_MexInvio);
            this.Controls.Add(this.btnConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(346, 219);
            this.Name = "FrmPrincipale";
            this.Text = "Sms_Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tbCntrlListaChat.ResumeLayout(false);
            this.tbPgGlbl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtBx_MexInvio;
        private System.Windows.Forms.ListBox lstClient;
        private System.Windows.Forms.RichTextBox rchTxtMex;
        private System.Windows.Forms.Button btnSendMex;
        private System.Windows.Forms.TabControl tbCntrlListaChat;
        private System.Windows.Forms.TabPage tbPgGlbl;
    }
}


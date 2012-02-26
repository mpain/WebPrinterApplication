namespace WebPrinterApplication
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonControl = new System.Windows.Forms.Button();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.textBoxServerPort = new System.Windows.Forms.TextBox();
            this.comboBoxPrinterPort = new System.Windows.Forms.ComboBox();
            this.labelSmtpGateway = new System.Windows.Forms.Label();
            this.groupBoxCommon = new System.Windows.Forms.GroupBox();
            this.groupBoxCaption = new System.Windows.Forms.GroupBox();
            this.textBoxCaption = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSmtpGateway = new System.Windows.Forms.TextBox();
            this.textBoxSmtpPort = new System.Windows.Forms.TextBox();
            this.textBoxSmtpUser = new System.Windows.Forms.TextBox();
            this.textBoxSmtpPassword = new System.Windows.Forms.TextBox();
            this.checkBoxSmtpUseSsl = new System.Windows.Forms.CheckBox();
            this.labelSmtpPort = new System.Windows.Forms.Label();
            this.labelSmtpUser = new System.Windows.Forms.Label();
            this.labelSmtpPassword = new System.Windows.Forms.Label();
            this.textBoxSmtpFrom = new System.Windows.Forms.TextBox();
            this.textBoxSmtpTo = new System.Windows.Forms.TextBox();
            this.labelSmtpFrom = new System.Windows.Forms.Label();
            this.labelSmtpTo = new System.Windows.Forms.Label();
            this.labelPrinterPort = new System.Windows.Forms.Label();
            this.groupBoxCommon.SuspendLayout();
            this.groupBoxCaption.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonControl
            // 
            this.buttonControl.Location = new System.Drawing.Point(12, 372);
            this.buttonControl.Name = "buttonControl";
            this.buttonControl.Size = new System.Drawing.Size(291, 39);
            this.buttonControl.TabIndex = 0;
            this.buttonControl.Text = "Start";
            this.buttonControl.UseVisualStyleBackColor = true;
            this.buttonControl.Click += new System.EventHandler(this.OnButtonStartClick);
            // 
            // labelServerPort
            // 
            this.labelServerPort.AutoSize = true;
            this.labelServerPort.Location = new System.Drawing.Point(7, 50);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(62, 13);
            this.labelServerPort.TabIndex = 1;
            this.labelServerPort.Text = "Server port:";
            // 
            // textBoxServerPort
            // 
            this.textBoxServerPort.Location = new System.Drawing.Point(191, 47);
            this.textBoxServerPort.Name = "textBoxServerPort";
            this.textBoxServerPort.Size = new System.Drawing.Size(95, 20);
            this.textBoxServerPort.TabIndex = 2;
            this.textBoxServerPort.Text = "8088";
            // 
            // comboBoxPrinterPort
            // 
            this.comboBoxPrinterPort.FormattingEnabled = true;
            this.comboBoxPrinterPort.Location = new System.Drawing.Point(191, 20);
            this.comboBoxPrinterPort.Name = "comboBoxPrinterPort";
            this.comboBoxPrinterPort.Size = new System.Drawing.Size(94, 21);
            this.comboBoxPrinterPort.TabIndex = 3;
            // 
            // labelSmtpGateway
            // 
            this.labelSmtpGateway.AutoSize = true;
            this.labelSmtpGateway.Location = new System.Drawing.Point(8, 22);
            this.labelSmtpGateway.Name = "labelSmtpGateway";
            this.labelSmtpGateway.Size = new System.Drawing.Size(85, 13);
            this.labelSmtpGateway.TabIndex = 1;
            this.labelSmtpGateway.Text = "SMTP Gateway:";
            // 
            // groupBoxCommon
            // 
            this.groupBoxCommon.Controls.Add(this.textBoxServerPort);
            this.groupBoxCommon.Controls.Add(this.comboBoxPrinterPort);
            this.groupBoxCommon.Controls.Add(this.labelServerPort);
            this.groupBoxCommon.Controls.Add(this.labelPrinterPort);
            this.groupBoxCommon.Location = new System.Drawing.Point(12, 12);
            this.groupBoxCommon.Name = "groupBoxCommon";
            this.groupBoxCommon.Size = new System.Drawing.Size(291, 79);
            this.groupBoxCommon.TabIndex = 4;
            this.groupBoxCommon.TabStop = false;
            this.groupBoxCommon.Text = "Common Settings";
            // 
            // groupBoxCaption
            // 
            this.groupBoxCaption.Controls.Add(this.textBoxCaption);
            this.groupBoxCaption.Location = new System.Drawing.Point(12, 98);
            this.groupBoxCaption.Name = "groupBoxCaption";
            this.groupBoxCaption.Size = new System.Drawing.Size(291, 54);
            this.groupBoxCaption.TabIndex = 5;
            this.groupBoxCaption.TabStop = false;
            this.groupBoxCaption.Text = "Ticket Caption";
            // 
            // textBoxCaption
            // 
            this.textBoxCaption.Location = new System.Drawing.Point(7, 20);
            this.textBoxCaption.Name = "textBoxCaption";
            this.textBoxCaption.Size = new System.Drawing.Size(278, 20);
            this.textBoxCaption.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxSmtpUseSsl);
            this.groupBox1.Controls.Add(this.textBoxSmtpTo);
            this.groupBox1.Controls.Add(this.textBoxSmtpFrom);
            this.groupBox1.Controls.Add(this.textBoxSmtpPassword);
            this.groupBox1.Controls.Add(this.textBoxSmtpUser);
            this.groupBox1.Controls.Add(this.textBoxSmtpPort);
            this.groupBox1.Controls.Add(this.textBoxSmtpGateway);
            this.groupBox1.Controls.Add(this.labelSmtpTo);
            this.groupBox1.Controls.Add(this.labelSmtpFrom);
            this.groupBox1.Controls.Add(this.labelSmtpPassword);
            this.groupBox1.Controls.Add(this.labelSmtpUser);
            this.groupBox1.Controls.Add(this.labelSmtpPort);
            this.groupBox1.Controls.Add(this.labelSmtpGateway);
            this.groupBox1.Location = new System.Drawing.Point(12, 158);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 208);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mail Settings";
            // 
            // textBoxSmtpGateway
            // 
            this.textBoxSmtpGateway.Location = new System.Drawing.Point(127, 19);
            this.textBoxSmtpGateway.Name = "textBoxSmtpGateway";
            this.textBoxSmtpGateway.Size = new System.Drawing.Size(158, 20);
            this.textBoxSmtpGateway.TabIndex = 2;
            this.textBoxSmtpGateway.Text = "smtp.yandex.ru";
            // 
            // textBoxSmtpPort
            // 
            this.textBoxSmtpPort.Location = new System.Drawing.Point(127, 45);
            this.textBoxSmtpPort.Name = "textBoxSmtpPort";
            this.textBoxSmtpPort.Size = new System.Drawing.Size(158, 20);
            this.textBoxSmtpPort.TabIndex = 2;
            this.textBoxSmtpPort.Text = "25";
            // 
            // textBoxSmtpUser
            // 
            this.textBoxSmtpUser.Location = new System.Drawing.Point(128, 71);
            this.textBoxSmtpUser.Name = "textBoxSmtpUser";
            this.textBoxSmtpUser.Size = new System.Drawing.Size(158, 20);
            this.textBoxSmtpUser.TabIndex = 2;
            this.textBoxSmtpUser.Text = "expert.terminal";
            // 
            // textBoxSmtpPassword
            // 
            this.textBoxSmtpPassword.Location = new System.Drawing.Point(127, 97);
            this.textBoxSmtpPassword.Name = "textBoxSmtpPassword";
            this.textBoxSmtpPassword.PasswordChar = '*';
            this.textBoxSmtpPassword.Size = new System.Drawing.Size(158, 20);
            this.textBoxSmtpPassword.TabIndex = 2;
            this.textBoxSmtpPassword.Text = "Qwe4Rty7";
            // 
            // checkBoxSmtpUseSsl
            // 
            this.checkBoxSmtpUseSsl.AutoSize = true;
            this.checkBoxSmtpUseSsl.Location = new System.Drawing.Point(128, 123);
            this.checkBoxSmtpUseSsl.Name = "checkBoxSmtpUseSsl";
            this.checkBoxSmtpUseSsl.Size = new System.Drawing.Size(66, 17);
            this.checkBoxSmtpUseSsl.TabIndex = 3;
            this.checkBoxSmtpUseSsl.Text = "use SSL";
            this.checkBoxSmtpUseSsl.UseVisualStyleBackColor = true;
            // 
            // labelSmtpPort
            // 
            this.labelSmtpPort.AutoSize = true;
            this.labelSmtpPort.Location = new System.Drawing.Point(8, 48);
            this.labelSmtpPort.Name = "labelSmtpPort";
            this.labelSmtpPort.Size = new System.Drawing.Size(62, 13);
            this.labelSmtpPort.TabIndex = 1;
            this.labelSmtpPort.Text = "SMTP Port:";
            // 
            // labelSmtpUser
            // 
            this.labelSmtpUser.AutoSize = true;
            this.labelSmtpUser.Location = new System.Drawing.Point(8, 74);
            this.labelSmtpUser.Name = "labelSmtpUser";
            this.labelSmtpUser.Size = new System.Drawing.Size(65, 13);
            this.labelSmtpUser.TabIndex = 1;
            this.labelSmtpUser.Text = "SMTP User:";
            // 
            // labelSmtpPassword
            // 
            this.labelSmtpPassword.AutoSize = true;
            this.labelSmtpPassword.Location = new System.Drawing.Point(8, 100);
            this.labelSmtpPassword.Name = "labelSmtpPassword";
            this.labelSmtpPassword.Size = new System.Drawing.Size(89, 13);
            this.labelSmtpPassword.TabIndex = 1;
            this.labelSmtpPassword.Text = "SMTP Password:";
            // 
            // textBoxSmtpFrom
            // 
            this.textBoxSmtpFrom.Location = new System.Drawing.Point(127, 146);
            this.textBoxSmtpFrom.Name = "textBoxSmtpFrom";
            this.textBoxSmtpFrom.Size = new System.Drawing.Size(158, 20);
            this.textBoxSmtpFrom.TabIndex = 2;
            this.textBoxSmtpFrom.Text = "expert.terminal@yandex.ru";
            // 
            // textBoxSmtpTo
            // 
            this.textBoxSmtpTo.Location = new System.Drawing.Point(127, 172);
            this.textBoxSmtpTo.Name = "textBoxSmtpTo";
            this.textBoxSmtpTo.Size = new System.Drawing.Size(158, 20);
            this.textBoxSmtpTo.TabIndex = 2;
            this.textBoxSmtpTo.Text = "sergey.samoylov@gmail.com";
            // 
            // labelSmtpFrom
            // 
            this.labelSmtpFrom.AutoSize = true;
            this.labelSmtpFrom.Location = new System.Drawing.Point(8, 149);
            this.labelSmtpFrom.Name = "labelSmtpFrom";
            this.labelSmtpFrom.Size = new System.Drawing.Size(30, 13);
            this.labelSmtpFrom.TabIndex = 1;
            this.labelSmtpFrom.Text = "From";
            // 
            // labelSmtpTo
            // 
            this.labelSmtpTo.AutoSize = true;
            this.labelSmtpTo.Location = new System.Drawing.Point(8, 175);
            this.labelSmtpTo.Name = "labelSmtpTo";
            this.labelSmtpTo.Size = new System.Drawing.Size(20, 13);
            this.labelSmtpTo.TabIndex = 1;
            this.labelSmtpTo.Text = "To";
            // 
            // labelPrinterPort
            // 
            this.labelPrinterPort.AutoSize = true;
            this.labelPrinterPort.Location = new System.Drawing.Point(8, 23);
            this.labelPrinterPort.Name = "labelPrinterPort";
            this.labelPrinterPort.Size = new System.Drawing.Size(61, 13);
            this.labelPrinterPort.TabIndex = 1;
            this.labelPrinterPort.Text = "Printer port:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 423);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxCaption);
            this.Controls.Add(this.buttonControl);
            this.Controls.Add(this.groupBoxCommon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "WebPrinter Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.groupBoxCommon.ResumeLayout(false);
            this.groupBoxCommon.PerformLayout();
            this.groupBoxCaption.ResumeLayout(false);
            this.groupBoxCaption.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonControl;
        private System.Windows.Forms.Label labelServerPort;
        private System.Windows.Forms.TextBox textBoxServerPort;
        private System.Windows.Forms.ComboBox comboBoxPrinterPort;
        private System.Windows.Forms.Label labelSmtpGateway;
        private System.Windows.Forms.GroupBox groupBoxCommon;
        private System.Windows.Forms.GroupBox groupBoxCaption;
        private System.Windows.Forms.TextBox textBoxCaption;
        private System.Windows.Forms.Label labelPrinterPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxSmtpUseSsl;
        private System.Windows.Forms.TextBox textBoxSmtpTo;
        private System.Windows.Forms.TextBox textBoxSmtpFrom;
        private System.Windows.Forms.TextBox textBoxSmtpPassword;
        private System.Windows.Forms.TextBox textBoxSmtpUser;
        private System.Windows.Forms.TextBox textBoxSmtpPort;
        private System.Windows.Forms.TextBox textBoxSmtpGateway;
        private System.Windows.Forms.Label labelSmtpTo;
        private System.Windows.Forms.Label labelSmtpFrom;
        private System.Windows.Forms.Label labelSmtpPassword;
        private System.Windows.Forms.Label labelSmtpUser;
        private System.Windows.Forms.Label labelSmtpPort;
    }
}


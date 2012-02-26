using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebPrinterApplication.Server;
using System.IO.Ports;
using WepPrinterApplication.Printer;
using System.IO;
using log4net;
using WebPrinterApplication.Mail;

namespace WebPrinterApplication
{
    public partial class MainForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        private WebServer server;

        private ITicketPrinter ticketPrinter;
        private IOrderTicketProcessor ticketProcessor;
        private IMailService mailService;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OnButtonStartClick(object sender, EventArgs e)
        {
            bool isStart = buttonControl.Text.Equals("Start");


            bool completed = true;
            if (server != null)
            {
                if (isStart)
                {
                    int port = parsePort();
                    textBoxServerPort.Text = port.ToString();
                    

                    String printerPortName = comboBoxPrinterPort.Text;

                    if (!String.IsNullOrEmpty(printerPortName))
                    {
                        StartServer(port, printerPortName);
                    }
                    else
                    {
                        completed = false;
                        MessageBox.Show("A valid printer port must be set for continue.", "Error");
                        log.Error("There is no a valid serial port presented on the current system.");
                    }
                }
                else
                {
                    StopServer();
                }
            }

            if (completed)
            {
                textBoxServerPort.Enabled = !isStart;
                comboBoxPrinterPort.Enabled = !isStart;

                textBoxSmtpGateway.Enabled = !isStart;
                textBoxSmtpPort.Enabled = !isStart;
                textBoxSmtpUser.Enabled = !isStart;
                textBoxSmtpPassword.Enabled = !isStart;
                checkBoxSmtpUseSsl.Enabled = !isStart;
                textBoxSmtpFrom.Enabled = !isStart;
                textBoxSmtpTo.Enabled = !isStart;
                buttonControl.Text = isStart ? "Stop" : "Start";
            }
        }

        private int parsePort() {
            int port;
            return Int32.TryParse(textBoxServerPort.Text, out port) ? port : 8088;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            log.Info("Creating aan instance of a web server...");
            server = new WebServer();
            server.OrderEvent += new WebServer.OrderHandler(OnServerOrderEvent);

            log.Info("Wiring instances of a ticket printer wireframe...");
            wireBeans();

            log.Info("Loading existing serial port names...");
            string[] names = SerialPort.GetPortNames();
            comboBoxPrinterPort.Items.AddRange(names);

            if (names != null && names.Length > 0)
            {
                comboBoxPrinterPort.Text = names[0];
            }

        }

        void OnServerOrderEvent(OrderData data)
        {
            if (!String.IsNullOrEmpty(textBoxCaption.Text)) {
                data.Caption = textBoxCaption.Text;
            }
            ticketProcessor.PrintTicket(data);

            MailSettings settings = createMailSettings(data);
            if (!String.IsNullOrEmpty(settings.To))
            {
                mailService.Send(settings);
            }
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }

        private void wireBeans()
        {
            mailService = new MailService();
            ticketPrinter = new Vkp80();
            ticketProcessor = new OrderTicketProcessor() {
                Printer = ticketPrinter
            };
        }

        private void StartServer(int serverPort, String printerPortName)
        {
            log.InfoFormat("Starting services for a server port [{0}] and a printer port [{1}]...", serverPort, printerPortName);
            server.Start(serverPort);
            ticketPrinter.Start(printerPortName, false);
            ticketProcessor.Start(Path.Combine(Application.StartupPath, @"Html\logo.jpg"));
        }

        private void StopServer()
        {
            log.Info("Stopping services...");
            ticketProcessor.Stop();
            ticketPrinter.Stop();
            server.Stop();
            mailService.Stop();
        }

        private MailSettings createMailSettings(OrderData data)
        {
            MailSettings settings = new MailSettings();
            settings.SmtpGateway = textBoxSmtpGateway.Text;

            int port;
            settings.SmtpPort = Int32.TryParse(textBoxSmtpPort.Text, out port) ? port : 25;
            settings.User = textBoxSmtpUser.Text;
            settings.Password = textBoxSmtpPassword.Text;
            settings.From = textBoxSmtpFrom.Text;
            settings.To = textBoxSmtpTo.Text;
            settings.UseSsl = checkBoxSmtpUseSsl.Checked;
            settings.Subject = data.Caption;

            settings.Body = String.Format("Имя: {0}\nТелефон: {1}\nEmail: {2}\nСообщение: {3}\n", data.Name, data.Phone, data.Email, data.Message);

            return settings;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using log4net;
using System.Threading;

namespace WebPrinterApplication.Mail
{
    public class MailSettings
    {
        public String From;
        public String To;
        public String SmtpGateway;
        public int SmtpPort;
        public bool UseSsl;
        public String User;
        public String Password;

        public String Subject;
        public String Body;
    }

    public interface IMailService
    {
        void Send(MailSettings settings);
        void Stop();


    }
    public class MailService : IMailService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MailService));

        private object lockObject = new Object();

        private Thread thread;

        public void Send(MailSettings settings)
        {
            try
            {
                Stop();

                thread = new Thread(new ParameterizedThreadStart(SendWorker))
                {
                    Name = "Mail Thread",
                    Priority = ThreadPriority.Lowest
                };
                


                thread.Start(settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                if (thread != null && thread.IsAlive)
                {
                    thread.Abort();

                    while (thread.IsAlive)
                    {
                        Thread.Sleep(0);
                    }
                }
                thread = null;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Mail thread closing error: {}", ex.Message);
            }
        }

        private void SendWorker(object parameter)
        {
            Monitor.Enter(lockObject);

            try
            {
                if (parameter is MailSettings)
                {
                    var settings = parameter as MailSettings;

                    SendMessage(settings);
                }
            }
            catch (Exception e)
            {
                logger.ErrorFormat("A mail message wasn't sent. {}", e);
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }

        private bool SendMessage(MailSettings settings)
        {
            var result = false;
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(settings.From);
                    mail.To.Add(settings.To);

                    mail.Subject = settings.Subject;
                    mail.Body = settings.Body;

                    using (var smtp = new SmtpClient(settings.SmtpGateway, settings.SmtpPort))
                    {
                        smtp.EnableSsl = settings.UseSsl;
                        smtp.Credentials = new NetworkCredential(settings.User, settings.Password);
                        smtp.Send(mail);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error during sending an e-mail", ex);
            }
            return result;
        }
    }
}

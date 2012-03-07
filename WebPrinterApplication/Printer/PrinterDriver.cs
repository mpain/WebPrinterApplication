using System;
using System.Timers;
using WebPrinterApplication.Common;
using System.Threading;
using Timer = System.Timers.Timer;

namespace WepPrinterApplication.Printer
{
    public abstract class PrinterDriver : SerialPortDriver, ITicketPrinter {
        private const int TIMER_INTERVAL = 1000;

        protected String printerPortName;
        protected bool printerCheckStatus;

        private readonly object lockObject = new object();
        public object LockObject {
            get { return lockObject; }
        }

        /// <summary>
        /// Таймер запроса статуса принтера
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Флаг наличия неисправности принтера
        /// </summary>
        public bool PrinterMalfunction { get; set; }

        #region Print fonts size, width and intention
        private int captionFontSize = 25;
        public int CaptionFontSize {
            get { return captionFontSize; }
            set { captionFontSize = value; }
        }

        private int mainFontSize = 14;
        public int MainFontSize {
            get { return mainFontSize; }
            set { mainFontSize = value; }
        }

        private int mainTextFont = 0x01;
        public int MainTextFont {
            get { return mainTextFont; }
            set { mainTextFont = value; }
        }

        private int captionTextFont = 0x03;
        public int CaptionTextFont {
            get { return captionTextFont; }
            set { captionTextFont = value; }
        }

        private PrintMode mode = PrintMode.Graphic;
        public PrintMode Mode {
            get { return mode; }
            set { mode = value; }
        }

        private int printWidth = 54;
        /// <summary>
        /// Ширина активной области печати в байтах
        /// </summary>
        public int PrintWidth {
            get { return printWidth; }
            set { printWidth = value; }
        }

        private int printIntention = 3;
        /// <summary>
        /// Размер отступа печати в байтах
        /// </summary>
        public int PrintIntention {
            get { return printIntention; }
            set { printIntention = value; }
        }
        #endregion

        public void Start(String printerPortName, bool printerCheckStatus) {
            this.printerPortName = printerPortName;
            this.printerCheckStatus = printerCheckStatus;

            OnReceive += ReceiveResponse;
            Connect();
            if(!IsConnected) {
                return; // throw new Exception("Невозможно подсоединиться к последовательному порту принтера.");
            }

            

            if (!printerCheckStatus)
            {
                timer = new Timer { Interval = TIMER_INTERVAL, Enabled = true, AutoReset = true };
                timer.Elapsed += new ElapsedEventHandler(OnSendStatusRequest);
                timer.Start();
            }
           
        }

        void OnSendStatusRequest(object sender, ElapsedEventArgs e) {
            if (!printerCheckStatus)
            {
                return;
            }
            if (Monitor.TryEnter(lockObject)) {
                try {
                    SendRequest();
                } catch (Exception) {
                    Console.WriteLine("Сбой запроса статуса принтера");
                } finally {
                    Monitor.Exit(lockObject);
                }
            }
        }

        protected virtual void SendRequest() {
        }

        protected virtual void ReceiveResponse(SerialResponseArgs e) {
        }

        public void Stop() {
            Disconnect();
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }

        public void Dispose() {
            if(IsConnected) {
                PortClose();
            }
        }

        #region Abstract procedures
        public virtual void PrintByteArray(byte[] data) { }
        public virtual void PrintMonochromeBitmap(byte[] data, int height, int width) { }
        public virtual void PrintStart() { }
        public virtual void PrintEnd() { }
        public virtual bool Connect() { return false; }
        public virtual void Disconnect() { OnReceive -= ReceiveResponse; }
        #endregion
    }
}

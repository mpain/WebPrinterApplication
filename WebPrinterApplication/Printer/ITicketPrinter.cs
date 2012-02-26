using System;

namespace WepPrinterApplication.Printer
{
    public delegate void PrinterStatusEventHandler(bool status);

    public interface ITicketPrinter : IDisposable {
        void Start(String printerPortName, bool printerCheckStatus);
        void Stop();
        void PrintByteArray(byte[] data);
        void PrintMonochromeBitmap(byte[] data, int height, int width);
        void PrintStart();
        void PrintEnd();
        bool Connect();
        void Disconnect();

        int CaptionFontSize {
            get;
            set;
        }

        int MainFontSize {
            get;
            set;
        }

        int MainTextFont {
            get;
            set;
        }

        int CaptionTextFont {
            get;
            set;
        }

        PrintMode Mode {
            get;
        }

        int PrintWidth {
            get;
        }

        int PrintIntention {
            get;
            set;
        }

        bool PrinterMalfunction {
            get;
        }

        object LockObject {
            get;
        }
    }
}

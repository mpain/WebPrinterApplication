#region Using Directives

using System;
using System.Text;
using System.IO.Ports;
using log4net;

#endregion

namespace WebPrinterApplication.Common {
    public delegate void SerialResponeEventHandler(SerialResponseArgs e);

    public class SerialResponseArgs : EventArgs {
        public readonly byte[] data;

        public SerialResponseArgs(byte[] data) {
            this.data = data;
        }
    }

    /// <summary>
    /// Реализует весь основной функционал для работы с последовательным портом.
    /// </summary>
    abstract public class SerialPortDriver {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SerialPortDriver));

        protected SerialPort SerialPort;

        public event SerialResponeEventHandler OnReceive;

        public virtual bool IsConnected {
            get {
                if(SerialPort == null)
                    return false;

                return SerialPort.IsOpen;
            }
        }

        ~SerialPortDriver() {
            PortClose();
        }


        public void PortInit(string portName, int baudrate, Parity parity) {
            PortInit(portName, baudrate, parity, Handshake.None);
        }

        public void PortInit(string portName, int baudrate, Parity parity, Handshake shakeType) {
            PortInit(portName, baudrate, parity, shakeType, 8);
        }

        public void PortInit(string portName, int baudrate, Parity parity, Handshake shakeType, int dataBits) {
            PortInit(portName, baudrate, parity, shakeType, dataBits, StopBits.One);
        }

        public void PortInit(string portName, int baudrate, Parity parity, Handshake shakeType, int dataBits, StopBits stopBits)
        {
            string[] pn = SerialPort.GetPortNames();

            foreach (var port in pn)
            {
                if (port.StartsWith(portName))
                {
                    SerialPort = new SerialPort(portName, baudrate)
                    {
                        Parity = parity,
                        StopBits = stopBits,
                        DataBits = dataBits,
                        Handshake = shakeType,
                        Encoding = Encoding.ASCII
                    };

                    SerialPort.DataReceived += new SerialDataReceivedEventHandler(OnSerialDataReceived);
                    SerialPort.Open();
                    break;
                }
            }
        }

        public void PortClose() {
            if (SerialPort != null) {
                SerialPort.Close();
                SerialPort.Dispose();
                SerialPort = null;
            }
        }

        public void OnSerialDataReceived(object sender, SerialDataReceivedEventArgs args) {
            int bytesReaded = SerialPort.BytesToRead;
            if (bytesReaded > 0) {
                var data = new byte[bytesReaded];
                SerialPort.Read(data, 0, data.Length);

                if (OnReceive != null) {
                    OnReceive(new SerialResponseArgs(data));
                }
            }             
        }
        
        public void SendData(byte[] data) {
            if(SerialPort != null && SerialPort.IsOpen && data != null && data.Length > 0) {
                try {
                    SerialPort.Write(data, 0, data.Length);
                } catch (Exception) {
                    logger.Error(String.Format("Send data to port {0} failed", SerialPort.PortName));
                }
            }
        }

        public void SendData(byte[] data, int start, int len) {
            if(SerialPort != null && SerialPort.IsOpen && data != null && data.Length > 0) {
                try {
                    SerialPort.Write(data, start, len);
                } catch (Exception) {
                    logger.Error(String.Format("Send data to port {0} failed", SerialPort.PortName));
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using log4net;
using System.Web;
using WepPrinterApplication.Printer;

namespace WebPrinterApplication.Server
{
    class WebServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebServer));
        private static String CR_LF = "\r\n\r\n";

        public delegate void OrderHandler(OrderData data);

        private Thread thread;

        private volatile bool threadRunning;

        private FileStorage storage;

        public WebServer()
        {
            storage = new FileStorage(Path.Combine(Application.StartupPath, "Html"), new String[] {
                "index.html",
                "success.html"
            });
        }

        public void Start(int port)
        {
            lock (this)
            {
                Stop();

                int[] parameters = {port};
                thread = new Thread(new ParameterizedThreadStart(StartListen));
                thread.Start(port);
            }
        }

        public void Stop()
        {
            lock (this)
            {
                if (thread != null)
                {
                    listener.Stop();

                    threadRunning = false;
                    thread.Join();

                    thread = null;
                }
            }
        }

        private TcpListener listener;

        private void StartListen(object port) {
            try
            {
                listener = new TcpListener(IPAddress.Any, (int)port);
                listener.Start();

                threadRunning = true;
                while (threadRunning)
                {
                    Socket socket = listener.AcceptSocket();
                    try
                    {
                        log.InfoFormat("Socket Type {0}", socket.SocketType);
                        if (socket.Connected)
                        {
                            log.InfoFormat("CLient from IP address {0} connected\n", socket.RemoteEndPoint);

                            Byte[] requestBuffer = new Byte[1024];
                            int requestLength = socket.Receive(requestBuffer, requestBuffer.Length, 0);


                            String request = Encoding.ASCII.GetString(requestBuffer, 0, requestLength);
                            log.InfoFormat("Request: {0}", request);
                            bool result = handlePrintRequest(request);

                            String fileName = (result ? "success" : "index") + ".html";
                            log.InfoFormat("Answering with file named '{0}'", fileName);
                            SendToBrowser(storage.GetFile(fileName), "200 OK", socket);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is ThreadAbortException)
                        {
                            throw e;
                        }
                        log.ErrorFormat("Error: {0}", e.Message);
                    }
                    finally
                    {
                        socket.Close();
                    }
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error: {0}", e.Message);
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }
            }
            
        }

        public event OrderHandler OrderEvent;

        private bool handlePrintRequest(String request)
        {
            String[] strings = request.Split(new String[] {"\r\n"}, StringSplitOptions.None);
            if (strings.Length == 0)
            {
                return false;
            }

            String[] protoString = strings[0].Split(new String[] { " " }, StringSplitOptions.None);
            if (protoString.Length != 3)
            {
                return false;
            }

            if (protoString[0].ToUpper() != "POST" ||
                !protoString[2].StartsWith("HTTP") ||
                !protoString[1].EndsWith("order/index.html"))
            {
                return false;
            }

            int paramsStartIndex = request.IndexOf(CR_LF);
 
            String parameters = request.Substring(paramsStartIndex + CR_LF.Length);

            OrderData order = new OrderData();

            String[] pairs = parameters.Split('&');
            foreach (String pair in pairs)
            {
                String[] data = pair.Split('=');
                String value = !String.IsNullOrEmpty(data[1]) ? data[1] : "";

                value = HttpUtility.UrlDecode(value, Encoding.UTF8);
                

                switch (data[0].ToLower()) {
                    case "caption":
                        order.Caption = value;
                        break;
                    case "name":
                        order.Name = value;
                        break;
                    case "phone":
                        order.Phone = value;
                        break;
                    case "email":
                        order.Email = value;
                        break;
                    case "message":
                        order.Message = value;
                        break;
                }               
            }

            if (OrderEvent != null)
            {
                OrderEvent(order);
            }

            return true;
        }

        public void SendHeader(int length, String statusMessage, Socket socket)
        {

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("HTTP/1.1 {0} \r\n", statusMessage);
            builder.Append("Server: mpain-1978-server\r\n");
            builder.Append("Content-Type: text/html; charset=utf-8\r\n");
            builder.Append("Accept-Ranges: bytes\r\n");
            builder.AppendFormat("Content-Length: {0}\r\n\r\n", length);


            String header = builder.ToString();
            log.InfoFormat("Sending header: {0}", header);
            Byte[] buffer = Encoding.UTF8.GetBytes(header);

            SendToBrowser(buffer, socket);
        }



        public void SendToBrowser(String data, String statusMessage, Socket socket)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            SendHeader(buffer.Length, statusMessage, socket);
            SendToBrowser(buffer, socket);
        }


        public void SendToBrowser(byte[] buffer, Socket socket)
        {
            int counter = 0;

            try
            {
                if (socket.Connected)
                {
                    if ((counter = socket.Send(buffer, buffer.Length, 0)) == -1)
                    {
                        log.InfoFormat("Socket Error cannot Send Packet");
                    }
                    else
                    {
                        log.InfoFormat("No. of bytes those has been just sent {0}", counter);
                    }
                }
                else
                {
                    log.InfoFormat("Connection Dropped....");
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("Error Occurred : {0} ", e);

            }
        }

    }
}

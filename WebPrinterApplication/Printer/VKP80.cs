using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using WebPrinterApplication.Common;
using log4net;

namespace WepPrinterApplication.Printer
{
    public class Vkp80 : PrinterDriver {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Vkp80));
        private const int SLEEP_TIME = 5000;

        public Vkp80() {
            PrintWidth = 76;
            MainFontSize = 18;
            Mode = PrintMode.Graphic;
            PrintIntention = 6;
        }

        public override void PrintStart() {
            /* Инициализация */
            var bytes = new byte[] {
                0x18,                                           // Отмена текущей печатаемой линии
                0x1d, 0xf6,                                     // Сброс метки
                0x1b, 0x32,                                     // Установка величины шага протяжки в 1/6 дюйма
                0x1b, 0x2a, 0x6f, 0x31, 0x51,                   // <1B>*o1Q (Нормальная скорость печати)
                0x1b, 0x2a, 0x74, 0x32, 0x30, 0x30, 0x52,       // <1B>*t200R (Разрешение печати 200 dpi)
                0x1d, 0x65, 0x05,                               // Выброс билета (Ejecting)
                0x1d, 0xf6,                                     // Сброс метки
                0x1b, 0x26, 0x6c, 0x34, 0x36, 0x37, 0x36, 0x52, // <1B>&1 4676 R (Длинна поля печати при ширине 80 мм и бумаге в рулоне)
                0x1b, 0x26, 0x6b, 0x34, 0x57,                   // <1B>&k4W (Плотность печати 0 <25>)
                0x1b, 0x2a, 0x70, 0x30, 0x59,                   // <1B>*p 10 Y (Абсолютный сдвиг по оси Y на 10 точек)
                0x0d,                                           // Печать и перевод каретки
                0x1b, 0x2a, 0x62, 0x32, 0x4d                    // Установка режима сжатия RLE
            };
            SendOverCom(bytes, 0, bytes.Length);
        }

        public override void PrintEnd() {
            /* Протяжка и обрезка чека */
            var bytes = new byte[] {
                0x1b, 0x2a, 0x62, 0x30, 0x4d,                   // Установка режима печати без сжатия
                0x1b, 0x2a, 0x72, 0x42,                         // <1B>*rB (Конец растровой графики)
                0x1b, 0x64, 0x04,                               // Протяжка на 4 пункта по 1/6 дюйма
                0x1b, 0x69,                                     // Полная обрезка чека
                0x1d, 0x65, 0x03, 0x00,                         // Протяжка чека на 0 мм
                0x1d, 0x65, 0x08, 0x05,                         // Выдача чека на 5 х 7.3 мм
                0x1b, 0x2a, 0x74, 0x32, 0x30, 0x30, 0x52,       // <1B>*t200R (Разрешение печати 200 dpi)
                0x1d, 0x50, 0x00, 0x00                          // Установка нулевых сдвигов по осям
            };
            SendOverCom(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Производит отправку данных через последовательный порт
        /// </summary>
        /// <param name="bytes">Массив данных для вывода</param>
        /// <param name="startIndex">Начальный индекс в массиве данных для вывода</param>
        /// <param name="length">Длинна передаваемых данных</param>
        private void SendOverCom(byte[] bytes, int startIndex, int length)
        {
            var isWorking = true;
            while (isWorking) {
                var startTicks = DateTime.Now.Ticks;
                while (!SerialPort.CtsHolding) {
                    if ((DateTime.Now.Ticks - startTicks) > SLEEP_TIME * 10000) {
                        logger.Debug("CTS wait timeout");
                        throw new Exception();
                    }
                    Thread.Sleep(0);
                }

                if (SerialPort.CtsHolding) {
                    SendData(bytes, startIndex, length);
                    isWorking = false;
                }
            }
        }

        /// <summary>
        /// Производит соединение с последовательным портом
        /// </summary>
        /// <returns></returns>
        public override bool Connect() {
            if (!IsConnected) {
                PortInit(printerPortName, 115200, Parity.None, Handshake.RequestToSend);
            }

            return IsConnected;
        }

        /// <summary>
        /// Производит отсоединение от последовательного порта
        /// </summary>
        public override void Disconnect() {
            base.Disconnect();
            PortClose();
        }

        private enum RleState {
            Search,
            Accumulate
        }

        /// <summary>
        /// Печатает монохромное изображение
        /// </summary>
        /// <param name="data">Изображение</param>
        /// <param name="height">Высота изображения</param>
        /// <param name="width">Ширина изображения</param>
        public override void PrintMonochromeBitmap(byte[] data, int height, int width) {
            var printList = new ArrayList();

            for (var y = 0; y < height; y++) {
                printList.Add(ProcessLine(data, y * width, width));
            }

            PrintArrayList(printList);
        }

        //private void PrintArrayList(ArrayList list) {
        //    int length = (from object o in list where o != null select ((byte[]) o).Length).Sum();

        //    var resultArray = new byte[length];
        //    var pos = 0;
        //    foreach (byte[] stringData in from object o in list where o != null select (byte[]) o)
        //    {
        //        Array.Copy(stringData, 0, resultArray, pos, stringData.Length);
        //        pos += stringData.Length;
        //    }

        //    SendOverCom(resultArray, 0, resultArray.Length);
        //}

        private void PrintArrayList(ArrayList list)
        {
            int length = 0;
            foreach (byte[] element in list)
            {
                if (element != null)
                {
                    length += element.Length;
                }
            }

            var resultArray = new byte[length];
            var pos = 0;
            
            foreach (byte[] stringData in list)
            {
                if (stringData == null)
                {
                    continue;
                }

                Array.Copy(stringData, 0, resultArray, pos, stringData.Length);
                pos += stringData.Length;
            }

            SendOverCom(resultArray, 0, resultArray.Length);
        }

        private byte[] ProcessLine(byte[] data, int startIndex, int width) {
            var stringRle = new byte[PrintWidth * 3];

            byte prevSymbol = 0x00;
            var dataIndex = 0;
            byte symbolCount = 0;

            var state = RleState.Search;

            for (var x = 0; x < width; x++) {
                var pos = startIndex + x;
                switch (state) {
                    case RleState.Search:
                        if (x != 0 && prevSymbol == data[pos]) {
                            state = RleState.Accumulate;
                            symbolCount = 2;
                        }
                        stringRle[dataIndex++] = data[pos];
                        break;
                    case RleState.Accumulate:
                        if (prevSymbol == data[pos]) {
                            symbolCount++;
                        } else {
                            stringRle[dataIndex++] = symbolCount;
                            stringRle[dataIndex++] = data[pos];
                            state = RleState.Search;
                        }

                        if (x == (width - 1)) {
                            if (dataIndex > 2 && prevSymbol == 0x00) {
                                dataIndex -= 2;
                            } else {
                                stringRle[dataIndex++] = symbolCount;
                            }
                        }
                        break;
                }
                prevSymbol = data[pos];
            }

            int len = dataIndex;
            int arrayLength = len + 5;
            if (len > 9) {
                arrayLength++;
            }

            dataIndex = 0;
            var printData = new byte[arrayLength];
            printData[dataIndex++] = 0x1b;
            printData[dataIndex++] = 0x2a;
            printData[dataIndex++] = 0x62;

            if (len > 9) {
                printData[dataIndex++] = (byte)((len / 10) + 0x30);
            }
            printData[dataIndex++] = (byte)((len % 10) + 0x30);
            printData[dataIndex] = 0x57;


            Array.Copy(stringRle, 0, printData, (len > 9) ? 6 : 5, len);
            return printData;
        }

        private bool isPacket;
        private readonly byte[] packet = new byte[4];
        private int packetPos;

        protected override void SendRequest() {
            // Команда запроса статуса принтера VKP80
            var bytes = new byte[] {
                0x10, 0x04, 0x14
            };

            SendOverCom(bytes, 0, bytes.Length); 
        }

        protected override void ReceiveResponse(SerialResponseArgs e) {
            const byte SIGNATURE_FIRST = 0x10;
            const byte SIGNATURE_SECOND = 0x0F;
            
            if (e.data == null || e.data.Length == 0) {
                return;
            }

            byte[] source = e.data;
            foreach (byte data in source) {
                if (!isPacket && data == SIGNATURE_FIRST) {
                    isPacket = true;
                    packetPos = -1;
                    continue;
                }

                if (isPacket && packetPos == -1) {
                    if (data != SIGNATURE_SECOND) {
                        isPacket = false;
                    } else {
                        packetPos++;
                    }
                    continue;
                }
                if (isPacket) {
                    // Сохранение тела пакета
                    if (packetPos < packet.Length) {
                        packet[packetPos++] = data;
                    }

                    if (packetPos == packet.Length) {
                        // Парсинг пакета
                        var status = false;
                        foreach (byte b in packet) {
                            if (b != 0) {
                                status = true;
                                break;
                            }
                        }

                        PrinterMalfunction = status;

                        // Сброс состояния
                        isPacket = false;
                    }
                }
            }
        }
    }
}

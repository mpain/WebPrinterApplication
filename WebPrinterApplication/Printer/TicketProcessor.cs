using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using log4net;

namespace WepPrinterApplication.Printer
{
    public abstract class TicketProcessor {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PrinterDriver));

        public ITicketPrinter Printer { get; set; }


        #region Константы
        protected const string FONT_NAME = "Courier New";
        protected const string MONEY_FORMAT = "#####0.00";
        protected const string STRING_DOTS = "...";
        #endregion

        #region Переменные
        private Font fontCaption;
        private Font fontMain;

        private byte[] imgLogo;

        /// <summary>
        /// Ширина первой колонки текста при печати строки в две колонки
        /// </summary>
        private int firstColumnWidth = 15;
        public int FirstColumnWidth
        {
            get { return firstColumnWidth; }
            set { firstColumnWidth = value; }
        }

        #endregion

        #region Функции печати
        #region Печать графики
        private void GraphPrintLine(ref PrinterGraphInfo info, PrinterLineInfo line) {
            if (line.isCentered) {
                GraphPrintCentered(ref info, line);
            } else {
                GraphPrintJustified(ref info, line);
            }
        }

        private void GraphPrintCentered(ref PrinterGraphInfo info, PrinterLineInfo line) {
            var format = new StringFormat {Alignment = StringAlignment.Center};

            Graphics g = info.graphics;
            string str = (!String.IsNullOrEmpty(line.value)) ? line.label + " " + line.value : line.label;
            SizeF stringSize = g.MeasureString(str, GetGraphicalFontByType(line.fontType), new SizeF(info.width, 300), format);

            if (!info.isMeasureOnly) {
                g.DrawString(str,
                    GetGraphicalFontByType(line.fontType),
                    Brushes.Black,
                    new RectangleF(new PointF(Printer.PrintIntention * 8, info.coordY),
                    new SizeF(info.width, stringSize.Height)),
                    format);
            }
            info.coordY += stringSize.Height;
        }

        private void GraphPrintJustified(ref PrinterGraphInfo info, PrinterLineInfo line) {
            var format = new StringFormat {Alignment = StringAlignment.Near};

            Graphics g = info.graphics;

            float oldWidth = 0;
            float labelHeight = 0, valueHeight = 0;

            if (!String.IsNullOrEmpty(line.label)) {
                SizeF labelStringSize = g.MeasureString(line.label, GetGraphicalFontByType(line.fontType), new SizeF(info.width, 200), format);
                oldWidth = labelStringSize.Width;

                if (!info.isMeasureOnly)
                {
                    g.DrawString(line.label,
                        GetGraphicalFontByType(line.fontType),
                        Brushes.Black,
                        new RectangleF(new PointF(Printer.PrintIntention * 8, info.coordY),
                        new SizeF(info.width, labelStringSize.Height)),
                        format);
                }

                labelHeight = labelStringSize.Height;
            }

            if (!String.IsNullOrEmpty(line.value)) {
                var boldFont = new Font(GetGraphicalFontByType(line.fontType), FontStyle.Regular | FontStyle.Bold | FontStyle.Italic);
                format.Alignment = StringAlignment.Far;

                SizeF valueStringSize = g.MeasureString(line.value,
                    boldFont,
                    new SizeF(info.width - oldWidth, 200),
                    format);

                if (!info.isMeasureOnly) {
                    g.DrawString(line.value,
                        boldFont,
                        Brushes.Black,
                        new RectangleF(new PointF(Printer.PrintIntention * 8 + oldWidth, info.coordY),
                        new SizeF(info.width - oldWidth, valueStringSize.Height)),
                        format);
                }
                valueHeight = valueStringSize.Height;
                boldFont.Dispose();
            }
            info.coordY += Math.Max(labelHeight, valueHeight);
        }

        /// <summary>
        /// Печатает набор строк в виде графики
        /// </summary>
        /// <param name="lines">Строки для вывода</param>
        private void GraphPrintLines(IEnumerable<PrinterLineInfo> lines) {
            int bmpWidth = Printer.PrintWidth * 8;
            int width = bmpWidth - 2 * Printer.PrintIntention * 8;

            int bmpHeight;
            var measureImg = new Bitmap(bmpWidth, 1, PixelFormat.Format32bppPArgb);
            try {
                Graphics g = Graphics.FromImage(measureImg);
                try {
                    var info = new PrinterGraphInfo(g, width) {isMeasureOnly = true};

                    foreach (var line in lines) {
                        GraphPrintLine(ref info, line);
                    }

                    bmpHeight = (int)info.coordY;
                } finally {
                    g.Dispose();
                }
            } finally {
                measureImg.Dispose();
            }

            var img = new Bitmap(bmpWidth, bmpHeight, PixelFormat.Format32bppPArgb);
            try {
                Graphics g = Graphics.FromImage(img);
                try {
                    g.FillRectangle(Brushes.White, new Rectangle(0, 0, img.Width, img.Height));

                    var info = new PrinterGraphInfo(g, width);
                    foreach (var line in lines) {
                        GraphPrintLine(ref info, line);
                    }

                    var data = GraphConvertToMonochrome(img);
                    img.Save(@"C:\ticket.bmp", ImageFormat.Png);
                    
                    Printer.PrintMonochromeBitmap(data, img.Height, img.Width / 8);
                } finally {
                    g.Dispose();
                }
            } finally {
                img.Dispose();
            }
        }

        /// <summary>
        /// Конвертирует исходное изображение в черно-белый формат
        /// </summary>
        /// <param name="img">Исходное изображение</param>
        /// <returns>Конвертированное изображение</returns>
        private static byte[] GraphConvertToMonochrome(Bitmap img) {
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            var bmpData = img.LockBits(rect, ImageLockMode.ReadOnly, img.PixelFormat);
            try {
                /* Размерность результирующего массива */
                int bytes = (img.Width * img.Height) / 8;

                /* Результирующий массив */
                var dataValues = new byte[bytes];

                /* Исходный массив - получается копирование данных из объекта Bitmap */
                var source = new byte[bmpData.Height * bmpData.Stride];
                Marshal.Copy(bmpData.Scan0, source, 0, source.Length);

                /* Текущий индекс выходного байтового массива */
                int arrPtr = 0;
                for (int y = 0; y < bmpData.Height; y++) {
                    /* Рабочий выходной байт */
                    byte b = 0;
                    for (int x = 0; x < bmpData.Width; x++) {
                        /* Местоположение текущего пикселя */
                        int index = y * bmpData.Stride + x * 4;
                        /* Цвет пикселя */
                        Color pixelColor = Color.FromArgb(source[index + 2],
                                                            source[index + 1],
                                                            source[index + 0]);

                        /* Принятие решения о монохромном цвете пикселя значению его яркости */
                        if (pixelColor.GetBrightness() < 0.7) {
                            b |= 1;
                        }

                        /* Проверка на достижение границы выходного байта */
                        if ((index > 0) && ((index >> 2) % 8 == 0)) {
                            dataValues[arrPtr++] = b;
                        }

                        /* Сдвиг выходного байта влево на пиксель*/
                        b <<= 1;
                    }
                }
                return dataValues;
            } finally {
                img.UnlockBits(bmpData);
            }
        }
        #endregion

        #region Печать текста
        private void TextPrintLines(IEnumerable<PrinterLineInfo> lines) {
            var info = new PrinterTextInfo(Printer.PrintWidth / 2);
            foreach (var line in lines) {
                TextPrintLine(ref info, line);
            }

            Printer.PrintByteArray(Encoding.GetEncoding(1251).GetBytes(info.data));
        }

        private void TextPrintLine(ref PrinterTextInfo info, PrinterLineInfo line) {
            TextSetFont(ref info, line);
            if (line.isCentered) {
                TextAddCenteredLine(ref info, line);
            } else {
                TextAddJustifiedLine(ref info, line);
            }
            TextAddFeed(ref info);
        }

        public void TextAddCenteredLine(ref PrinterTextInfo info, PrinterLineInfo line) {
            string str = (!String.IsNullOrEmpty(line.value)) ? line.label + " " + line.value : line.label;

            string text;
            int startIndex = -1;
            bool isWorking = true;

            while (isWorking) {
                int oldIndex = startIndex + 1;
                startIndex = str.IndexOf('\n', oldIndex, str.Length - oldIndex);
                if (startIndex < 0) {
                    text = str.Substring(oldIndex);
                    isWorking = false;
                } else {
                    text = str.Substring(oldIndex, startIndex - oldIndex);
                }

                int len = text.Length;
                int spaces, i;
                string tail;

                for (i = 0; len - i > info.width; ) {
                    int findPos = text.LastIndexOf(' ', i + info.width, info.width);

                    int width = (findPos < 0) ? info.width : findPos - i;
                    tail = text.Substring(i, width);
                    spaces = (info.width - tail.Length) / 2;
                    info.data += tail.PadLeft(spaces + tail.Length, ' ');

                    TextAddNewLine(ref info);
                }

                tail = text.Substring(i);
                spaces = (info.width - tail.Length) / 2;
                info.data += tail.PadLeft(spaces + tail.Length, ' ');
                TextAddNewLine(ref info);
            }
        }

        public void TextAddJustifiedLine(ref PrinterTextInfo info, PrinterLineInfo line) {
            logger.Debug(line.label + " " + line.value + " " + line.label.Length + " " + line.value.Length);
            if (line.label.Length > firstColumnWidth) {
                line.label = line.label.Substring(0, firstColumnWidth - STRING_DOTS.Length);
                line.label += STRING_DOTS;
            }

            info.data += (line.label + " ");

            int firstLen = info.width - (line.label.Length + 1) % info.width;

            bool isFirst = true;

            int len = line.value.Length;
            int spaces, i, stringWidth;
            string tail;

            for (i = 0; len - i > ((isFirst) ? firstLen : info.width); ) {
                stringWidth = (isFirst) ? firstLen : info.width;
                if (isFirst) {
                    isFirst = false;
                }

                int findPos = line.value.LastIndexOf(' ', i + stringWidth, stringWidth);

                int width = (findPos < 0) ? stringWidth : findPos - i;
                tail = line.value.Substring(i, width);
                spaces = stringWidth - tail.Length;
                info.data += tail.PadLeft(spaces + tail.Length, '.');

                i += width;
                TextAddNewLine(ref info);
            }

            stringWidth = (isFirst) ? firstLen : info.width;
            tail = line.value.Substring(i);
            spaces = stringWidth - tail.Length;
            info.data += tail.PadLeft(spaces + tail.Length, '.');
            TextAddNewLine(ref info);
        }

        public void TextAddNewLine(ref PrinterTextInfo info) {
            info.data += '\n';
        }

        public void TextAddFeed(ref PrinterTextInfo info) {
            info.data += (char)0x1D;
            info.data += (char)0x05;
        }

        public void TextSetFont(ref PrinterTextInfo info, PrinterLineInfo line) {
            info.data += (line.fontType == PrinterLineFontType.Caption) ? (char)Printer.CaptionTextFont : (char)Printer.MainTextFont;
        }
        #endregion
        #endregion

        #region Инициализация/деинициализация

        public void Start(String logoPath)
        {
            fontCaption = new Font(FONT_NAME, Printer.CaptionFontSize, FontStyle.Bold);
            fontMain = new Font(FONT_NAME, Printer.MainFontSize, FontStyle.Bold);


            imgLogo = LoadImage(Path.Combine(Application.StartupPath, logoPath));
        }

        public void Stop()
        {
            ThreadTerminate();

            if (fontCaption != null)
            {
                fontCaption.Dispose();
            }

            if (fontMain != null)
            {
                fontMain.Dispose();
            }
        }
        #endregion

        #region Поток печати
        private Thread printThread;

        protected void ThreadStart(List<PrinterLineInfo> infos) {
            try {
                ThreadTerminate();

                // ReSharper disable RedundantDelegateCreation
                printThread = new Thread(new ParameterizedThreadStart(Worker))
                {
                    Name = "Print Thread",
                    Priority = ThreadPriority.Lowest
                };
                // ReSharper restore RedundantDelegateCreation


                // В случае вывода графики необходимо снижение приоритета потока, т.к. это может привести к
                // подтормаживанию анимации при выводе графики пользовательского интерфейса

                printThread.Start(infos);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        protected void ThreadTerminate() {
            try {
                if (printThread != null && printThread.IsAlive) {
                    printThread.Abort();

                    while (printThread.IsAlive) {
                        Thread.Sleep(0);
                    }
                }
                printThread = null;
            } catch (Exception ex) {
                logger.Error("Print thread closing error:" + ex.Message);
            }
        }

        private void Worker(object parameter) {
            Monitor.Enter(Printer.LockObject);

            try {
                if (parameter is List<PrinterLineInfo>) {
                    var infos = parameter as List<PrinterLineInfo>;

                    Printer.PrintStart();
                    PrintLogo();
                    if (Printer.Mode == PrintMode.Graphic) {
                        GraphPrintLines(infos.ToArray());
                    } else {
                        TextPrintLines(infos.ToArray());
                    }
                    Printer.PrintEnd();
                }
            } catch (Exception e) {
                logger.Error("Print worker thread terminated cause error: " + e);
            } finally {
                Monitor.Exit(Printer.LockObject);
            }
        }

        #endregion

        #region Функции работы со шрифтами
        private Font GetGraphicalFontByType(PrinterLineFontType type) {
            switch (type) {
                case PrinterLineFontType.Caption:
                    return fontCaption;
                default:
                    return fontMain;
            }
        }
        #endregion

        #region Строковые функции
        protected string AddSpace(string source) {
            return source + " ";
        }

        protected string AddColon(string source) {
            return source + ":";
        }

        //protected string AddCurrency(string source) {
        //    return source + Settings.TicketCurrency;
        //}
        #endregion

        #region Загрузка и печать логотипа
        private byte[] LoadImage(string path)
        {
            logger.Info(String.Format("Loading image: {0}", path));
            try
            {
                if (path != String.Empty)
                {
                    var img = (Bitmap) Image.FromFile(path);
                    int printArea = (Printer.PrintWidth - 2*Printer.PrintIntention)*8;
                    double k = (img.Width > printArea) ? printArea/img.Width : 1;

                    var temp = new Bitmap(Printer.PrintWidth*8, (int) (img.Height*k), PixelFormat.Format32bppPArgb);
                    Graphics g = Graphics.FromImage(temp);
                    g.FillRectangle(Brushes.White, new Rectangle(0, 0, temp.Width, temp.Height));

                    g.DrawImage(img,
                                new Rectangle(Printer.PrintIntention*8, 0, (int) (img.Width*k), (int) (img.Height*k)),
                                0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                    img.Dispose();
                    g.Dispose();

                    return GraphConvertToMonochrome(temp);
                }
            } catch (Exception ex)
            {
                logger.Error("Failed during loading image logo.", ex);
            }
            return null;
        }

        private void PrintLogo() {
            if (imgLogo != null) {
                Printer.PrintMonochromeBitmap(imgLogo, imgLogo.Length / Printer.PrintWidth, Printer.PrintWidth);
            }
        }

        #endregion
    }

}

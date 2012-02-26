using System;
using System.Drawing;

namespace WepPrinterApplication.Printer
{
    public enum PrintMode {
        Graphic,
        Text
    }

    public struct PrinterTextInfo {
        public string data;
        public int width;
        public PrinterTextInfo(int width) {
            this.width = width;
            this.data = String.Empty;
        }
    }

    public struct PrinterGraphInfo {
        public bool isMeasureOnly;
        public Graphics graphics;
        public float coordY;
        public float width;

        public PrinterGraphInfo(Graphics g, float width) {
            this.graphics = g;
            this.width = width;
            this.coordY = 0;
            this.isMeasureOnly = false;
        }

    }
}

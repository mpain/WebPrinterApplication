namespace WepPrinterApplication.Printer
{
    public enum PrinterLineFontType {
        Default,
        Caption
    }

    public class PrinterLineInfo {
        public bool isCentered;
        public PrinterLineFontType fontType;
        public string label;
        public string value;

        public PrinterLineInfo(bool isCentered, PrinterLineFontType fontType, string label, string value) {
            this.isCentered = isCentered;
            this.fontType = fontType;
            this.label = label;
            this.value = value;
        }

        public PrinterLineInfo(bool isCentered, PrinterLineFontType fontType, string label)
            : this(isCentered, fontType, label, null) {
        }

        public override string ToString()
        {
            return string.Format("IsCentered: {0}, FontType: {1}, Label: '{2}', Value: '{3}'", isCentered, fontType, label, value);
        }
    }
}

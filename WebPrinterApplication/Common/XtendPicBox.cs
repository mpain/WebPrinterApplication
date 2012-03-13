using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace WebPrinterApplication.Common
{

    /// <summary>
    /// Extend the panel control to provide a
    /// scrollable picturebox control
    /// </summary>
    public partial class XtendPicBox : Panel
    {

#region Local Member Variables


        PictureBox innerPicture = new PictureBox();
        private bool mAutoScroll = true;

#endregion


#region Constructor


        /// <summary>
        /// Constructor - set up the inner
        /// picture box so that is plants itself
        /// in the top left corner of the panel 
        /// and so that its size mode is always
        /// set to normal.  At normal, the picture
        /// will appear at full size.
        /// </summary>
        public XtendPicBox()
        {
            InitializeComponent();

            // add the inner picture
            innerPicture.Top = 0;
            innerPicture.Left = 0;
            innerPicture.SizeMode = PictureBoxSizeMode.Normal;
            
            Controls.Add(innerPicture);
        }

#endregion


#region Properties


        /// <summary>
        /// Allow control consumer to set the image
        /// used in the internal picture box through
        /// this public and browsable property -
        /// set the editor to the file name editor
        /// to make the control easier to use at
        /// design time (to provide an interface for
        /// browsing to and selecting the image file
        /// </summary>
        [Browsable(false)]
        public Image Image
        {
            get
            {
                return (innerPicture.Image != null) ? innerPicture.Image : null;
            }
            set
            {
                UpdateImage(value);
            }
        }

        private void UpdateImage(Image image)
        {
            if (innerPicture == null)
            {
                return;
            }

            if (innerPicture.InvokeRequired)
            {
                innerPicture.Invoke(new UpdateImageDelegate(UpdateImage), new object[] { image });
            }
            else
            {
                if (innerPicture.Image != null)
                {
                    innerPicture.Image.Dispose();
                }

                innerPicture.Image = image;

                if (innerPicture.Image != null)
                {
                    innerPicture.Size = innerPicture.Image.Size;
                }
            }
        }

        public delegate void UpdateImageDelegate(Image image);
        /// <summary>
        /// Override the autoscroll property
        /// and use the browsable attribute
        /// to hide it from control consumer -
        /// The property will always be set
        /// to true so that the picturebox will
        /// always scroll
        /// </summary>
        [Browsable(false)]
        public override bool AutoScroll
        {
            get
            {
                return mAutoScroll;
            }
            set
            {
                mAutoScroll = value;
            }
        }

#endregion


    }
}

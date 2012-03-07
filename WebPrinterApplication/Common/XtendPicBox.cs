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
        private string mPictureFile = string.Empty;
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
        [Category("Image File")]
        [Browsable(true)]
        [Description("Set path to image file.")]
        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor),
          typeof(System.Drawing.Design.UITypeEditor))]
        public string PictureFile
        {
            get
            {
                return mPictureFile;
            }
            set
            {
                mPictureFile = value;

                if (!string.IsNullOrEmpty(mPictureFile))
                {
                    
                    // set the image to the image file
                    innerPicture.Image = Image.FromFile(mPictureFile);
                    UpdateInnerSize(innerPicture.Image.Size);
                }
                else
                {
                    if (innerPicture.Image != null)
                    {
                        innerPicture.Image.Dispose();
                        innerPicture.Image = null;
                    }
                }
            }
        }

        private void UpdateInnerSize(Size size)
        {
            if (innerPicture.InvokeRequired)
            {
                innerPicture.Invoke(new SetSize(UpdateInnerSize), new object[] { size });
            }
            else
            {
                innerPicture.Size = size;
            }
        }

        public delegate void SetSize(Size size);
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

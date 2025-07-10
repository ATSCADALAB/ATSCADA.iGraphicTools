using ATSCADA.ToolExtensions.Data;
using ATSCADA.ToolExtensions.ExtensionMethods;
using ATSCADA.ToolExtensions.TagCollection;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;

namespace ATSCADA.iGraphicTools.Image
{
    [ToolboxBitmap(typeof(System.Windows.Forms.PictureBox))]
    public class iImage3Pos : System.Windows.Forms.PictureBox
    {
        private readonly string locationImageDefault = @"C:\Program Files\ATPro\ATSCADA\GraphicLib\BUTTONS\Buttons1\large\ButtonGreen.png";

        private readonly ImageXMLFile imageXMLFile = new ImageXMLFile();

        private ITag tagControl;

        private DataTool dataPosition1;

        private DataTool dataPosition2;

        private DataTool dataPosition3;

        private System.Drawing.Image position1Image;

        private System.Drawing.Image position2Image;

        private System.Drawing.Image position3Image;

        private string position1ImageName;

        private string position2ImageName;

        private string position3ImageName;

        private string imageCollection = "";

        private iDriver driver;

        [Category("ATSCADA Settings")]
        [Description("Select driver object.")]
        public iDriver Driver
        {
            get => driver;
            set
            {
                if (driver != null) driver.ConstructionCompleted -= Driver_ConstructionCompleted;
                driver = value;
                if (driver != null) driver.ConstructionCompleted += Driver_ConstructionCompleted;
            }
        }

        [Category("ATSCADA Settings")]
        [Description("Select tag for ATSCADA control.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string TagName { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Enter position 1 value, the value will be written between 2 quotation marks (\"\").\nIf select from SmartTag editor, the position 1 value will by the value of tag is selected.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string Position1 { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Enter position 2 value, the value will be written between 2 quotation marks (\"\").\nIf select from SmartTag editor, the position 2 value will by the value of tag is selected.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string Position2 { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Enter position 3 value, the value will be written between 2 quotation marks (\"\").\nIf select from SmartTag editor, the position 3 value will by the value of tag is selected.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string Position3 { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Choose images.")]
        [Editor(typeof(Image3PosSettingsEditor), typeof(UITypeEditor))]
        public string ImageCollection
        {
            get => this.imageCollection;
            set
            {
                if (this.imageCollection == value) return;
                this.imageCollection = value;

                Deserialization();                
            }
        }

        public iImage3Pos()
        {
            this.Size = new Size(100, 100);
            this.BackColor = Color.Transparent;
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            if (File.Exists(this.locationImageDefault))
                this.Image = System.Drawing.Image.FromFile(this.locationImageDefault);
        }

        private void Deserialization()
        {
            try
            {
                var dataSerialization = this.imageCollection.Split(',');
                if (dataSerialization.Length != 3) return;

                this.position1ImageName = dataSerialization[0];
                this.position2ImageName = dataSerialization[1];
                this.position3ImageName = dataSerialization[2];

                var initImageName = this.imageXMLFile.GetImageLocationPath(this.position1ImageName);
                if (File.Exists(initImageName))
                    this.Image = System.Drawing.Image.FromFile(initImageName);
                else
                {
                    initImageName = this.imageXMLFile.GetAbsoluteAdd(this.position1ImageName, "____");
                    if (File.Exists(initImageName))
                        this.Image = System.Drawing.Image.FromFile(initImageName);
                }
            }
            catch { return; }
        }

        private void Driver_ConstructionCompleted()
        {
            this.tagControl = DriverExtensionMethod.GetTagByName(this.driver, TagName);
            if (this.tagControl == null) return;            
            
            this.dataPosition1 = new DataTool(this.driver, Position1);
            this.dataPosition2 = new DataTool(this.driver, Position2);
            this.dataPosition3 = new DataTool(this.driver, Position3);

            var locationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\GraphicLib\\";

            var position1ImagePath = locationPath + this.position1ImageName;
            if (File.Exists(position1ImagePath))
                this.position1Image = System.Drawing.Image.FromFile(position1ImagePath);

            var position2ImagePath = locationPath + this.position2ImageName;
            if (File.Exists(position2ImagePath))
                this.position2Image = System.Drawing.Image.FromFile(position2ImagePath);

            var position3ImagePath = locationPath + this.position3ImageName;
            if (File.Exists(position3ImagePath))
                this.position3Image = System.Drawing.Image.FromFile(position3ImagePath);

            this.tagControl.TagValueChanged += (sender, e) => UpdateImage();
            this.tagControl.TagStatusChanged += (sender, e) => UpdateImage();
            UpdateImage();
        }        

        private void UpdateImage()
        {            
            if (this.tagControl.Value == this.dataPosition1.Value)
                this.SynchronizedInvokeAction(() => this.Image = this.position1Image);
            else if (this.tagControl.Value == this.dataPosition2.Value)
                this.SynchronizedInvokeAction(() => this.Image = this.position2Image);
            else if (this.tagControl.Value == this.dataPosition3.Value)
                this.SynchronizedInvokeAction(() => this.Image = this.position3Image);
        }
    }
}

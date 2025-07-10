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
    public class iImage : System.Windows.Forms.PictureBox
    {
        private readonly string locationImageDefault = @"C:\Program Files\ATPro\ATSCADA\GraphicLib\BUTTONS\Buttons1\large\ButtonGreen.png";

        private readonly ImageXMLFile imageXMLFile = new ImageXMLFile();

        private ITag tagControl;

        private DataTool dataActive;

        private DataTool dataPassive;

        private System.Drawing.Image activeImage;

        private System.Drawing.Image passiveImage;

        private string activeImageName;

        private string passiveImageName;

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
        [Description("Enter active value, the value will be written between 2 quotation marks (\"\").\nIf select from SmartTag editor, the active value will by the value of tag is selected.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string ActiveValue { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Enter passive value, the value will be written between 2 quotation marks (\"\").\nIf select from SmartTag editor, the passive value will by the value of tag is selected.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string PassiveValue { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Choose images.")]
        [Editor(typeof(ImageSettingsEditor), typeof(UITypeEditor))]
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

        public iImage()
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
                if (dataSerialization.Length != 2) return;

                this.activeImageName = dataSerialization[0];
                this.passiveImageName = dataSerialization[1];

                var initImageName = this.imageXMLFile.GetImageLocationPath(this.passiveImageName);                
                if (File.Exists(initImageName))
                    this.Image = System.Drawing.Image.FromFile(initImageName);
                else
                {
                    initImageName = this.imageXMLFile.GetAbsoluteAdd(this.passiveImageName, "____");
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

            this.dataActive = new DataTool(this.driver, ActiveValue);
            this.dataPassive = new DataTool(this.driver, PassiveValue);

            var locationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\GraphicLib\\";

            var activeImagePath = locationPath + this.activeImageName;
            if(File.Exists(activeImagePath))
                this.activeImage = System.Drawing.Image.FromFile(activeImagePath);

            var passiveImagePath = locationPath + this.passiveImageName;
            if (File.Exists(passiveImagePath))
                this.passiveImage = System.Drawing.Image.FromFile(passiveImagePath);

            this.tagControl.TagValueChanged += (sender, e) => UpdateImage();
            this.tagControl.TagStatusChanged += (sender, e) => UpdateImage();
            UpdateImage();
        }        

        private void UpdateImage()
        {            
            if (this.tagControl.Value == this.dataActive.Value)
                this.SynchronizedInvokeAction(() => this.Image = this.activeImage);
            else if (this.tagControl.Value == this.dataPassive.Value)
                this.SynchronizedInvokeAction(() => this.Image = this.passiveImage);            
        }
    }
}

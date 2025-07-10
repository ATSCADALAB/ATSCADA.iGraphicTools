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
    [ToolboxBitmap(typeof(System.Windows.Forms.Button))]
    public class iImagePush : System.Windows.Forms.PictureBox
    {
        private readonly string locationImageDefault = @"C:\Program Files\ATPro\ATSCADA\GraphicLib\BUTTONS\Buttons1\large\ButtonGreen.png";

        private readonly ImageXMLFile imageXMLFile = new ImageXMLFile();

        private ITag tagControl;

        private DataTool dataPress;

        private DataTool dataNormal;

        private System.Drawing.Image normalImage;

        private System.Drawing.Image pressImage;

        private string normalImageName;

        private string pressImageName;

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
        [Description("Enter press value, the value will be written between 2 quotation marks (\"\").\nIf select from SmartTag editor, the value will by the value of tag is selected.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string PressValue { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Enter press value, the value will be written between 2 quotation marks (\"\").\nIf select from SmartTag editor, the value will by the value of tag is selected.")]
        [Editor(typeof(SmartTagEditor), typeof(UITypeEditor))]
        public string NormalValue { get; set; }

        [Category("ATSCADA Settings")]
        [Description("Choose images.")]
        [Editor(typeof(PushSettingsEditor), typeof(UITypeEditor))]
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

        public iImagePush()
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

                this.normalImageName = dataSerialization[0];
                this.pressImageName = dataSerialization[1];

                var initImageName = this.imageXMLFile.GetImageLocationPath(this.normalImageName);
                if (File.Exists(initImageName))
                    this.Image = System.Drawing.Image.FromFile(initImageName);
                else
                {
                    initImageName = this.imageXMLFile.GetAbsoluteAdd(this.normalImageName, "____");
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

            this.dataNormal = new DataTool(this.driver, NormalValue);
            this.dataPress = new DataTool(this.driver, PressValue);

            var locationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\GraphicLib\\";

            var normalImagePath = locationPath + this.normalImageName;
            if (File.Exists(normalImagePath))
                this.normalImage = System.Drawing.Image.FromFile(normalImagePath);

            var pressImagePath = locationPath + this.pressImageName;
            if (File.Exists(pressImagePath))
                this.pressImage = System.Drawing.Image.FromFile(pressImagePath);

            ActionWrite();
        }

        
        private void ActionWrite()
        {
            if (this.tagControl is Tag externalTag && externalTag.AccessRight == "ReadOnly") return;

            this.MouseDown += (sender, e) => ActionPress();
            this.MouseUp += (sender, e) => ActionRelease();
        }

        private void ActionPress()
        {
            if (this.pressImage == null) return;
            this.SynchronizedInvokeAction(() =>
            {
                this.tagControl.ASynWrite(this.dataPress.Value);
                this.Image = this.pressImage;
            });
        }   
        
        private void ActionRelease()
        {
            if (this.normalImage == null) return;
            this.SynchronizedInvokeAction(() =>
            {
                this.tagControl.ASynWrite(this.dataNormal.Value);
                this.Image = this.normalImage;
            });
        }
    }
}

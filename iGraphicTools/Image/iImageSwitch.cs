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
    public class iImageSwitch : System.Windows.Forms.PictureBox
    {
        private readonly string locationImageDefault = @"C:\Program Files\ATPro\ATSCADA\GraphicLib\BUTTONS\Buttons1\large\ButtonGreen.png";

        private readonly ImageXMLFile imageXMLFile = new ImageXMLFile();

        private ITag tagControl;

        private DataTool dataPosition1;

        private DataTool dataPosition2;

        private System.Drawing.Image position1Image;

        private System.Drawing.Image position2Image;

        private string position1ImageName;

        private string position2ImageName;

        private string imageCollection = "";

        private bool isWritting = false;

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
        [Description("Choose images.")]
        [Editor(typeof(SwitchSettingsEditor), typeof(UITypeEditor))]
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

        public iImageSwitch()
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

                this.position1ImageName = dataSerialization[0];
                this.position2ImageName = dataSerialization[1];

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
            
            var locationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\GraphicLib\\";

            var position1ImagePath = locationPath + this.position1ImageName;
            if (File.Exists(position1ImagePath))
                this.position1Image = System.Drawing.Image.FromFile(position1ImagePath);

            var position2ImagePath = locationPath + this.position2ImageName;
            if (File.Exists(position2ImagePath))
                this.position2Image = System.Drawing.Image.FromFile(position2ImagePath);

            ActionWrite();
            this.tagControl.TagValueChanged += (sender, e) => UpdateImage();
            this.tagControl.TagStatusChanged += (sender, e) => UpdateImage();
            UpdateImage();
        }

        
        private void ActionWrite()
        {
            if (this.tagControl is Tag externalTag && externalTag.AccessRight == "ReadOnly") return;
            this.Click += (sender, e) => WriteValue();
        }

        private void WriteValue()
        {
            if (isWritting) return;
            this.isWritting = true;

            if (this.tagControl.Value == this.dataPosition1.Value)
                this.tagControl.ASynWrite(this.dataPosition2.Value);
            else if (this.tagControl.Value == this.dataPosition2.Value)
                this.tagControl.ASynWrite(this.dataPosition1.Value);
            else
            {
                this.isWritting = false;
                return;
            }

            this.SynchronizedInvokeAction(async () =>
            {
                this.Visible = false;
                await System.Threading.Tasks.Task.Delay(50);
                this.Visible = true;

                this.isWritting = false;
            });
        }

        private void UpdateImage()
        {            
            if (this.tagControl.Value == this.dataPosition1.Value)
                this.SynchronizedInvokeAction(() => this.Image = this.position1Image);
            else if (this.tagControl.Value == this.dataPosition2.Value)
                this.SynchronizedInvokeAction(() => this.Image = this.position2Image);           
        }
    }
}

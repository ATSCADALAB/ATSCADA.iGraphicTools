using System;
using System.IO;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.Image
{
    public partial class frmImageSettings : Form
    {
        private readonly ImageXMLFile imageXMLFile = new ImageXMLFile();

        private string activeImagePath = string.Empty;

        private string passiveImagePath = string.Empty;
        
        public bool IsCanceled { get; set; }

        public string DataSerialization { get; set; } = "";

        public frmImageSettings()
        {
            InitializeComponent();            

            this.txtActive.Enabled = false;
            this.txtPassive.Enabled = false;

            this.Load += FrmAdderSettings_Load;

            this.btnActive.Click += BtnActive_Click;
            this.btnPassive.Click += BtnPassive_Click;

            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;            
        }
       
        private void FrmAdderSettings_Load(object sender, EventArgs e)
        {
            if (DataSerialization == "") return;

            var itemDataConverters = DataSerialization.Split(',');
            var countItems = itemDataConverters.Length;
            if (countItems != 2) return;

            this.txtActive.Text = itemDataConverters[0];
            this.txtPassive.Text = itemDataConverters[1];
        }

        private void BtnActive_Click(object sender, EventArgs e)
        {
            string path = @"C:\Program Files\ATPro\ATSCADA\GraphicLib\";
            if (!Directory.Exists(path))
            {
                path = @"C:\Program Files (x86)\ATPro\ATSCADA\GraphicLib\";
                if (!Directory.Exists(path))
                    path = @"C:\";
            }

            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = path,
                Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif",
                Title = "ATSCADA - Select Image"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var fileName = openFileDialog.FileName;

            try
            {
                this.txtActive.Text = fileName.Split('\\')[openFileDialog.FileName.Split('\\').Length - 1];
            }
            finally
            {
                this.activeImagePath = fileName;
            } 
        }

        private void BtnPassive_Click(object sender, EventArgs e)
        {
            string path = @"C:\Program Files\ATPro\ATSCADA\GraphicLib\";
            if (!Directory.Exists(path))
            {
                path = @"C:\Program Files (x86)\ATPro\ATSCADA\GraphicLib\";
                if (!Directory.Exists(path))
                    path = @"C:\";
            }

            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = path,
                Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif",
                Title = "ATSCADA - Select Image"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var fileName = openFileDialog.FileName;

            try
            {
                this.txtPassive.Text = fileName.Split('\\')[openFileDialog.FileName.Split('\\').Length - 1];
            }
            finally
            {
                this.passiveImagePath = fileName;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var activeImageName = this.txtActive.Text;
            var passiveImageName = this.txtPassive.Text;

            DataSerialization = $"{activeImageName},{passiveImageName}";
           
            if (!string.IsNullOrEmpty(activeImageName))
                this.imageXMLFile.AppendXML(activeImageName, "____", this.activeImagePath);

            if (!string.IsNullOrEmpty(passiveImagePath))
                this.imageXMLFile.AppendXML(passiveImageName, "____", this.passiveImagePath);

            this.IsCanceled = false;
            this.Hide();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.IsCanceled = true;
            this.Hide();
        }
    }
}

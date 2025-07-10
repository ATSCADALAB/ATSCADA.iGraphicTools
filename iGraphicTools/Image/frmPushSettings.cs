using System;
using System.IO;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.Image
{
    public partial class frmPushSettings : Form
    {
        private readonly ImageXMLFile imageXMLFile = new ImageXMLFile();

        private string normalImagePath = string.Empty;

        private string pressImagePath = string.Empty;

        public bool IsCanceled { get; set; }

        public string DataSerialization { get; set; } = "";

        public frmPushSettings()
        {
            InitializeComponent();

            this.txtNormal.Enabled = false;
            this.txtPress.Enabled = false;

            this.Load += FrmAdderSettings_Load;

            this.btnNormal.Click += BtnNormal_Click;
            this.btnPress.Click += BtnPress_Click;

            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }

        private void FrmAdderSettings_Load(object sender, EventArgs e)
        {
            if (DataSerialization == "") return;

            var itemDataConverters = DataSerialization.Split(',');
            var countItems = itemDataConverters.Length;
            if (countItems != 2) return;

            this.txtNormal.Text = itemDataConverters[0];
            this.txtPress.Text = itemDataConverters[1];
        }

        private void BtnNormal_Click(object sender, EventArgs e)
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
                this.txtNormal.Text = fileName.Split('\\')[openFileDialog.FileName.Split('\\').Length - 1];
            }
            finally
            {
                this.normalImagePath = fileName;
            }
        }

        private void BtnPress_Click(object sender, EventArgs e)
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
                this.txtPress.Text = fileName.Split('\\')[openFileDialog.FileName.Split('\\').Length - 1];
            }
            finally
            {
                this.pressImagePath = fileName;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var normalImageName = this.txtNormal.Text;
            var pressImageName = this.txtPress.Text;

            DataSerialization = $"{normalImageName},{pressImageName}";
            var imageXMLfile = new ImageXMLFile();

            if (!string.IsNullOrEmpty(normalImageName))
                this.imageXMLFile.AppendXML(normalImageName, "____", this.normalImagePath);

            if (!string.IsNullOrEmpty(pressImagePath))
                this.imageXMLFile.AppendXML(pressImageName, "____", this.pressImagePath);

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

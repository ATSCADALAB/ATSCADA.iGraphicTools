using System;
using System.IO;
using System.Windows.Forms;

namespace ATSCADA.iGraphicTools.Image
{
    public partial class frmImage3PosSettings : Form
    {
        private readonly ImageXMLFile imageXMLFile = new ImageXMLFile();

        private string position1ImagePath = string.Empty;

        private string position2ImagePath = string.Empty;

        private string position3ImagePath = string.Empty;

        public bool IsCanceled { get; set; }

        public string DataSerialization { get; set; } = "";
        public frmImage3PosSettings()
        {
            InitializeComponent();

            this.Load += FrmImage3PosSettings_Load;

            this.txtPosition1.Enabled = false;
            this.txtPosition2.Enabled = false;
            this.txtPosition3.Enabled = false;

            this.btnPosition1.Click += BtnPosition1_Click;
            this.btnPosition2.Click += BtnPosition2_Click;
            this.btnPosition3.Click += BtnPosition3_Click;

            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }

        private void FrmImage3PosSettings_Load(object sender, EventArgs e)
        {
            if (DataSerialization == "") return;

            var itemDataConverters = DataSerialization.Split(',');
            var countItems = itemDataConverters.Length;
            if (countItems != 3) return;

            this.txtPosition1.Text = itemDataConverters[0];
            this.txtPosition2.Text = itemDataConverters[1];
            this.txtPosition3.Text = itemDataConverters[2];
        }

        private void BtnPosition1_Click(object sender, EventArgs e)
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
                this.txtPosition1.Text = fileName.Split('\\')[openFileDialog.FileName.Split('\\').Length - 1];
            }
            finally
            {
                this.position1ImagePath = fileName;
            }
        }

        private void BtnPosition2_Click(object sender, EventArgs e)
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
                this.txtPosition2.Text = fileName.Split('\\')[openFileDialog.FileName.Split('\\').Length - 1];
            }
            finally
            {
                this.position2ImagePath = fileName;
            }
        }

        private void BtnPosition3_Click(object sender, EventArgs e)
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
                this.txtPosition3.Text = fileName.Split('\\')[openFileDialog.FileName.Split('\\').Length - 1];
            }
            finally
            {
                this.position3ImagePath = fileName;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var position1ImageName = this.txtPosition1.Text;
            var position2ImageName = this.txtPosition2.Text;
            var position3ImageName = this.txtPosition3.Text;

            DataSerialization = $"{position1ImageName},{position2ImageName},{position3ImageName}";           

            if (!string.IsNullOrEmpty(position1ImageName))
                this.imageXMLFile.AppendXML(position1ImageName, "____", this.position1ImagePath);

            if (!string.IsNullOrEmpty(position2ImageName))
                this.imageXMLFile.AppendXML(position2ImageName, "____", this.position2ImagePath);

            if (!string.IsNullOrEmpty(position3ImageName))
                this.imageXMLFile.AppendXML(position3ImageName, "____", this.position3ImagePath);

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

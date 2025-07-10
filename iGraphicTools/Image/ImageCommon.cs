using ATSCADA.ToolExtensions.PropertyEditor;
using EnvDTE;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

namespace ATSCADA.iGraphicTools.Image
{
    public class ImageXMLFile
    {
        public void AppendXML(string name, string tag, string address)
        {
            try
            {
                if (string.IsNullOrEmpty(name) ||
                    string.IsNullOrEmpty(tag) ||
                    string.IsNullOrEmpty(address)) return;

                string path = "C:\\Program Files\\ATPro\\ATSCADA\\DesignerFiles\\";
                if (!Directory.Exists(path))
                {
                    path = "C:\\Program Files (x86)\\ATPro\\ATSCADA\\DesignerFiles\\";
                    if (!Directory.Exists(path))
                        return;
                }
                string str = path + "Image_file.XML";
                if (!File.Exists(str))
                    File.WriteAllText(str, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r<Root>\r</Root>");

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(str);              

                XmlNode xmlNode = xmlDocument.SelectSingleNode("Root");
                XmlElement element = xmlDocument.CreateElement("Img");
                XmlAttribute attribute1 = xmlDocument.CreateAttribute(nameof(name));
                attribute1.Value = name;
                element.Attributes.Append(attribute1);
                XmlAttribute attribute2 = xmlDocument.CreateAttribute(nameof(tag));
                attribute2.Value = tag;
                element.Attributes.Append(attribute2);
                XmlAttribute attribute3 = xmlDocument.CreateAttribute(nameof(address));
                attribute3.Value = address;
                element.Attributes.Append(attribute3);

                xmlNode.AppendChild((XmlNode)element);
                xmlDocument.Save(str);
            }
            catch
            {
                return;
            }
        }

        public void Remove(string name, string tag)
        {
            try
            {
                string path = "C:\\Program Files\\ATPro\\ATSCADA\\DesignerFiles\\";
                if (!Directory.Exists(path))
                {
                    path = "C:\\Program Files (x86)\\ATPro\\ATSCADA\\DesignerFiles\\";
                    if (!Directory.Exists(path))
                        return;
                }
                string str = path + "Image_file.XML";
                if (!File.Exists(str))
                    File.WriteAllText(str, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r<Root>\r</Root>");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(str);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("Root");
                foreach (XmlElement childNode in xmlNode.ChildNodes)
                {
                    if (childNode.Attributes[0].Value == name && childNode.Attributes[1].Value == tag)
                    {
                        xmlNode.RemoveChild((XmlNode)childNode);
                        break;
                    }
                }
                xmlDocument.Save(str);
            }
            catch
            {
                return;
            }
        }

        public string GetAbsoluteAdd(string name, string tag)
        {
            try
            {
                string path = "C:\\Program Files\\ATPro\\ATSCADA\\DesignerFiles\\";
                if (!Directory.Exists(path))
                {
                    path = "C:\\Program Files (x86)\\ATPro\\ATSCADA\\DesignerFiles\\";
                    if (!Directory.Exists(path))
                        return string.Empty;
                }
                string str = path + "Image_file.XML";
                if (!File.Exists(str))
                    File.WriteAllText(str, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r<Root>\r</Root>");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(str);
                foreach (XmlElement childNode in xmlDocument.SelectSingleNode("Root").ChildNodes)
                {
                    if (childNode.Attributes[0].Value == name && childNode.Attributes[1].Value == tag)
                        return childNode.Attributes[2].Value;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetImageLocationPath(string name)
        {
            try
            {
                DTE activeObject = (DTE)Marshal.GetActiveObject("VisualStudio.DTE");
                Project project = null;

                if (activeObject.ActiveSolutionProjects is Array solutionProjects && solutionProjects.Length > 0)
                    project = solutionProjects.GetValue(0) as Project;

                if (project != null)
                {
                    string locationPath = Path.GetDirectoryName(project.FullName);
                    return $"{locationPath}\\bin\\Debug\\GraphicLib\\{name}";
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }        
    }

    public class ImageSettingsEditor : PropertyEditorBase
    {
        private frmImageSettings control;

        protected override Control GetEditControl(string PropertyName, object CurrentValue)
        {
            this.control = new frmImageSettings()
            {
                DataSerialization = (string)CurrentValue
            };

            return this.control;
        }

        protected override object GetEditedValue(Control EditControl, string PropertyName, object OldValue)
        {
            if (this.control == null) return OldValue;
            return this.control.IsCanceled ? OldValue : this.control.DataSerialization;
        }
    }

    public class Image3PosSettingsEditor : PropertyEditorBase
    {
        private frmImage3PosSettings control;

        protected override Control GetEditControl(string PropertyName, object CurrentValue)
        {
            this.control = new frmImage3PosSettings()
            {
                DataSerialization = (string)CurrentValue
            };

            return this.control;
        }

        protected override object GetEditedValue(Control EditControl, string PropertyName, object OldValue)
        {
            if (this.control == null) return OldValue;
            return this.control.IsCanceled ? OldValue : this.control.DataSerialization;
        }
    }

    public class SwitchSettingsEditor : PropertyEditorBase
    {
        private frmSwitchSettings control;

        protected override Control GetEditControl(string PropertyName, object CurrentValue)
        {
            this.control = new frmSwitchSettings()
            {
                DataSerialization = (string)CurrentValue
            };

            return this.control;
        }

        protected override object GetEditedValue(Control EditControl, string PropertyName, object OldValue)
        {
            if (this.control == null) return OldValue;
            return this.control.IsCanceled ? OldValue : this.control.DataSerialization;
        }
    }

    public class PushSettingsEditor : PropertyEditorBase
    {
        private frmPushSettings control;

        protected override Control GetEditControl(string PropertyName, object CurrentValue)
        {
            this.control = new frmPushSettings()
            {
                DataSerialization = (string)CurrentValue
            };

            return this.control;
        }

        protected override object GetEditedValue(Control EditControl, string PropertyName, object OldValue)
        {
            if (this.control == null) return OldValue;
            return this.control.IsCanceled ? OldValue : this.control.DataSerialization;
        }
    }
}

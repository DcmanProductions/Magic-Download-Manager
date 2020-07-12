using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public static class FileUtilities
    {
        public static string OpenFolder(string path, string description)
        {
            path = path == "" ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : path;
            using (var folder = new FolderBrowserDialog())
            {
                folder.SelectedPath = path;
                folder.Description = description;
                folder.ShowNewFolderButton = true;
                var result = folder.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return folder.SelectedPath;
                }
                else
                {
                    return path;
                }
            }
            return path;
        }
    }
}

using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using File = System.IO.File;

namespace com.drewchaseproject.MDM.Library.Objects
{
    public class ShortcutFile
    {
        public string TargetExecutable { get; private set; }
        public string ShortcutPath { get; private set; }
        public string ShortcutOutputDirectory { get; private set; }
        public string ShortcutName { get; private set; }
        public bool ForceCreate { get; private set; }

        private IWshShortcut shortcut;

        public ShortcutFile(string name, string outputFolder, string targetFileLocation, string shortcutDescription, bool force = true)
        {
            ForceCreate = force;
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
            ShortcutOutputDirectory = outputFolder;
            ShortcutName = name;
            ShortcutPath = Path.Combine(ShortcutOutputDirectory, ShortcutName + ".lnk");
            WshShell shell = new WshShell();
            shortcut = (IWshShortcut) shell.CreateShortcut(ShortcutPath);
            shortcut.Description = shortcutDescription;
            shortcut.IconLocation = targetFileLocation;
            shortcut.TargetPath = targetFileLocation;
        }

        public void Save()
        {
            // if force == true then always create if force != true only create if file exists
            if (ForceCreate)
            {
                if (File.Exists(ShortcutPath))
                    File.Delete(ShortcutPath);
                shortcut.Save();
            }
            else
            {
                if (File.Exists(ShortcutPath))
                {
                    File.Delete(ShortcutPath);
                    shortcut.Save();
                }
            }

        }

    }
}

using com.drewchaseproject.MDM.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.drewchaseproject.MDM.Library.Objects
{
    public static class Shortcuts
    {
        public static List<ShortcutFile> AsList
        {
            get
            {
                string name = "Magic Download Manager";
                string description = "Launches Magic Download Manager";
                string appdir = Values.Singleton.LauncherExe;
                return new ShortcutFile[]
                {
                    new ShortcutFile(name, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), appdir, description, false),
                    new ShortcutFile(name, System.IO.Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.Programs), Values.Singleton.CompanyName), appdir, description)
                }.ToList();
            }
        }
    }
}

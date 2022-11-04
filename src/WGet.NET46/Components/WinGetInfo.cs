//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//

using System;
using System.IO;
using System.Linq;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetInfo"/> class offers informations about the installed winget version.
    /// </summary>
    public class WinGetInfo
    {
        private const string _versionCmd = "--version";

        internal readonly ProcessManager _processManager;

        /// <summary>
        /// Gets if winget is installed on the system.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if winget is installed or <see langword="false"/> if not.
        /// </returns>
        public bool WinGetInstalled
        {
            get
            {
                if (CheckWinGetVersion() != string.Empty)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the version number of the winget installation.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> with the version number.
        /// </returns>
        public string WinGetVersion
        {
            get
            {
                return CheckWinGetVersion();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetInfo"/> class.
        /// </summary>
        public WinGetInfo()
        {
            string wingetExecutableFullPath = "winget";

            DirectoryInfo d = new DirectoryInfo("C:\\Program Files\\WindowsApps");
            FileInfo[] fileInfos = d.GetFiles("winget.exe", SearchOption.AllDirectories);
            if(fileInfos.Length >0)
            {
                wingetExecutableFullPath = fileInfos.Last().FullName;
            }

            _processManager = new ProcessManager(wingetExecutableFullPath);
        }

        private string CheckWinGetVersion()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_versionCmd);

                for (int i = 0; i < result.Output.Length; i++)
                {
                    if (result.Output[i].StartsWith("v"))
                    {
                        return result.Output[i].Trim();
                    }
                }
            }
            catch
            {
                //No handling needed, winget can not be accessed
            }

            return string.Empty;
        }
    }
}
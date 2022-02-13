﻿//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WGetNET
{
    /// <summary>
    /// The <see cref="WGetNET.WinGetPackageManager"/> class offers methods to manage packages with winget.
    /// </summary>
    public class WinGetPackageManager
    {
        private const string _listCmd = "list";
        private const string _searchCmd = "search {0}";
        private const string _installCmd = "install {0}";
        private const string _upgradeCmd = "upgrade {0}";
        private const string _getUpgradeableCmd = "upgrade";
        private const string _uninstallCmd = "uninstall {0}";
        private const string _exportCmd = "export -o {0}";
        private const string _importCmd = "import -i {0} --ignore-unavailable";

        private readonly ProcessManager _processManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WGetNET.WinGetPackageManager"/> class.
        /// </summary>
        public WinGetPackageManager()
        {
            _processManager = new ProcessManager();
        }

        //---Search------------------------------------------------------------------------------------
        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name.
        /// </summary>
        /// <param name="packageName">The name of the package for the search.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        public List<WinGetPackage> SearchPackage(string packageName)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_searchCmd, packageName));

                return ToPackageList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception)
            {
                return new List<WinGetPackage>();
            }
        }

        /// <summary>
        /// Uses the winget search function to search for a package that maches the given name.
        /// </summary>
        /// <param name="packageName">The name of the package for the search.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        public async Task<List<WinGetPackage>> SearchPackageAsync(string packageName)
        {
            return await Task.Run(() => SearchPackage(packageName));
        }
        //---------------------------------------------------------------------------------------------

        //---Install-----------------------------------------------------------------------------------
        /// <summary>
        /// Gets a list of all installed packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        public List<WinGetPackage> GetInstalledPackages()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_listCmd);

                return ToPackageList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The search of installed packages failed.", e);
            }
        }

        /// <summary>
        /// Gets a list of all installed packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        public async Task<List<WinGetPackage>> GetInstalledPackagesAsync()
        {
            return await Task.Run(() => GetInstalledPackages());
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public bool InstallPackage(string packageId)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_installCmd, packageId));

                //Check if installation was succsessfull
                if (result.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package installtion failed.", e);
            }
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public bool InstallPackage(WinGetPackage package)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_installCmd, package.PackageId));

                //Check if installation was succsessfull
                if (result.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package installtion failed.", e);
            }
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> InstallPackageAsync(string packageId)
        {
            return await Task.Run(() => InstallPackage(packageId));
        }

        /// <summary>
        /// Install a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the installation.</param>
        /// <returns>
        /// <see langword="true"/> if the installation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> InstallPackageAsync(WinGetPackage package)
        {
            return await Task.Run(() => InstallPackage(package));
        }
        //---------------------------------------------------------------------------------------------

        //---Uninstall---------------------------------------------------------------------------------
        /// <summary>
        /// Uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public bool UninstallPackage(string packageId)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_uninstallCmd, packageId));

                //Check if uninstallation was succsessfull
                if (result.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package uninstalltion failed.", e);
            }
        }

        /// <summary>
        /// Uninsatll a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public bool UninstallPackage(WinGetPackage package)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_uninstallCmd, package.PackageId));

                //Check if uninstallation was succsessfull
                if (result.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("The package uninstalltion failed.", e);
            }
        }

        /// <summary>
        /// Uninsatll a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> UninstallPackageAsync(string packageId)
        {
            return await Task.Run(() => UninstallPackage(packageId));
        }

        /// <summary>
        /// Uninsatll a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> for the uninstallation.</param>
        /// <returns>
        /// <see langword="true"/> if the uninstallation was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> UninstallPackageAsync(WinGetPackage package)
        {
            return await Task.Run(() => UninstallPackage(package));
        }
        //---------------------------------------------------------------------------------------------

        //---Upgrade-----------------------------------------------------------------------------------
        /// <summary>
        /// Get all upgradeable packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        public List<WinGetPackage> GetUpgradeablePackages()
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(_getUpgradeableCmd);

                return ToPackageList(result.Output);
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Getting updateable packages failed.", e);
            }
        }

        /// <summary>
        /// Get all upgradeable packages.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{T}"/> of <see cref="WGetNET.WinGetPackage"/> instances.
        /// </returns>
        public async Task<List<WinGetPackage>> GetUpgradeablePackagesAsync()
        {
            return await Task.Run(() => GetUpgradeablePackages());
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for upgrade.</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public bool UpgradePackage(string packageId)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_upgradeCmd, packageId));

                //Check if installation was succsessfull
                if (result.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading the package failed.", e);
            }
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that for the upgrade</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public bool UpgradePackage(WinGetPackage package)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_upgradeCmd, package.PackageId));

                //Check if installation was succsessfull
                if (result.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Upgrading the package failed.", e);
            }
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="packageId">The id or name of the package for upgrade.</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> UpgradePackageAsync(string packageId)
        {
            return await Task.Run(() => UpgradePackage(packageId));
        }

        /// <summary>
        /// Upgrades a package using winget.
        /// </summary>
        /// <param name="package">The <see cref="WGetNET.WinGetPackage"/> that for the upgrade</param>
        /// <returns>
        /// <see langword="true"/> if the upgrade was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> UpgradePackageAsync(WinGetPackage package)
        {
            return await Task.Run(() => UpgradePackage(package));
        }
        //---------------------------------------------------------------------------------------------

        //---Other------------------------------------------------------------------------------------
        /// <summary>
        /// Exports a list of all installed winget packages as json to the given file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public bool ExportPackagesToFile(string file)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_exportCmd, file));

                //Check if installation was succsessfull
                if (result.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Exporting packages failed.", e);
            }
        }

        /// <summary>
        /// Exports a list of all installed winget packages as json to the given file.
        /// </summary>
        /// <param name="file">The file for the export.</param>
        /// <returns>
        /// <see langword="true"/> if the export was successfull or <see langword="false"/> if it failed.
        /// </returns>
        public async Task<bool> ExportPackagesToFileAsync(string file)
        {
            return await Task.Run(() => ExportPackagesToFile(file));
        }

        /// <summary>
        /// Imports packages and trys to installes/upgrade all pakages in the list, if possible.
        /// </summary>
        /// <remarks>
        /// This may take some time and winget may not install/upgrade all packages.
        /// </remarks>
        /// <param name="file">The file with the package data for the import.</param>
        /// <returns>
        /// <see langword="true"/> if the import was compleatly successfull or 
        /// <see langword="false"/> if some or all packages failed to install.
        /// </returns>
        public bool ImportPackagesFromFile(string file)
        {
            try
            {
                ProcessResult result =
                    _processManager.ExecuteWingetProcess(
                        String.Format(_importCmd, file));

                //Check if installation was succsessfull
                if (result.ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Win32Exception)
            {
                throw new WinGetNotInstalledException();
            }
            catch (Exception e)
            {
                throw new WinGetActionFailedException("Importing packages failed.", e);
            }
        }

        /// <summary>
        /// Imports packages and trys to installes/upgrade all pakages in the list, if possible.
        /// </summary>
        /// <remarks>
        /// This may take some time and winget may not install/upgrade all packages.
        /// </remarks>
        /// <param name="file">The file with the package data for the import.</param>
        /// <returns>
        /// <see langword="true"/> if the import was compleatly successfull or 
        /// <see langword="false"/> if some or all packages failed to install.
        /// </returns>
        public async Task<bool> ImportPackagesFromFileAsync(string file)
        {
            return await Task.Run(() => ImportPackagesFromFile(file));
        }
        //---------------------------------------------------------------------------------------------

        private List<WinGetPackage> ToPackageList(List<string> output)
        {
            //Get top line index
            int topLineIndex = 0;
            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].Contains("------"))
                {
                    topLineIndex = i;
                    break;
                }
            }

            //Get start indexes of each tabel colum
            int nameStartIndex = 0;

            int idStartIndex = 0;
            bool idStartIndexSet = false;

            int versionStartIndex = 0;
            bool versionStartIndexSet = false;

            int extraInfoStartIndex = 0;
            bool extraInfoStartIndexSet = false;

            int labelLine = topLineIndex - 1;
            bool checkForChar = false;
            for (int i = 0; i < output[labelLine].Length; i++)
            {
                if (output[labelLine][i] != ' ' && checkForChar)
                {
                    if (!idStartIndexSet)
                    {
                        idStartIndex = i;
                        idStartIndexSet = true;
                        checkForChar = false;
                    }
                    else if (!versionStartIndexSet)
                    {
                        versionStartIndex = i;
                        versionStartIndexSet = true;
                        checkForChar = false;
                    }
                    else if (!extraInfoStartIndexSet)
                    {
                        extraInfoStartIndex = i;
                        extraInfoStartIndexSet = true;
                        checkForChar = false;
                    }
                    else if (idStartIndexSet && versionStartIndexSet && extraInfoStartIndexSet)
                    {
                        //Breake the loop if all indexes are set
                        break;
                    }
                }
                else if (output[labelLine][i] == ' ')
                {
                    checkForChar = true;
                }
            }

            //Remove unneeded output Lines
            output.RemoveRange(0, topLineIndex + 1);

            List<WinGetPackage> resultList = new List<WinGetPackage>();

            foreach (string line in output)
            {
                string name = line
                    .Substring(nameStartIndex, idStartIndex - 1)
                    .Trim();
                string winGetId = line
                    .Substring(idStartIndex, (versionStartIndex - idStartIndex) - 1)
                    .Trim();
                string version = line
                    .Substring(versionStartIndex, (extraInfoStartIndex - versionStartIndex) - 1)
                    .Trim();

                resultList.Add(new WinGetPackage() { PackageName = name, PackageId = winGetId, PackageVersion = version });
            }

            return resultList;
        }
    }
}

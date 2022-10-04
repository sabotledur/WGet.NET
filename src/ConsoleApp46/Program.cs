using System;
using System.Collections.Generic;
using System.Linq;
using WGetNET;

namespace ConsoleApp46
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WinGetInfo wingetInfo = new WinGetInfo();
            if (wingetInfo.WinGetInstalled)
            {
                Console.WriteLine("WinGet is installed.");
            }
            else
            {
                Console.WriteLine("WinGet is NOT installed.");
            }

            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine();

            WinGetPackageManager packageManager = new WinGetPackageManager();
            List<WinGetPackage> winGetPackages = packageManager.GetUpgradeablePackages();

            if (winGetPackages.Count > 0)
                winGetPackages.RemoveAt(winGetPackages.Count - 1);

            Console.WriteLine("Package Name | Package ID | Package Version | Available Version | Source");

            foreach (WinGetPackage package in winGetPackages.OrderBy(x => x.PackageName))
            {
                Console.WriteLine($"{package.PackageName} | {package.PackageId} | {package.PackageVersion} | {package.PackageVersionAvailable} | {package.PackageSource}");
            }


            Console.WriteLine();
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine();

            List<WinGetPackage> installedPackages = packageManager.GetInstalledPackages();
            foreach (WinGetPackage package in installedPackages.OrderBy(x => x.PackageName))
            {
                Console.WriteLine($"{package.PackageName} | {package.PackageId} | {package.PackageVersion} | {package.PackageVersionAvailable} | {package.PackageSource}");
            }

            Console.ReadLine();
        }
    }
}

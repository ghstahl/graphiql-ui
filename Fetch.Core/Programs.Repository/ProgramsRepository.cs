using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Programs.Models;

namespace Programs.Repository
{
    public class ProgramsRepository : IProgramsRepository
    {
        public static object Lock = new object();
        private List<InstalledApp> _records { get; set; }
        private Process[] _processlist { get; set; }

        public void LoadInstall(bool soft = false)
        {
            GetInstalledApps(soft);
        }

        public InstalledApp[] PageInstalled(int offset, int count)
        {
            LoadInstall(true);

            var newArray = _records.Skip(offset).Take(count).ToArray();
            return newArray;
        }

        public int InstallCount
        {
            get
            {
                LoadInstall(true);
                return _records.Count;
            }
        }

        public int ProcessCount
        {
            get
            {
                LoadProcesses(true);
                return _processlist.Length;
            }
        }

        public bool IsInstalled(string displayName)
        {
            LoadInstall();
            var query = from item in _records
                where item.DisplayName == displayName
                select item;
            var any = query.Any();
            return any;

        }

        public void LoadProcesses(bool soft)
        {
            lock (Lock)
            {
                if (soft && _processlist != null)
                    return;

                _processlist = Process.GetProcesses();
            }
        }

        public ProcessApp[] PageProcess(int offset, int count)
        {
            LoadProcesses(true);
            var newArray = _processlist.Skip(offset).Take(count).ToArray();
            var query = from item in newArray
                let s = new ProcessApp(item)
                select s;
            return query.ToArray();
        }

        public bool IsRunning(string processName)
        {
            LoadProcesses(false);
            var query = from item in _processlist
                where item.ProcessName == processName
                select item;
            var any = query.Any();
            return any;
        }

        public void GetInstalledApps(bool soft = false)
        {
            lock (Lock)
            {
                if (soft && _records != null)
                {
                    return;
                }
                string[] keys = new string[]
                {
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                    @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
                };

                _records = new List<InstalledApp>();
                foreach (var uninstallKey in keys)
                {
                    using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
                    {
                        var subkeys = rk.GetSubKeyNames();
                        var dd = (from name in subkeys
                            let app = new InstalledApp(rk, name)
                            where (app.UnInstallPath != null && app.DisplayName != null)
                            select app).ToList();
                        _records.AddRange(dd);
                    }
                }
            }
        }

        public LaunchUrlResult LaunchUrl(string url)
        {
            try
            {
                var process = System.Diagnostics.Process.Start(url);
                return new LaunchUrlResult()
                {
                    Ok = process != null,
                    Message = (process == null) ? string.Format("could not launch url:[{0}]", url) : null
                };
            }
            catch (Exception e)
            {

                return new LaunchUrlResult()
                {
                    Ok = false,
                    Message = e.Message
                };
            }

        }

        public LaunchResult LaunchSpecial(LaunchSpecialQuery query)
        {
            try
            {

                string root;
                switch (query.Special)
                {
                    case "CommonApplicationData":
                        root = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                        break;
                    default:
                        return new LaunchResult()
                        {
                            Ok = false,
                            Message = string.Format("Special:[{0}],is not known.", query.Special)
                        };

                }
                var launchPath = Path.Combine(root, query.SubPath);

                Process proc = new Process();
                proc.StartInfo.FileName = launchPath;
                var ok = proc.Start();

                return new LaunchResult()
                {
                    Ok = ok,
                    Message =
                        (!ok) ? string.Format("could not launch special:[{0},{1}]", query.Special, query.SubPath) : null
                };
            }
            catch (Exception e)
            {

                return new LaunchResult()
                {
                    Ok = false,
                    Message = e.Message
                };
            }
        }
    }
}
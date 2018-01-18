using System;
using Programs.Models;

namespace Programs.Repository
{
    public interface IProgramsRepository
    {
        void LoadInstall(bool soft = false);
        void LoadProcesses(bool soft = false);
        InstalledApp[] PageInstalled(int offset, int count);
        int InstallCount { get; }
        bool IsInstalled(string displayName);
        int ProcessCount { get; }
        LaunchUrlResult LaunchUrl(string url);
        ProcessApp[] PageProcess(int offset, int count);
        bool IsRunning(string processName);
        LaunchResult LaunchSpecial(LaunchSpecialQuery query);
    }
}

using System.Diagnostics;

namespace GitSync;

public class GitHelper(string gitUser, string gitPassword, string gitRepo, string gitBranch, string localPath)
{
    public void Sync()
    {
        if (Directory.Exists(localPath))
        {
            PullChanges();
        }
        else
        {
            CloneRepository();
        }
    }

    private Task CloneRepository()
    {
        var cloneUrl = $"https://{gitUser}:{gitPassword}@{gitRepo}";
        var processInfo = new ProcessStartInfo("git", $"clone -b {gitBranch} {cloneUrl} {localPath}")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return ExecuteProcess(processInfo);
    }

    private Task PullChanges()
    {
        var processInfo = new ProcessStartInfo("git", $"-C {localPath} pull origin {gitBranch}")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return ExecuteProcess(processInfo);
    }

    private async Task ExecuteProcess(ProcessStartInfo processInfo)
    {
        using var process = new Process();
        process.StartInfo = processInfo;
        process.Start();
        await process.WaitForExitAsync();

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        Console.WriteLine(process.ExitCode != 0 ? $"Error: {error}" : $"Output: {output}");
    }
}
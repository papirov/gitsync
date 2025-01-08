using GitSync;

var localPath = Environment.GetEnvironmentVariable("GITSYNC_LOCALPATH");
var gitRepo = Environment.GetEnvironmentVariable("GITSYNC_REPOPATH");
var gitBranch = Environment.GetEnvironmentVariable("GITSYNC_BRANCH");
var gitUser = Environment.GetEnvironmentVariable("GITSYNC_USER");
var gitPassword = Environment.GetEnvironmentVariable("GITSYNC_PASSWORD");
var refreshInMinutes = int.Parse(Environment.GetEnvironmentVariable("GITSYNC_REFRESH_IN_EVERY_MINS") ?? "60");

if (string.IsNullOrEmpty(localPath) || string.IsNullOrEmpty(gitRepo) || string.IsNullOrEmpty(gitBranch) || string.IsNullOrEmpty(gitUser) || string.IsNullOrEmpty(gitPassword))
{
    Console.WriteLine("Missing environment variables");
    return;
}

while (true)
{
    try
    {
        var git = new GitHelper(localPath, gitRepo, gitBranch, gitUser, gitPassword);
        git.Sync();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        Console.WriteLine($"Sleeping for {refreshInMinutes} minutes");
    }
    await Task.Delay(TimeSpan.FromMinutes(refreshInMinutes));
}



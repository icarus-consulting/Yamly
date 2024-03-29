using Nuke.Common;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.OpenCover;
using Nuke.Common.Utilities.Collections;
using System;
using System.Runtime.InteropServices;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.FullBuild);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    private string NuGetFeed => "https://api.nuget.org/v3/index.json";
    private string NUGET_TOKEN = Environment.GetEnvironmentVariable("NUGET_TOKEN");

    private Version Version;

    Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            TestsDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            ArtifactsDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution)
            );
        });

    Target DefaultVersion => _ => _
       .Executes(() =>
       {
           ProjectModelTasks.Initialize();
           Version = Version.Parse(Solution.GetProject("Yamly").GetProperty("Version"));
       });


    Target VersionFromTag => _ => _
        .DependsOn(DefaultVersion)
        .OnlyWhenDynamic(() => IsServerBuild && AppVeyor.Instance.RepositoryTag)
        .Executes(() =>
        {
            Version = Version.Parse(AppVeyor.Instance.RepositoryTagName);
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .DependsOn(VersionFromTag)
        .Executes(() =>
        {
            Console.WriteLine($"Using Version '{Version}'");
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVersion(Version.ToString())
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution.GetProject("Test.Yamly"))
                .SetNoBuild(true)
                .SetNoRestore(true)
                .SetConfiguration(Configuration)
            );
        });
    Target Pack => _ => _
        .DependsOn(Test)
        .DependsOn(VersionFromTag)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.GetProject("Yamly"))
                .SetConfiguration(Configuration)
                .SetNoBuild(true)
                .SetVersion(Version.ToString())
                .EnableIncludeSymbols()
                .SetOutputDirectory(ArtifactsDirectory)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
            );
        });

    Target PushPackage => _ => _
        .OnlyWhenDynamic(() => IsServerBuild && AppVeyor.Instance.RepositoryTag && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        .DependsOn(Pack)
        .Executes(() =>
        {
            DotNetNuGetPush(s => s
                .SetSource(NuGetFeed)
                .SetApiKey(NUGET_TOKEN)
                .CombineWith(ArtifactsDirectory.GlobFiles("*.nupkg", "*.snupkg"), (_, v) => _
                    .SetTargetPath(v)
                ),
                degreeOfParallelism: 2,
                completeOnFailure: false
            );
        });

    Target FullBuild => _ => _
        .DependsOn(Test)
        .DependsOn(PushPackage)
        .Executes(() =>
        {

        });
}

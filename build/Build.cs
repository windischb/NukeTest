using System;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]

[GitHubActions(
    "test",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new []{ "master"},
    InvokedTargets = new []{ nameof(Test1)},
    ImportGitHubTokenAs = nameof(GitHubToken)
    )]

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Test1);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    [CI] readonly GitHubActions GitHubActions;

    [Parameter("GitHub Token")] public readonly string GitHubToken;

    AbsolutePath SourceDirectory => RootDirectory / "source";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Test1 => _ => _
        .Executes(() =>
        {

            var json = JsonSerializer.Serialize(GitHubActions, new JsonSerializerOptions() {WriteIndented = true});
            Logger.Info(json);

            Logger.Info("GithubTOKEN");
            Logger.Info(GitHubToken);

        });

   
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using BepInEx.AssemblyPublicizer;
using build.Utils;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Net;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;
using dotenv.net;

namespace build;

public static class Build
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .UseLifetime<BuildLifetime>()
            .Run(args);
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class BuildContext : FrostingContext
{
    #region Arguments

    public readonly string MsBuildConfiguration;
    public AbsolutePath? GameDir { get; }
    
    public readonly string? Version;
    public readonly bool SkipPatching;

    #endregion

    
    #region Settings

    public string[] References { get; }
    public string[] AssetBundles { get; }
    public CSharpProject Project { get; }
    public string ManifestAuthor { get; }
    public string NetcodePatcherRelease { get; }
    public string LethalEmotesApiVersion { get; }

    #endregion

    
    #region Env

    public AbsolutePath SolutionPath { get; }
    public bool UseStubbedLibs { get; }
    public AbsolutePath[] DeployTargets { get; }

    #endregion

    public readonly AbsolutePath GameReferencesDir = new AbsolutePath("../") / ".gameReferences";
    public readonly AbsolutePath ToolsDir = new AbsolutePath("../") / ".tools";
    public AbsolutePath PatcherDir { get; }
    public readonly AbsolutePath StubbedLibsDir = new AbsolutePath("../") / "libs";
    public readonly AbsolutePath StubbedFilesPath = new AbsolutePath("../") / "libs" / "stubbed-files.zip";
    public AbsolutePath BuildDir { get; }
    public AbsolutePath UnityDir { get; }
    public AbsolutePath UnityAssetBundlesDir { get; }

    public BuildContext(ICakeContext context) : base(context)
    {
        PatcherDir = ToolsDir / "netcode-patcher";
        
        DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { "../.env" }));

        SkipPatching = context.HasArgument("skipPatching");
        MsBuildConfiguration = context.Argument<string>("configuration", "Debug");
        Version = context.EnvironmentVariable("RELEASE_VERSION");

        SolutionPath = context.GetFiles("../*.sln")
            .First()
            .FullPath;

        var settings = ProjectBuildSettings.LoadFromFile("../build-settings.json");
        if (settings is null)
            throw new InvalidOperationException();

        var projectFilePath = (AbsolutePath)"../" / settings.ProjectFile;
        References = settings.References;
        AssetBundles = settings.AssetBundles;
        Project = new CSharpProject(projectFilePath);
        ManifestAuthor = settings.ManifestAuthor;
        NetcodePatcherRelease = settings.NetcodePatcherRelease;
        LethalEmotesApiVersion = settings.LethalEmotesApiVersion;

        UseStubbedLibs = context.Environment.GetEnvironmentVariable("USE_STUBBED_LIBS") is not null;
        GameDir = GetGameDirArg(context);

        string deployTargetEnv = context.Environment.GetEnvironmentVariable("DEPLOY_TARGETS");
        if (deployTargetEnv is not null)
        {
            DeployTargets = deployTargetEnv
                .Split(";")
                .Select(dir => new AbsolutePath(dir))
                .ToArray();
        }
        else
        {
            DeployTargets = [];
        }

        BuildDir = Project.Directory / "bin" / MsBuildConfiguration / "netstandard2.1";
        UnityDir = (AbsolutePath)"../" / settings.UnityDir;
        UnityAssetBundlesDir = UnityDir / "AssetBundles" / "StandaloneWindows";
    }

    private AbsolutePath? GetGameDirArg(ICakeContext context)
    {
        return UseStubbedLibs ? null : new AbsolutePath(context.Arg("gameDir"));
    }
}

[TaskName("FetchRefs")]
public sealed class FetchReferences : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        if (context.UseStubbedLibs)
            return false;

        return context.References.Any(reference => !File.Exists(context.GameReferencesDir / reference));
    }

    public override void Run(BuildContext context)
    {
        if (!Directory.Exists(context.GameReferencesDir))
            Directory.CreateDirectory(context.GameReferencesDir);
        
        AbsolutePath srcDir = context.GameDir! / "Lethal Company_Data" / "Managed";

        foreach (var reference in context.References)
        {
            AbsolutePath srcFile = srcDir / reference;
            AbsolutePath dstFile = context.GameReferencesDir / reference;
            
            File.Copy(srcFile, dstFile, true);
        }
    }
}

[TaskName("FetchStubbedLibs")]
public sealed class FetchStubbedLibs : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        if (!context.UseStubbedLibs)
            return false;

        return context.References.Any(reference => !File.Exists(context.StubbedLibsDir / reference));
    }

    public override void Run(BuildContext context)
    {
        if (!File.Exists(context.StubbedFilesPath))
            throw new InvalidOperationException();

        var refsToFind = new List<string>(context.References);

        using var archive = ZipFile.OpenRead(context.StubbedFilesPath);
        foreach (var entry in archive.Entries)
        {
            var foundRefName = refsToFind
                .SingleOrDefault(reference => entry.Name == reference);

            if (string.IsNullOrEmpty(foundRefName))
                continue;
            
            entry.ExtractToFile(context.StubbedLibsDir / foundRefName, true);

            refsToFind.Remove(foundRefName);
        }

        if (refsToFind.Count != 0)
            throw new InvalidOperationException();
    }
}

[TaskName("SetupNetcode")]
public sealed class SetupNetcodePatcher : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        if (context.SkipPatching)
            return false;
        
        if (!Directory.Exists(context.PatcherDir))
            return true;

        if (Directory.GetFiles(context.PatcherDir).Length == 0)
            return true;

        if (Directory.GetFiles(context.PatcherDir / "deps").Length == 0)
            return true;
        
        return false;
    }

    public override void Run(BuildContext context)
    {
        if (Directory.Exists(context.PatcherDir))
            Directory.Delete(context.PatcherDir, true);

        Directory.CreateDirectory(context.PatcherDir);
        
        ZipFile.ExtractToDirectory(context.StubbedFilesPath, context.PatcherDir / "deps");
        File.WriteAllText(context.PatcherDir / "version", context.NetcodePatcherRelease);
    }
}

[TaskName("Restore")]
public sealed class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetRestore(context.SolutionPath);
        
        using var dotnetToolRestore = new Process();
        dotnetToolRestore.StartInfo.FileName = "dotnet";
        dotnetToolRestore.StartInfo.Arguments = $"tool restore";
        dotnetToolRestore.StartInfo.CreateNoWindow = false;
        dotnetToolRestore.StartInfo.UseShellExecute = true;
        dotnetToolRestore.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        
        dotnetToolRestore.Start();
        dotnetToolRestore.WaitForExit();
    }
}

[TaskName("UpdateAssetBundles")]
public sealed class UpdateAssetBundles : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        if (!context.UnityAssetBundlesDir.DirExists())
        {
            context.Log.Warning($"Could not find `{context.UnityAssetBundlesDir}`!");
            return;
        }

        var projectAssetBundlesDir = context.Project.Directory / "AssetBundles";
        projectAssetBundlesDir.EnsureDirectoryExists();

        context.UnityAssetBundlesDir.GlobFiles(context.AssetBundles)
            .CopyFilesTo(projectAssetBundlesDir);
    }
}

[TaskName("Build")]
[IsDependentOn(typeof(RestoreTask))]
[IsDependentOn(typeof(FetchReferences))]
[IsDependentOn(typeof(FetchStubbedLibs))]
[IsDependentOn(typeof(UpdateAssetBundles))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetBuild(context.SolutionPath, new DotNetBuildSettings
        {
            Configuration = context.MsBuildConfiguration
        });
    }
}

[TaskName("PatchNetcode")]
[IsDependentOn(typeof(SetupNetcodePatcher))]
[IsDependentOn(typeof(BuildTask))]
public sealed class PatchNetcode : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        if (context.SkipPatching)
            return false;

        return true;
    }

    public override void Run(BuildContext context)
    {
        AbsolutePath patcherPluginsDir = context.PatcherDir / "plugins";

        if (Directory.Exists(patcherPluginsDir))
            Directory.Delete(patcherPluginsDir, true);
        Directory.CreateDirectory(patcherPluginsDir);
        
        context.BuildDir.GlobFiles("*.dll", "*.pdb")
            .CopyFilesTo(patcherPluginsDir);
        
        using var patcher = new Process();
        patcher.StartInfo.FileName = "dotnet";
        patcher.StartInfo.Arguments = $"tool run netcode-patch ./plugins ./deps";
        patcher.StartInfo.WorkingDirectory = context.PatcherDir;
        patcher.StartInfo.CreateNoWindow = false;
        patcher.StartInfo.UseShellExecute = true;
        patcher.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        patcher.Start();
        patcher.WaitForExit();
        
        patcherPluginsDir.GlobFiles("*_original.*")
            .DeleteFiles();
        
        patcherPluginsDir.GlobFiles("*.dll", "*.pdb")
            .CopyFilesTo(context.BuildDir);
    }
}

[TaskName("BuildAndPatch")]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(PatchNetcode))]
public sealed class BuildAndPatch : FrostingTask<BuildContext>;

[TaskName("DeployDepsUnity")]
public sealed class DeployDepsToUnity : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        if (context.GameDir is null)
            return false;
        
        AbsolutePath destDir = context.UnityDir / "Packages" / context.Project.Name;
        if (!Directory.Exists(destDir))
            return true;

        string[] depsToFind = [ "BepInEx.dll", "0Harmony.dll", "MonoMod.RuntimeDetour.dll", "LethalEmotesAPI.dll", "LethalEmotesApi.Ui.dll" ];
        var stubbedDeps = destDir.GlobFiles(depsToFind);
        if (stubbedDeps.Count != depsToFind.Length)
            return true;

        return false;
    }

    public override void Run(BuildContext context)
    {
        AbsolutePath destDir = context.UnityDir / "Packages" / context.Project.Name;
        destDir.EnsureDirectoryExists();

        var depsToStub = new List<AbsolutePath>();
        depsToStub.AddRange(GetBepInExFiles(context));
        depsToStub.AddRange(GetLethalEmotesApiFiles(context));

        var stubOptions = new AssemblyPublicizerOptions
        {
            Strip = true,
            IncludeOriginalAttributesAttribute = false
        };
        
        foreach (var dep in depsToStub)
            AssemblyPublicizer.Publicize(dep, destDir / dep.Name, stubOptions);
    }

    private List<AbsolutePath> GetBepInExFiles(BuildContext context)
    {
        var coreDir = context.GameDir! / "BepInEx" / "core";
        return coreDir.GlobFiles("BepInEx.dll", "0Harmony.dll", "MonoMod.RuntimeDetour.dll");
    }


    private List<AbsolutePath> GetLethalEmotesApiFiles(BuildContext context)
    {
        var apiDir = context.ToolsDir / "LethalEmotesApi";
        if (apiDir.DirExists())
            Directory.Delete(apiDir, true);
        
        apiDir.EnsureDirectoryExists();
        
        var url= $"https://github.com/Wet-Boys/LethalEmotesAPI/releases/download/{context.LethalEmotesApiVersion}/Gemmumoddo-LethalEmotesAPI-{context.LethalEmotesApiVersion}.zip";
        var apiZip = apiDir / "LethalEmotesApi.zip";
        context.DownloadFile(url, apiZip);
        
        ZipFile.ExtractToDirectory(apiZip, apiDir);
        File.Delete(apiZip);

        var dllDir = apiDir / "LethalEmotesApi";

        return dllDir.GlobFiles("*.dll");
    }
}

[TaskName("DeployUnity")]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(DeployDepsToUnity))]
public sealed class DeployToUnity : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        AbsolutePath unityPkgDir = context.UnityDir / "Packages";
        
        AbsolutePath destDir = unityPkgDir / context.Project.Name;

        if (!Directory.Exists(destDir))
            Directory.CreateDirectory(destDir);
            
        context.BuildDir.GlobFiles("*.dll", "*.pdb")
            .CopyFilesTo(destDir);
        
        AbsolutePath packageFile = destDir / "package.json";
        if (!File.Exists(packageFile))
            File.WriteAllText(packageFile, "{\"name\": \"com.gemumoddo.lc_emotes2_and_knuckles_featuring_dante\",\"version\": \"1.0.0\"}");
    }
}

[TaskName("Deploy")]
[IsDependentOn(typeof(BuildAndPatch))]
public sealed class DeployToGame : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var project = context.Project;
        var assetBundlesDir = context.BuildDir / "AssetBundles";
        
        foreach (var target in context.DeployTargets)
        {
            AbsolutePath destDir = target / project.Name;
            destDir.EnsureDirectoryExists();
            
            context.BuildDir.GlobFiles("*.dll", "*.pdb")
                .CopyFilesTo(destDir);
            
            assetBundlesDir.GlobFiles("*")
                .CopyFilesTo(destDir / "AssetBundles");
        }
    }
}

[TaskName("DebugMod")]
[IsDependentOn(typeof(DeployToGame))]
public sealed class DebugMod : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        string host;
        var args = "start steam://rungameid/1966720";
        
        if (OperatingSystem.IsWindows())
        {
            host = "cmd.exe";
            args = $"/C {args}";
        }
        else
        {
            host = "/bin/bash";
            args = $"-c \"{args}\"";
        }
        
        using var startGame = new Process();
        startGame.StartInfo.FileName = host;
        startGame.StartInfo.Arguments = args;
        startGame.StartInfo.CreateNoWindow = false;
        startGame.StartInfo.UseShellExecute = true;
        startGame.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        startGame.Start();
        startGame.WaitForExit();
    }
}

[TaskName("ThunderstoreChecklist")]
public sealed class ThunderstoreChecklist : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        bool throwOnFail = string.Equals(context.MsBuildConfiguration, "release",
            StringComparison.InvariantCultureIgnoreCase);

        var warnMsg = "\nThis will cause the build to fail under the `Release` configuration!";

        if (context.Version is null)
        {
            var msg = "Environment Variable `RELEASE_VERSION` must be set!";
            if (throwOnFail)
                throw new InvalidOperationException(msg);

            context.Log.Error($"{msg}{warnMsg}");
            return;
        }

        if (!PluginVersionValidate(context))
        {
            var msg = "Plugin Version failed to validate!";
            if (throwOnFail)
                throw new InvalidOperationException(msg);
            
            context.Log.Error($"{msg}{warnMsg}");
            return;
        }

        if (!ManifestVersionValidate(context))
        {
            var msg = "Manifest Version failed to validate!";
            if (throwOnFail)
                throw new InvalidOperationException(msg);
            
            context.Log.Error($"{msg}{warnMsg}");
            return;
        }
        
        context.Log.Information("Thunnderstore Checklist passed!");
    }

    private bool PluginVersionValidate(BuildContext context)
    {
        var versionRegex =
            new Regex("\\s*public\\s+const\\s+string\\s+ModVersion\\s+=\\s+\"(?<version>\\d\\.\\d\\.\\d)\";");
        var pluginPath = context.Project.Directory / "LcEmotes2AndKnucklesFeaturingDantePlugin.cs";

        using var fileStream = File.OpenRead(pluginPath);
        using var streamReader = new StreamReader(fileStream);

        string? pluginVersion = null;
        do
        {
            var lineRead = streamReader.ReadLine();
            if (lineRead is null)
                return false;
            
            var match = versionRegex.Match(lineRead);
            if (!match.Success)
                continue;

            pluginVersion = match.Groups["version"].Value;
            
        } while (pluginVersion is null);

        return pluginVersion == context.Version;
    }

    private bool ManifestVersionValidate(BuildContext context)
    {
        AbsolutePath manifestFile = "manifest.json";
        
        var manifest = JsonSerializer.Deserialize<ThunderStoreManifest>(File.ReadAllText("../" / manifestFile));
        if (manifest is null)
            return false;

        return manifest.version_number == context.Version;
    }
}

[TaskName("BuildThunderstore")]
[IsDependentOn(typeof(BuildAndPatch))]
public sealed class BuildThunderstorePackage : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        AbsolutePath manifestFile = "manifest.json";
        AbsolutePath iconFile = "icon.png";
        AbsolutePath readmeFile = "README.md";
        AbsolutePath changelogFile = "CHANGELOG.md";
        
        var project = context.Project;
        
        AbsolutePath publishDir = context.BuildDir / "publish";

        if (Directory.Exists(publishDir))
            Directory.Delete(publishDir, true);

        Directory.CreateDirectory(publishDir);

        var modDir = publishDir / "plugins" / project.Name;
        Directory.CreateDirectory(modDir);
            
        context.BuildDir.GlobFiles("*.dll")
            .CopyFilesTo(modDir);

        var assetBundlesDir = context.BuildDir / "AssetBundles";
        assetBundlesDir.GlobFiles("*")
            .CopyFilesTo(modDir / "AssetBundles");
            
        File.Copy("../" / manifestFile, publishDir / manifestFile, true);
        File.Copy("../" / iconFile, publishDir / iconFile, true);
        File.Copy("../" / readmeFile, publishDir / readmeFile, true);
        File.Copy("../" / changelogFile, publishDir / changelogFile, true);

        var manifest = JsonSerializer.Deserialize<ThunderStoreManifest>(File.ReadAllText(publishDir / manifestFile));

        var destDir = context.BuildDir / "upload";
        if (Directory.Exists(destDir))
            Directory.Delete(destDir, true);

        Directory.CreateDirectory(destDir);

        var version = context.Version ?? manifest!.version_number;
        var destFile = destDir / $"{context.ManifestAuthor}-{manifest!.name}-{version}.zip";
        if (File.Exists(destFile))
            File.Delete(destFile);
            
        ZipFile.CreateFromDirectory(publishDir, destFile);
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(BuildAndPatch))]
public class DefaultTask : FrostingTask;
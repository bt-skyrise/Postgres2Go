// Tools
#tool                   "xunit.runner.console"

// Parameters
var configuration       = Argument("Configuration", "Release");
var target              = Argument("Target", "Default");

// Variables
var buildNumber =
    HasArgument("BuildNumber") ? Argument<int>("BuildNumber") :
    AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number : 
    0;
var outputDir           = Directory("./artifacts");
var projectDir          = GetFiles("./src/Postgres2Go/*.csproj").FirstOrDefault();
var solution            = GetFiles("./src/*.sln").FirstOrDefault();
var tests               = GetFiles("./src/Postgres2Go.Tests/*.csproj").FirstOrDefault();
var testResultsDir      = Directory("./test-results/");


// Tasks
Task("Clean")
    .Does(() => {
        Information("Removing previous build artifacts");
        
        if(DirectoryExists(testResultsDir.Path))
            DeleteDirectory(testResultsDir.Path, 
                new DeleteDirectorySettings()
                {
                    Recursive = true
                });
        
        if(DirectoryExists(outputDir.Path))
            DeleteDirectory(outputDir.Path, 
                new DeleteDirectorySettings()
                {
                    Recursive = true
                });
    });

Task("Prepare-Directories")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        Information("Prepare directory for tests output");
        EnsureDirectoryExists(testResultsDir.Path);

        Information("Prepare directory for lib output");
        EnsureDirectoryExists(outputDir.Path);
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Prepare-Directories")
    .Does(() => {
        Information("Restoring nuget packages for solution");
        DotNetCoreRestore(solution.FullPath);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => {
        Information("Build solution");
        DotNetCoreBuild(solution.FullPath,
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        
        var resultsFile = $"..\\..\\..\\test-results\\{tests.GetFilenameWithoutExtension()}.xml";
        var testProject = tests.FullPath;
        
        Information($"Run tests of {tests.GetFilenameWithoutExtension()}");

        DotNetCoreTest(testProject,
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true,
                    ArgumentCustomization = args => args
                        .Append($"-r {testResultsDir.Path.FullPath}")
                        .Append($"-l trx;logfilename={resultsFile}")
                });

    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        var revision = buildNumber.ToString("D4");
                
        DotNetCorePack(
                projectDir.GetDirectory().FullPath,
                new DotNetCorePackSettings()
                {
                    Configuration = configuration,
                    OutputDirectory = outputDir.Path,
                    //VersionSuffix = revision
                });
    });

Task("Default")
    .IsDependentOn("Pack");

// Run
RunTarget(target);

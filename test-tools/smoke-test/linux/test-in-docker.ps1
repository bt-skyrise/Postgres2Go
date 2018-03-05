Param(
    [string]$packageSource = "https://api.nuget.org/v3/index.json",
    [switch]$sourceIsLocalDir,
    [string]$packageVersion
)

$packageVersionArg =    if ($packageVersion) { "--package-version $packageVersion" } 
                        else { "" }

$packageSourceArg = if ($sourceIsLocalDir) { "--package-source /package" } 
                    else { "--package-source $packageSource"}


$mountedPackageDir =    if ($sourceIsLocalDir) { $packageSource } 
                        else { "../package" }

#postgres won't run as a root :(
docker run `
    -it `
    --rm `
    --name postgres2go-smoke-test `
    -v "$(resolve-path $mountedPackageDir):/package" `
    -v "$(resolve-path ../project):/project" `
    -v "$(resolve-path .):/test" `
    microsoft/dotnet:2.0-sdk-stretch `
    /bin/bash -c @"
    apt update && \
    apt install sudo && \
    useradd -ms /bin/bash testuser && \
    sudo adduser testuser sudo && \
    sudo -u testuser bash -c 'cd /test; ./test.sh $packageSourceArg $packageVersionArg'
"@   
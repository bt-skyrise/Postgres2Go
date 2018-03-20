# Smoke test

This directory contains a set of tools for the package developers to smoke test the functionality on different environments.

## Linux

### Testing on Linux or WSL

To test with default settings:

```bash
cd linux; ./test.sh
```
To verify the results:
* Examine the exit code
* Output should end with `TEST SUCCESSFUL`

Parameters:

`-s|--package-source` - specify package source (could be local directory), defaults to the nuget.org feed

`-v|--package-version` - specify package version, defaults to empty, so latest, non-prerelase version is selected

`-t|--test-dir` - specify temp directory used to run the test, defalts to `/tmp/postgres2go-test`

`-p|--project-dir` - point to the directory containing the test project

`-c|--clear-cache` - clear the local package cache before testing, useful when you need to reinstall modified package, but with the same version as before

Example:

```bash
./test.sh --package-source /my/local/dir --package-version 0.3.0 --clear-cache
```

## Testing with Docker

You can also test from Windows, inside a docker container. You need to have [Docker for Windows](https://docs.docker.com/docker-for-windows/) installed.

```powershell
cd linux
./test-in-docker.ps1
```

Parameters:

`-packageSource` - specify package source, defaults to nuget.org feed

`-sourceIsLocalDir` - set this switch if source specified with `-packageSource` is a local directory

`-packageVersion` - select package version to test

`-image` - select the image to test on, defaults to `microsoft/dotnet:2.0-sdk-stretch`

Example:

```powershell
.\test-in-docker.ps1 -packageSource ../package  -sourceIsLocalDir -packageVersion 0.3.0 -image microsoft/dotnet:2.1-sdk-stretch
```
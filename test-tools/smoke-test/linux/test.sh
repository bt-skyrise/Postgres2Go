#!/bin/bash
# assumes dotnet SDK is available

set -e # exit on error

# defaults
PROJECT_DIR="../project"
PACKAGE_SOURCE="https://api.nuget.org/v3/index.json"
TEST_DIR="/tmp/postgres2go-test"
CLEAR_PACKAGE_CACHE=true

while [[ $# -gt 0 ]]
do
key="$1"

case $key in
    -p|--project-dir)
    PROJECT_DIR="$2"
    shift 2
    ;;
    -s|--package-source)
    PACKAGE_SOURCE="$2"
    shift 2
    ;;
    -s|--package-version)
    PACKAGE_VERSION="$2"
    shift 2
    ;;
    -t|--test-dir)
    TEST_DIR="$2"
    shift 2
    ;;
    -nc|--no-clear-cache)
    CLEAR_PACKAGE_CACHE=false
    shift 
    ;;
esac
done

echo "Configuration:"
echo "PROJECT_DIR = $PROJECT_DIR"
echo "TEST_DIR = $TEST_DIR"
echo "PACKAGE_SOURCE = $PACKAGE_SOURCE"
echo "PACKAGE_VERSION = $PACKAGE_VERSION"
echo "CLEAR_PACKAGE_CACHE = $CLEAR_PACKAGE_CACHE"

echo "Clearing test directory"
if [ -d "$TEST_DIR" ]; then rm -rf $TEST_DIR; fi

if [ "$CLEAR_PACKAGE_CACHE" = true ] ; then
    echo "Clearing dotnet package cache"
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true dotnet nuget locals all --clear 
fi

echo "Copying project to test directory"
mkdir $TEST_DIR
cp -r $PROJECT_DIR/* $TEST_DIR

echo "Adding Postgres2Go package to the project"
CMD="dotnet add $TEST_DIR/Postgres2GoSmokeTest.csproj package Postgres2Go -s $PACKAGE_SOURCE"
if [ ! -z "$PACKAGE_VERSION" ] ; then CMD="$CMD -v $PACKAGE_VERSION"; fi
eval "$CMD"

echo "Running the test"
dotnet run --project $TEST_DIR/Postgres2GoSmokeTest.csproj
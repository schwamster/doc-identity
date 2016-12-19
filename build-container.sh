#!bin/bash
set -e
dotnet restore
dotnet test test/doc-identity.test/project.json -xml $(pwd)/testresults/out.xml
rm -rf $(pwd)/publish/web
dotnet publish src/doc-identity/project.json -c release -o $(pwd)/publish/web
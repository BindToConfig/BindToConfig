dotnet tool install --tool-path tools coverlet.console
dotnet tool install --tool-path tools dotnet-reportgenerator-globaltool
dotnet tool install --tool-path tools coveralls.net
tools\coverlet "src\BindToConfig.UnitTests\bin\Debug\netcoreapp2.1\BindToConfig.UnitTests.dll" --target "dotnet" --targetargs "test src\BindToConfig.UnitTests\BindToConfig.UnitTests.csproj --no-build" -f opencover
tools\reportgenerator "-reports:coverage.opencover.xml" "-targetdir:coveragereport"
tools\csmacnz.coveralls --opencover -i coverage.opencover.xml --repoToken %coverallsToken% --commitId %commitId% --commitBranch %commitBranch% --commitAuthor "%commitAuthor%" --commitEmail %commitEmail%  --commitMessage "%commitMessage%" --jobId %jobId%
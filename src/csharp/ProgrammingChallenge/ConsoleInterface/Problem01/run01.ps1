$targetTestDataFile = "TestData01.txt"

$scriptDir = Split-Path $script:MyInvocation.MyCommand.Path
$targetProject = "..\ConsoleInterface.csproj"

$testDataFile = Join-Path -Path $scriptDir -ChildPath $targetTestDataFile
$projectPath = Join-Path -Path $scriptDir -ChildPath $targetProject

Write-Output "Running '$projectPath' with data file '$testDataFile'."

Get-Content $testDataFile | dotnet run --project $projectPath
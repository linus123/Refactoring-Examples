#tool "nuget:?package=roundhouse&version=1.0.2"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var databaseName = "RefactoringExample";
var localSqlServer = "(localdb)\\MSSQLLocalDB";

var sqlScriptFolderPath = "./ProductionCode/DatabaseScripts/RefactoringExample";
var sqlVersionFile = "./ProductionCode/DatabaseScripts/_BuildInfo.xml";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("ResetDatabase")
    .Does(() =>
{
    Information($"** Dropping Database '{databaseName}' on '{localSqlServer}' **");

    RoundhouseDrop(new RoundhouseSettings{
        ServerName = localSqlServer,
        DatabaseName = databaseName,
        Silent = true
    });

    Information($"** Migrating Database '{databaseName}' with folder '{sqlScriptFolderPath}' on '{localSqlServer}' **");

    RoundhouseMigrate(new RoundhouseSettings{
        ServerName = localSqlServer,
        DatabaseName = databaseName,
        SqlFilesDirectory = sqlScriptFolderPath,
        Environment = "LOCAL",
        Silent = true,
        VersionFile = sqlVersionFile,
        VersionXPath = "//buildInfo/version"
    });

});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("ResetDatabase");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

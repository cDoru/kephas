@echo off
if [%1]==[] goto usage

@echo on
nuget push Kephas.Core.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Logging.NLog.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Logging.Log4Net.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Composition.Mef.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Platform.Net.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Model.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Data.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Data.Client.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Data.Model.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Data.InMemory.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Serialization.Json.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Messaging.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Messaging.Model.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Mail.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Mail.Net46.%1.nupkg -Source https://www.nuget.org/api/v2/package
nuget push Kephas.Web.Owin.%1.nupkg -Source https://www.nuget.org/api/v2/package

@echo Done.
pause

goto :eof
:usage
@echo Usage: %0 ^<version-number^>
pause
exit /B 1
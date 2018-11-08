@echo off
if "%1"=="get" goto getpackages
if "%1"=="add" goto addpackage
if "%1"=="remove" goto removepackage
goto usage
:getpackages
powershell Get-AppxProvisionedPackage -online
goto end
:addpackage
powershell Add-AppxProvisionedPackage -online -SkipLicense -PackagePath %2
goto end
:removepackage
powershell Remove-AppxProvisionedPackage -online -PackageName %2
goto end
:usage
echo "pkg get|add|remove [package path]|[package name]"
:end




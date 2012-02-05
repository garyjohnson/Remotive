SET BUILD_TYPE=Release
SET PLATFORM=x86
if "%1" == "Debug" set BUILD_TYPE=Debug
if "%2" == "x64" set PLATFORM=x64

REM Determine whether we are on an 32 or 64 bit machine
if "%PROCESSOR_ARCHITECTURE%"=="x86" if "%PROCESSOR_ARCHITEW6432%"=="" goto x86

set ProgramFilesPath=%ProgramFiles(x86)%
goto startInstall

:x86
set ProgramFilesPath=%ProgramFiles%

:startInstall
pushd "%~dp0"

SET WIX_BUILD_LOCATION=%ProgramFilesPath%\Windows Installer XML v3.5\bin
SET INTERMEDIATE_PATH=..\obj\%Platform%\%BUILD_TYPE%\
SET OUTPUTNAME=..\bin\%Platform%\%BUILD_TYPE%\RemotiveServer.msi

REM Cleanup leftover intermediate files

del /f /q "%INTERMEDIATE_PATH%\*.wixobj"
del /f /q "%OUTPUTNAME%"

REM Build the MSI for the setup package

"%WIX_BUILD_LOCATION%\candle.exe" Setup.wxs -dBuildType=%BUILD_TYPE% -dPlatform=%Platform% -out "%INTERMEDIATE_PATH%\Setup.wixobj"
"%WIX_BUILD_LOCATION%\light.exe" "%INTERMEDIATE_PATH%\setup.wixobj" -cultures:en-US -ext "%ProgramFilesPath%\Windows Installer XML v3.5\bin\WixUIExtension.dll" -ext "%ProgramFilesPath%\Windows Installer XML v3.5\bin\WixUtilExtension.dll" -ext "%ProgramFilesPath%\Windows Installer XML v3.5\bin\WixFirewallExtension.dll" -loc Setup-en-us.wxl -out "%OUTPUTNAME%" -notidy

popd
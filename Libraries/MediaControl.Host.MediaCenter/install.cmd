@ECHO OFF 
ECHO. 
ECHO.Usage: DevInstall.cmd [/u][/debug] 
ECHO.   

set CompanyName=Sharkfist
set AssemblyName=MediaControl.Host.MediaCenter
set RegistrationName=Registration
set ProgramImage=Application.png
set BuildTarget=x64

ECHO Determine whether we are on an 32 or 64 bit machine 
if "%PROCESSOR_ARCHITECTURE%"=="x86" if "%PROCESSOR_ARCHITEW6432%"=="" goto x86

ECHO.On an x64 machine 
set ProgramFilesPath=%ProgramFiles(x86)%
ECHO.   

goto unregister

:x86

	ECHO.On an x86 machine
	set BuildTarget=x86
	set ProgramFilesPath=%ProgramFiles%
	ECHO.

:unregister

	ECHO.*** Unregistering and deleting assemblies ***
	ECHO.      

	ECHO.Unregister and delete previously installed files (which may fail if nothing is registered)
	ECHO.

	ECHO.Unregister the application entry points
	%windir%\ehome\RegisterMCEApp.exe /allusers "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\%RegistrationName%.xml" /u
	ECHO.

	ECHO.Remove the DLL from the Global Assembly cache
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /u "%AssemblyName%"
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /u "MediaControl.Common"
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /u "Interop.WMPLib"
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /u "Id3Tag.Net"
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /u "CoreAudioApi"

	ECHO.Delete the folder containing the DLLs and supporting files (silent if successful)
	rd /s /q "%ProgramFilesPath%\%CompanyName%\%AssemblyName%"
	rd /s /q "%ProgramFilesPath%\%CompanyName%
	ECHO.

	REM Exit out if the /u uninstall argument is provided, leaving no trace of program files.
	if "%1"=="/u" goto exit

:releasetype

	REM evaluate the second argument
	if "%1"=="/debug" goto debug
	
	ECHO.Using the release version of the binaries
	set ReleaseType=Release
	ECHO.

	goto checkbin

:debug

	ECHO.Using the Debug version of the binaries
	set ReleaseType=Debug
	ECHO.

:checkbin

	ECHO "%CD%\bin\%BuildTarget%\%ReleaseType%\%AssemblyName%.dll"
	if exist "%CD%\bin\%BuildTarget%\%ReleaseType%\%AssemblyName%.dll" goto register
	ECHO.Cannot find %ReleaseType% binaries.
	ECHO.Build solution as %ReleaseType% and run script again.
	goto exit

:register

	ECHO.*** Copying and registering assemblies ***
	ECHO.

	ECHO.Create the path for the binaries and supporting files (silent if successful)
	md "%ProgramFilesPath%\%CompanyName%\%AssemblyName%"
	ECHO.

	ECHO.Copy the binaries to program files
	copy /y ".\bin\%BuildTarget%\%ReleaseType%\*.dll" "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\"
	ECHO.

	ECHO.Copy the registration XML to program files
	copy /y ".\%RegistrationName%.xml" "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\"
	ECHO.

	ECHO.Copy the program image to program files
	copy /y ".\%ProgramImage%" "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\"
	ECHO.

	ECHO.Register the DLL with the global assembly cache
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /if "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\Interop.WMPLib.dll"
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /if "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\MediaControl.Common.dll"
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /if "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\%AssemblyName%.dll"
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /if "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\Id3Tag.Net.dll"
	"%ProgramFilesPath%\Microsoft SDKs\Windows\v7.0A\Bin\gacutil.exe" /if "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\CoreAudioApi.dll"
	ECHO.

	ECHO.Register the application with Windows Media Center
	%windir%\ehome\RegisterMCEApp.exe /allusers "%ProgramFilesPath%\%CompanyName%\%AssemblyName%\%RegistrationName%.xml"
	ECHO.

:exit

PAUSE
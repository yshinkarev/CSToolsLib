@echo off

echo Clean.
rd /S /Q bin
rd /S /Q obj

echo Build.
call msbuild CSToolsLib.sln /t:Rebuild /p:Configuration=Release

if %errorlevel% NEQ 0 (
	call check-error-level
	exit 1 /B
)

echo Copy to cmd.
set DIR=bin\Release
copy /Y /B %DIR%\CommandLine.dll %MY_CMD%
copy /Y /B %DIR%\Common.Utils.dll %MY_CMD%
copy /Y /B %DIR%\CSToolsLib.exe %MY_CMD%

echo Clean.
rd /S /Q bin
rd /S /Q obj
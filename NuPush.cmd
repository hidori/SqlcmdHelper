@echo off

set CMD_HOME=%~dp0
call "%CMD_HOME%config.cmd"

for %%I in ("%TARGET_DIR%%TARGET%\*.nupkg") do (
	set TARGET_PACKAGE=%%I
)

%ECHO% "%NUGET_BIN%" push "%TARGET_PACKAGE%"

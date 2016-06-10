@echo off

set CMD_HOME=%~dp0
call "%CMD_HOME%config.cmd"

%ECHO% "%NUGET_BIN%" pack "%TARGET_DIR%%TARGET%\%TARGET%.nuspec" -OutputDir "%TARGET_DIR%%TARGET%"

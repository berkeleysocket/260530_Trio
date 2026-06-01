@echo off
set OUTPUT_PATH=.
flatc.exe --csharp -o %OUTPUT_PATH% Monster.fbs
pause
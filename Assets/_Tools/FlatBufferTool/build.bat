@echo off
set OUTPUT_PATH=C:\GitHub\260530_Trio\Assets\_Scripts\Runtime\Shared\Packet
flatc.exe --csharp -o %OUTPUT_PATH% RoomPacket.fbs
pause
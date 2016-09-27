start STAFProc.exe

REM pause "Pause for a few seconds so STAFProc can start."

sleep 5

staf local service add service safsmaps  library jstaf execute c:/safs/lib/safsmaps.jar PARMS dir c:\safs\Project\datapool
staf local service add service safsvars  library jstaf execute c:/safs/lib/safsvars.jar
staf local service add service safsinput library jstaf execute c:/safs/lib/safsinput.jar PARMS dir c:\safs\Project\datapool
staf local service add service safslogs  library jstaf execute c:/safs/lib/safslogs.jar PARMS dir c:\safs\Project\datapool


REM Start TestComplete in interactive mode for debugging purposes, for the AMO project

REM "C:\Program Files (x86)\Automated QA\TestComplete 8\Bin\TestComplete.exe" /safs.project.config:C:\Automation\webreportstudio\tidtest.ini

REM "C:\Program Files\Automated QA\TestComplete 8\Bin\TestComplete.exe" /safs.project.config:c:\automation\TCAFS.ini

REM "C:\Program Files\Automated QA\TestComplete 8\Bin\TestComplete.exe" "C:\safs\TCAFS\TCAFS.pjs" /r /p:TCAFS /t:"Script|StepDriver|Main" /safs.project.config:c:\automation\TCAFS.ini

REM "C:\Program Files\SmartBear\TestComplete 9\Bin\TestComplete.exe" "C:\safs\TCAFS\TCAFS.pjs" /r /p:TCAFS /t:"Script|StepDriver|Main" /safs.project.config:c:\automation\TCAFS.ini

"C:\Program Files\SmartBear\TestComplete 10\Bin\TestComplete.exe" "C:\safs\TCAFS\TCAFS.pjs" /r /p:TCAFS /t:"Script|StepDriver|Main" /safs.project.config:c:\automation\TCAFS.ini

pause
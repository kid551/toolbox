# TCAFS Debug

This project is used to clarify the process of debugging TCAFS.

### Adjust the Environment Variables
Changed the tool from TestExecute to TestComplete, you can refer to the following configuration.

Set your ```TESTCOMPLETE_EXE``` and ```TESTCOMPLETE_HOME``` System variables appropriately.

| Env Variable       | Value |
| ------------       | ----- |
| ```TESTCOMPLETE_EXE```   | TestComplete.exe |
| ```TESTCOMPLETE_HOME```  | C:\Program Files (x86)\SmartBear\TestComplete 10 |

### Adjust TCAFS.vbs file
```bat
binpath = "%TESTCOMPLETE_HOME%\bin\"
executable = binpath & "TestComplete.exe"
status = env("TESTCOMPLETE_EXE")
```

### Debug Steps

**Version 1**

1. ```%SAFS%```\Project\SAFSTCAFSDebuggerStartup.bat
	- Add break point in code
	- Run your test, launched by SAFSDriver (eg: runTCAFStest.bat)
	- Hit the break point
2. ```%SAFS%```\Project\SAFSTCAFSShutdown.bat  (close TestComplete)


**Version 2**

1.	Run SAFSTCAFSDebuggerStartup.bat, Or Launch TC and run stepdriver
2.	Run tests to debug
3.	SAFSTCAFSShutdown.bat  (Quit)


### Command in TCAFS.vbs
```command = executable &" "& suitename & " /r /p:" & projectname & " /u:" & scriptname & " /rt:Main /e /SilentMode /ns"& safsconfig & passthru```

Reference URL: https://support.smartbear.com/viewarticle/81120/.

| Command Option | Description |
| -------------- | ----------- |
| /r             | run |
| /p:            | project name|
| /u:            | unit name, i.e script name |
| /rt:           | routine name, the specified function name |
| /e:            | exit |
| /SilentMode    | TestComplete works in Silent mode if this argument is sepcified |
| /ns            | Open TestComplete without displaying the splash screen |
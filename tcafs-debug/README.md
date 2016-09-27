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

- **/project**:project_name  **/test**:test_name
```"C:\Program Files\SmartBear\TestComplete 10\Bin\TestComplete.exe" "C:\safs\TCAFS\TCAFS.pjs" /r /p:TCAFS /t:"Script|StepDriver|Main" /safs.project.config:c:\automation\TCAFS.ini```

- **/project**:project_name  **/unit**:unit_name  **/routine**:routine_name
```command = executable &" "& suitename & " /r /p:" & projectname & " /u:" & scriptname & " /rt:Main /e /SilentMode /ns"& safsconfig & passthru```

Reference URL: https://support.smartbear.com/viewarticle/81120/.

| Command Option | Description |
| -------------- | ----------- |
| /r             | run |
| /t or /test    | Test_name is the full name of the needed test. The full name of a test includes the name of the test's parent project item and the test name, which are separated by the pipe character (vertical bar). For script tests, the full name also includes the the name of the unit. Enclose your tests' full names in quotation marks. |
| /p:            | project name|
| /u:            | unit name, i.e script name |
| /rt:           | routine name, the specified function name |
| /e:            | exit |
| /SilentMode    | TestComplete works in Silent mode if this argument is sepcified |
| /ns            | Open TestComplete without displaying the splash screen |

### Three Levels of Test in SAFS
| Level | File Extension |
| --- | --- |
| Step | .SDD |
| Suite | .STD |
| Cycle | .CDD |


### DDDriverCommands (Data-Driven Driver Command)

Function provided for DDE(Data-Driven Engine?) users:

**Data Table Sample**

| DDriverCommand Name | Description |
| --- | --- |
| B   | Define a Named Block within the file |
| C   | StepDriver Command |
| S   | SKIP this Record |
| T   | Perform a ComponentFunction action or test |

``` c, Version , "1.0" ```

``` c, SetApplicationMap , "AppMap.map" ``` 

``` c, LaunchApplication , MyApp , "C:\SomeDir\MyApp.exe", , , "AppMap.map" ``` 

``` c,    WaitForGUI     , LoginWindow , LoginWindow , 30 ``` 

Do some LoginWindow tests here....

``` t, LoginWindow , UserIDField , VerifyProperty , "Text" , "userid"``` 

``` t, LoginWindow , UserIDField , SetTextValue   , ^USER = "MyUserID"``` 

``` t, LoginWindow ,   OKButton  ,   Click``` 

``` t, MainWindow  , MainWindow  , VerifyProperty , "Caption", ^USER``` 


### Control process among TCAFS components

All the IPCs are controlled by STAF in SAFS. Thus if you want to startup or shutdown program, you need to obey some orders of running.

1. "SAFSTCAFSDebuggerStartup.bat" --> Open up the TestComplete
2. "Test Composer" --> Open up the browser and TestComplete
3. "STAF" control all the IPCs

In order to close the TCAFS running program, you need to 

1. Close the TestComplete by using 'SAFSTCAFSShutdown.bat'/'Stop TestComplete'
2. Close the STAF
3. Close the SAFSTCAFSDebuggerStartup

To open again,

1. Start the STAF
2. Start the SAFSTESTLOG_Startup.bat
3. Start the SAFSTCAFSDebuggerStartup
4. Start the Test Composer




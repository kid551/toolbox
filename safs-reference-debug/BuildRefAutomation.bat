@echo off
setlocal enableDelayedExpansion

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
::
:: This file is used to generate SAFS build. Before run it, 'Ant' and 'git' 
:: should be installed first. And 
::     - 'antcontrib.jar', 
::     - 'apache-commons-net.jar'
::     - 'jakarta-oro-2.0.8.jar'
:: need to be put into %ANT_HOME%/lib folder. More 
:: details can be found in 
::               https://github.com/SAFSDEV/Core/blob/master/developer_setup.md
:: 
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

REM Copy "C:\SeleniumPlusDev\build\safs\bootstrap.build.xml" .

Echo Remove the existing old 'build' folder and 'bootstrap.build.xml'
rd /s /q "build"


Echo Clone build repository from GitHub
git clone "https://github.com/SAFSDEV/build.git"
Copy "build\safs\bootstrap.build.xml" .


REM Remove the 'buid' file
rd /s /q "build"


Echo Execute Ant Script
Call ant -f bootstrap.build.xml bootstrapbuild

REM Call ant update.safs.reference

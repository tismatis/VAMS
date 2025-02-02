@echo off
setlocal enabledelayedexpansion

CLS

echo Run   Execution Time (ms) > results.txt
echo ---------------------------- >> results.txt

for /L %%i in (1,1,25) do (
    rem Run the program and store output in a temporary file
    lASILLite.exe > temp_output.txt

    rem Read temp_output.txt line by line
    for /f "tokens=5" %%a in ('findstr /C:"Function Main executed in" temp_output.txt') do (
        set time=%%a
        echo %%i     !time! ms >> results.txt
    )
)

rem Display the results table
type results.txt

rem Clean up temporary file
del temp_output.txt

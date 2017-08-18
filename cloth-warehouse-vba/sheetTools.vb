
' *****************************************************************
' *
' * General tool subs/functions library
' *
' *****************************************************************



' Get the last non-empty row at (by default) column "a"
'
' - wSheet Worksheet,target sheet
' - col String, optional, the non-empty column
'
Function getLastNonEmptyRow(wSheet As Worksheet, Optional col As String = "a") As Integer
    getLastNonEmptyRow = wSheet.Range(col & "65536").End(3).Row
End Function



' Get specified region. It requires the top-left and bottom-right coordinate.
'
' - wSheet WorkSheet, the sheet which contains the specified range
' - startRow, the top-left point's row index
' - endColumn, the bottom-right point's column index
'
Function getRegion(wSheet As Worksheet, startRow, endColumn) As Range
    Set getRegion = wSheet.Range(printf("a{0}:{1}{2}", startRow, endColumn, getLastNonEmptyRow(wSheet)))
End Function



' Copy a row after the last non-empty row of target sheet at (by default) column "a".
'
' - copiedRow, the copied row
' - targetSheet Worksheet, the target worksheet
'
Function appendRowToSheet(copiedRow, targetSheet As Worksheet, Optional col As String = "a")
    copiedRow.Copy targetSheet.Range(printf("{0}{1}", col, getLastNonEmptyRow(targetSheet) + 1))
End Function



' Save the target workbook file to ".\bak" directory, and
' append timestamp to the name of target file.
'
' - workbookName, the target saved workbook name
'
Sub bakupFile(workbookName)
    ' Here we assume the bakuped file is ended with '.xls'
    preFileName = Replace(workbookName, ".xls", "")
    
    ' Merge the "YYYY-MM-DD" and "HHMMSS" into time stamp
    newDate = Replace(Date, "-", "")
    newDate = Replace(newDate, "/", "")
    newTime = Left(Replace(Time(), ":", ""), 4)
    timeStamp = newDate & newTime
    
    currentDir = Application.ActiveWorkbook.Path
    src = currentDir & "\" & fileName
    dest = currentDir & "\bak\" & preFileName & "-" & timeStamp & ".xls"
    
    Workbooks(workbookName).SaveCopyAs dest
End Sub



' Implement formatting feature of C language "printf()" method
'
' - mask, the formatting string, which can contain "{0}", "{1}" etc,.
' - tokens, the parameters that will be replaced in "{0}", "{1}" etc,.
'
Public Function printf(mask As String, ParamArray tokens()) As String
    Dim i As Long
    
    For i = 0 To UBound(tokens)
        mask = Replace(mask, "{" & i & "}", tokens(i))
    Next
    
    printf = mask
End Function


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
Sub backupFile(workbookName)
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



' Generate the expression of searched line.
'
'   - searchedRow Integer, the row index of searched line
'   - columns(), the columns of searched row
'
' For example:
'     "a12 & b12 & d12 & f12"
'
Function genSearchedLines(searchedRow As Integer, columns()) As String
    Dim searchedExpression As String
        
    Dim i As Long
    For i = 0 To UBound(columns)
    
        If i = 0 Then
            searchedExpression = printf("{0}{1}", columns(i), searchedRow)
        Else
            searchedExpression = searchedVal & printf("&{0}{1}", columns(i), searchedRow)
        End If
    Next
    
    genSearchedLines = searchedExpression
    
End Function



' Generate searched area. By default, the searched area is from the
' first row to line just before the searched row.
'
' For example:
'     "a1:a10 & b1:b10 & d1:d10 & f1:f10"
'
Function genSearchedArea(searchedRow As Integer, columns()) As String
    Dim searchedArea As String
    endRow = searchedRow - 1
    
    Dim i As Long
    For i = 0 To UBound(columns)
    
        If i = 0 Then
            searchedArea = printf("{0}1:{0}{1}", columns(i), endRow)
        Else
            searchedArea = searchedArea & printf("&{0}1:{0}{1}", columns(i), endRow)
        End If
    Next
    
    genSearchedArea = searchedArea
    
End Function



' Get the row index of line which matches the searched row.
'
'   - searchedSheet Worksheet, the worksheet to search
'   - searchColumnArray, the array of columns that will be searched
'   - searchRow, the row index of searched line
'
Function getMatchedIndex(searchedSheet As Worksheet, searchColumnArray(), searchRow As Integer)

    matchExpression = printf("Match({0}, {1}, 0)", _
                            genSearchedLines(searchRow, searchColumnArray), _
                            genSearchedArea(searchRow, searchColumnArray))
                            
    getMatchedIndex = searchedSheet.Evaluate(matchExpression)
    
End Function


Public Function stringFormat(mask As String, ParamArray tokens()) As String
    Dim i As Long
    
    For i = 0 To UBound(tokens)
        mask = Replace(mask, "{" & i & "}", tokens(i))
    Next
    
    stringFormat = mask
End Function

Function getLastRowIndex(wSheet As Worksheet, Optional col As Integer = 1) As Integer
    getLastRowIndex = wSheet.Cells(wSheet.Rows.Count, col).End(xlUp).row    
End Function

Function getLastColIndex(wSheet As Worksheet, Optional row As Integer = 1) As Integer
    getLastColIndex = wSheet.Cells(row, wSheet.columns.Count).End(xlToLeft).Column    
End Function

' Construct virtual clipboard in a temporary sheet.
Function concatenateTwoRanges(r1 As Range, r2 As Range) As Range
    
    Dim virtualCPB As Worksheet
    Set virtualCPB = Workbooks("控制中心.xlsm").Sheets("virtualCPB")
    virtualCPB.Rows(1).Delete
    
    r1.Copy virtualCPB.Cells(1, 1)
    r2.Copy virtualCPB.Cells(1, getLastColIndex(virtualCPB) + 1)
    
    ' Get first row range.
    Set concatenateTwoRanges = virtualCPB.Range( _
                            virtualCPB.Cells(1, 1), _
                            virtualCPB.Cells(1, getLastColIndex(virtualCPB)) _
                                          )
End Function

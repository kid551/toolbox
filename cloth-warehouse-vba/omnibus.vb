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

' arr() is an array of ranges you want to range.
'
' For example, you can call this method like this:
'
'   <======
'   Dim arrE() As Variant
'     arrE = Array(w1.Cells(1, 5), w1.Cells(1, 3), w1.Cells(1, 2), w1.Cells(1, 1))
'   concatenateCells(arrE).Copy w1.Range("a15")
'   ======>
'
Function concatenateCells(arr() As Variant)
    Dim virtualCPB As Worksheet
    Set virtualCPB = Workbooks("控制中心.xlsm").Sheets("virtualCPB")
    virtualCPB.Rows(1).Delete
    
    pos = 1
    For i = 0 To UBound(arr)
        arr(i).Copy virtualCPB.Cells(1, pos)
        pos = getLastColIndex(virtualCPB) + 1
    Next
    
    Set concatenateCells = virtualCPB.Range( _
                            virtualCPB.Cells(1, 1), _
                            virtualCPB.Cells(1, getLastColIndex(virtualCPB)) _
                                          )
End Function

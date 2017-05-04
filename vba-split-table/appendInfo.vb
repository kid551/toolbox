Sub appendInfo()
    Dim currCellRow, st1Lst
    currCellRow = Selection.Row
    st1Lst = Sheets(1).Range("a65536").End(3).Row
    stG3FstEmpty = (Sheets("3����").Range("a65536").End(3).Row + 1)
    
    ' Get region of current cell to last column "o" cell.
    currRowToLast = "a" & currCellRow & ":o" & st1Lst
    
    ' Get 503's last non-empty plus one cell in column "a", i.e. first empty cell.
    stG3Empty = "a" & stG3FstEmpty
    
    Sheets(1).Range(currRowToLast).Copy Sheets("3����").Range(stG3Empty)
End Sub



Function getColorDict() As Object
    Set getColorDict = CreateObject("Scripting.Dictionary")
    
    getColorDict.Add "501", "1���"
    getColorDict.Add "502", "2����"
    getColorDict.Add "503", "3����"
    getColorDict.Add "504", "4ǳ��"
    getColorDict.Add "505", "5��ɫ"
    getColorDict.Add "506", "6ǳ��"
    getColorDict.Add "508", "8����"
    getColorDict.Add "509", "9���"
    getColorDict.Add "510", "10���"
    getColorDict.Add "511", "11�����"
    getColorDict.Add "512", "12���"
    getColorDict.Add "514", "14�װ�"
    getColorDict.Add "601", "601"
    getColorDict.Add "602", "602"
    getColorDict.Add "603", "603����"
    getColorDict.Add "604", "604���"
    getColorDict.Add "605", "605�̻�"
    getColorDict.Add "606", "606���"
    getColorDict.Add "607", "607���"
    getColorDict.Add "608", "608"
    getColorDict.Add "609", "609"
    getColorDict.Add "610", "610"
    getColorDict.Add "611", "611"
    getColorDict.Add "612", "612"
    getColorDict.Add "����", "����"
    getColorDict.Add "����ʹ����ɫ", "����ʹ����ɫ"
End Function

Function getLastRowIndx(ByVal sheetName)
    ' get the last non-empy row index of column "a"
    getLastRowIndx = Sheets(sheetName).Range("a65536").End(3).Row
End Function

Function getAddedRegion(ByVal sheetName, ByVal lstColIndx) As Range
    currRow = Selection.Row
    sheet1LastRowIndx = getLastRowIndx(sheetName)
    
    ' Get region string of current cell to last column "lstCol" cell.
    addedRegionStr = "a" & currRow & ":" & lstColIndx & sheet1LastRowIndx
    
    Set getAddedRegion = Sheets(sheetName).Range(addedRegionStr)
End Function

Sub appendInfoRByR()
    Dim colorDict As Object
    Set colorDict = getColorDict()
    
    For Each iRow In getAddedRegion(1, "o").Rows
        corrSheetName = colorDict(Right(iRow.Cells(2), 3))
        corrSheetStartIndx = "a" & (getLastRowIndx(corrSheetName) + 1)
        
        iRow.Copy Sheets(corrSheetName).Range(corrSheetStartIndx)
    Next
    
End Sub

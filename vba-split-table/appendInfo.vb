Sub appendInfo()
    Dim currCellRow, st1Lst
    currCellRow = Selection.Row
    st1Lst = Sheets(1).Range("a65536").End(3).Row
    stG3FstEmpty = (Sheets("3军绿").Range("a65536").End(3).Row + 1)
    
    ' Get region of current cell to last column "o" cell.
    currRowToLast = "a" & currCellRow & ":o" & st1Lst
    
    ' Get 503's last non-empty plus one cell in column "a", i.e. first empty cell.
    stG3Empty = "a" & stG3FstEmpty
    
    Sheets(1).Range(currRowToLast).Copy Sheets("3军绿").Range(stG3Empty)
End Sub



Function getColorDict() As Object
    Set getColorDict = CreateObject("Scripting.Dictionary")
    
    getColorDict.Add "501", "1芥黄"
    getColorDict.Add "502", "2土黄"
    getColorDict.Add "503", "3军绿"
    getColorDict.Add "504", "4浅卡"
    getColorDict.Add "505", "5杏色"
    getColorDict.Add "506", "6浅灰"
    getColorDict.Add "508", "8军黄"
    getColorDict.Add "509", "9深军"
    getColorDict.Add "510", "10深灰"
    getColorDict.Add "511", "11深藏青"
    getColorDict.Add "512", "12深卡其"
    getColorDict.Add "514", "14米白"
    getColorDict.Add "601", "601"
    getColorDict.Add "602", "602"
    getColorDict.Add "603", "603卡其"
    getColorDict.Add "604", "604深卡其"
    getColorDict.Add "605", "605绿灰"
    getColorDict.Add "606", "606深卡其"
    getColorDict.Add "607", "607深灰"
    getColorDict.Add "608", "608"
    getColorDict.Add "609", "609"
    getColorDict.Add "610", "610"
    getColorDict.Add "611", "611"
    getColorDict.Add "612", "612"
    getColorDict.Add "灰杏", "灰杏"
    getColorDict.Add "乔雄使用颜色", "乔雄使用颜色"
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

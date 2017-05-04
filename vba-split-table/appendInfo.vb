Sub appendInfo()
    Dim currCellRow, st1Lst
    currCellRow = Selection.Row
    st1Lst = Sheets(1).Range("a65536").End(3).Row
    stG3FstEmpty = (Sheets("3¾üÂÌ").Range("a65536").End(3).Row + 1)
    
    ' Get region of current cell to last column "o" cell.
    currRowToLast = "a" & currCellRow & ":o" & st1Lst
    
    ' Get 503's last non-empty plus one cell in column "a", i.e. first empty cell.
    stG3Empty = "a" & stG3FstEmpty
    
    Sheets(1).Range(currRowToLast).Copy Sheets("3¾üÂÌ").Range(stG3Empty)
End Sub



Function getColorDict() As Object
    Set getColorDict = CreateObject("Scripting.Dictionary")
    
    getColorDict.Add "503", "3¾üÂÌ"
    getColorDict.Add "504", "4Ç³¿¨"
End Function

Function getAddedRegion(ByVal num, ByVal lstColIndx) As Range
    currRow = Selection.Row
    sheet1LastRowIndx = Sheets(num).Range("a65536").End(3).Row
    
    ' Get region string of current cell to last column "lstCol" cell.
    addedRegionStr = "a" & currRow & ":" & lstColIndx & sheet1LastRowIndx
    
    ' Dim addedRegion As Range
    Set getAddedRegion = Sheets(1).Range(addedRegionStr)
End Function

Sub getSubName()
    Dim colorDict As Object
    Set colorDict = getColorDict()
    
    Dim addedRegion As Range
    Set addedRegion = getAddedRegion(1, "o")
        
    For Each iRow In addedRegion.Rows
        corrSheetName = colorDict(Right(iRow.Cells(2), 3))
        corrSheetFirstEmpty = (Sheets(corrSheetName).Range("a65536").End(3).Row + 1)
                
        iRow.Copy Sheets(corrSheetName).Range("a" & corrSheetFirstEmpty)
    Next
    
End Sub
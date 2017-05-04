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

Sub getSubName()
    ' Create color code dictionary
    Dim colorDict As Object
    Set colorDict = CreateObject("Scripting.Dictionary")
    
    colorDict("503") = "3?¨¹?¨¬"
    colorDict("504") = "4?3?¡§"
    
    currRow = Selection.Row
    sheet1LastRow = Sheets(1).Range("a65536").End(3).Row
    
    ' Get region string of current cell to last column "o" cell.
    addedRegionStr = "a" & currRow & ":o" & sheet1LastRow
    
    Dim addedRegion As Range
    Set addedRegion = Sheets(1).Range(addedRegionStr)
        
    For Each iRow In addedRegion.Rows
        corrSheetName = colorDict(Right(iRow.Cells(2), 3))
        corrSheetFirstEmpty = (Sheets(corrSheetName).Range("a65536").End(3).Row + 1)
                
        iRow.Copy Sheets(corrSheetName).Range("a" & corrSheetFirstEmpty)
    Next
    
End Sub
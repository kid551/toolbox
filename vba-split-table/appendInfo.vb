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

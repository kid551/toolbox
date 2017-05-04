Sub split()

Dim ar, I, k, r, term, col
Dim d As Object
Application.DisplayAlerts = False


Set d = CreateObject("Scripting.Dictionary")

Sheet1.Select

'Sample:
'    Range("A1").CurrentRegion.Sort Key1:=[c1], Order1:=xlAscending, Header:=xlYes   'order by column "bank": [c1]'
'
'Comments:
'    Range("a65536").End(3): the cell from last cell of column "a", i.e. a65536, to the top which is the 
'    first non-empty cell. So the whole part means: column "a"'s last non-empty cell.
Range("a3:q" & Range("a65536").End(3).Row).Sort Key1:=[d1], Order1:=xlAscending, Header:=xlNo

'ar = Range([a2], [h65536].End(3)) Choose the area from [a2] to the last non-empty element of column h'

ar = Range("a3:q" & Range("a65536").End(3).Row) 'Choose the area from [a3] to h column and row which is the last non-empty of column a'



'Count the number of entries with same key'
For I = 1 To UBound(ar)
    d(ar(I, 4)) = d(ar(I, 4)) + 1
Next

k = d.keys

On Error Resume Next

For I = 0 To UBound(k)
    'Parameter 1 indicates xlWhole, parameter 2 indicates xlPart'
    'r = Sheet1.[e:e].Find(k(I), , , 1).Row
    r = Sheet1.[d:d].Find(k(I), , , xlWhole).Row
    
    Worksheets(k(I)).Delete
    Worksheets.Add(after:=Sheets(Sheets.Count)).Name = k(I)
    
    'Copy the 1st line and 2nd line(the meaning of [1:1]) of Sheet1 to another sheet starting from [a1]'
    Sheet1.[1:2].Copy [a1]
    'Copy d(K(I)) lines from the r_th line to another sheet starting from [a3]'
    Sheet1.Rows(r).Resize(d(k(I))).Copy [a3]
        
    'Re-compute "Total Price" column
    'Range("g2") = "=e2 - f2"
        
    'For ii = 3 To ActiveSheet.Range("a65536").End(3).Row
    '    Range("g" + CStr(ii)) = "=e" + CStr(ii) + "-f" + CStr(ii) + "+g" + CStr(ii - 1)
    'Next
    
Next

'Recover the original sheet, order by column [a1]'
Sheet1.Select
Range("a3:q" & Range("a65536").End(3).Row).Sort Key1:=[a1], Order1:=xlAscending, Header:=xlNo
'Range("A1").CurrentRegion.Sort Key1:=[a1], Order1:=xlAscending, Header:=xlYes

End Sub


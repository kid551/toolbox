Sub split()

Dim ar, I, k, r, term, col
Dim d As Object
Application.DisplayAlerts = False


Set d = CreateObject("Scripting.Dictionary")

Sheet1.Select
'Range("A1").CurrentRegion.Sort Key1:=[d1], Order1:=xlAscending, Header:=xlYes   'order by column "bank": [e1]'
Range("a3:p" & Range("a65536").End(3).Row).Sort Key1:=[d1], Order1:=xlAscending, Header:=xlNo

'ar = Range([a2], [h65536].End(3))  Choose the area from [a2] to the last non-empty element of column h'

ar = Range("a3:p" & Range("a65536").End(3).Row) 'Choose the area from [a3] to h column and row which is the last non-empty of column a'


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
    For ii = 3 To ActiveSheet.Range("a65536").End(3).Row
        Range("j" + CStr(ii)) = "=h" + CStr(ii) * "i" + CStr(ii)
    Next
    
        
    'Re-compute "Residual" column
    Range("n3") = "=j3 - l3 - m3"
        
    For ii = 4 To ActiveSheet.Range("a65536").End(3).Row
        Range("n" + CStr(ii)) = "=j" + CStr(ii) + "-l" + CStr(ii) + "-m" + CStr(ii) + "+n" + CStr(ii - 1)
    Next
    
Next

'Recover the original sheet, order by column [a1]'
Sheet1.Select
'Range("A1").CurrentRegion.Sort Key1:=[a1], Order1:=xlAscending, Header:=xlYes
Range("a3:p" & Range("a65536").End(3).Row).Sort Key1:=[a1], Order1:=xlAscending, Header:=xlNo


'Re-compute "total price" column
Range("n3") = "=j3 - l3 - m3"


'Re-compute "residual" column
Range("n3") = "=j3 - l3 - m3"
        
For ii = 4 To ActiveSheet.Range("a65536").End(3).Row
    Range("n" + CStr(ii)) = "=j" + CStr(ii) + "-l" + CStr(ii) + "-m" + CStr(ii) + "+n" + CStr(ii - 1)
Next

End Sub


Private Sub Workbook_Open()

End Sub

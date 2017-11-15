Sub test4()
    
    Dim warehouseWorkbook As Workbook
    Dim w1 As Worksheet
    Dim w2 As Worksheet
    
    Set warehouseWorkbook = Workbooks("常熟出入库存表.xls")
    Set w1 = warehouseWorkbook.Sheets("terry")
    Set w2 = warehouseWorkbook.Sheets("工作表1")
    
    Dim searchColArr()
    searchColArr = Array("a", "b", "c")

    MsgBox sheetTools.getLastNonEmptyRow(w2)
    MsgBox sheetTools.getLastNonEmptyRow(w2, "b")
End Sub

Sub test2()

    Dim warehouseWorkbook As Workbook
    Dim w1 As Worksheet
    Dim w2 As Worksheet
    
    Set warehouseWorkbook = Workbooks("常熟出入库存表.xls")
    Set w1 = warehouseWorkbook.Sheets("terry")
    Set w2 = warehouseWorkbook.Sheets("工作表1")
       
    Dim searchColArr()
    searchColArr = Array("a", "b", "c")

 
    resRow = getMatchedIndex(w1, searchColArr, 12)
    If IsError(resRow) Then
        MsgBox "Not Found"
        Err.Clear
    Else
        MsgBox resRow
    End If
End Sub

Sub test1()

    Dim warehouseWorkbook As Workbook
    Dim w1 As Worksheet
    Dim w2 As Worksheet
    
    Set warehouseWorkbook = Workbooks("常熟出入库存表.xls")
    Set w1 = warehouseWorkbook.Sheets("terry")
    Set w2 = warehouseWorkbook.Sheets("工作表1")
       
    Dim searchColArr()
    searchColArr = Array("b", "e", "f", "g", "h", "i")
    
    MsgBox getMatchedIndex(w2, searchColArr, 6125)
End Sub

Sub test()
    
    Dim warehouseWorkbook As Workbook
    Set warehouseWorkbook = Workbooks("常熟出入库存表.xls")
    
    ' warehouseWorkbook.Sheets("工作表1").Range($B$3:$B$6126)
        
    ' Application.MATCH(1,INDEX((warehouseWorkbook.Sheets("工作表1").Range("$B$3:$B$6126")="C32X21 133X78 505"),0)+2
    
    ' Application.MATCH(1,INDEX((B3:B6126="C32X21 133X78 505") * (E$3:$E$6126="聚祥染厂") * ($F$3:$F$6126="17.05.12") * ($G$3:$G$6126=122)*($I$3:$I$6126=129),),0)+2
    
    Set w1 = warehouseWorkbook.Sheets("terry")
    
    c1 = warehouseWorkbook.Sheets("terry").Range("a2").Address(False, False)
    c2 = warehouseWorkbook.Sheets("terry").Range("b2").Address(False, False)
    c3 = warehouseWorkbook.Sheets("terry").Range("c2").Address(False, False)
    
    reg1 = warehouseWorkbook.Sheets("terry").Range("a5:a9").Address(False, False)
    reg2 = warehouseWorkbook.Sheets("terry").Range("b5:b9").Address(False, False)
    reg3 = warehouseWorkbook.Sheets("terry").Range("c5:c9").Address(False, False)
    
    eva_express = stringFormat("Match({0}&{1}&{2}, {3}&{4}&{5}, 0)", c1, c2, c3, reg1, reg2, reg3)
    
    cc = Evaluate(eva_express)
    
    MsgBox cc
    
    
End Sub


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
    
    getColorDict.Add "C32", "32�ܿ��"
    getColorDict.Add "C16", "16�ܿ��"
End Function

Function getLastRowIndx(ByVal sheetName)
    ' get the last non-empy row index of column "a"
    getLastRowIndx = Sheets(sheetName).Range("a65536").End(3).Row
End Function

Function getAddedRegion(ByVal sheetName, ByVal lstColIndx, ByVal startRow) As Range
    currRow = startRow
    sheet1LastRowIndx = getLastRowIndx(sheetName)
    
    ' Get region string of current cell to last column "lstCol" cell.
    addedRegionStr = "a" & currRow & ":" & lstColIndx & sheet1LastRowIndx
    
    Set getAddedRegion = Sheets(sheetName).Range(addedRegionStr)
End Function

Function copyRowToSheet(ByVal copiedRow, ByVal sheetName)
    corrSheetStartIndx = "a" & (getLastRowIndx(sheetName) + 1)
    
    copiedRow.Copy Sheets(sheetName).Range(corrSheetStartIndx)
End Function

Sub appendInfoRByR()
    Dim controlCenter As Workbook
    Set controlCenter = Workbooks("��������.xlsm")
    
    warehouseWBName = controlCenter.Sheets(1).Range("b2")
    startPos = controlCenter.Sheets(1).Range("b3")
    Workbooks(warehouseWBName).Activate
    
    Dim colorDict As Object
    Set colorDict = getColorDict()
    
    For Each iRow In getAddedRegion(1, "o", startPos).Rows
        Call copyRowToSheet(iRow, colorDict(Right(iRow.Cells(2), 3)))
        Call copyRowToSheet(iRow, colorDict(Left(iRow.Cells(2), 3)))
    Next
    
End Sub


Sub copyToWorkBook()
    warehouseWBName = Range("b2")
    
    Dim warehouse As Workbook
    Set warehouse = Workbooks(warehouseWBName)
    
    Dim customer As Workbook
    Set customer = Workbooks(warehouseWBName)
    
        
    warehouse.Sheets(1).Rows(4775).Copy customer.Sheets(1).Cells(4777, 1)
End Sub

Sub test()

End Sub


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
    firstColIndx = "a"
    
    ' ***********************
    
    currRow = startRow
    sheet1LastRowIndx = getLastRowIndx(sheetName)
    
    ' Get region string of current cell to last column "lstCol" cell.
    addedRegionStr = firstColIndx & currRow & ":" & lstColIndx & sheet1LastRowIndx
    
    Set getAddedRegion = Sheets(sheetName).Range(addedRegionStr)
End Function

Function copyRowToSheet(ByVal copiedRow, ByVal sheetName)
    firstColIndx = "a"
    
    ' ***********************
    
    corrSheetStartIndx = firstColIndx & (getLastRowIndx(sheetName) + 1)
    
    copiedRow.Copy Sheets(sheetName).Range(corrSheetStartIndx)
End Function

Sub appendInfoRByR()
    controlCenterWBName = "��������.xlsm"
    controlCenterMainSheetIndx = 1
    ccWHNameCell = "b2"
    ccWHPosCell = "b3"
    
    ' ***********************
    
    Dim controlCenter As Workbook
    Set controlCenter = Workbooks(controlCenterWBName)
    
    warehouseWBName = controlCenter.Sheets(controlCenterMainSheetIndx).Range(ccWHNameCell)
    startPos = controlCenter.Sheets(controlCenterMainSheetIndx).Range(ccWHPosCell)
    Workbooks(warehouseWBName).Activate
    
    Dim colorDict As Object
    Set colorDict = getColorDict()
    
    
    
    rowStartPos = 1
    colEndPos = "o"
    rowKeyPos = 2
    colorCodeOffset = 3
    greighTypeOffset = 3
    
    For Each iRow In getAddedRegion(rowStartPos, colEndPos, startPos).Rows
        Call copyRowToSheet(iRow, colorDict(Right(iRow.Cells(rowKeyPos), colorCodeOffset)))
        Call copyRowToSheet(iRow, colorDict(Left(iRow.Cells(rowKeyPos), greighTypeOffset)))
    Next
    
End Sub


Sub copyToWorkBook()
    controlCenterWBName = "��������.xlsm"
    controlCenterMainSheetIndx = 1
    ccWHNameCell = "b2"
    ccWHPosCell = "b3"
    
    ' ***********************
    
    warehouseWBName = Workbooks(controlCenterWBName).Sheets(controlCenterMainSheetIndx).Range(ccWHNameCell)
    
    Dim warehouse As Workbook
    Set warehouse = Workbooks(warehouseWBName)
    
    Dim customer As Workbook
    Set customer = Workbooks(warehouseWBName)
        
    
    
    whMainSTIndx = 1
    whRowStartIndx = 4775
    ctMainSTIndx = 1
    ctRowStartIndx = 4777
    ctColStartIndx = 1
    
    warehouse.Sheets(whMainSTIndx).Rows(whRowStartIndx).Copy customer.Sheets(ctMainSTIndx).Cells(ctRowStartIndx, ctColStartIndx)
End Sub

Sub test()

End Sub


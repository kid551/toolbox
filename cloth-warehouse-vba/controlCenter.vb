
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
    getColorDict.Add "515", "15����"
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



' Get the cell content of control center main worksheet.
'
Function getControlCenterCell(cellPos)
    Set getControlCenterCell = Workbooks("��������.xlsm").Sheets(1).Range(cellPos)
End Function


' **********************************************************************
' *
' * Split the added region of warehouse's main sheet, and copy each row
' * to its corresponding worksheet.
' *
' * The algorithm of this process is simple:
' *   1. get the added region
' *   2. copy each row to sub-sheet of color type
' *   3. copy each row to sub-sheet of greigh cloth type
' *
' **********************************************************************
'
Sub warehouseMainSheetToSubSheet()
    ' Get warehouse name & its worksheet, where the value is embedded in cell "b2"
    warehouseWBName = getControlCenterCell("b2")
    sheetTools.bakupFile (warehouseWBName)
    Dim warehouseWB As Workbook
    Set warehouseWB = Workbooks(warehouseWBName)
    Set colorDict = getColorDict()
    
    
    ' - the copied main sheet is "Sheets(1)"
    ' - the start row is embedded in cell "b3" of control center main sheet
    ' - the column boundary of copied region is "o"
    '
    For Each iRow In sheetTools.getRegion(warehouseWB.Sheets(1), getControlCenterCell("b3"), "o").Rows
        ' Get cloth specification, e.g. "C32X21 133X78 504", which embedded in column "b"
        specCell = iRow.columns("b")
        
        ' Get the color key of "colorDict", e.g. "504", which is the 3 right part of "specCell"
        colorKey = Right(specCell, 3)
        
        ' Get the greigh cloth key of "colorDict", e.g. "C32" or "C16", which is the 3 left part of "specCell"
        greighKey = Left(specCell, 3)
                
        Call sheetTools.copyRowToSheet(iRow, warehouseWB.Sheets(colorDict(colorKey)))
        Call sheetTools.copyRowToSheet(iRow, warehouseWB.Sheets(colorDict(greighKey)))
    Next
    
End Sub




' ********************************************************************
' *
' * Build "customer" sheet by copying from main sheet of "warehouse".
' *
' ********************************************************************

' Construct the customer row from the warehouse row.
'
' The algorithm of this process is:
'   1. construct the new row's "a:h" columns
'   2. add hyperlink at column "d"
'   3. construct the new row's "i, j, k, o" columns
'
' - copiedRow, the copied row from warehouse
' - targetSheet Worksheet, the sheet where to copy content
' - unitPrice, the unit price, which will be used in computation
'
Sub buildCustomerRow(copiedRow, targetSheet As Worksheet, unitPrice)
    ' Construct a new row of columns "a:h" by
    '   merge columns "a:e", "h", "i", "l" of copied row.
    Dim newBuildRange As Range
    Set newBuildRange = Union(copiedRow.columns("a:e"), _
                            copiedRow.columns("h"), _
                            copiedRow.columns("i"), _
                            copiedRow.columns("l"))
    
    ' Before copying to "a:h" columns, store the start row index first
    startRow = sheetTools.getLastNonEmptyRow(targetSheet) + 1
    
    ' Copy new build range after last non-empty row
    Call sheetTools.copyRowToSheet(newBuildRange, targetSheet)
    
    ' Add corresponding hyperlink of customer name, which is embedded in column "d".
    customerCell = "d" & startRow
    subCTField = targetSheet.Range(customerCell) & "!A3"
    With targetSheet
        .Hyperlinks.Add .Range(customerCell), Address:="", SubAddress:=subCTField
    End With

    ' construct the "i, j, k, o" columns
    targetSheet.Range("i" & startRow) = printf("=H{0}*G{1}", startRow, startRow)
    targetSheet.Range("j" & startRow) = unitPrice
    targetSheet.Range("k" & startRow) = printf("=J{0}*I{1}", startRow, startRow)
    targetSheet.Range("o" & startRow) = printf("=O{0}+K{1}-M{2}-N{3}", startRow - 1, startRow, startRow, startRow)
    
End Sub


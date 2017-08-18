
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
    getColorDict.Add "515", "15亮白"
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
    
    getColorDict.Add "C32", "32总库存"
    getColorDict.Add "C16", "16总库存"
End Function



' Get the cell content of control center main worksheet.
'
Function getControlCenterCell(cellPos)
    Set getControlCenterCell = Workbooks("控制中心.xlsm").Sheets(1).Range(cellPos)
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

' Copy the customer row from the warehouse row with some contruction.
'
' The algorithm of this process is:
'   1. construct the new row's "a:h" columns, then copy to target sheet.
'   2. add hyperlink at column "d" of target sheet.
'   3. construct the new row's "i, j, k, o" columns, then copy to target sheet.
'
' - copiedRow, the copied row from warehouse
' - targetSheet Worksheet, the sheet where to copy content
' - unitPrice, the unit price, which will be used in computation
'
Sub copyCustomerRow(copiedRow, targetSheet As Worksheet, unitPrice)
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



' Build the customer sheet by copying from warehouse sheet.
'
' Attension:
'     the copy only happen when the "c" column of warehouse main
'     sheet is "售, 退"
'
' The algorithm is:
'   - get the added region in warehouse
'   - copy each row of above region with some construction and condition
'
Sub buildCustomerWorkbook()
    ' ========================================================
    ' = Do the preparation work, build the worksheet accroding
    ' = to control center main worksheet.
    ' ========================================================
    
    ' Get the warehouse main sheet, which is embedded in
    ' cell "b2" of control center main sheet
    Dim warehouseMainSheet As Worksheet
    Set warehouseMainSheet = Workbooks(getControlCenterCell("b2").Value).Sheets(1)
    
    ' Get the start row of added region in warehouse worksheet, which is embedded in
    ' cell "b3" of control center main sheet
    warehouseStartRow = getControlCenterCell("b3")
    
    ' Get the customer main sheet, which is embedded in
    ' cell "b5" of control center main sheet
    Dim customerMainSheet As Worksheet
    Set customerMainSheet = Workbooks(getControlCenterCell("b5").Value).Sheets(1)
    
    bakupFile (getControlCenterCell("b5").Value)
    
    ' Get the unit price in customer sheet, which is embedded in
    ' cell "b6" of control center main sheet
    unitPrice = getControlCenterCell("b6")
    
    ' Record the start row of added region in customer worksheet. Its value will be stored in
    ' cell "b7" of control center main sheet
    Dim customerStartRowCell As Range
    Set customerStartRowCell = getControlCenterCell("b7")
    ' Assign the value in cell instead of cell "Range", which is different from above line
    customerStartRowCell = sheetTools.getLastNonEmptyRow(customerMainSheet) + 1
    
    
    ' ========================================================
    ' = The algorithm meat.
    ' = Real work start here.
    ' ========================================================
    
    ' The column boundary of warehouse main sheet is "o"
    For Each iRow In sheetTools.getRegion(warehouseMainSheet, warehouseStartRow, "o").Rows
        ' Only when the "c" column of warehouse main sheet is "售, 退", the copy can happen
        If iRow.columns("c") = "售" Or iRow.columns("c") = "退" Then
            Call copyCustomerRow(iRow, customerMainSheet, unitPrice)
        End If
    Next
    
    MsgBox "请修改 **非统一布匹** 的单价！"
End Sub



' *****************************************************************
' *
' * Split the added region of customer's main sheet, and copy each row
' * to its corresponding worksheet.
' *
' * The algorithm is:
' *     1. get the added region in customer main sheet
' *     2. copy each row to corresponding sub-sheet
' *
' *****************************************************************
'
Sub customerMainSheetToSubSheet()
    ' Get the customer main sheet, which is embedded in
    ' cell "b5" of control center main sheet
    Dim customerWorkbook As Workbook
    Set customerWorkbook = Workbooks(getControlCenterCell("b5").Value)
    
    bakupFile (getControlCenterCell("b5").Value)
    
    
    ' - the customer main sheet is "Sheets(1)"
    ' - the added region in customer main sheet is embedded in "b7"
    ' - the column boundary of customer main sheet is "o"
    For Each iRow In sheetTools.getRegion(customerWorkbook.Sheets(1), getControlCenterCell("b7"), "o").Rows
        ' Get the customer name in column "d", which is also the sub-sheet name, e.g. "张三"
        customerSubSheetName = iRow.columns("d")
            
        Call sheetTools.copyRowToSheet(iRow, customerWorkbook.Sheets(customerSubSheetName))
    Next
End Sub


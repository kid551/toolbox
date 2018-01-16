
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
    getColorDict.Add "15", "西裤15"
    getColorDict.Add "21", "西裤21"
    getColorDict.Add "22", "西裤22"
    getColorDict.Add "25", "西裤25"
    getColorDict.Add "31", "西裤31"
    getColorDict.Add "32", "西裤32"
    getColorDict.Add "28-1", "西裤28-1"
    getColorDict.Add "28-2", "西裤28-2"
    getColorDict.Add "38-1", "西裤38-1"
    getColorDict.Add "38-2", "西裤38-2"
    
    getColorDict.Add "C32X21", "32总库存"
    getColorDict.Add "C16X7", "16总库存"
    getColorDict.Add "T95C5", "T95C5总库存"
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
    sheetTools.backupFile (warehouseWBName)
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
        
        
        Dim Arrc
        Arrc = Split(specCell, " ")
        
        ' Get the color key of "colorDict", e.g. "504", which last right part of "specCell"
        colorKey = Arrc(UBound(Arrc))
        
        ' Get the greigh cloth key of "colorDict", e.g. "C32X21" or "C16X7", which is first left part of "specCell"
        greighKey = Arrc(0)
        
        Call sheetTools.appendRowToSheet(iRow, warehouseWB.Sheets(colorDict(colorKey)))
        Call sheetTools.appendRowToSheet(iRow, warehouseWB.Sheets(colorDict(greighKey)))
    Next
    
End Sub



' ********************************************************************
' *
' * Build "customer" sheet by copying from main sheet of "warehouse".
' *
' ********************************************************************

' Generate the customer row from the warehouse row with some contruction.
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
Sub genCustomerRow(copiedRow, targetSheet As Worksheet, unitPrice)
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
    Call sheetTools.appendRowToSheet(newBuildRange, targetSheet)
    
    ' Add corresponding hyperlink of customer name, which is embedded in column "d".
    customerCell = "d" & startRow
    subCTField = targetSheet.Range(customerCell) & "!A3"
    With targetSheet
        .Hyperlinks.Add .Range(customerCell), Address:="", SubAddress:=subCTField
    End With

    ' construct the "i, j, k, o" columns
    targetSheet.Range("i" & startRow) = stringFormat("=H{0}*G{1}", startRow, startRow)
    targetSheet.Range("j" & startRow) = unitPrice
    targetSheet.Range("k" & startRow) = stringFormat("=J{0}*I{1}", startRow, startRow)
    targetSheet.Range("o" & startRow) = stringFormat("=O{0}+K{1}-M{2}-N{3}", startRow - 1, startRow, startRow, startRow)
    
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
Sub buildCustomerMainSheet()
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
    
    backupFile (getControlCenterCell("b5").Value)
    
    ' Get the unit price in customer sheet, which is embedded in
    ' cell "b6" of control center main sheet
    unitPrice = getControlCenterCell("b6")
    
    ' Record values in UI:
    '     record the start row of added region in customer worksheet. Its
    '     value will be stored in cell "b7" of control center main sheet
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
            Call genCustomerRow(iRow, customerMainSheet, unitPrice)
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
    
    backupFile (getControlCenterCell("b5").Value)
    
    
    ' - the customer main sheet is "Sheets(1)"
    ' - the added region in customer main sheet is embedded in "b7"
    ' - the column boundary of customer main sheet is "o"
    For Each iRow In sheetTools.getRegion(customerWorkbook.Sheets(1), getControlCenterCell("b7"), "o").Rows
        ' Get the customer name in column "d", which is also the sub-sheet name, e.g. "张三"
        customerSubSheetName = iRow.columns("d")
            
        Call sheetTools.appendRowToSheet(iRow, customerWorkbook.Sheets(customerSubSheetName))
    Next
End Sub



' *****************************************************************
' *
' * Build "customer summary" main sheet by copying from "customer".
' *
' *****************************************************************

' Determine the "start row" of added region according to if they have
' same "date, greigh cloth, sell type, customer name"
'
' - mergeKey, the dictionary key to determine the merge condition
' - mergeDict, the merge dictionary, which is used for merge determination
' - targetSheet, the target sheet where copy the contents
'
Function getMergeStartRow(mergeKey, ByRef mergeDict As Object, targetSheet As Worksheet)

    If mergeDict(mergeKey) <> 0 Then
        ' the item has appeared, thus we need to "begin" at the last non-empty row
        getMergeStartRow = sheetTools.getLastNonEmptyRow(targetSheet)
    Else
        ' the item hasn't appeared, thus we need to start "follow" the last non-empty row
        getMergeStartRow = sheetTools.getLastNonEmptyRow(targetSheet) + 1
    End If
    
End Function



' Generate the customer summary row from copied customer sheet row.
'
' - copiedRow, the  copied contents row
' - targetSheet Worksheet, the sheet to which copy contents
' - unitPrice, the unit price which involves computation
' - mergeDict Object, the dictionary to distinguish merge conditions
'
' Attension:
'     When the four domains "date, greigh cloth, sell type, customer name"
'     are match, the row should be merged into the same row.
'
' The algorithm is:
'     1. get the start row of added region in target sheet
'        based on if this item has appeared before.
'     2. generate the unchanged column "a:d", "g", "i"
'     3. add corresponding hyperlink to column "d"
'     4. generate the changed column "e, f, h, j"
'
Sub genCustomerSummaryRow(copiedRow, targetSheet As Worksheet, ByRef mergeDict As Object)
    ' Construct a new row of columns "a:d" from columns "a:d" of copied row.
    Dim newBuildRange As Range
    Set newBuildRange = copiedRow.columns("a:d")
    
    
    ' When two rows' all domains of
    '   - "date", which is embedded in column "a"
    '   - "greigh cloth", which is embedded in column "b"
    '   - "sell type", which is embedded in column "c"
    '   - "customer name", which is embedded in column "d"
    ' are same, the two rows can be summarized in one row. Thus, we'll
    ' use the concatenation string of four domains as "key" to determine if merge
    
    mergeKey = stringFormat("{0} {1} {2} {3}", copiedRow.columns("a"), _
                                                        copiedRow.columns("b"), _
                                                        copiedRow.columns("c"), _
                                                        copiedRow.columns("d"))
    startRow = getMergeStartRow(mergeKey, mergeDict, targetSheet)
        
    ' ================================================
    ' = Generate the unchanged column "a:d", "g", "i"
    ' = in target sheet
    ' ================================================
    '
    ' Copy the columns "a:d" of "copied row" to the start row of target sheet.
    copiedRow.columns("a:d").Copy targetSheet.Range(stringFormat("{0}{1}", "a", startRow))
        
    
    ' Generate column "g" of target sheet by
    ' copying the unit price at column "j" of copied row
    targetSheet.Range(stringFormat("{0}{1}", "g", startRow)) = copiedRow.columns("j")
       
    ' Generate column "i" of target sheet by
    ' copying the payment of column "m" in copied row
    '
    targetSheet.Range(stringFormat("{0}{1}", "i", startRow)) = copiedRow.columns("m")
                                
    ' Add corresponding hyperlink of customer name, which is embedded in column "d".
    customerCell = "d" & startRow
    subCTField = targetSheet.Range(customerCell) & "!A3"
    With targetSheet
        .Hyperlinks.Add .Range(customerCell), Address:="", SubAddress:=subCTField
    End With
                   
    ' ================================================
    ' = Generate the changed column "e, f, h, j"
    ' = in target sheet
    ' ================================================
    
    ' If the item has appeared:
    '     sum the "count of cloth", which is at column "e", from column "h" of copied row,
    '     and "length of cloth", which is at column "f", from column "g" of copied row.
    '
    If mergeDict(mergeKey) <> 0 Then
        ' sum cloth count
        targetSheet.Range(stringFormat("{0}{1}", "e", startRow)) = _
                    targetSheet.Range(stringFormat("{0}{1}", "e", startRow)) + copiedRow.columns("h")
                    
        ' sum cloth length
        targetSheet.Range(stringFormat("{0}{1}", "f", startRow)) = _
                    targetSheet.Range(stringFormat("{0}{1}", "f", startRow)) + copiedRow.columns("g")

        ' sum the counts of same "merge key"
        mergeDict(mergeKey) = mergeDict(mergeKey) + 1
    Else
        targetSheet.Range(stringFormat("{0}{1}", "e", startRow)) = copiedRow.columns("h")
        targetSheet.Range(stringFormat("{0}{1}", "f", startRow)) = copiedRow.columns("g")
        mergeDict(mergeKey) = 1
    End If
    
    
    ' Generate column "h"
    targetSheet.Range(stringFormat("{0}{1}", "h", startRow)) = stringFormat("=G{0}*F{1}", startRow, startRow)
    
    ' Generate column "j"
    targetSheet.Range(stringFormat("{0}{1}", "j", startRow)) = stringFormat("=J{0}+H{1}-I{2}", startRow - 1, startRow, startRow)
        
End Sub



' Build the customer summary workbook from customer main sheet.
'
Sub buildCustomerSummaryMainSheet()
    
    ' ========================================================
    ' = Do the preparation work, build the worksheet accroding
    ' = to control center main worksheet.
    ' ========================================================
    
    ' Get the customer main sheet, which is embedded in
    ' cell "b5" of control center main sheet
    Dim customerMainSheet As Worksheet
    Set customerMainSheet = Workbooks(getControlCenterCell("b5").Value).Sheets(1)
    
    ' Get the start row of added region in customer worksheet, which is embedded in
    ' cell "b7" of control center main sheet.
    customerStartRow = getControlCenterCell("b7")
    
    ' Get customer summary main worksheet, which is embedded in cell "b9"
    customerSummaryName = getControlCenterCell("b9")
    Dim customerSummaryMainSheet As Worksheet
    Set customerSummaryMainSheet = Workbooks(customerSummaryName).Sheets(1)
    
    sheetTools.backupFile (customerSummaryName)
    
    ' Record values in UI:
    '     record the start row of added region in customer summary worksheet. Its
    '     value will be stored in cell "b11" of control center main sheet
    Dim customerSummaryStartRowCell As Range
    Set customerSummaryStartRowCell = getControlCenterCell("b11")
    ' Assign the value in cell instead of cell "Range", which is different from above line
    customerSummaryStartRowCell = sheetTools.getLastNonEmptyRow(customerSummaryMainSheet) + 1
           
    ' The merge dictionary, which is used to distinguish merge condition
    Dim mergeDict As Object
    Set mergeDict = CreateObject("Scripting.Dictionary")
    
    
    ' ========================================================
    ' = The algorithm meat.
    ' = Real work start here.
    ' ========================================================
    
    ' The column boundary of customer sheet is "o"
    For Each iRow In getRegion(customerMainSheet, customerStartRow, "o").Rows
        Call genCustomerSummaryRow(iRow, customerSummaryMainSheet, mergeDict)
    Next
    
    ' ========================================================
    ' = Check if the summary number equals customer main sheet.
    ' ========================================================
    res = sheetTools.compareCellOfLastRow(customerMainSheet, "o", customerSummaryMainSheet, "j")
    If res = False Then
        a = MsgBox("【欠款总金额】与【客户明细表自己看】不符合！", vbCritical)
        End
    End If
End Sub



' *****************************************************************
' *
' * Split "customer summary" main sheet, and build its sub-sheet.
' *
' *****************************************************************
'
'
' The algorithm is:
'   - build the new range
'   - append the new build range to target sheet
'   - recompute column "g"
'   - recompute column "i"
'
Function genCustomerSummarySubSheetRow(ByVal copiedRow, targetSheet As Worksheet)
    ' Construct a new row of columns "a:i" by
    '   merge columns "a:c", "e:j" of copied row.
    Dim newBuildRange As Range
    Set newBuildRange = Union(copiedRow.columns("a:c"), copiedRow.columns("e:j"))
        
    startRow = sheetTools.getLastNonEmptyRow(targetSheet) + 1
    
    Call sheetTools.appendRowToSheet(newBuildRange, targetSheet)
        
    ' Recompute "total gross" at column "g"
    targetSheet.Range(stringFormat("{0}{1}", "g", startRow)) = _
            stringFormat("=F{0}*E{1}", startRow, startRow)
    
    ' Recompute "debt" at column "i"
    targetSheet.Range(stringFormat("{0}{1}", "i", startRow)) = _
            stringFormat("=I{0}+G{1}-H{2}", startRow - 1, startRow, startRow)
    
End Function



' Generate the bottom contents of customer summary sub-sheet
'
' The algorithm is:
'
'     1. find the start row of customer summary sub-sheet
'     2. clear the contents below start row in row gap lines
'     3. Assign "合计" literal name
'     4. Compute the sum at column "d, e, g"
'     5. Add the border lines for added region
'
Sub genCustomerSummarySubSheetBottom(customerSummaryWorkbook As Workbook, customerDict As Object)
    
    For Each customerKey In customerDict.Keys()
        
        ' ==================
        ' = Prepration work
        ' ==================
        
        ' Find the sub-sheet based on the customer key in customer dictionary
        Dim customerSubSheet As Worksheet
        Set customerSubSheet = customerSummaryWorkbook.Sheets(customerKey)
        
        rowGapLines = 3
        
        
        ' ===================================================
        ' = find the start row of customer summary sub-sheet
        ' ===================================================
        
        ' Here, the region has been added. So, the start row here
        ' is below the added region
        '
        subSheetStartRow = sheetTools.getLastNonEmptyRow(customerSubSheet) + 1
        
        ' Clear all contents of row gaps, which is below start row in "rowGapLiens" rows.
        '   - the boundary column of subsheet is "i"
              
        
        ' =====================================================
        ' = clear the contents below start row in row gap lines
        ' =====================================================
        '
        customerSubSheet.Range(stringFormat("{0}{1}:{2}{3}", _
                                        "a", subSheetStartRow, _
                                        "i", subSheetStartRow + rowGapLines)).Clear
                                        
                                        
        ' ===============================================================
        ' = Assign "合计" at the cell "b{subSheetStartRow + rowGapLines}"
        ' ===============================================================
        '
        customerSubSheet.Range(stringFormat("{0}{1}", _
                        "b", subSheetStartRow + rowGapLines)) = "合计"
        ' Make "合计" align at center
        customerSubSheet.Range(stringFormat("{0}{1}", _
                        "b", subSheetStartRow + rowGapLines)).HorizontalAlignment = xlCenter
        
        
        ' =====================================
        ' = Compute the sum at column "d, e, g"
        ' =====================================
        '
        ' Compute the 3 total items from the "4th"row :
        '   1. total counts of cloth, which is embedded in column "d"
        '   2. total length of cloth, which is embedded in column "e"
        '   3. total gross per customer, which is embedded in column "g"
        '
        sumStartRow = 4
        ' 1. total counts:
        countColumn = "d"
        customerSubSheet.Range(stringFormat("{0}{1}", countColumn, subSheetStartRow + rowGapLines)) _
                    = stringFormat("=Sum({0}{1}:{2}{3})", countColumn, sumStartRow, countColumn, subSheetStartRow - 1)
        ' 2. total length:
        lengthColumn = "e"
        customerSubSheet.Range(stringFormat("{0}{1}", lengthColumn, subSheetStartRow + rowGapLines)) _
                    = stringFormat("=Sum({0}{1}:{2}{3})", lengthColumn, sumStartRow, lengthColumn, subSheetStartRow - 1)
        ' 3. total gross:
        grossColumn = "g"
        customerSubSheet.Range(stringFormat("{0}{1}", grossColumn, subSheetStartRow + rowGapLines)) _
                    = stringFormat("=Sum({0}{1}:{2}{3})", grossColumn, sumStartRow, grossColumn, subSheetStartRow - 1)
        
        
        ' ========================================
        ' = Add the border lines for added region
        ' ========================================
        '
        ' Add the border lines for added region
        '   - the boundary column of subsheet is "i"
        numberOfAddedRows = customerDict(customerKey)
        customerSubSheet.Range(stringFormat("{0}{1}:{2}{3}", _
                "a", subSheetStartRow - numberOfAddedRows, "i", subSheetStartRow + rowGapLines)).Borders.LineStyle = 1
                
    Next
    
End Sub

Sub customerSummaryMainSheetToSubSheet()
    ' Get the customer main sheet, which is embedded in
    ' cell "b5" of control center main sheet
    Dim customerWorkbook As Workbook
    Set customerWorkbook = Workbooks(getControlCenterCell("b5").Value)
    
    ' Get customer summary main worksheet, which is embedded in cell "b9"
    customerSummaryName = getControlCenterCell("b9")
    Dim customerSummaryWorkbook As Workbook
    Set customerSummaryWorkbook = Workbooks(customerSummaryName)
    Dim customerSummaryMainSheet As Worksheet
    Set customerSummaryMainSheet = customerSummaryWorkbook.Sheets(1)
    
    ' Get the start row of added region in customer summary worksheet, which is embedded in
    ' cell "b11" of control center main sheet.
    customerSummaryStartRow = getControlCenterCell("b11")
    
    sheetTools.backupFile (customerSummaryName)
    
    Dim customerDict As Object
    Set customerDict = CreateObject("Scripting.Dictionary")
    
    ' Build the customer summary sub-sheet,
    '   - customer dictionary key is embedded in column "d"
    '   - the boundary column of added region in customer summary
    '     main sheet is "j"
    '
    For Each iRow In getRegion(customerSummaryMainSheet, customerSummaryStartRow, "j").Rows
        ' Get the customer key, which is embedded in column "d" of added region row
        customerKey = iRow.columns("d")
        
        Call genCustomerSummarySubSheetRow(iRow, customerSummaryWorkbook.Sheets(customerKey))
        
        ' Record the number of items for each customer sub-sheet
        If customerDict(customerKey) <> 0 Then
            customerDict(customerKey) = customerDict(customerKey) + 1
        Else
            customerDict(customerKey) = 1
        End If
    Next
    
    ' Generate the "bottom section" of each customer summary sub-sheet
    Call genCustomerSummarySubSheetBottom(customerSummaryWorkbook, customerDict)
    
    
    
    ' ==================================================================
    ' = Check if the debts in sub-customer-sheets of "customer workbook"
    ' = and "customer summary workbook" are the same.
    ' ==================================================================
    For Each nameKey In customerDict.Keys
        res = sheetTools.compareCellOfLastRow(customerWorkbook.Sheets(nameKey), "o", _
                                              customerSummaryWorkbook.Sheets(nameKey), "i")
        
        If res = False Then
            a = MsgBox("两个【" & nameKey & "】分表不符合！", vbCritical)
        End If
    Next
       
End Sub


' Get the summarized details of warehouse
'
' 1. Iterate all sheets in warehouse workbook
' 2. Get the name of eacah sheet
' 3. Get the last remaining length of each color
'
Sub getRemainLengthDetails()
    Dim warehouseWorkbook As Workbook
    Dim shimoWorkbook As Workbook
    Set warehouseWorkbook = Workbooks("常熟出入库存表.xls")
    Set shimoWorkbook = Workbooks("石墨笔记.xlsx")
        
    Dim mainShimoWorksheet As Worksheet
    Set mainShimoWorksheet = shimoWorkbook.Sheets(1)
    
    Call sheetTools.getWorkbookSummary(warehouseWorkbook, "o", mainShimoWorksheet)
    Call sheetTools.getWorkbookSummary(warehouseWorkbook, "n", mainShimoWorksheet)
    
End Sub

Sub getRemainCustomerDebts()
    Dim customerWorkbook As Workbook
    Dim shimoWorkbook As Workbook
    Set customerWorkbook = Workbooks("常熟客户明细表自己看.xls")
    Set shimoWorkbook = Workbooks("石墨笔记.xlsx")
    
    Dim customerShimoWorksheet As Worksheet
    Set customerShimoWorksheet = shimoWorkbook.Sheets(2)
    
    Call sheetTools.getWorkbookSummary(customerWorkbook, "o", customerShimoWorksheet)
    
End Sub




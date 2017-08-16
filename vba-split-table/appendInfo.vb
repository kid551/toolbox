
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




' *****************************************************************
' *
' * General tool subs/functions library
' *
' *****************************************************************

Function getLastRowIndx(ByVal wbName, ByVal sheetName)
    ' get the last non-empy row index of column "a"
    getLastRowIndx = Workbooks(wbName).Sheets(sheetName).Range("a65536").End(3).Row
End Function

Function getAddedRegion(ByVal wbName, ByVal sheetName, ByVal lstColIndx, ByVal startRow) As Range
    firstColIndx = "a"
    
    ' ***********************
    
    currRow = startRow
    sheet1LastRowIndx = getLastRowIndx(wbName, sheetName)
    
    ' Get region string of current cell to last column "lstCol" cell.
    addedRegionStr = firstColIndx & currRow & ":" & lstColIndx & sheet1LastRowIndx
    
    Set getAddedRegion = Workbooks(wbName).Sheets(sheetName).Range(addedRegionStr)
End Function

Function copyRowToSheet(ByVal copiedRow, ByVal targetWBName, ByVal sheetName)
    firstColIndx = "a"
    
    ' ***********************
    
    corrSheetStartIndx = firstColIndx & (getLastRowIndx(targetWBName, sheetName) + 1)
    
    copiedRow.Copy Workbooks(targetWBName).Sheets(sheetName).Range(corrSheetStartIndx)
End Function

Function getCellContents(ByVal wbName, ByVal stName, ByVal cPos) As Range
    Set getCellContents = Workbooks(wbName).Sheets(stName).Range(cPos)
End Function

Sub bakupFile(ByVal fileName)
    ' Here we assume the bakuped file is ended with '.xls'
    preFileName = Replace(fileName, ".xls", "")
    
    newDate = Replace(Date, "-", "")
    newDate = Replace(newDate, "/", "")
    newTime = Left(Replace(Time(), ":", ""), 4)
    timeStamp = newDate & newTime
    
    prePath = Application.ActiveWorkbook.Path
    src = prePath & "\" & fileName
    dest = prePath & "\bak\" & preFileName & "-" & timeStamp & ".xls"
    
    Workbooks(fileName).SaveCopyAs dest
End Sub

Public Function printf(mask As String, ParamArray tokens()) As String
    Dim i As Long
    
    For i = 0 To UBound(tokens)
        mask = Replace(mask, "{" & i & "}", tokens(i))
    Next
    
    printf = mask
End Function

Function genSearchedLines(searchedRow As Integer, columns()) As String
    Dim searchedVal As String
        
    Dim i As Long
    
    For i = 0 To UBound(columns)
    
        If i = 0 Then
            searchedVal = printf("{0}{1}", columns(i), searchedRow)
        Else
            searchedVal = searchedVal & printf("&{0}{1}", columns(i), searchedRow)
        End If
    Next
    
    genSearchedLines = searchedVal
    
End Function

Function genSearchedArr(searchedRow As Integer, columns()) As String
    Dim searchedArr As String
    
    Dim i As Long
    
    For i = 0 To UBound(columns)
    
        If i = 0 Then
            searchedArr = printf("{0}1:{0}{1}", columns(i), (searchedRow - 1))
        Else
            searchedArr = searchedArr & printf("&{0}1:{0}{1}", columns(i), (searchedRow - 1))
        End If
    Next
    
    genSearchedArr = searchedArr
    
End Function

Function getMatchedIndx(searchedSheet As Worksheet, searchColArr(), searchRow As Integer)

    eva_exp = printf("Match({0}, {1}, 0)", _
                        genSearchedLines(searchRow, searchColArr), _
                        genSearchedArr(searchRow, searchColArr))
                            
    getMatchedIndx = searchedSheet.Evaluate(eva_exp)
    
End Function




' *****************************************************************
' *
' * Split "warehouse" main sheet, and build sub-sheet.
' *
' *****************************************************************

Sub appendInfoRByR()
    controlCenterWBName = "控制中心.xlsm"
    controlCenterMainSheetName = 1
    ccWHNameCell = "b2"
    ccWHPosCell = "b3"
    
    ' ***********************
    
    Dim controlCenter As Workbook
    Set controlCenter = Workbooks(controlCenterWBName)
    
    warehouseWBName = controlCenter.Sheets(controlCenterMainSheetName).Range(ccWHNameCell)
    bakupFile (warehouseWBName)
    
    startRowPos = controlCenter.Sheets(controlCenterMainSheetName).Range(ccWHPosCell)
        
    Dim colorDict As Object
    Set colorDict = getColorDict()
    
    
    
    colStartPos = 1
    colEndPos = "o"
    rowKeyPos = 2
    colorCodeOffset = 3
    greighTypeOffset = 3
    
    For Each iRow In getAddedRegion(warehouseWBName, colStartPos, colEndPos, startRowPos).Rows
        Call copyRowToSheet(iRow, warehouseWBName, colorDict(Right(iRow.Cells(rowKeyPos), colorCodeOffset)))
        Call copyRowToSheet(iRow, warehouseWBName, colorDict(Left(iRow.Cells(rowKeyPos), greighTypeOffset)))
    Next
    
End Sub




' *****************************************************************
' *
' * Build "customer" sheet by copying from "warehouse".
' *
' *****************************************************************

Sub buildSellRow(ByVal copiedRow, ByVal wbName, ByVal sheetName, ByVal unitPrice)
    firstDomain = "a:e"
    secondDomain = "h"
    thirdDomain = "i"
    fourthDomain = "l"

    Dim copiedRange As Range
    Set copiedRange = Union(copiedRow.Columns(firstDomain), copiedRow.Columns(secondDomain), copiedRow.Columns(thirdDomain), copiedRow.Columns(fourthDomain))
    
    ctRowStartIndx = getLastRowIndx(wbName, sheetName) + 1
    Call copyRowToSheet(copiedRange, wbName, sheetName)
    
    customerCell = "d" & ctRowStartIndx
    subCTField = getCellContents(wbName, sheetName, customerCell) & "!A3"
    With Workbooks(wbName).Sheets(sheetName)
        .Hyperlinks.Add .Range(customerCell), Address:="", SubAddress:=subCTField
    End With
                
    Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, 9) = "=H" & ctRowStartIndx & "*G" & ctRowStartIndx
    Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, 10) = unitPrice
    Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, 11) = "=J" & ctRowStartIndx & "*I" & ctRowStartIndx
    Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, 15) = "=o" & CStr(ctRowStartIndx - 1) & "+K" & ctRowStartIndx & "-M" & ctRowStartIndx & "-N" & ctRowStartIndx
    
End Sub

Sub copyToCustomerWorkBook()
    controlCenterWBName = "控制中心.xlsm"
    controlCenterMainSheetName = 1
    
    ccWHNameCell = "b2"
    ccWHPosCell = "b3"
    
    ccCTNameCell = "b5"
    ccUnitPriceCell = "b6"
    ccCTPosCell = "b7"
    
    whMainSTName = 1
    whLstColIndx = "o"
    ctMainSTName = 1
    ctColStartIndx = 1
    
    ' ***********************
    
    warehouseWBName = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccWHNameCell)
    warehouseRowStartPos = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccWHPosCell)
    
    customerWBName = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTNameCell)
    unitPrice = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccUnitPriceCell)
    bakupFile (customerWBName)
    
    ' Record the start position of new added region in "Cell" of "Control Center".
    Dim customerStartPos As Range
    Set customerStartPos = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTPosCell)
    customerStartPos = getLastRowIndx(customerWBName, ctMainSTName) + 1
    
    For Each iRow In getAddedRegion(warehouseWBName, whMainSTName, whLstColIndx, warehouseRowStartPos).Rows
        If iRow.Columns("c") = "售" Or iRow.Columns("c") = "退" Then
            Call buildSellRow(iRow, customerWBName, ctMainSTName, unitPrice)
        End If
    Next
    
    MsgBox "请修改 **非统一布匹** 的单价！"
End Sub




' *****************************************************************
' *
' * Split "customer" main sheet, and build sub-sheet.
' *
' *****************************************************************

Sub splitCustomerInfoRByR()
    controlCenterWBName = "控制中心.xlsm"
    controlCenterMainSheetName = 1
    ccCTNameCell = "b5"
    ccCTPosCell = "b7"
    
    ' ***********************
    
    Dim controlCenter As Workbook
    Set controlCenter = Workbooks(controlCenterWBName)
    
    customerWBName = controlCenter.Sheets(controlCenterMainSheetName).Range(ccCTNameCell)
    startRowPos = controlCenter.Sheets(controlCenterMainSheetName).Range(ccCTPosCell)
    bakupFile (customerWBName)
    
    
    colStartPos = 1
    colEndPos = "o"
    rowKeyPos = "d"
    
    For Each iRow In getAddedRegion(customerWBName, colStartPos, colEndPos, startRowPos).Rows
        Call copyRowToSheet(iRow, customerWBName, iRow.Columns(rowKeyPos).Value)
    Next
    
End Sub




' *****************************************************************
' *
' * Build "customer summary" main sheet by copying from "customer".
' *
' *****************************************************************

Sub buildSummarySellRow(ByVal copiedRow, ByVal wbName, ByVal sheetName, ByVal unitPrice, ByRef ctDict As Object)
    firstDomain = "a:d"
    secondDomain = "h"
    thirdDomain = "g"

    Dim copiedRange As Range
    Set copiedRange = copiedRow.Columns(firstDomain)
    
    ctGreighDomain = "b"
    ctType = "c"
    ctCustomerDomain = "d"
    ctKey = copiedRow.Columns(ctGreighDomain) & " " & copiedRow.Columns(ctType) & " " & copiedRow.Columns(ctCustomerDomain)
    If ctDict(ctKey) <> 0 Then
        ctRowStartIndx = getLastRowIndx(wbName, sheetName)
    Else
        ctRowStartIndx = getLastRowIndx(wbName, sheetName) + 1
    End If
    
    copiedRow.Columns("a:d").Copy Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, 1)
    Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, 9) = copiedRow.Columns("m")
    
    customerCell = "d" & ctRowStartIndx
    subCTField = getCellContents(wbName, sheetName, customerCell) & "!A3"
    With Workbooks(wbName).Sheets(sheetName)
        .Hyperlinks.Add .Range(customerCell), Address:="", SubAddress:=subCTField
    End With
    
    
    clothCountIndx = 5
    clothLengthIndx = 6
    If ctDict(ctKey) <> 0 Then
        Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, clothCountIndx) = Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, 5) + copiedRow.Columns(secondDomain)
        Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, clothLengthIndx) = Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, 6) + copiedRow.Columns(thirdDomain)
        ctDict(ctKey) = ctDict(ctKey) + 1
    Else
        Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, clothCountIndx) = copiedRow.Columns(secondDomain)
        Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, clothLengthIndx) = copiedRow.Columns(thirdDomain)
        ctDict(ctKey) = 1
    End If
    
    
    unitPriceIndx = 7
    
    totalGrossIndx = 8
    debtIndx = 10
    copiedRow.Columns("j").Copy Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, unitPriceIndx)
    Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, totalGrossIndx) = "=G" & ctRowStartIndx & "*F" & ctRowStartIndx
    Workbooks(wbName).Sheets(sheetName).Cells(ctRowStartIndx, debtIndx) = "=J" & (ctRowStartIndx - 1) & "+H" & ctRowStartIndx & "-I" & ctRowStartIndx
        
End Sub

Sub copyToSummaryCTWB()
    controlCenterWBName = "控制中心.xlsm"
    controlCenterMainSheetName = 1
    
    ccCTNameCell = "b5"
    ccCTPosCell = "b7"
    
    ccCTSNameCell = "b9"
    ccUnitPriceCell = "b6"
    ccCTSPosCell = "b11"
    
    ctMainSTName = 1
    ctLstColIndx = "o"
    ctSMainSTName = 1
    ctSColStartIndx = 1
    
    ' ***********************
    
    customerWBName = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTNameCell)
    customerRowStartPos = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTPosCell)
    
    customerSWBName = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTSNameCell)
    unitPrice = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccUnitPriceCell)
    bakupFile (customerSWBName)
    
    ' Record the start position of new added region in "Cell" of "Control Center".
    Dim customerSStartPos As Range
    Set customerSStartPos = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTSPosCell)
    customerSStartPos = getLastRowIndx(customerSWBName, ctSMainSTName) + 1
    
        
    Dim customerDict As Object
    Set customerDict = CreateObject("Scripting.Dictionary")
    
    For Each iRow In getAddedRegion(customerWBName, ctMainSTName, ctLstColIndx, customerRowStartPos).Rows
        Call buildSummarySellRow(iRow, customerSWBName, ctSMainSTName, unitPrice, customerDict)
        
    Next
    
End Sub




' *****************************************************************
' *
' * Split "customer summary" main sheet, and build sub-sheet.
' *
' *****************************************************************

Function copyRowToSummarySubSheet(ByVal copiedRow, ByVal targetWBName, ByVal sheetName)
    firstColIndx = "a"
    
    ' ***********************
    
    rowIndx = (getLastRowIndx(targetWBName, sheetName) + 1)
    corrSheetStartIndx = firstColIndx & rowIndx
    
    copiedRow.Copy Workbooks(targetWBName).Sheets(sheetName).Range(corrSheetStartIndx)
    
    totolGrossIndx = "g"
    debtIndx = "i"
    Workbooks(targetWBName).Sheets(sheetName).Range(totolGrossIndx & rowIndx) = "=F" & rowIndx & "*E" & rowIndx
    Workbooks(targetWBName).Sheets(sheetName).Range(debtIndx & rowIndx) = "=I" & (rowIndx - 1) & "+G" & rowIndx & "-H" & rowIndx
    
End Function

Sub sumCellAbove(ByVal wbName, ByVal sheetName, ByVal colIndx, ByVal rowTopIndx, ByVal rowStartIndx, ByVal rowGapLines)
    Workbooks(wbName).Sheets(sheetName).Range(colIndx & rowStartIndx) = "=sum(" & colIndx & rowTopIndx & ":" & colIndx & (rowStartIndx - rowGapLines - 1) & ")"
End Sub

Sub splitCustomerSummaryInfoRByR()
    controlCenterWBName = "控制中心.xlsm"
    controlCenterMainSheetName = 1
    ccCTSNameCell = "b9"
    ccCTSPosCell = "b11"
    
    ' ***********************
    
    Dim controlCenter As Workbook
    Set controlCenter = Workbooks(controlCenterWBName)
    
    customerSWBName = controlCenter.Sheets(controlCenterMainSheetName).Range(ccCTSNameCell)
    startSRowPos = controlCenter.Sheets(controlCenterMainSheetName).Range(ccCTSPosCell)
    bakupFile (customerSWBName)
    
    
    colStartPos = 1
    colEndPos = "j"
    rowKeyPos = "d"
    
    
    Dim ctDict As Object
    Set ctDict = CreateObject("Scripting.Dictionary")
    
    
    firstDomain = "a:c"
    secondDomain = "e:j"
    ctKeyDomain = "d"
    For Each iRow In getAddedRegion(customerSWBName, colStartPos, colEndPos, startSRowPos).Rows
        Set cpRg = Union(iRow.Columns(firstDomain), iRow.Columns(secondDomain))
        Call copyRowToSummarySubSheet(cpRg, customerSWBName, iRow.Columns(rowKeyPos).Value)
        
        ctKey = cpRg.Columns(ctKeyDomain).Value
        If ctDict(ctKey) <> 0 Then
            ctDict(ctKey) = ctDict(ctKey) + 1
        Else
            ctDict(ctKey) = 1
        End If
    Next
    
    
    
    For Each iCtKey In ctDict.Keys()
        rowGapLines = 3
        rowStartIndx = getLastRowIndx(customerSWBName, iCtKey) + rowGapLines + 1
        colEndIndx = "i"
        
        ' Clear below contents and formats
        Workbooks(customerSWBName).Sheets(iCtKey).Range("a" & (rowStartIndx - rowGapLines) & ":" & colEndIndx & rowStartIndx).Clear
        
        
        totalWordColIndx = "b"
        Workbooks(customerSWBName).Sheets(iCtKey).Range(totalWordColIndx & rowStartIndx) = "合计"
        Workbooks(customerSWBName).Sheets(iCtKey).Range(totalWordColIndx & rowStartIndx).HorizontalAlignment = xlCenter
        
        
        rowTopIndx = 4
        clothCountColIndx = "d"
        clothLengthColIndx = "e"
        clothGrossAmount = "g"
        Call sumCellAbove(customerSWBName, iCtKey, clothCountColIndx, rowTopIndx, rowStartIndx, rowGapLines)
        Call sumCellAbove(customerSWBName, iCtKey, clothLengthColIndx, rowTopIndx, rowStartIndx, rowGapLines)
        Call sumCellAbove(customerSWBName, iCtKey, clothGrossAmount, rowTopIndx, rowStartIndx, rowGapLines)
        
        ' Add border lines for added region
        rowOffset = ctDict(iCtKey)
        filledRange = "a" & (rowStartIndx - rowGapLines - rowOffset) & ":" & colEndIndx & rowStartIndx
        Workbooks(customerSWBName).Sheets(iCtKey).Range(filledRange).Borders.LineStyle = 1
    Next
    
End Sub




' *****************************************************************
' *
' * Lab for testing.
' *
' *****************************************************************

Sub test()
    newDate = Replace(Date, "-", "")
    newTime = Left(Replace(Time(), ":", ""), 4)
    res = newDate & newTime
    
    prePath = Application.ActiveWorkbook.Path
    source = prePath & "\控制中心.xlsm"
    dest = prePath & "\bak\控制中心-" & res & ".xlsm"
    
    Workbooks("控制中心.xlsm").SaveCopyAs dest
End Sub




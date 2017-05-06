Function getColorDict() As Object
    Set getColorDict = CreateObject("Scripting.Dictionary")
    
    getColorDict.Add "501", "1??"
    getColorDict.Add "502", "2??"
    getColorDict.Add "503", "3??"
    getColorDict.Add "504", "4??"
    getColorDict.Add "505", "5??"
    getColorDict.Add "506", "6??"
    getColorDict.Add "508", "8??"
    getColorDict.Add "509", "9??"
    getColorDict.Add "510", "10??"
    getColorDict.Add "511", "11???"
    getColorDict.Add "512", "12???"
    getColorDict.Add "514", "14??"
    getColorDict.Add "601", "601"
    getColorDict.Add "602", "602"
    getColorDict.Add "603", "603??"
    getColorDict.Add "604", "604???"
    getColorDict.Add "605", "605??"
    getColorDict.Add "606", "606???"
    getColorDict.Add "607", "607??"
    getColorDict.Add "608", "608"
    getColorDict.Add "609", "609"
    getColorDict.Add "610", "610"
    getColorDict.Add "611", "611"
    getColorDict.Add "612", "612"
    getColorDict.Add "??", "??"
    getColorDict.Add "??????", "??????"
    
    getColorDict.Add "C32", "32???"
    getColorDict.Add "C16", "16???"
End Function

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

Sub appendInfoRByR()
    controlCenterWBName = "????.xlsm"
    controlCenterMainSheetName = 1
    ccWHNameCell = "b2"
    ccWHPosCell = "b3"
    
    ' ***********************
    
    Dim controlCenter As Workbook
    Set controlCenter = Workbooks(controlCenterWBName)
    
    warehouseWBName = controlCenter.Sheets(controlCenterMainSheetName).Range(ccWHNameCell)
    startPos = controlCenter.Sheets(controlCenterMainSheetName).Range(ccWHPosCell)
        
    Dim colorDict As Object
    Set colorDict = getColorDict()
    
    
    
    colStartPos = 1
    colEndPos = "o"
    rowKeyPos = 2
    colorCodeOffset = 3
    greighTypeOffset = 3
    
    For Each iRow In getAddedRegion(warehouseWBName, colStartPos, colEndPos, startPos).Rows
        Call copyRowToSheet(iRow, warehouseWBName, colorDict(Right(iRow.Cells(rowKeyPos), colorCodeOffset)))
        Call copyRowToSheet(iRow, warehouseWBName, colorDict(Left(iRow.Cells(rowKeyPos), greighTypeOffset)))
    Next
    
End Sub

Function getCellContents(ByVal wbName, ByVal stName, ByVal cPos) As Range
    Set getCellContents = Workbooks(wbName).Sheets(stName).Range(cPos)
End Function

Sub buildSellRow(ByVal copiedRow, ByVal wbName, ByVal sheetName, ByVal unitPrice)
    firstDomain = "a:e"
    secondDomain = "h"
    thirdDomain = "i"
    fourthDomain = "l"

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
    controlCenterWBName = "????.xlsm"
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
    warehouseStartPos = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccWHPosCell)
    
    customerWBName = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTNameCell)
    unitPrice = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccUnitPriceCell)
    
    Dim customerStartPos As Range
    Set customerStartPos = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTPosCell)
    customerStartPos = getLastRowIndx(customerWBName, ctMainSTName) + 1
    
    For Each iRow In getAddedRegion(warehouseWBName, whMainSTName, whLstColIndx, warehouseStartPos).Rows
        If iRow.Columns("c") = "?" Then
            Call buildSellRow(iRow, customerWBName, ctMainSTName, unitPrice)
        End If
    Next
    
    MsgBox "??? **?????** ???!"
End Sub

Sub splitCustomerInfoRByR()
    controlCenterWBName = "????.xlsm"
    controlCenterMainSheetName = 1
    ccCTNameCell = "b5"
    ccCTPosCell = "b7"
    
    ' ***********************
    
    Dim controlCenter As Workbook
    Set controlCenter = Workbooks(controlCenterWBName)
    
    customerWBName = controlCenter.Sheets(controlCenterMainSheetName).Range(ccCTNameCell)
    startPos = controlCenter.Sheets(controlCenterMainSheetName).Range(ccCTPosCell)
    
    
    colStartPos = 1
    colEndPos = "o"
    rowKeyPos = "d"
    
    For Each iRow In getAddedRegion(customerWBName, colStartPos, colEndPos, startPos).Rows
        Call copyRowToSheet(iRow, customerWBName, iRow.Columns(rowKeyPos).Value)
    Next
    
End Sub

Sub test()
    Workbooks("????.xlsm").Sheets(1).Range("b8") = 3000
End Sub


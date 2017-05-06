Function getColorDict() As Object
    Set getColorDict = CreateObject("Scripting.Dictionary")
    
    getColorDict.Add "501", "1????"
    getColorDict.Add "502", "2¨ª¨¢??"
    getColorDict.Add "503", "3?¨¹?¨¬"
    getColorDict.Add "504", "4?3?¡§"
    getColorDict.Add "505", "5D¨®¨¦?"
    getColorDict.Add "506", "6?3?¨°"
    getColorDict.Add "508", "8?¨¹??"
    getColorDict.Add "509", "9¨¦??¨¹"
    getColorDict.Add "510", "10¨¦??¨°"
    getColorDict.Add "511", "11¨¦?2??¨¤"
    getColorDict.Add "512", "12¨¦??¡§??"
    getColorDict.Add "514", "14?¡Á¡ã¡Á"
    getColorDict.Add "601", "601"
    getColorDict.Add "602", "602"
    getColorDict.Add "603", "603?¡§??"
    getColorDict.Add "604", "604¨¦??¡§??"
    getColorDict.Add "605", "605?¨¬?¨°"
    getColorDict.Add "606", "606¨¦??¡§??"
    getColorDict.Add "607", "607¨¦??¨°"
    getColorDict.Add "608", "608"
    getColorDict.Add "609", "609"
    getColorDict.Add "610", "610"
    getColorDict.Add "611", "611"
    getColorDict.Add "612", "612"
    getColorDict.Add "?¨°D¨®", "?¨°D¨®"
    getColorDict.Add "??D?¨º1¨®???¨¦?", "??D?¨º1¨®???¨¦?"
    
    getColorDict.Add "C32", "32¡Á¨¹?a¡ä?"
    getColorDict.Add "C16", "16¡Á¨¹?a¡ä?"
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
    controlCenterWBName = "?????DD?.xlsm"
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
    
    
    
    rowStartPos = 1
    colEndPos = "o"
    rowKeyPos = 2
    colorCodeOffset = 3
    greighTypeOffset = 3
    
    For Each iRow In getAddedRegion(warehouseWBName, rowStartPos, colEndPos, startPos).Rows
        Call copyRowToSheet(iRow, warehouseWBName, colorDict(Right(iRow.Cells(rowKeyPos), colorCodeOffset)))
        Call copyRowToSheet(iRow, warehouseWBName, colorDict(Left(iRow.Cells(rowKeyPos), greighTypeOffset)))
    Next
    
End Sub

Function getCellContents(ByVal wbName, ByVal stName, ByVal cPos) As Range
    Set getCellContents = Workbooks(wbName).Sheets(stName).Range(cPos)
End Function

Sub copyToWorkBook()
    controlCenterWBName = "?????DD?.xlsm"
    controlCenterMainSheetName = 1
    ccWHNameCell = "b2"
    ccWHPosCell = "b3"
    ccCTNameCell = "b5"
    
    ' ***********************
    
    warehouseWBName = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccWHNameCell)
    customerWBName = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccCTNameCell)
    warehouseStartPos = getCellContents(controlCenterWBName, controlCenterMainSheetName, ccWHPosCell)
            
    whMainSTName = 1
    whLstColIndx = "o"
    ctMainSTName = 1
    ctColStartIndx = 1
    
    
    Dim copiedRange As Range
    For Each iRow In getAddedRegion(warehouseWBName, whMainSTName, whLstColIndx, warehouseStartPos).Rows
        If iRow.Columns("c") = "¨º?" Then
            Set copiedRange = Union(iRow.Columns("a:e"), iRow.Columns("h"), iRow.Columns("i"), iRow.Columns("l"))
            
            ctRowStartIndx = getLastRowIndx(customerWBName, ctMainSTName) + 1
            Call copyRowToSheet(copiedRange, customerWBName, ctMainSTName)
                        
            unitPrice = 9.5
            Workbooks(customerWBName).Sheets(ctMainSTName).Cells(ctRowStartIndx, 9) = "=H" & ctRowStartIndx & "*G" & ctRowStartIndx
            Workbooks(customerWBName).Sheets(ctMainSTName).Cells(ctRowStartIndx, 10) = unitPrice
            Workbooks(customerWBName).Sheets(ctMainSTName).Cells(ctRowStartIndx, 11) = "=J" & ctRowStartIndx & "*I" & ctRowStartIndx
            Workbooks(customerWBName).Sheets(ctMainSTName).Cells(ctRowStartIndx, 15) = "=o" & CStr(ctRowStartIndx - 1) & "+K" & ctRowStartIndx & "-M" & ctRowStartIndx & "-N" & ctRowStartIndx
                        
        End If
    Next
End Sub

Sub test()

End Sub


'========================================================================================
'	
'   Filename: PS6000VBCon.vb
'
'	Description:
'	    This file demonstrates how to use VB .NET on the PicoScope 6000 Series 
'       oscilloscopes the ps6000 driver.
'       The application shows how to call the API functions open a connection to an 
'       oscilloscope and print out device information.
'
'   Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Module PS6000VBCon

    ' ******************************************************************************************************************************************************************
    ' getDeviceInfo - Reads and displays the scopes device information.
    '
    ' Parameters - handle    - The device handle
    ' *******************************************************************************************************************************************************************
    Sub getDeviceInfo(ByVal handle As Short)

        Dim infoText(8) As String
        Dim str As String
        Dim requiredSize As Short
        Dim i As UInteger

        infoText(0) = "Driver Ver:        "
        infoText(1) = "USB Ver:           "
        infoText(2) = "Hardware Ver:      "
        infoText(3) = "Variant:           "
        infoText(4) = "Batch / Serial:    "
        infoText(5) = "Cal Date:          "
        infoText(6) = "Kernel Driver Ver: "
        infoText(7) = "Digital H/W Ver:   "
        infoText(8) = "Analogue H/W Ver:  "

        For i = 0 To 8

            str = "            "
            ps6000GetUnitInfo(handle, str, CShort(str.Length), requiredSize, CType(i, InfoType))
            infoText(i) += str
            Console.WriteLine(infoText(i) & vbTab)

        Next i

    End Sub

    Sub Main()

        Dim status As UInteger
        Dim handle As Short
        Dim ch As ConsoleKeyInfo

        Console.WriteLine("PicoScope 6000 Series (ps6000) VB .NET Example Program" _
                          & vbNewLine & "======================================================" & vbNewLine)

        Dim serial As String = vbNullString

        status = ps6000OpenUnit(handle, serial)

        If handle = 0 Then
            Console.WriteLine("Unit not opened")
            ch = Console.ReadKey(True)
            Exit Sub
        End If

        getDeviceInfo(handle) 'Read and display the device information

        Console.WriteLine(vbNewLine)

        Sleep(5000)

        ' Close the unit at the end
        ps6000CloseUnit(handle)


    End Sub

End Module

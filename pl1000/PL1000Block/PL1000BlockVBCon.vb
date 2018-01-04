'===================================================================================================
'	
'   Filename: USBDRDAQBVCon.vb
'
'	Description: 
'	This file demonstrates how to use VB .NET on the PicoLog 1000 Series data loggers using the 
'   pl1000 driver.
'   It shows how to connect to a device, display device information and collect a block of data.
'
'   Copyright (C) 2018 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================
Imports System.Text
Imports System.Threading

Module PL1000BlockVBCon

    Dim handle As Short
    Dim status As UInteger = PICO_OK

    Sub Main()

        Console.WriteLine("PicoLog 1000 Series (pl1000) Driver VB .NET Console Example")
        Console.WriteLine()

        status = pl1000OpenUnit(handle)

        If status <> PICO_OK Then
            MsgBox("Unit not opened", vbOKOnly, "Error Message")
            Console.WriteLine("PicoLog 1000 Series device not opened.")
            Thread.Sleep(2000)
            Exit Sub
        End If

        Dim requiredSize As Integer
        Dim strBuilder As New StringBuilder

        strBuilder.Capacity = 40
        strBuilder.Length = 40

        Dim str = strBuilder.ToString()

        ' Get the unit information
        Console.WriteLine("Unit Opened:-")

        status = pl1000GetUnitInfo(handle, str, str.Length, requiredSize, PicoInfo.PICO_VARIANT_INFO) ' Variant
        Console.WriteLine("Variant: {0}", str.Substring(0, requiredSize))

        status = pl1000GetUnitInfo(handle, str, str.Length, requiredSize, PicoInfo.PICO_BATCH_AND_SERIAL) ' Serial number
        Console.WriteLine("Serial number: {0}", str.Substring(0, requiredSize))

        status = pl1000GetUnitInfo(handle, str, str.Length, requiredSize, PicoInfo.PICO_DRIVER_VERSION) ' Driver version
        Console.WriteLine("Driver version: {0}", str.Substring(0, requiredSize))

        Console.WriteLine()


        ' Stop the device
        status = pl1000Stop(handle)

        ' Close the device
        Call pl1000CloseUnit(handle)

        Console.WriteLine("PicoLog 1000 Series Device closed.")

    End Sub

End Module

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
        Dim us_for_block As Double
        Dim ideal_no_of_samples As Double
        Dim channels As PL1000Block.PL1000Inputs
        Dim no_of_channels As Integer
        Dim method As PL1000Block.BlockMethod
        Dim ready As Integer
        Dim noOfValues As Double
        Dim overflow As Integer
        Dim triggerIndex As Double
        Dim strBuilder As New StringBuilder
        Dim values As UShort() = New UShort(9) {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}

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

        ' Set sampling speed
        us_for_block = 1000
        ideal_no_of_samples = 10
        channels = (PL1000Block.PL1000Inputs.PL1000_CHANNEL_1)
        no_of_channels = 1
        status = pl1000SetInterval(handle, us_for_block, ideal_no_of_samples, channels, no_of_channels)

        ' Run block
        method = PL1000Block.BlockMethod.BM_SINGLE

        status = pl1000Run(handle, ideal_no_of_samples, method)

        ' Wait for block to be collected
        ready = 0
        While ready = 0
            status = pl1000Ready(handle, ready)
        End While

        ' Get values
        noOfValues = ideal_no_of_samples
        overflow = 0
        triggerIndex = 1
        status = pl1000GetValues(handle, values(0), noOfValues, overflow, triggerIndex)

        Console.WriteLine("First ADC count data value.")
        Console.WriteLine(values(0))

        ' Stop the device
        status = pl1000Stop(handle)

        ' Close the device
        Call pl1000CloseUnit(handle)

        Console.WriteLine("PicoLog 1000 Series Device closed.")

    End Sub

End Module

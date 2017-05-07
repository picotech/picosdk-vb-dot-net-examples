'===================================================================================================
'	
'   Filename: USBDRDAQBVCon.vb
'
'	Description: 
'	This file demonstrates how to use VB .NET on the USB DrDAQ using the usbdrdaq driver.
'   It shows how to connect to a device, display device information and collect some data.
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================
Imports System.Text

Module USBDRDAQVBCon

    ' Define variables
    Dim handle As Short
    Dim status As UInteger


    Sub Main()

        Console.WriteLine("USB DrDAQ (usbdrdaq) Driver VB .NET Console Example")
        Console.WriteLine()

        status = UsbDrDaqOpenUnit(handle)

        If status <> PICO_OK Then
            MsgBox("Unit not opened", vbOKOnly, "Error Message")
            Console.WriteLine("USB DrDAQ not opened.")
            Sleep(2000)
            Exit Sub
        End If

        Dim requiredSize As Integer
        Dim strBuilder As New StringBuilder

        strBuilder.Capacity = 40
        strBuilder.Length = 40

        Dim str = strBuilder.ToString()

        ' Get the unit information
        Console.WriteLine("Unit Opened:-")

        status = UsbDrDaqGetUnitInfo(handle, str, str.Length, requiredSize, UsbDrDaqInfo.USBDrDAQ_VARIANT_INFO) ' Variant
        Console.WriteLine("Variant: {0}", str.Substring(0, requiredSize))

        status = UsbDrDaqGetUnitInfo(handle, str, str.Length, requiredSize, UsbDrDaqInfo.USBDrDAQ_BATCH_AND_SERIAL) ' Serial number
        Console.WriteLine("Serial number: {0}", str.Substring(0, requiredSize))

        status = UsbDrDaqGetUnitInfo(handle, str, str.Length, requiredSize, UsbDrDaqInfo.USBDrDAQ_DRIVER_VERSION) ' Driver version
        Console.WriteLine("Driver version: {0}", str.Substring(0, requiredSize))

        Console.WriteLine()

        'Set Digital output

        status = UsbDrDaqSetDO(handle, 1, 1)
        status = UsbDrDaqSetDO(handle, 1, 0)

        'Collect a 1 second block containing 1000 samples (100 per channel)

        Dim channels(9) As UsbDrDaqInputs

        channels(0) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_EXT1         ' Ext. sensor 1
        channels(1) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_EXT2         ' Ext. sensor 2
        channels(2) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_EXT3         ' Ext. sensor 3
        channels(3) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_SCOPE        ' Scope channel
        channels(4) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_PH           ' PH
        channels(5) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_RES          ' Resistance
        channels(6) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_LIGHT        ' Light
        channels(7) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_TEMP         ' Thermistor
        channels(8) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_MIC_WAVE     ' Microphone waveform
        channels(9) = UsbDrDaqInputs.USB_DRDAQ_CHANNEL_MIC_LEVEL    ' Microphone level


        Dim totalSamples As UInteger = 1000

        ' Set the sampling interval
        status = UsbDrDaqSetInterval(handle, 1000000, totalSamples, channels(0), channels.Length)

        If status <> PICO_OK Then
            MsgBox("Settings error", vbOKOnly, "Error Message")
            Call UsbDrDaqCloseUnit(handle)
            Exit Sub
        End If

        Dim ready As Short


        ' Start data capture
        ready = 0
        status = UsbDrDaqRun(handle, totalSamples, BlockMethod.BM_SINGLE)

        If status <> PICO_OK Then
            MsgBox("Run error", vbOKOnly, "Error Message")
            Call UsbDrDaqCloseUnit(handle)
            Exit Sub
        End If

        Do While ready = 0
            status = UsbDrDaqReady(handle, ready)
            Sleep(5)
        Loop

        ' Retrieve data values
        Dim values(999) As Single
        Dim nValues As UInteger = 100
        Dim overflow As Short
        Dim triggerIndex As UInteger
        Dim inputName As String

        status = UsbDrDaqGetValuesF(handle, values(0), nValues, overflow, triggerIndex)

        'Display values - these will need to be scaled according to the channel scaling
        For index = 1 To channels.Length

            inputName = [Enum].GetName(GetType(UsbDrDaqInputs), index)
            Console.Write("{0}" + vbTab, inputName.Remove(0, 18))

        Next

        Console.WriteLine()

        For i = 0 To nValues - 1

            For j = 0 To channels.Length - 1

                Console.Write("{0:0.##}" + vbTab, values(i * channels.Length + j))

            Next

            Console.WriteLine()

        Next

        ' Reset ready flag if we need to collect another block of data
        ready = 0

        ' Stop the device
        status = UsbDrDaqStop(handle)

        ' Close the device
        Call UsbDrDaqCloseUnit(handle)

        Console.WriteLine("Unit Closed.")

    End Sub

End Module

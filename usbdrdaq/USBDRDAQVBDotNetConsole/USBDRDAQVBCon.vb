'===================================================================================================
'	
' Filename: USBDRDAQBVBCon.vb
'
'	Description: 
'	This file demonstrates how to use VB .NET on the USB DrDAQ using the usbdrdaq driver.
'   It shows how to connect to a device, display device information and collect some data.
'
' Copyright (C) 2016-2019 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================
Imports System.Text
Imports System.Threading

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

    ' Set the channels on which to collect data. To find and set the appropriate scaling value
    ' for each channel use the UsbDrDaqGetScalings() and UsbDrDaqSetScalings() functions.

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


    Dim numSamplesPerChannel As UInteger = 100

    ' Set the sampling interval to 100 samples per second for each channel.
    ' This corresponds to an overall sampling rate for the device of 1 kS/s.
    status = UsbDrDaqSetInterval(handle, 1000000, numSamplesPerChannel, channels(0), channels.Length)

    If status <> PICO_OK Then
      Console.WriteLine("Error:- UsbDrDaqSetInterval returned status code {0} (0x{1})", status, status.ToString("X"))
      Call ExitSub()
    End If

    ' Setup the signal generator to output a ramp up waveform with a frequency of 5 Hz
    ' and a peak-to-peak voltage of 1 V.

    Dim offsetVoltage As Integer = 0
    Dim peakToPeakVoltage As UInteger = 2000000 ' ±1 V
    Dim frequency As Short = 5

    status = UsbDrDaqSetSigGenBuiltIn(handle, offsetVoltage, peakToPeakVoltage, frequency, UsbDrDaqWave.USB_DRDAQ_RAMP_UP)

    If status <> PICO_OK Then
      Console.WriteLine("Error:- UsbDrDaqSetSigGenBuiltIn returned status code {0} (0x{1})", status, status.ToString("X"))
      Call ExitSub()
    End If

    Console.WriteLine("Please connect the Sig Gen output to the Scope input and press <Enter> to start data collection.")

    While (Console.ReadKey(True).Key <> ConsoleKey.Enter)


    End While

    Console.WriteLine()
    Console.WriteLine("Starting data capture...")

    Dim ready As Short

    ' Start data capture
    ready = 0
    status = UsbDrDaqRun(handle, numSamplesPerChannel, BlockMethod.BM_SINGLE)

    If status <> PICO_OK Then
      Console.WriteLine("Error:- UsbDrDaqRun returned status code {0} (0x{1})", status, status.ToString("X"))
      Call ExitSub()
    End If

    Do While ready = 0
      status = UsbDrDaqReady(handle, ready)
      Sleep(5)
    Loop

    ' Retrieve data values
    Dim values(numSamplesPerChannel * channels.Length - 1) As Single
    Dim numValuesToCollectPerChannel As UInteger = numSamplesPerChannel
    Dim overflow As Short
    Dim triggerIndex As UInteger
    Dim inputName As String

    status = UsbDrDaqGetValuesF(handle, values(0), numValuesToCollectPerChannel, overflow, triggerIndex)

    ' Display values - these will need to be scaled according to the channel scaling.
    ' Use the UsbDrDaqGetChannelInfo() function to obtain the necessary information to
    ' scale the data.

    For index = 1 To channels.Length

      inputName = [Enum].GetName(GetType(UsbDrDaqInputs), index)
      Console.Write("{0}" + vbTab, inputName.Remove(0, 18))

    Next

    Console.WriteLine()

    For i = 0 To numValuesToCollectPerChannel - 1

      For j = 0 To channels.Length - 1

        Console.Write("{0:0.##}" + vbTab, values(i * channels.Length + j))

      Next

      Console.WriteLine()

    Next

    ' Reset ready flag if we need to collect another block of data
    ready = 0

    Console.WriteLine()
    Console.WriteLine("Data capture complete.")

    ' Stop the device
    status = UsbDrDaqStop(handle)

    ' Stop the signal generator
    status = UsbDrDaqStopSigGen(handle)

    Call ExitSub()

  End Sub

  Sub ExitSub()

    ' Close the connection to the device
    Call UsbDrDaqCloseUnit(handle)

    Console.WriteLine(vbNewLine)

    Console.WriteLine("Exiting application..." & vbNewLine)

    Thread.Sleep(5000)

  End Sub

End Module

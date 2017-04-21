'===================================================================================================
'	Filename:			PS5000ABlockVBCon.vb
'
'	Description: 
'	This file demonstrates how to use VB .NET on the PicoScope 5000 series Flexible Resolution 
'   Oscilloscopes using the ps5000a driver.
'   The application shows how to connect to a device, display device information and collect data in 
'   block mode.
'
'   Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Imports System.IO

Module PS5000ABlockVBCon

    Dim mvConvert = True        ' Flag to show if values get converted to mV or stay as ADC counts

    ' Delegate

    Public ps5000aBlockCallback As ps5000aBlockReady

    ' Define variables

    Dim handle As Short
    Dim status As UInteger
    Dim serial As String
    Dim maxADCValue As Short
    Dim channelCount As Short
    Dim totalSamples As UInteger

    Public deviceReady As Boolean

    ' ******************************************************************************************************************************************************************
    ' GetDeviceInfo - Reads and displays the scopes device information. Fills out the UnitModel Structure depending upon device type.
    '
    ' Parameters: handle - the device handle
    ' *******************************************************************************************************************************************************************
    Sub getDeviceInfo(ByVal handle As Short)

        Dim infoText(8) As String
        Dim infoStr As String
        Dim requiredSize As Integer
        Dim i As Integer
        Dim chrs(5) As Char

        infoText(0) = "Driver Ver:        "
        infoText(1) = "USB Ver:           "
        infoText(2) = "Hardware Ver:      "
        infoText(3) = "Variant:           "
        infoText(4) = "Batch / Serial:    "
        infoText(5) = "Cal Date:          "
        infoText(6) = "Kernel Driver Ver: "
        infoText(7) = "Digital H/W Ver:   "
        infoText(8) = "Analogue H/W Ver:  "

        For i = 0 To infoText.Length - 1

            infoStr = "                    "
            ps5000aGetUnitInfo(handle, infoStr, CShort(infoStr.Length), requiredSize, i)
            infoText(i) += infoStr
            Console.WriteLine(infoText(i) & vbTab)

            If i = PicoInfo.PICO_VARIANT_INFO Then

                channelCount = Convert.ToInt16(infoStr.ToCharArray(1, 1))

            End If

        Next i

        Console.WriteLine(vbNewLine)

    End Sub

    ' ******************************************************************************************************************************************************************
    ' Main -        Entry point to the application
    '
    '
    ' *******************************************************************************************************************************************************************
    Sub Main()

        Dim status As UInteger
        Dim handle As Short


        Console.WriteLine("PicoScope 5000 Series (ps5000a) VB .NET Block Capture Example Program" & vbNewLine _
                          & "=====================================================================" & vbNewLine)

        ' Device Connection and Setup
        ' ===========================

        ' Open device with 12-bit resolution
        status = ps5000aOpenUnit(handle, vbNullString, PS5000AImports.DeviceResolution.PS5000A_DR_12BIT)

        If status = PicoStatus.PICO_OK Then

            Console.WriteLine("Device DC Powered" & vbNewLine)

        ElseIf status = PicoStatus.PICO_POWER_SUPPLY_NOT_CONNECTED Then

            ' Change power status if USB powered
            status = ps5000aChangePowerSource(handle, status)

            If status = PicoStatus.PICO_OK Then

                Console.WriteLine("Device USB Powered" & vbNewLine)

            Else

                Console.WriteLine("Error changing power source - exiting application.")
                Exit Sub

            End If

        ElseIf status = PicoStatus.PICO_NOT_FOUND Then

            Console.WriteLine("Device not found." & vbNewLine)
            Exit Sub

        Else

            Console.WriteLine("Device not opened." & vbNewLine)
            Exit Sub

        End If

        ' Find maximum ADC count (resolution dependent) - this value is used for scaling values between ADC counts and millivolts
        status = ps5000aMaximumValue(handle, maxADCValue)

        'Read and display the device information
        getDeviceInfo(handle)

        ' Channel Setup
        ' -------------

        ' Setup Channel A - 5V range, DC coupling, 0 Analogue offset
        status = ps5000aSetChannel(handle, PS5000AImports.Channel.PS5000A_CHANNEL_A, CShort(1), PS5000AImports.CouplingMode.PS5000A_DC, PS5000AImports.VoltageRange.PS5000A_5V, 0.0)

        ' Turn off other channels

        ' Turn off Channel B
        status = ps5000aSetChannel(handle, PS5000AImports.Channel.PS5000A_CHANNEL_B, CShort(0), PS5000AImports.CouplingMode.PS5000A_DC, PS5000AImports.VoltageRange.PS5000A_5V, 0.0)

        ' Only disable Channels C and D on a 4-channel device if the power supply is connected
        Dim powerStatus As UInteger
        powerStatus = ps5000aCurrentPowerSource(handle)

        If channelCount = 4 And powerStatus = PicoStatus.PICO_POWER_SUPPLY_CONNECTED Then

            status = ps5000aSetChannel(handle, PS5000AImports.Channel.PS5000A_CHANNEL_C, CShort(0), PS5000AImports.CouplingMode.PS5000A_DC, PS5000AImports.VoltageRange.PS5000A_5V, 0.0)
            status = ps5000aSetChannel(handle, PS5000AImports.Channel.PS5000A_CHANNEL_D, CShort(0), PS5000AImports.CouplingMode.PS5000A_DC, PS5000AImports.VoltageRange.PS5000A_5V, 0.0)

        End If

        ' Verify Timebase for sampling interval and maximum number of samples
        ' -------------------------------------------------------------------

        ' Confirm timebase corresponding to desired sampling interval is valid.
        ' Timebase indices can be calculated using the formulae in the Timebases section of the Programmer's Guide

        Dim timebase As UInteger
        Dim numPreTriggerSamples As Integer
        Dim numPostTriggerSamples As Integer
        Dim timeIntervalNs As Single
        Dim maxSamples As Integer
        Dim segmentIndex As UInteger
        Dim getTimebase2Status As UInteger

        timebase = 65
        numPreTriggerSamples = 0
        numPostTriggerSamples = 10000
        totalSamples = numPreTriggerSamples + numPostTriggerSamples

        timeIntervalNs = CSng(0.0)
        maxSamples = 0
        segmentIndex = 0
        getTimebase2Status = PicoStatus.PICO_INVALID_TIMEBASE ' Initialise as invalid timebase


        Do Until getTimebase2Status = PicoStatus.PICO_OK

            getTimebase2Status = ps5000aGetTimebase2(handle, timebase, totalSamples, timeIntervalNs, maxSamples, segmentIndex)

            If getTimebase2Status <> PicoStatus.PICO_OK Then

                timebase = timebase + 1

            End If

        Loop

        Console.WriteLine("Timebase: {0} Sample interval: {1}ns, Max Samples: {2}", timebase, timeIntervalNs, maxSamples, vbNewLine)

        ' Setup trigger
        ' -------------

        ' Setup a trigger such that the device will trigger on a rising edge through 500 mV on Channel A

        Dim threshold As Short
        Dim delay As UInteger
        Dim autoTriggerMs As Short

        ' Convert the threshold from millivolts to an ADC count
        threshold = PicoFunctions.mvToAdc(500, PS5000AImports.VoltageRange.PS5000A_5V, maxADCValue)

        Console.WriteLine("Trigger threshold: {0}mV ({1} ADC Counts)", 500, threshold, vbNewLine)

        delay = 0
        autoTriggerMs = 1000 ' Auto-trigger after 1 second if trigger event has not occurred

        status = ps5000aSetSimpleTrigger(handle, CShort(1), PS5000AImports.Channel.PS5000A_CHANNEL_A, threshold, PS5000AImports.ThresholdDirection.PS5000A_RISING, delay, autoTriggerMs)


        ' Data Capture
        ' ============

        ' Capture block of data from the device using a trigger.

        ' Create instance of delegate (callback function)
        ps5000aBlockCallback = New ps5000aBlockReady(AddressOf BlockCallback)

        Dim timeIndisposedMs As Integer = 0

        Console.WriteLine("Collecting {0} samples...", (numPreTriggerSamples + numPostTriggerSamples), vbNewLine)

        status = ps5000aRunBlock(handle, numPreTriggerSamples, numPostTriggerSamples, timebase, timeIndisposedMs, segmentIndex, ps5000aBlockCallback, IntPtr.Zero)

        While deviceReady = False

            Sleep(10)

        End While


        ' Setup data buffers to retrieve data values - this can also be carried out prior to data collection

        Dim valuesChA() As Short
        ReDim valuesChA(totalSamples - 1)

        status = ps5000aSetDataBuffer(handle, PS5000AImports.Channel.PS5000A_CHANNEL_A, valuesChA(0), CInt(totalSamples), segmentIndex, PS5000AImports.RatioMode.PS5000A_RATIO_MODE_NONE)

        ' Retrieve values

        Dim startIndex As UInteger
        Dim downSampleRatio As UInteger
        Dim overflow As Short

        startIndex = 0
        downSampleRatio = 1
        overflow = 0

        status = ps5000aGetValues(handle, startIndex, totalSamples, downSampleRatio, PS5000AImports.RatioMode.PS5000A_RATIO_MODE_NONE, segmentIndex, overflow)

        Console.WriteLine("Retrieved {0} samples.", totalSamples, vbNewLine)


        ' Process the Data
        ' ----------------

        ' In this example, the data is converted to millivolts and output to file

        Dim valuesChAMv() As Single
        ReDim valuesChAMv(totalSamples - 1)

        Dim fileName As String
        fileName = "ps5000a_triggered_block.txt"

        Using outfile As New StreamWriter(fileName)

            outfile.WriteLine("Triggered Block Data")
            outfile.WriteLine("Data is output in millivolts.")
            outfile.WriteLine()
            outfile.WriteLine("Channel A")
            outfile.WriteLine()

            For index = 0 To totalSamples - 1

                valuesChAMv(index) = PicoFunctions.adcToMv(valuesChA(index), PS5000AImports.VoltageRange.PS5000A_5V, maxADCValue)
                outfile.Write(valuesChAMv(index).ToString)
                outfile.WriteLine()

            Next

        End Using

        Console.WriteLine("Data written to file {0}", fileName)

        ' Reset deviceReady flag if we need to collect another block of data
        deviceReady = False

        ' Stop the device
        status = ps5000aStop(handle)

        ' Close the connection to the device
        ps5000aCloseUnit(handle)

        Console.WriteLine(vbNewLine)

        Console.WriteLine("Exiting application..." & vbNewLine)

        Sleep(5000)

    End Sub

    ' Block callback function

    Public Sub BlockCallback(handle As Short, status As UInteger, pVoid As IntPtr)
        ' flag to say done reading data
        If status <> PicoStatus.PICO_CANCELLED Then
            deviceReady = True
        End If
    End Sub

End Module

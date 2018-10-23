'===================================================================================================
'	
'   Filename: PS5000ARapidBlockVBCon.vb
'
'	Description: 
'	This file demonstrates how to use VB .NET on the PicoScope 5000 Series Flexible Resolution 
'   Oscilloscopes using the ps5000a driver API functions.
'   It shows how to connect to a device, display device information and collect data in 
'   rapid block mode.
'
'   Copyright © 2016-2018 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Imports System.IO
Imports System.Globalization
Imports System.Threading

Module PS5000ARapidBlockVBCon

    ' Structures

    Structure ChannelSettings

        Public enabled As Boolean
        Public couplingMode As PS5000AImports.CouplingMode
        Public voltageRange As PS5000AImports.VoltageRange

    End Structure

    ' Delegate

    Public ps5000aBlockCallback As ps5000aBlockReady

    ' Define variables

    Dim handle As Short
    Dim status As UInteger
    Dim serial As String
    Dim maxADCValue As Short
    Dim channelCount As Short
    Dim numAvailableChannels As Integer ' Indicate if 2 channels or 4 channels (power supply connected for PicoScope 544XA/B devices)
    Dim totalSamples As UInteger

    Public deviceReady As Boolean


    ' ******************************************************************************************************************************************************************
    ' GetDeviceInfo - Reads and displays the scopes device information.
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
        Dim chSettings(3) As ChannelSettings


        Console.WriteLine("PicoScope 5000 Series (ps5000a) VB .NET Rapid Block Capture Example Program" & vbNewLine _
                          & "===========================================================================" & vbNewLine)

        ' Device Connection and Setup
        ' ===========================

        ' Open device with 14-bit resolution
        status = ps5000aOpenUnit(handle, vbNullString, PS5000AImports.DeviceResolution.PS5000A_DR_12BIT)

        If status = PicoStatus.PICO_OK Then

            Console.WriteLine("Device DC Powered" & vbNewLine)

        ElseIf status = PicoStatus.PICO_POWER_SUPPLY_NOT_CONNECTED Or status = PicoStatus.PICO_USB3_0_DEVICE_NON_USB3_0_PORT Then

            ' Change power status if USB powered
            status = ps5000aChangePowerSource(handle, status)

            If status = PicoStatus.PICO_OK Then

                Console.WriteLine("Device USB Powered" & vbNewLine)

            Else

                Console.WriteLine("Error changing power source - exiting application.")
                ps5000aCloseUnit(handle)
                Exit Sub

            End If

        ElseIf status = PicoStatus.PICO_NOT_FOUND Then

            Console.WriteLine("Device not found - exiting application." & vbNewLine)
            Thread.Sleep(5000)
            Exit Sub

        Else

            Console.WriteLine("Device not opened (ps5000aOpenUnit returned status code {0} (0x{1}))", status, status.ToString("X") & vbNewLine)
            Thread.Sleep(5000)
            Exit Sub

        End If

        ' Find maximum ADC count (resolution dependent) - this value is used for scaling values between ADC counts and millivolts
        status = ps5000aMaximumValue(handle, maxADCValue)

        'Read and display the device information
        getDeviceInfo(handle)

        ' Channel Setup
        ' -------------

        ' Setup Channel A - 5V range, DC coupling, 0 Analogue offset

        chSettings(0).enabled = True
        chSettings(0).couplingMode = CouplingMode.PS5000A_DC
        chSettings(0).voltageRange = PS5000AImports.VoltageRange.PS5000A_5V

        status = ps5000aSetChannel(handle, PS5000AImports.Channel.PS5000A_CHANNEL_A, CShort(chSettings(0).enabled), chSettings(0).couplingMode, chSettings(0).voltageRange, 0.0)

        If status <> PicoStatus.PICO_OK Then

            Console.WriteLine("Error:- ps5000aSetChannel on channel A returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

        End If

        ' Turn off other channels

        ' Turn off Channel B

        chSettings(1).enabled = False
        chSettings(1).couplingMode = CouplingMode.PS5000A_DC
        chSettings(1).voltageRange = PS5000AImports.VoltageRange.PS5000A_5V

        status = ps5000aSetChannel(handle, PS5000AImports.Channel.PS5000A_CHANNEL_B, CShort(chSettings(1).enabled), chSettings(1).couplingMode, chSettings(1).voltageRange, 0.0)

        If status <> PicoStatus.PICO_OK Then

            Console.WriteLine("Error:- ps5000aSetChannel on channel B returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

        End If

        ' Only disable Channels C and D on a 4-channel device if the power supply is connected
        Dim powerStatus As UInteger
        powerStatus = ps5000aCurrentPowerSource(handle)

        If channelCount = 4 And powerStatus = PicoStatus.PICO_POWER_SUPPLY_CONNECTED Then

            chSettings(2).enabled = False
            chSettings(2).couplingMode = CouplingMode.PS5000A_DC
            chSettings(2).voltageRange = PS5000AImports.VoltageRange.PS5000A_5V

            status = ps5000aSetChannel(handle, PS5000AImports.Channel.PS5000A_CHANNEL_C, CShort(chSettings(2).enabled), chSettings(2).couplingMode, chSettings(2).voltageRange, 0.0)

            If status <> PicoStatus.PICO_OK Then

                Console.WriteLine("Error:- ps5000aSetChannel on channel C returned status code {0} (0x{1})", status, status.ToString("X"))
                Call ExitSub()

            End If

            chSettings(3).enabled = False
            chSettings(3).couplingMode = CouplingMode.PS5000A_DC
            chSettings(3).voltageRange = PS5000AImports.VoltageRange.PS5000A_5V

            status = ps5000aSetChannel(handle, PS5000AImports.Channel.PS5000A_CHANNEL_D, CShort(chSettings(3).enabled), chSettings(3).couplingMode, chSettings(3).voltageRange, 0.0)

            If status <> PicoStatus.PICO_OK Then

                Console.WriteLine("Error:- ps5000aSetChannel on channel D returned status code {0} (0x{1})", status, status.ToString("X"))
                Call ExitSub()

            End If

            numAvailableChannels = 4

        Else

            numAvailableChannels = 2 ' Set to 2 if 2 channels or 4 channel device on USB power

        End If


        ' Segment the Device Memory and Set Number of Captures
        ' ----------------------------------------------------

        Dim maxSegments As UInteger = 0
        Dim nSegments As UInteger = 16
        Dim nMaxSamples As Integer = 0
        Dim nRapidCaptures As UInteger = 10

        status = ps5000aGetMaxSegments(handle, maxSegments)

        If status <> PicoStatus.PICO_OK Then

            Console.WriteLine("Error:- ps5000aGetMaxSegments returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

        End If

        status = PS5000AImports.ps5000aMemorySegments(handle, nSegments, nMaxSamples) ' Note the maximum number of samples per segment (shared between all active channels)

        If status <> PicoStatus.PICO_OK Then

            Console.WriteLine("Error:- ps5000aMemorySegments returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

        End If

        status = PS5000AImports.ps5000aSetNoOfCaptures(handle, nRapidCaptures)

        If status <> PicoStatus.PICO_OK Then

            Console.WriteLine("Error:- ps5000aSetNoOfCaptures returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

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
        segmentIndex = 0 ' This will be used in the call to ps5000aRunBlock
        getTimebase2Status = PicoStatus.PICO_INVALID_TIMEBASE ' Initialise as invalid timebase


        Do Until getTimebase2Status = PicoStatus.PICO_OK

            getTimebase2Status = ps5000aGetTimebase2(handle, timebase, totalSamples, timeIntervalNs, maxSamples, segmentIndex)

            If getTimebase2Status <> PicoStatus.PICO_OK Then

                timebase = timebase + 1

            End If

        Loop

        Console.WriteLine("Timebase: {0} Sample interval: {1} ns, Max Samples: {2}", timebase, timeIntervalNs, maxSamples, vbNewLine)
        Console.WriteLine()

        ' Setup trigger
        ' -------------

        ' Setup a trigger such that the device will trigger on a rising edge through 500 mV on Channel A

        Dim threshold As Short
        Dim delay As UInteger
        Dim autoTriggerMs As Short

        ' Convert the threshold from millivolts to an ADC count
        threshold = PicoFunctions.mvToAdc(500, PS5000AImports.VoltageRange.PS5000A_5V, maxADCValue)

        Console.WriteLine("Trigger threshold: {0} mV ({1} ADC counts)", 500, threshold, vbNewLine)
        Console.WriteLine()

        delay = 0
        autoTriggerMs = 1000 ' Auto-trigger after 1 second if trigger event has not occurred

        status = ps5000aSetSimpleTrigger(handle, CShort(1), PS5000AImports.Channel.PS5000A_CHANNEL_A, threshold, PS5000AImports.ThresholdDirection.PS5000A_RISING, delay, autoTriggerMs)

        If status <> PicoStatus.PICO_OK Then

            Console.WriteLine("Error:- ps5000aSetSimpleTrigger returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

        End If

        ' Setup data buffers
        ' ------------------

        ' It can be more efficient to setup the data buffers prior to starting data collection

        ' Set up a jagged array to hold the data values
        Dim values As Short()()() = New Short(nRapidCaptures - 1)()() {}
        Dim pinned As PinnedArray.PinnedArray(Of Short)(,) = New PinnedArray.PinnedArray(Of Short)(nRapidCaptures - 1, channelCount - 1) {}

        For segment As UInteger = 0 To nRapidCaptures - 1

            values(segment) = New Short(channelCount - 1)() {}

            For channel As Integer = 0 To numAvailableChannels - 1

                If chSettings(channel).enabled Then

                    values(segment)(channel) = New Short(totalSamples - 1) {}
                    pinned(segment, channel) = New PinnedArray.PinnedArray(Of Short)(values(segment)(channel))

                    status = PS5000AImports.ps5000aSetDataBuffer(handle, channel, values(segment)(channel)(0), CInt(totalSamples), segment, PS5000AImports.RatioMode.PS5000A_RATIO_MODE_NONE)

                    If status <> PicoStatus.PICO_OK Then

                        Dim ch As String
                        ch = "A" + ChrW(channel)


                        Console.WriteLine("ps5000aSetDataBuffer Channel {0} segment {1} status = {2} (0x{3})", ChrW(Asc("A") + channel), segment, status, status.ToString("X"))

                    End If
                Else

                    ' If the channel is not enabled, pass a null pointer
                    status = PS5000AImports.ps5000aSetDataBuffer(handle, channel, Nothing, 0, segment, PS5000AImports.RatioMode.PS5000A_RATIO_MODE_NONE)

                    If status <> PicoStatus.PICO_OK Then

                        Console.WriteLine("ps5000aSetDataBuffer Channel {0} segment {1} status = {2} (0x{3})", ChrW(Asc("A") + channel), segment, status, status.ToString("X"))

                    End If
                End If
            Next
        Next

        ' Setup built-in signal generator
        ' ------------------------------- 

        ' In this example the signal generator output will be used to provide an input signal (1 kHz, 4 Vpp sine wave)

        Dim offset As Integer = 0
        Dim peakToPeak As UInteger = 4000000
        Dim frequency As Double = 1000.0
        Dim increment As Double = 0.0
        Dim dwellTime As Double = 0.0
        Dim shots As UInteger = 0
        Dim sweeps As UInteger = 0
        Dim extInThreshold As Short = 0

        status = PS5000AImports.ps5000aSetSigGenBuiltInV2(handle, offset, peakToPeak, PS5000AImports.WaveType.PS5000A_SINE, frequency, frequency, increment, dwellTime, PS5000AImports.SweepType.PS5000A_UP,
                                                          PS5000AImports.ExtraOperations.PS5000A_ES_OFF, shots, sweeps, PS5000AImports.SigGenTrigType.PS5000A_SIGGEN_RISING,
                                                          PS5000AImports.SigGenTrigSource.PS5000A_SIGGEN_NONE, extInThreshold)

        If status <> PicoStatus.PICO_OK Then

            Console.WriteLine("Error:- ps5000aSetSigGenBuiltInV2 returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

        End If

        Console.WriteLine("Please connect the signal generator output to Channel A and press any key to start data collection.")

        Console.ReadKey(vbTrue)
        Console.WriteLine()
        Console.WriteLine("Starting data capture...")


        ' Data Capture
        ' ============

        ' Capture blocks of data from the device using a trigger.

        Console.WriteLine("Collecting {0} waveforms ({1} samples per waveform).", nRapidCaptures, (numPreTriggerSamples + numPostTriggerSamples), vbNewLine)

        ' Create instance of delegate (callback function)
        ps5000aBlockCallback = New ps5000aBlockReady(AddressOf BlockCallback)

        Dim timeIndisposedMs As Integer = 0

        Dim keyPress As Boolean = False

        status = ps5000aRunBlock(handle, numPreTriggerSamples, numPostTriggerSamples, timebase, timeIndisposedMs, segmentIndex, ps5000aBlockCallback, IntPtr.Zero)

        If status <> PicoStatus.PICO_OK Then

            Console.WriteLine("Error:- ps5000aRunBlock returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

        End If

        While ((deviceReady = False) And (keyPress = False))

            Thread.Sleep(10)

            If Console.KeyAvailable Then                ' Check if the user has hit a key to indicate they want to stop waiting for data collection

                keyPress = True
                Console.WriteLine("Data collection cancelled.")

            End If

        End While

        Console.WriteLine()

        ' Find the number of captures

        Dim nCaptures As UInteger = 0

        status = PS5000AImports.ps5000aGetNoOfCaptures(handle, nCaptures)

        If status = PicoStatus.PICO_OK Then

            Console.WriteLine("Captured {0} waveforms.", nCaptures)

        Else

            Console.WriteLine("Error:- ps5000aGetNoofCaptures returned status code {0} (0x{1})", status, status.ToString("X"))
            Call ExitSub()

        End If


        If nCaptures > 0 Then

            ' Retrieve values

            Dim fromSegmentIndex = 0
            Dim toSegmentIndex = nCaptures - 1
            Dim downSampleRatio As UInteger = 1
            Dim overflow(nRapidCaptures - 1) As Short


            status = PS5000AImports.ps5000aGetValuesBulk(handle, totalSamples, fromSegmentIndex, toSegmentIndex, downSampleRatio, PS5000AImports.RatioMode.PS5000A_RATIO_MODE_NONE, overflow(0))

            If status = PicoStatus.PICO_OK Then

                Console.WriteLine("Retrieved {0} samples per segment.", totalSamples, vbNewLine)

            Else

                Console.WriteLine("Error: ps5000aGetValuesBulk returned with status {0} ({1})\n", status, status.ToString("X"))

            End If


            ' Process the Data
            ' ----------------

            ' In this example, the data is converted to millivolts and output to file
            ' Separate files are generated for each channel

            Dim valueMv As Single

            Dim fileNameSb As New Text.StringBuilder
            Dim fileStub As String
            Dim fileName As String
            Dim channelName As String

            For channel As Integer = 0 To numAvailableChannels - 1

                If chSettings(channel).enabled Then

                    channelName = ChrW(Asc("A") + channel)
                    fileNameSb.Append("ps5000a_rapid_block_ch").Append(channelName)
                    fileStub = fileNameSb.ToString().Trim(Convert.ToChar(0))
                    fileName = String.Concat(fileStub, ".txt")

                    Using outfile As New StreamWriter(fileName)

                        outfile.WriteLine("Rapid Block Data")
                        outfile.WriteLine("Data is output in millivolts.")
                        outfile.WriteLine()
                        outfile.WriteLine("Channel {0}", channelName)
                        outfile.WriteLine()

                        For segment As UInteger = 0 To nCaptures - 1

                            outfile.Write("Segment {0}", segment)
                            outfile.Write(vbTab)

                        Next

                        outfile.WriteLine()

                        For index = 0 To totalSamples - 1

                            For segment As UInteger = 0 To nCaptures - 1

                                valueMv = PicoFunctions.adcToMv(pinned(segment, channel).Target(index), chSettings(channel).voltageRange, maxADCValue)
                                outfile.Write(valueMv.ToString("0.0000", CultureInfo.InvariantCulture))
                                outfile.Write(vbTab)

                            Next

                            outfile.WriteLine()

                        Next

                        Console.WriteLine("Data written to file {0}", fileName)

                    End Using

                End If

                fileName = ""
                fileNameSb.Clear()

            Next

        End If

        ' Reset deviceReady flag if we need to collect another set of rapid captures
        deviceReady = False

        ' Stop the device
        status = ps5000aStop(handle)

        ' Un-pin the arrays
        For Each p As PinnedArray.PinnedArray(Of Short) In pinned
            If p IsNot Nothing Then
                p.Dispose()
            End If
        Next

        Call ExitSub()

    End Sub


    ' Block callback function

    Public Sub BlockCallback(handle As Short, status As UInteger, pVoid As IntPtr)
        ' flag to say done reading data
        If status <> PicoStatus.PICO_CANCELLED Then
            deviceReady = True
        End If
    End Sub

    Sub ExitSub()

        ' Close the connection to the device
        Call ps5000aCloseUnit(handle)

        Console.WriteLine(vbNewLine)

        Console.WriteLine("Exiting application..." & vbNewLine)

        Thread.Sleep(5000)

    End Sub


End Module


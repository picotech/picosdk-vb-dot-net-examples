'===================================================================================================
'
'	Filename:			PS2000BlockVBCon.vb
'
'	Description: 
'	This file demonstrates how to use VB .NET on the PicoScope 2000 Series oscilloscopes using the 
'   ps2000 driver.
'
'   It shows how to connect to a device, display device information, setup the signal generator and 
'   collect data in block mode.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Imports System.IO
Imports System.Threading

Module PS2000Block

    ' Define variables

    Dim status As Short
    Dim ps2000Handle As Short

    ' Arrays for data collection
    Dim times() As Integer
    Dim valuesChA() As Short
    Dim valuesChB() As Short

    ' ******************************************************************************************************************************************************************
    ' getDeviceInfo - Reads and displays the scopes device information.
    '
    ' Parameters: handle - the device handle
    ' *******************************************************************************************************************************************************************
    Sub getDeviceInfo(ByVal handle As Short)

        Dim infoText(7) As String
        Dim infoStr As String
        Dim i As UInteger

        infoText(0) = "Driver Ver:        "
        infoText(1) = "USB Ver:           "
        infoText(2) = "Hardware Ver:      "
        infoText(3) = "Variant:           "
        infoText(4) = "Batch / Serial:    "
        infoText(5) = "Cal Date:          "
        infoText(6) = "Error code:        "
        infoText(7) = "Kernel Driver Ver: "

        For i = 0 To infoText.Length - 1

            If i <> 6 Then

                infoStr = "                    "
                ps2000_get_unit_info(handle, infoStr, CShort(infoStr.Length), i)
                infoText(i) += infoStr
                Console.WriteLine(infoText(i) & vbTab)

            End If

        Next i

        Console.WriteLine(vbNewLine)

    End Sub

    Sub Main()

        Console.WriteLine("PicoScope 2000 Series (ps2000) VB .NET Block Capture Example Program" & vbNewLine _
                          & "====================================================================" & vbNewLine)

        ' Device Connection and Setup
        ' ===========================

        ' Open Device
        ps2000Handle = ps2000_open_unit()

        If ps2000Handle = 0 Then
            Console.WriteLine("Device not opened.")
            Exit Sub
        End If

        ' Display Unit Information
        getDeviceInfo(ps2000Handle)

        ' Turn off ETS
        Call ps2000_set_ets(ps2000Handle, 0, 0, 0) ' Turn ETS off

        ' Set Channels
        ' ------------

        Dim channelARange As VoltageRange = VoltageRange.PS2000_5V
        Dim channelBRange As VoltageRange = VoltageRange.PS2000_5V

        status = ps2000_set_channel(ps2000Handle, Channel.PS2000_CHANNEL_A, CShort(1), CShort(1), channelARange) ' Channel A: enabled DC coupling 5V range
        status = ps2000_set_channel(ps2000Handle, Channel.PS2000_CHANNEL_B, CShort(0), CShort(1), channelBRange) ' Channel B: not enabled DC coupling 5V range


        ' Verify Timebase for Sampling Interval
        ' -------------------------------------

        'Find the maximum number of samples the time interval (in TimeUnits)
        'the most suitable time units and the maximum oversample at the current timebase

        Dim timebaseStatus As Short = 0
        Dim timebase As Short = 10
        Dim numSamples As Integer = 2000
        Dim timeInterval As Integer
        Dim timeUnits As TimeUnits
        Dim oversample As Short = 1
        Dim maxSamples As Integer = 0

        Do Until timebaseStatus = 1

            timebaseStatus = ps2000_get_timebase(ps2000Handle, timebase, numSamples, timeInterval, timeUnits, oversample, maxSamples)

            If timebaseStatus = 0 Then

                timebase = timebase + 1

            End If

        Loop

        Console.WriteLine("Timebase: {0}, Time interval: {1}, Time Units: {2}, Max Samples: {3}", timebase, timeInterval, timeUnits, maxSamples, vbNewLine)


        ' Setup trigger
        ' -------------

        ' Setup a trigger such that the device will trigger on a rising edge through 500 mV on Channel A, auto trigger set to 5 seconds

        Dim threshold As Short = 0

        threshold = mvToAdc(500, channelARange, PS2000Imports.PS2000_MAX_VALUE)

        status = ps2000_set_trigger2(ps2000Handle, Channel.PS2000_CHANNEL_A, threshold, CShort(0), CSng(-50.0), 5000)


        ' Signal Generator
        ' ================

        ' Use in-built function generator in order to provide a test signal (1 kHz, 2 Vpp sine wave)

        Dim offsetVoltage As Integer = 0
        Dim peakToPeak As UInteger = 2000000 ' +/- 1 Volt
        Dim waveType As WaveType = WaveType.PS2000_SINE
        Dim startFrequency As Single = 1000.0 ' Hertz
        Dim stopFrequency As Single = 1000.0 ' Hertz
        Dim increment As Single = 0.0
        Dim dwellTime As Single = 0.0
        Dim sweepType As SweepType = SweepType.PS2000_UP
        Dim numSweeps As UInteger = 0

        Console.WriteLine("Connect the AWG output to Channel A and press <Enter> to continue.")

        Do While Console.ReadKey().Key <> ConsoleKey.Enter
        Loop

        status = ps2000_set_sig_gen_built_in(ps2000Handle, offsetVoltage, peakToPeak, waveType, startFrequency, stopFrequency, increment, dwellTime, sweepType, numSweeps)

        If status = 0 Then

            Call ErrorHandler(ps2000Handle, "ps2000_set_sig_gen_built_in")

        End If

        ' Data Capture
        ' ============

        ' Capture block of data from the device using a trigger.

        Dim timeIndisposedMs As Integer = 0
        Dim isReady As Short = 0

        Console.WriteLine("Collecting {0} samples...", numSamples, vbNewLine)
        Console.WriteLine("Press any key to cancel data collection.")


        ' Start device collecting data then wait for completion
        status = ps2000_run_block(ps2000Handle, numSamples, timebase, oversample, timeIndisposedMs)

        If status = 0 Then

            Call ErrorHandler(ps2000Handle, "ps2000_run_block")

        End If

        While isReady = 0 AndAlso Console.KeyAvailable = False

            isReady = ps2000_ready(ps2000Handle)
            Thread.Sleep(5)

        End While

        If Console.KeyAvailable Then

            ' Clear the key
            Console.ReadKey(True)

        End If

        Console.WriteLine()

        If isReady = 1 Then

            ' Set up buffers to collect time and channel data
            ReDim times(numSamples - 1)
            ReDim valuesChA(numSamples - 1)
            ReDim valuesChB(numSamples - 1)

            Dim overflow As Short
            Dim numSamplesCollected As Integer = 0

            ' Retrieve the number samples collected, time and channel data
            numSamplesCollected = ps2000_get_times_and_values(ps2000Handle, times(0), valuesChA(0), valuesChB(0), Nothing, Nothing, overflow, timeUnits, numSamples)

            If numSamplesCollected = 0 Then

                Console.WriteLine("Data not available.")
                Call ErrorHandler(ps2000Handle, "ps2000_get_times_and_values")

            Else

                Console.WriteLine("Collected {0} samples.", numSamplesCollected, vbNewLine)

            End If

            ' Process the Data
            ' ----------------

            ' In this example, the data is converted to millivolts and output to file

            Dim valuesChAMv() As Single
            ReDim valuesChAMv(numSamplesCollected - 1)

            Dim fileName As String
            fileName = "ps2000_triggered_block.txt"

            Console.WriteLine("Writing data to file {0}...", fileName, vbNewLine)

            Using outfile As New StreamWriter(fileName)

                outfile.WriteLine("Triggered Block Data")
                outfile.WriteLine("Data is output in millivolts.")
                outfile.WriteLine()
                outfile.Write("Time ({0}){1}", TimeUnitsToString(timeUnits), vbTab)
                outfile.Write("Channel A")
                outfile.WriteLine()

                For index = 0 To numSamplesCollected - 1

                    outfile.Write("{0}{1}", times(index).ToString, vbTab)

                    valuesChAMv(index) = PicoFunctions.adcToMv(valuesChA(index), channelARange, PS2000_MAX_VALUE) ' Use the voltage range specified in the call to ps2000_set_channel
                    outfile.Write(valuesChAMv(index).ToString)
                    outfile.WriteLine()

                Next

                outfile.Flush()
                outfile.Close()
                outfile.Dispose()

            End Using

            Console.WriteLine("Completed write to data file.")

        Else

            Console.WriteLine("Data collection cancelled.")

        End If

        ' Reset isReady flag if we need to collect another block of data
        isReady = 0

        ' Stop the device
        ps2000_stop(ps2000Handle)

        ' Close the connection to the device
        Call ps2000_close_unit(ps2000Handle)

        Console.WriteLine(vbNewLine)

        Console.WriteLine("Exiting application..." & vbNewLine)

        Thread.Sleep(5000)

    End Sub


    Private Sub ErrorHandler(ByVal handle As Short, ByVal functionName As String)

        Dim infoStr As String

        infoStr = "                    "
        ps2000_get_unit_info(handle, infoStr, CShort(infoStr.Length), Info.PS2000_ERROR_CODE)

        Console.WriteLine("Error: {0} was not called correctly.", functionName, vbNewLine)
        Call ps2000_close_unit(handle)
        Console.WriteLine("Exiting application...")

        Thread.Sleep(5000)

    End Sub

    Private Function TimeUnitsToString(ByVal timeUnits As TimeUnits) As String

        Select Case timeUnits

            Case PS2000Imports.TimeUnits.PS2000_FS
                Return "fs"

            Case PS2000Imports.TimeUnits.PS2000_PS
                Return "ps"

            Case PS2000Imports.TimeUnits.PS2000_NS
                Return "ns"

            Case PS2000Imports.TimeUnits.PS2000_US
                Return "us"

            Case PS2000Imports.TimeUnits.PS2000_MS
                Return "ms"

            Case PS2000Imports.TimeUnits.PS2000_S
                Return "s"

            Case Else

                Return ""

        End Select

    End Function

End Module

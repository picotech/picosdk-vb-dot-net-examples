'===================================================================================================
'
'	Filename:			PS2000BlockVBGui.vb
'
'	Description: 
'	This Windows Form Application demonstrates how to use VB .NET on the PicoScope 2000 Series 
'   oscilloscopes using the ps2000 driver.
'
'   It shows how to connect to a device, display device information, setup the signal generator and 
'   collect data in block mode.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Imports System.Threading

Public Class PS2000BlockVBGui

    ' Define variables

    Dim status As Short
    Dim ps2000Handle As Short

    Dim channelARange As VoltageRange = VoltageRange.PS2000_5V
    Dim channelBRange As VoltageRange = VoltageRange.PS2000_5V

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

        infoText(0) = "Driver Version:    "
        infoText(1) = "USB Version:       "
        infoText(2) = "Hardware Version:  "
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

                unitInfoTextBox.AppendText(infoText(i))
                unitInfoTextBox.AppendText(vbNewLine)

            End If

        Next i

        Console.WriteLine(vbNewLine)

    End Sub

    Private Sub OpenButton_Click(sender As Object, e As EventArgs) Handles openButton.Click

        ' Device Connection and Setup
        ' ===========================

        ' Open Device
        ps2000Handle = ps2000_open_unit()

        unitInfoTextBox.Clear()

        If ps2000Handle = 0 Then
            MessageBox.Show("Device not opened", "Open Device", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Display Unit Information
        getDeviceInfo(ps2000Handle)

        openButton.Enabled = False
        closeButton.Enabled = True
        getBlockDataButton.Enabled = True

        ' Turn off ETS
        Call ps2000_set_ets(ps2000Handle, 0, 0, 0) ' Turn ETS off

        ' Set Channels
        ' ------------

        status = ps2000_set_channel(ps2000Handle, Channel.PS2000_CHANNEL_A, CShort(1), CShort(1), channelARange) ' Channel A: enabled DC coupling 5V range
        status = ps2000_set_channel(ps2000Handle, Channel.PS2000_CHANNEL_B, CShort(0), CShort(1), channelBRange) ' Channel B: not enabled DC coupling 5V range

    End Sub

    Private Sub ErrorHandler(ByVal handle As Short, ByVal functionName As String)

        Dim infoStr As String
        Dim message As String
        Dim caption As String
        Dim buttons As MessageBoxButtons = MessageBoxButtons.OK
        Dim icon As MessageBoxIcon = MessageBoxIcon.Error

        infoStr = "                    "
        ps2000_get_unit_info(handle, infoStr, CShort(infoStr.Length), Info.PS2000_ERROR_CODE)

        message = String.Concat("Error: ", functionName, " was not called correctly.")
        caption = "Error Calling Function"

        MessageBox.Show(message, caption, buttons, icon)

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

    Private Sub GetBlockDataButton_Click(sender As Object, e As EventArgs) Handles getBlockDataButton.Click

        samplingIntervalTextBox.Clear()
        numSamplesCollectedTextBox.Clear()
        dataTextBox.Clear()

        ' Verify Timebase for Sampling Interval
        ' -------------------------------------

        'Find the maximum number of samples the time interval (in TimeUnits)
        'the most suitable time units and the maximum oversample at the current timebase

        Dim timebaseStatus As Short = 0
        Dim timebase As Short = CShort(timebaseNumericUpDown.Value)
        Dim numSamples As Integer = CInt(numSamplesNumericUpDown.Value)
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

        samplingIntervalTextBox.AppendText(String.Concat(timeInterval, " ", TimeUnitsToString(timeUnits)))

        ' Setup trigger
        ' -------------

        ' Setup a trigger such that the device will trigger on a rising edge through 500 mV on Channel A, auto trigger set to 5 seconds

        Dim threshold As Short = 0

        threshold = mvToAdc(500, channelARange, PS2000Imports.PS2000_MAX_VALUE)

        status = ps2000_set_trigger2(ps2000Handle, Channel.PS2000_CHANNEL_A, threshold, CShort(0), CSng(-50.0), 5000)

        ' Signal Generator
        ' ================

        ' Display message prompting user to connect signal generator to channel A

        Dim message As String = "Connect the AWG output to Channel A and click OK to continue."
        Dim caption As String = "Signal Generator"
        Dim button As MessageBoxButtons = MessageBoxButtons.OK
        Dim icon As MessageBoxIcon = MessageBoxIcon.None

        MessageBox.Show(message, caption, button, icon)

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

        status = ps2000_set_sig_gen_built_in(ps2000Handle, offsetVoltage, peakToPeak, waveType, startFrequency, stopFrequency, increment, dwellTime, sweepType, numSweeps)

        If status = 0 Then

            Call ErrorHandler(ps2000Handle, "ps2000_set_sig_gen_built_in")

        End If

        ' Data Capture
        ' ============

        ' Capture block of data from the device using a trigger.

        Dim timeIndisposedMs As Integer = 0
        Dim isReady As Short = 0

        ' Start device collecting data then wait for completion
        status = ps2000_run_block(ps2000Handle, numSamples, timebase, oversample, timeIndisposedMs)

        If status = 0 Then

            Call ErrorHandler(ps2000Handle, "ps2000_run_block")

        End If

        While isReady = 0

            isReady = ps2000_ready(ps2000Handle)
            Thread.Sleep(5)

        End While

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

                Call ErrorHandler(ps2000Handle, "ps2000_get_times_and_values")

            Else

                numSamplesCollectedTextBox.AppendText(numSamplesCollected)

            End If

            ' Process the Data
            ' ----------------

            ' In this example, the data is converted to millivolts and output to file

            Dim valuesChAMv() As Single
            ReDim valuesChAMv(numSamplesCollected - 1)

            dataTextBox.AppendText(String.Concat("Time (", TimeUnitsToString(timeUnits), ")", vbTab, vbTab, "Ch. A"))
            dataTextBox.AppendText(vbNewLine)

            For index = 0 To numSamplesCollected - 1

                valuesChAMv(index) = PicoFunctions.adcToMv(valuesChA(index), channelARange, PS2000_MAX_VALUE) ' Use the voltage range specified in the call to ps2000_set_channel

                dataTextBox.AppendText(String.Concat(times(index).ToString, vbTab, valuesChAMv(index).ToString))
                dataTextBox.AppendText(vbNewLine)

            Next



        Else

            MessageBox.Show("Data collection cancelled.", "Data Collection", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

        ' Reset isReady flag if we need to collect another block of data
        isReady = 0

        ' Stop the device
        ps2000_stop(ps2000Handle)

    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles closeButton.Click

        ' Close the connection to the device
        Call ps2000_close_unit(ps2000Handle)

        openButton.Enabled = True
        getBlockDataButton.Enabled = False

    End Sub

End Class

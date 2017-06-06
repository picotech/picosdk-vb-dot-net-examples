'===================================================================================================
'
'	Filename:			PS4000ABlockVBCon.vb
'
'	Description: 
'	This file demonstrates how to use VB .NET on the PicoScope 4000 series oscilloscopes using the 
'   ps4000a driver.
'   The application shows how to connect to a device, display device information and collect data in 
'   block mode.
'
'   Supported PicoScope models
'
'		PicoScope 4444
'		PicoScope 4824
'
'   Copyright © 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Imports System.IO
Imports System.Threading

Module PS4000ABlockVBCon

    ' Delegate

    Public ps4000aBlockCallback As ps4000aBlockReady

    ' Define variables

    Dim handle As Short
    Dim status As UInteger
    Dim serial As String
    Dim maxADCValue As Short
    Dim channelCount As Short
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
            ps4000aGetUnitInfo(handle, infoStr, CShort(infoStr.Length), requiredSize, i)
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
        Dim hasArbitraryWaveformGenerator As Boolean = False


        Console.WriteLine("PicoScope 4000 Series (ps4000a) VB .NET Block Capture Example Program" & vbNewLine _
                          & "=====================================================================" & vbNewLine)

        ' Device Connection and Setup
        ' ===========================

        ' Open device with 12-bit resolution (last parameter is ignored if the device has fixed resolution)
        status = ps4000aOpenUnitWithResolution(handle, vbNullString, PS4000AImports.DeviceResolution.PS4000A_DR_12BIT)

        If status = PicoStatus.PICO_OK Then

            Console.WriteLine("Device opened successfully:-" & vbNewLine)

        ElseIf status = PicoStatus.PICO_POWER_SUPPLY_NOT_CONNECTED Or status = PicoStatus.PICO_USB3_0_DEVICE_NON_USB3_0_PORT Then

            ' Change power status if USB powered
            status = ps4000aChangePowerSource(handle, status)

            If status = PicoStatus.PICO_OK Then

                Console.WriteLine("Device USB Powered" & vbNewLine)

            Else

                Console.WriteLine("Error changing power source - exiting application.")
                Thread.Sleep(2000)
                Exit Sub

            End If

        ElseIf status = PicoStatus.PICO_NOT_FOUND Then

            Console.WriteLine("Device not found - exiting application." & vbNewLine)
            Thread.Sleep(2000)
            Exit Sub

        Else

            Console.WriteLine("Device not opened - exiting application." & vbNewLine)
            Thread.Sleep(2000)
            Exit Sub

        End If

        ' Find maximum ADC count (resolution dependent) - this value is used for scaling values between ADC counts and millivolts
        status = ps4000aMaximumValue(handle, maxADCValue)

        'Read and display the device information
        getDeviceInfo(handle)

        ' Channel Setup
        ' -------------

        ' Setup Channel A - 5V range, DC coupling, 0 Analogue offset

        Dim chAProbeRange As PicoConnectProbeRange = PicoConnectProbeRange.PICO_X1_PROBE_5V
        status = ps4000aSetChannel(handle, PS4000AImports.Channel.PS4000A_CHANNEL_A, CShort(1), PS4000AImports.CouplingMode.PS4000A_DC, chAProbeRange, 0.0)

        If status <> PICO_OK Then

            Console.WriteLine("Error setting channel A")
            Call ErrorHandler(handle, status, "ps4000aSetChannel")
            Exit Sub

        End If

        ' Turn off other channels

        ' Turn off Channel B
        status = ps4000aSetChannel(handle, PS4000AImports.Channel.PS4000A_CHANNEL_B, CShort(0), PS4000AImports.CouplingMode.PS4000A_DC, PicoConnectProbeRange.PICO_X1_PROBE_5V, 0.0)

        If status <> PICO_OK Then

            Console.WriteLine("Error setting channel B")
            Call ErrorHandler(handle, status, "ps4000aSetChannel")
            Exit Sub

        End If

        If channelCount = 4 Or channelCount = 8 Then

            status = ps4000aSetChannel(handle, PS4000AImports.Channel.PS4000A_CHANNEL_C, CShort(0), PS4000AImports.CouplingMode.PS4000A_DC, PicoConnectProbeRange.PICO_X1_PROBE_5V, 0.0)

            If status <> PICO_OK Then

                Console.WriteLine("Error setting channel C")
                Call ErrorHandler(handle, status, "ps4000aSetChannel")
                Exit Sub

            End If

            status = ps4000aSetChannel(handle, PS4000AImports.Channel.PS4000A_CHANNEL_D, CShort(0), PS4000AImports.CouplingMode.PS4000A_DC, PicoConnectProbeRange.PICO_X1_PROBE_5V, 0.0)

            If status <> PICO_OK Then

                Console.WriteLine("Error setting channel D")
                Call ErrorHandler(handle, status, "ps4000aSetChannel")
                Exit Sub

            End If

        End If

        If channelCount = 8 Then

            status = ps4000aSetChannel(handle, PS4000AImports.Channel.PS4000A_CHANNEL_E, CShort(0), PS4000AImports.CouplingMode.PS4000A_DC, PicoConnectProbeRange.PICO_X1_PROBE_5V, 0.0)

            If status <> PICO_OK Then

                Console.WriteLine("Error setting channel E")
                Call ErrorHandler(handle, status, "ps4000aSetChannel")
                Exit Sub

            End If

            status = ps4000aSetChannel(handle, PS4000AImports.Channel.PS4000A_CHANNEL_F, CShort(0), PS4000AImports.CouplingMode.PS4000A_DC, PicoConnectProbeRange.PICO_X1_PROBE_5V, 0.0)

            If status <> PICO_OK Then

                Console.WriteLine("Error setting channel F")
                Call ErrorHandler(handle, status, "ps4000aSetChannel")
                Exit Sub

            End If

            status = ps4000aSetChannel(handle, PS4000AImports.Channel.PS4000A_CHANNEL_G, CShort(0), PS4000AImports.CouplingMode.PS4000A_DC, PicoConnectProbeRange.PICO_X1_PROBE_5V, 0.0)

            If status <> PICO_OK Then

                Console.WriteLine("Error setting channel G")
                Call ErrorHandler(handle, status, "ps4000aSetChannel")
                Exit Sub

            End If

            status = ps4000aSetChannel(handle, PS4000AImports.Channel.PS4000A_CHANNEL_H, CShort(0), PS4000AImports.CouplingMode.PS4000A_DC, PicoConnectProbeRange.PICO_X1_PROBE_5V, 0.0)


            If status <> PICO_OK Then

                Console.WriteLine("Error setting channel H")
                Call ErrorHandler(handle, status, "ps4000aSetChannel")
                Exit Sub

            End If

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

        timebase = 79
        numPreTriggerSamples = 0
        numPostTriggerSamples = 10000
        totalSamples = numPreTriggerSamples + numPostTriggerSamples

        timeIntervalNs = CSng(0.0)
        maxSamples = 0
        segmentIndex = 0
        getTimebase2Status = PicoStatus.PICO_INVALID_TIMEBASE ' Initialise as invalid timebase


        Do Until getTimebase2Status = PicoStatus.PICO_OK

            getTimebase2Status = ps4000aGetTimebase2(handle, timebase, totalSamples, timeIntervalNs, maxSamples, segmentIndex)

            If getTimebase2Status <> PicoStatus.PICO_OK Then

                timebase = timebase + 1

            End If

        Loop

        Console.WriteLine("Timebase: {0} Sample interval: {1} ns, Max Samples: {2}", timebase, timeIntervalNs, maxSamples, vbNewLine)

        ' Setup trigger
        ' -------------

        ' Setup a trigger such that the device will trigger on a rising edge through 500 mV on Channel A

        Dim threshold As Short
        Dim delay As UInteger
        Dim autoTriggerMs As Short

        ' Convert the threshold from millivolts to an ADC count
        threshold = PicoFunctions.mvToAdc(500, chAProbeRange, maxADCValue)

        Console.WriteLine("Trigger threshold: {0} mV ({1} ADC counts)", 500, threshold, vbNewLine)

        delay = 0
        autoTriggerMs = 1000 ' Auto-trigger after 1 second if trigger event has not occurred

        status = ps4000aSetSimpleTrigger(handle, CShort(1), PS4000AImports.Channel.PS4000A_CHANNEL_A, threshold, PS4000AImports.ThresholdDirection.PS4000A_RISING, delay, autoTriggerMs)

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps4000aSetSimpleTrigger")
            Exit Sub

        End If


        ' Set signal generator (if available on device)
        ' ---------------------------------------------

        Dim minArbWaveformValue As Short = 0
        Dim maxArbWaveformValue As Short = 0
        Dim minArbWaveformSize As UInteger = 0
        Dim maxArbWaveformSize As UInteger = 0

        status = ps4000aSigGenArbitraryMinMaxValues(handle, minArbWaveformValue, maxArbWaveformValue, minArbWaveformSize, maxArbWaveformSize)

        If maxArbWaveformSize > 0 Then

            hasArbitraryWaveformGenerator = True

            ' Use in-built function generator in order to provide a test signal (1 kHz, 2 Vpp sine wave)

            Console.WriteLine("Connect the AWG output to Channel A and press <Enter> to continue.")

            Do While Console.ReadKey().Key <> ConsoleKey.Enter
            Loop

            status = ps4000aSetSigGenBuiltIn(handle, 0, CUInt(2000000), WaveType.PS4000A_SINE, 1000.0, 1000.0, 0.0, 0.0, SweepType.PS4000A_UP, ExtraOperations.PS4000A_ES_OFF,
                                               CUInt(0), CUInt(0), SigGenTrigType.PS4000A_SIGGEN_RISING, SigGenTrigSource.PS4000A_SIGGEN_NONE, CShort(0))

            If status <> PICO_OK Then

                Call ErrorHandler(handle, status, "ps4000aSetSigGenBuiltIn")
                Exit Sub

            End If

        End If


        ' Data Capture
        ' ============

        ' Capture block of data from the device using a trigger.

        ' Create instance of delegate (callback function)
        ps4000aBlockCallback = New ps4000aBlockReady(AddressOf BlockCallback)

        Dim timeIndisposedMs As Integer = 0

        Console.WriteLine("Collecting {0} samples...", (numPreTriggerSamples + numPostTriggerSamples), vbNewLine)

        status = ps4000aRunBlock(handle, numPreTriggerSamples, numPostTriggerSamples, timebase, timeIndisposedMs, segmentIndex, ps4000aBlockCallback, IntPtr.Zero)

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps4000aRunBlock")
            Exit Sub

        End If

        While deviceReady = False AndAlso Console.KeyAvailable = False

            Thread.Sleep(10)

        End While

        ' Setup data buffers to retrieve data values - this can also be carried out prior to data collection

        Dim valuesChA() As Short
        ReDim valuesChA(totalSamples - 1)

        status = ps4000aSetDataBuffer(handle, PS4000AImports.Channel.PS4000A_CHANNEL_A, valuesChA(0), CInt(totalSamples), segmentIndex, PS4000AImports.RatioMode.PS4000A_RATIO_MODE_NONE)

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps4000aSetDataBuffer")
            Exit Sub

        End If

        ' Retrieve values

        Dim startIndex As UInteger
        Dim downSampleRatio As UInteger
        Dim overflow As Short

        startIndex = 0
        downSampleRatio = 1
        overflow = 0

        status = ps4000aGetValues(handle, startIndex, totalSamples, downSampleRatio, PS4000AImports.RatioMode.PS4000A_RATIO_MODE_NONE, segmentIndex, overflow)

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps4000aGetValues")
            Exit Sub

        End If

        Console.WriteLine("Retrieved {0} samples.", totalSamples, vbNewLine)


        ' Process the Data
        ' ----------------

        ' In this example, the data is converted to millivolts and output to file

        Dim valuesChAMv() As Single
        ReDim valuesChAMv(totalSamples - 1)

        Dim fileName As String
        fileName = "ps4000a_triggered_block.txt"

        Using outfile As New StreamWriter(fileName)

            outfile.WriteLine("Triggered Block Data")
            outfile.WriteLine("Data is output in millivolts.")
            outfile.WriteLine()
            outfile.WriteLine("Channel A")
            outfile.WriteLine()

            For index = 0 To totalSamples - 1

                valuesChAMv(index) = PicoFunctions.adcToMv(valuesChA(index), chAProbeRange, maxADCValue)
                outfile.Write(valuesChAMv(index).ToString)
                outfile.WriteLine()

            Next

        End Using

        Console.WriteLine("Data written to file {0}", fileName)

        ' Reset deviceReady flag if we need to collect another block of data
        deviceReady = False

        ' Stop the device
        status = ps4000aStop(handle)

        ' Close the connection to the device
        ps4000aCloseUnit(handle)

        Console.WriteLine(vbNewLine)

        Console.WriteLine("Exiting application..." & vbNewLine)

        Thread.Sleep(5000)

    End Sub

    ' Block callback function

    Public Sub BlockCallback(handle As Short, status As UInteger, pVoid As IntPtr)
        ' flag to say done reading data
        If status <> PicoStatus.PICO_CANCELLED Then
            deviceReady = True
        End If
    End Sub

    Private Sub ErrorHandler(ByVal handle As Short, ByVal status As UInteger, ByVal functionName As String)

        Console.WriteLine("Error: {0} returned with status code {1} (0x{2})", functionName, status, Hex(status))
        Call ps4000aCloseUnit(handle)
        Console.WriteLine("Exiting application...")

        Thread.Sleep(5000)

    End Sub

End Module

'========================================================================================
'	
'   Filename: PS6000BlockVBCon.vb
'
'	Description:
'	    This file demonstrates how to use VB .NET on the PicoScope 6000 Series 
'       oscilloscopes the ps6000 driver.
'       The application shows how to connect to a device, display device information 
'       and collect data in block mode.
'
'   Copyright © 2014-2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Imports System.IO
Imports System.Threading

Module PS6000BlockVBCon

    ' Delegate

    Public ps6000BlockCallback As ps6000BlockReady

    ' Define variables

    Dim handle As Short = 0
    Dim status As UInteger = PICO_OK
    Dim totalSamples As UInteger = 0

    Dim isDigitiser As Boolean = False
    Public deviceReady As Boolean = False

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

            ' Identify if the PicoScope is a digitiser (PicoScope 6407)
            If i = PicoInfo.PICO_VARIANT_INFO Then

                If String.Equals(str, "6407") Then

                    isDigitiser = True

                End If

            End If

        Next i

    End Sub

    ' ******************************************************************************************************************************************************************
    ' Main -        Entry point to the application
    '
    '
    ' *******************************************************************************************************************************************************************
    Sub Main()

        Dim status As UInteger
        Dim handle As Short
        Dim ch As ConsoleKeyInfo

        Console.WriteLine("PicoScope 6000 Series (ps6000) VB .NET Example Program" _
                          & vbNewLine & "======================================================" & vbNewLine)

        ' Device Connection and Setup
        ' ===========================

        Dim serial As String = vbNullString

        status = ps6000OpenUnit(handle, serial)

        If handle = 0 Then
            Console.WriteLine("Unit not opened")
            ch = Console.ReadKey(True)
            Exit Sub
        End If

        'Read and display the device information
        getDeviceInfo(handle) 'Read and display the device information

        Console.WriteLine()

        ' Channel Setup
        ' -------------

        ' Set channels - voltage range will vary if the device is a PicoScope 6407

        Dim voltageRange As VoltageRange = VoltageRange.PS6000_5V

        If isDigitiser = True Then

            voltageRange = VoltageRange.PS6000_100MV

        End If

        ' Setup Channel A - DC coupling, 0 V Analogue offset, Full bandwidth

        status = ps6000SetChannel(handle, Channel.PS6000_CHANNEL_A, CShort(1), Coupling.PS6000_DC_1M, voltageRange, CSng(0.0), BandwidthLimiter.PS6000_BW_FULL)

        If status <> PICO_OK Then

            Console.WriteLine("Error setting channel A")
            Call ErrorHandler(handle, status, "ps6000SetChannel")
            Exit Sub

        End If

        ' Turn off the other channels

        status = ps6000SetChannel(handle, Channel.PS6000_CHANNEL_B, CShort(0), Coupling.PS6000_DC_1M, voltageRange, CSng(0.0), BandwidthLimiter.PS6000_BW_FULL)

        If status <> PICO_OK Then

            Console.WriteLine("Error setting channel B")
            Call ErrorHandler(handle, status, "ps6000SetChannel")
            Exit Sub

        End If

        status = ps6000SetChannel(handle, Channel.PS6000_CHANNEL_C, CShort(0), Coupling.PS6000_DC_1M, voltageRange, CSng(0.0), BandwidthLimiter.PS6000_BW_FULL)

        If status <> PICO_OK Then

            Console.WriteLine("Error setting channel C")
            Call ErrorHandler(handle, status, "ps6000SetChannel")
            Exit Sub

        End If

        status = ps6000SetChannel(handle, Channel.PS6000_CHANNEL_D, CShort(0), Coupling.PS6000_DC_1M, voltageRange, CSng(0.0), BandwidthLimiter.PS6000_BW_FULL)

        If status <> PICO_OK Then

            Console.WriteLine("Error setting channel D")
            Call ErrorHandler(handle, status, "ps6000SetChannel")
            Exit Sub

        End If

        ' Verify Timebase for sampling interval and maximum number of samples
        ' -------------------------------------------------------------------

        ' Confirm timebase corresponding to desired sampling interval is valid.
        ' Timebase indices can be calculated using the formulae in the Timebases section of the Programmer's Guide

        Dim timebase As UInteger
        Dim numPreTriggerSamples As UInteger
        Dim numPostTriggerSamples As UInteger
        Dim timeIntervalNs As Single
        Dim maxSamples As UInteger
        Dim segmentIndex As UInteger
        Dim getTimebase2Status As UInteger

        timebase = 161
        numPreTriggerSamples = 0
        numPostTriggerSamples = 100000
        totalSamples = numPreTriggerSamples + numPostTriggerSamples

        timeIntervalNs = CSng(0.0)
        maxSamples = 0
        segmentIndex = 0
        getTimebase2Status = PicoStatus.PICO_INVALID_TIMEBASE ' Initialise as invalid timebase


        Do Until getTimebase2Status = PicoStatus.PICO_OK

            getTimebase2Status = ps6000GetTimebase2(handle, timebase, totalSamples, timeIntervalNs, CShort(1), maxSamples, segmentIndex)

            If getTimebase2Status <> PicoStatus.PICO_OK Then

                timebase = timebase + 1

            End If

        Loop

        Console.WriteLine("Timebase: {0} Sample interval: {1} ns, Max Samples: {2}", timebase, timeIntervalNs, maxSamples, vbNewLine)

        ' Setup trigger
        ' -------------

        ' Setup a trigger such that the device will trigger on a rising edge through 500 mV on Channel A

        Dim thresholdVoltage As Short = 500
        Dim threshold As Short
        Dim delay As UInteger
        Dim autoTriggerMs As Short

        ' Adjust the threshold if the device is a PicoScope 6407
        If isDigitiser = True Then

            thresholdVoltage = 50

        End If

        ' Convert the threshold from millivolts to an ADC count
        threshold = PicoFunctions.mvToAdc(thresholdVoltage, voltageRange, PS6000_MAX_VALUE)

        Console.WriteLine("Trigger threshold: {0} mV ({1} ADC counts)", thresholdVoltage, threshold, vbNewLine)
        Console.WriteLine()

        delay = 0
        autoTriggerMs = 2000 ' Auto-trigger after 2 seconds if trigger event has not occurred

        status = ps6000SetSimpleTrigger(handle, CShort(1), Channel.PS6000_CHANNEL_A, threshold, TriggerDirection.RISING, delay, autoTriggerMs)

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps6000SetSimpleTrigger")
            Exit Sub

        End If

        ' Signal Generator
        ' ================

        ' Use in-built function generator in order to provide a test signal (50 Hz, 2 Vpp sine wave)

        Console.WriteLine("Connect the Sig Gen output to Channel A and press <Enter> to continue.")

        Do While Console.ReadKey().Key <> ConsoleKey.Enter
        Loop

        status = ps6000SetSigGenBuiltInV2(handle, 0, CUInt(2000000), WaveType.PS6000_SINE, 50.0, 50.0, 0.0, 0.0, SweepType.PS6000_UP, ExtraOperations.PS6000_ES_OFF,
                                           CUInt(0), CUInt(0), SigGenTrigType.PS6000_SIGGEN_RISING, SigGenTrigSource.PS6000_SIGGEN_NONE, CShort(0))

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps6000SetSigGenBuiltInV2")
            Exit Sub

        End If

        ' Data Capture
        ' ============

        ' Capture block of data from the device using a trigger.

        ' Create instance of delegate (callback function)
        ps6000BlockCallback = New ps6000BlockReady(AddressOf BlockCallback)

        Dim timeIndisposedMs As Integer = 0

        Console.WriteLine("Collecting {0} samples...", (numPreTriggerSamples + numPostTriggerSamples), vbNewLine)
        Console.WriteLine("Press any key to cancel data collection.")

        status = ps6000RunBlock(handle, numPreTriggerSamples, numPostTriggerSamples, timebase, CShort(1), timeIndisposedMs, segmentIndex, ps6000BlockCallback, IntPtr.Zero)

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps6000RunBlock")
            Exit Sub

        End If

        ' Wait for the device to become ready (i.e. complete data collection)
        While deviceReady = False AndAlso Console.KeyAvailable = False

            Threading.Thread.Sleep(50)

        End While

        If Console.KeyAvailable Then

            ' Clear the key
            Console.ReadKey(True)

        End If

        Console.WriteLine()

        If deviceReady = True Then

            ' Setup data buffers to retrieve data values - this can also be carried out prior to data collection

            Dim valuesChA() As Short
            ReDim valuesChA(totalSamples - 1)

            status = ps6000SetDataBuffer(handle, Channel.PS6000_CHANNEL_A, valuesChA(0), CInt(totalSamples), RatioMode.PS6000_RATIO_MODE_NONE)

            ' Retrieve values

            Dim startIndex As UInteger
            Dim downSampleRatio As UInteger
            Dim overflow As Short

            startIndex = 0
            downSampleRatio = 1
            overflow = 0

            status = ps6000GetValues(handle, startIndex, totalSamples, downSampleRatio, RatioMode.PS6000_RATIO_MODE_NONE, segmentIndex, overflow)

            If status <> PICO_OK Then

                Call ErrorHandler(handle, status, "ps6000GetValues")
                Exit Sub

            End If

            Console.WriteLine("Retrieved {0} samples.", totalSamples, vbNewLine)


            ' Process the Data
            ' ----------------

            ' In this example, the data is converted to millivolts and output to file

            Dim valuesChAMv() As Single
            ReDim valuesChAMv(totalSamples - 1)

            Dim fileName As String
            fileName = "ps6000_triggered_block.txt"

            Using outfile As New StreamWriter(fileName)

                outfile.WriteLine("Triggered Block Data")
                outfile.WriteLine("Data is output in millivolts.")
                outfile.WriteLine()
                outfile.WriteLine("Channel A")
                outfile.WriteLine()

                For index = 0 To totalSamples - 1

                    valuesChAMv(index) = PicoFunctions.adcToMv(valuesChA(index), voltageRange, PS6000_MAX_VALUE) ' Use the voltage range specified in the call to ps6000SetChannel
                    outfile.Write(valuesChAMv(index).ToString)
                    outfile.WriteLine()

                Next

            End Using

            Console.WriteLine("Data written to file {0}", fileName)

        Else

            Console.WriteLine("Data collection cancelled.")

        End If

        ' Reset deviceReady flag if we need to collect another block of data
        deviceReady = False

        ' Stop the device
        status = ps6000Stop(handle)

        Console.WriteLine(vbNewLine)

        ' Close the unit at the end
        ps6000CloseUnit(handle)

        Console.WriteLine("Exiting application..." & vbNewLine)
        Thread.Sleep(5000)

    End Sub

    ' Block callback function

    Private Sub BlockCallback(ByVal handle As Short, ByVal status As UInteger, pVoid As IntPtr)

        ' Flag to say done reading data
        If status <> PicoStatus.PICO_CANCELLED Then
            deviceReady = True
        End If

    End Sub

    Private Sub ErrorHandler(ByVal handle As Short, ByVal status As UInteger, ByVal functionName As String)

        Console.WriteLine("Error: {0} returned with status code {1} (0x{2})", functionName, status, Hex(status))
        Call ps6000CloseUnit(handle)
        Console.WriteLine("Exiting application...")

        Thread.Sleep(5000)

    End Sub

End Module

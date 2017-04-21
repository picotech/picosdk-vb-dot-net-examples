'===================================================================================================
'
'	Filename:			PS2000ABlockVBCon.vb
'
'	Description: 
'	    This file demonstrates how to use VB .NET on the PicoScope 2000 series oscilloscopes using 
'       the ps2000a driver.
'       The application shows how to connect to a device, display device information and collect 
'       data in block mode.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'===================================================================================================

Imports System.IO

Module PS2000ABlockVBCon

    ' Delegate
    Public ps2000aBlockCallback As ps2000aBlockReady

    ' Define variables

    Dim handle As Short
    Dim status As UInteger
    Dim serial As String
    Dim maxADCValue As Short
    Dim channelCount As Short
    Dim totalSamples As UInteger

    Public deviceReady As Boolean = False
    Public isMSODevice As Boolean = False

    ' ******************************************************************************************************************************************************************
    ' GetDeviceInfo - Reads and displays the scopes device information.
    '
    ' Parameters: handle - the device handle
    ' *******************************************************************************************************************************************************************
    Sub getDeviceInfo(ByVal handle As Short)

        Dim infoText(8) As String
        Dim infoStr As String
        Dim infoStr2 As String
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
            ps2000aGetUnitInfo(handle, infoStr, CShort(infoStr.Length), requiredSize, i)
            infoStr2 = infoStr.TrimEnd(" ")
            infoText(i) += infoStr
            Console.WriteLine(infoText(i) & vbTab)

            If i = PicoInfo.PICO_VARIANT_INFO Then

                channelCount = Convert.ToInt16(infoStr2.ToCharArray(1, 1))

                If infoStr2.EndsWith("MSO", StringComparison.CurrentCultureIgnoreCase) Then

                    isMSODevice = True

                Else

                    isMSODevice = False

                End If

            End If

        Next i

        Console.WriteLine(vbNewLine)

    End Sub



    Sub Main()

        Console.WriteLine("PicoScope 2000 Series (ps2000a) VB .NET Block Capture Example Program" _
                          & vbNewLine & "=====================================================================" & vbNewLine)

        ' Device Connection and Setup
        ' ===========================

        ' Open device 
        status = ps2000aOpenUnit(handle, vbNullString)

        If status = PicoStatus.PICO_OK Then

            Console.WriteLine("Device opened successfully:-" & vbNewLine)

        ElseIf status = PicoStatus.PICO_NOT_FOUND Then

            Console.WriteLine("Device not found." & vbNewLine)
            Sleep(2000)
            Exit Sub

        Else

            Console.WriteLine("Device not opened." & vbNewLine)
            Sleep(2000)
            Exit Sub

        End If

        ' Find maximum ADC count this value is used for scaling values between ADC counts and millivolts
        status = ps2000aMaximumValue(handle, maxADCValue)

        'Read and display the device information
        getDeviceInfo(handle)

        ' Channel Setup
        ' -------------

        ' Setup Channel A - 5V range, DC coupling, 0 Analogue offset

        status = ps2000aSetChannel(handle, PS2000AImports.Channel.PS2000A_CHANNEL_A, CShort(1), PS2000AImports.CouplingMode.PS2000A_DC, PS2000AImports.VoltageRange.PS2000A_5V, 0.0)

        If status <> PICO_OK Then

            Console.WriteLine("Error setting channel A")
            Call ErrorHandler(handle, status, "ps2000aSetChannel")
            Exit Sub

        End If

        ' Turn off other channels

        ' Turn off Channel B
        status = ps2000aSetChannel(handle, PS2000AImports.Channel.PS2000A_CHANNEL_B, CShort(0), PS2000AImports.CouplingMode.PS2000A_DC, PS2000AImports.VoltageRange.PS2000A_5V, 0.0)

        If status <> PICO_OK Then

            Console.WriteLine("Error setting channel B")
            Call ErrorHandler(handle, status, "ps2000aSetChannel")
            Exit Sub

        End If

        ' Only disable Channels C and D if it is a 4-channel device 

        If channelCount = 4 Then

            status = ps2000aSetChannel(handle, PS2000AImports.Channel.PS2000A_CHANNEL_C, CShort(0), PS2000AImports.CouplingMode.PS2000A_DC, PS2000AImports.VoltageRange.PS2000A_5V, 0.0)

            If status <> PICO_OK Then

                Console.WriteLine("Error setting channel C")
                Call ErrorHandler(handle, status, "ps2000aSetChannel")
                Exit Sub

            End If

            status = ps2000aSetChannel(handle, PS2000AImports.Channel.PS2000A_CHANNEL_D, CShort(0), PS2000AImports.CouplingMode.PS2000A_DC, PS2000AImports.VoltageRange.PS2000A_5V, 0.0)

            If status <> PICO_OK Then

                Console.WriteLine("Error setting channel D")
                Call ErrorHandler(handle, status, "ps2000aSetChannel")
                Exit Sub

            End If

        End If

        ' If the device is a Mixed Signal Oscilloscope, turn off any Digital Ports

        If isMSODevice = True Then

            status = ps2000aSetDigitalPort(handle, DigitalPort.PS2000A_DIGITAL_PORT0, CShort(0), CShort(0))

            If status <> PICO_OK Then

                Console.WriteLine("Error setting Digital PORT0")
                Call ErrorHandler(handle, status, "ps2000aSetChannel")
                Exit Sub

            End If

            status = ps2000aSetDigitalPort(handle, DigitalPort.PS2000A_DIGITAL_PORT1, CShort(0), CShort(0))

            If status <> PICO_OK Then

                Console.WriteLine("Error setting Digital PORT1")
                Call ErrorHandler(handle, status, "ps2000aSetChannel")
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
        Dim timeIntervalNs As Integer
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

            getTimebase2Status = ps2000aGetTimebase2(handle, timebase, totalSamples, timeIntervalNs, CShort(1), maxSamples, segmentIndex)

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
        threshold = PicoFunctions.mvToAdc(500, PS2000AImports.VoltageRange.PS2000A_5V, maxADCValue)

        Console.WriteLine("Trigger threshold: {0} mV ({1} ADC Counts)", 500, threshold, vbNewLine)

        delay = 0
        autoTriggerMs = 2000 ' Auto-trigger after 2 seconds if trigger event has not occurred

        status = ps2000aSetSimpleTrigger(handle, CShort(1), PS2000AImports.Channel.PS2000A_CHANNEL_A, threshold, PS2000AImports.ThresholdDirection.PS2000A_RISING, delay, autoTriggerMs)

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps2000aSetSimpleTrigger")
            Exit Sub

        End If

        ' Signal Generator
        ' ================

        ' Use in-built function generator in order to provide a test signal (1 KHz, 2 Vpp sine wave)

        Console.WriteLine("Connect the AWG output to Channel A and press <Enter> to continue.")

        Do While Console.ReadKey().Key <> ConsoleKey.Enter
        Loop

        status = ps2000aSetSigGenBuiltInV2(handle, 0, CUInt(2000000), WaveType.PS2000A_SINE, 1000.0, 1000.0, 0.0, 0.0, SweepType.PS2000A_UP, ExtraOperations.PS2000A_ES_OFF, _
                                           CUInt(0), CUInt(0), SigGenTrigType.PS2000A_SIGGEN_RISING, SigGenTrigSource.PS2000A_SIGGEN_NONE, CShort(0))

        If status <> PICO_OK Then

            Call ErrorHandler(handle, status, "ps2000aSetSigGenBuiltInV2")
            Exit Sub

        End If

        ' Data Capture
        ' ============

        ' Capture block of data from the device using a trigger.

        ' Create instance of delegate (callback function)
        ps2000aBlockCallback = New ps2000aBlockReady(AddressOf BlockCallback)

        Dim timeIndisposedMs As Integer = 0

        Console.WriteLine("Collecting {0} samples...", (numPreTriggerSamples + numPostTriggerSamples), vbNewLine)
        Console.WriteLine("Press any key to cancel data collection.")

        status = ps2000aRunBlock(handle, numPreTriggerSamples, numPostTriggerSamples, timebase, CShort(1), timeIndisposedMs, segmentIndex, ps2000aBlockCallback, IntPtr.Zero)

        If status <> PICO_OK Then
            
            Call ErrorHandler(handle, status, "ps2000aRunBlock")
            Exit Sub

        End If

        Dim ready As Short = 0

        ' Wait for the device to become ready (i.e. complete data collection)
        While deviceReady = False AndAlso Console.KeyAvailable = False

            Threading.Thread.Sleep(50)

        End While

        If Console.KeyAvailable Then

                ' clear the key
                Console.ReadKey(True)

            End If

            Console.WriteLine()

        If deviceReady = True Then

            ' Setup data buffers to retrieve data values - this can also be carried out prior to data collection

            Dim valuesChA() As Short
            ReDim valuesChA(totalSamples - 1)

            status = ps2000aSetDataBuffer(handle, PS2000AImports.Channel.PS2000A_CHANNEL_A, valuesChA(0), CInt(totalSamples), segmentIndex, PS2000AImports.RatioMode.PS2000A_RATIO_MODE_NONE)

            ' Retrieve values

            Dim startIndex As UInteger
            Dim downSampleRatio As UInteger
            Dim overflow As Short

            startIndex = 0
            downSampleRatio = 1
            overflow = 0

            status = ps2000aGetValues(handle, startIndex, totalSamples, downSampleRatio, PS2000AImports.RatioMode.PS2000A_RATIO_MODE_NONE, segmentIndex, overflow)

            If status <> PICO_OK Then

                Call ErrorHandler(handle, status, "ps2000aGetValues")
                Exit Sub

            End If

            Console.WriteLine("Retrieved {0} samples.", totalSamples, vbNewLine)


            ' Process the Data
            ' ----------------

            ' In this example, the data is converted to millivolts and output to file

            Dim valuesChAMv() As Single
            ReDim valuesChAMv(totalSamples - 1)

            Dim fileName As String
            fileName = "ps2000a_triggered_block.txt"

            Using outfile As New StreamWriter(fileName)

                outfile.WriteLine("Triggered Block Data")
                outfile.WriteLine("Data is output in millivolts.")
                outfile.WriteLine()
                outfile.WriteLine("Channel A")
                outfile.WriteLine()

                For index = 0 To totalSamples - 1

                    valuesChAMv(index) = PicoFunctions.adcToMv(valuesChA(index), PS2000AImports.VoltageRange.PS2000A_5V, maxADCValue) ' Use the voltage range specified in the call to ps2000aSetChannel
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
        status = ps2000aStop(handle)

        ' Close the connection to the device
        ps2000aCloseUnit(handle)

        Console.WriteLine(vbNewLine)

        Console.WriteLine("Exiting application..." & vbNewLine)

        Sleep(5000)

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
        Call ps2000aCloseUnit(handle)
        Console.WriteLine("Exiting application...")

        Sleep(5000)
    End Sub

End Module

'========================================================================================
'	
'   Filename: PicoHRDLVBCon.vb
'
'	Description:
'	This file demonstrates how to use VB .NET with the PicoLog High Resolution Data 
'   Loggers. It shows the use of API call functions to collect enable a digital output 
'   and collect data.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Imports System.Threading

Module PicoHRDLVBCon

    Sub Main()

        Dim status As Short
        Dim picologHRDLHandle As Short
        Dim info As String

        Dim hasDigitalIO As Boolean = False
        Dim digitalInputEnabled As Boolean = False
        Dim keyPress As Boolean = False

        Dim ready As Integer = 0

        Console.WriteLine("PicoLog High Resolution Data Logger VB.NET Console application" & vbNewLine)

        Console.WriteLine("Opening connection to PicoLog High Resolution Data Logger..." & vbNewLine)

        ' Open the PicoLog High Resolution Data Logger device on USB port
        picologHRDLHandle = HRDLOpenUnit()

        If (picologHRDLHandle > 0) Then

            Console.WriteLine("PicoLog High Resolution Data Logger connected." & vbNewLine)

            info = "                    "

            'Output info

            Call HRDLGetUnitInfo(picologHRDLHandle, info, 20, HRDLInfo.HRDL_DRIVER_VERSION)
            Console.WriteLine("Driver Version       : " & info)
            info = "                    "

            Call HRDLGetUnitInfo(picologHRDLHandle, info, 20, HRDLInfo.HRDL_USB_VERSION)
            Console.WriteLine("USB Version          : " & info)
            info = "                    "

            Call HRDLGetUnitInfo(picologHRDLHandle, info, 20, HRDLInfo.HRDL_HARDWARE_VERSION)
            Console.WriteLine("Hardware Version     : " & info)
            info = "                    "

            Call HRDLGetUnitInfo(picologHRDLHandle, info, 20, HRDLInfo.HRDL_VARIANT_INFO)
            Console.WriteLine("Variant              : ADC-" & info)
            info = "                    "

            Call HRDLGetUnitInfo(picologHRDLHandle, info, 20, HRDLInfo.HRDL_BATCH_AND_SERIAL)
            Console.WriteLine("Batch/Serial         : " & info)
            info = "                    "

            Call HRDLGetUnitInfo(picologHRDLHandle, info, 20, HRDLInfo.HRDL_CAL_DATE)
            Console.WriteLine("Cal. Date            : " & info)
            info = "                    "

            Call HRDLGetUnitInfo(picologHRDLHandle, info, 20, HRDLInfo.HRDL_KERNEL_DRIVER_VERSION)
            Console.WriteLine("Kernel driver version: " & info)
            info = "                    "

            Console.WriteLine()
            Console.WriteLine("Setting up mains rejection..." & vbNewLine)

            status = HRDLSetMains(picologHRDLHandle, MainsRejection.HRDL_FIFTY_HERTZ) ' Reject 

            ' Set noise rejection

            Console.WriteLine("Setting up channels..." & vbNewLine)

            ' Disable all channels
            For i = 0 To 16

                Call HRDLSetAnalogInChannel(picologHRDLHandle, i, 0, 0, 0)

            Next i

            ' Set channel one to:
            '  enabled
            '  voltage range 2500 mV
            '  single ended

            status = HRDLSetAnalogInChannel(picologHRDLHandle, HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1, 1, HRDLRange.HRDL_2500_MV, 1)

            ' Find number of enabled analog channels

            Dim numberOfEnabledChannels As Short = 0

            status = HRDLGetNumberOfEnabledChannels(picologHRDLHandle, numberOfEnabledChannels)


            ' Set Digital Input 1 (ADC-24 only)

            Dim setDigitalIOStatus As Short = 0
            Dim directionOut As Short = CShort(HRDLDigitalIOChannel.HRDL_DIGITAL_IO_CHANNEL_2 + HRDLDigitalIOChannel.HRDL_DIGITAL_IO_CHANNEL_3 + HRDLDigitalIOChannel.HRDL_DIGITAL_IO_CHANNEL_4) ' Set channel 1 to Input, others to output
            Dim digitalOutPinState As Short = 0 ' Output state set to low
            Dim enabledDigitalInput As Short = 0

            ' If digital I/O are not all set to outputs then at least one must be an input
            If directionOut < 15 Then

                enabledDigitalInput = 1

            End If

            setDigitalIOStatus = HRDLSetDigitalIOChannel(picologHRDLHandle, directionOut, digitalOutPinState, enabledDigitalInput)

            If setDigitalIOStatus = 1 Then

                hasDigitalIO = True

                If enabledDigitalInput > 0 Then

                    digitalInputEnabled = True
                    numberOfEnabledChannels = numberOfEnabledChannels + 1

                End If

            Else

                Console.WriteLine("Digital I/O not supported on this device.")

            End If

            ' Set interval between conversion starts to 121 ms
            status = HRDLSetInterval(picologHRDLHandle, 121, HRDLConversionTime.HRDL_60MS)

            ' Start the data collection

            Dim numberOfSamples As Integer = 100

            Console.WriteLine("Starting data collection..." & vbNewLine)

            status = HRDLRun(picologHRDLHandle, numberOfSamples, BlockMethod.HRDL_BM_BLOCK)

            Console.WriteLine("Waiting for data (press any key to cancel)..." & vbNewLine)

            Do While ready <> 1 And Console.KeyAvailable <> True

                ready = HRDLReady(picologHRDLHandle)
                Thread.Sleep(100)

            Loop

            If ready = 1 Then


                Dim times(numberOfSamples - 1) As Integer
                Dim values(numberOfSamples - 1) As Integer

                Dim pinnedValues As PinnedArray.PinnedArray(Of Integer)() = New PinnedArray.PinnedArray(Of Integer)(0) {}
                pinnedValues(0) = New PinnedArray.PinnedArray(Of Integer)(values)

                Dim overflow As Short = 0
                Dim numberOfSamplesCollected As Integer = 0

                ' Get a block of 100 readings...
                ' We can call this routine repeatedly to get more blocks with the same settings
                numberOfSamplesCollected = HRDLGetTimesAndValues(picologHRDLHandle, times(0), values(0), overflow, numberOfSamples)

                If (numberOfSamplesCollected > 0) Then

                    ' Output data to console window

                    Console.WriteLine("Collected {0} samples" & vbNewLine, numberOfSamplesCollected)
                    Console.WriteLine("Time shown in each row corresponds to first value in each set." & vbNewLine)

                    If hasDigitalIO = True And digitalInputEnabled = True Then

                        Console.WriteLine("Times (ms)" & vbTab & "Digital" & vbTab & "Ch 1")
                        Console.WriteLine("----------" & vbTab & "-------" & vbTab & "----")

                    Else

                        Console.WriteLine("Times (ms)" & vbTab & "Ch 1")
                        Console.WriteLine("----------" & vbTab & "----")

                    End If

                    Dim timeCount As UInteger = 0
                    Dim channel As UInteger = 0

                    ' Output values to console, time shown in each row will correspond to the
                    ' first value in that set

                    For i = 0 To ((numberOfSamplesCollected / numberOfEnabledChannels) - 1)

                        If digitalInputEnabled = True Then

                            Console.Write("{0}" & vbTab & vbTab, times(i * timeCount))

                            ' Show integer value for now
                            Console.Write("{0}" & vbTab & vbTab, values(i * timeCount))

                            Console.Write("{0}", adc2mV(picologHRDLHandle, values((i * timeCount) + 1), HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1, HRDLRange.HRDL_2500_MV))

                            timeCount = timeCount + 1

                        Else

                            Console.Write("{0}" & vbTab & vbTab, times(i))
                            Console.Write("{0}", adc2mV(picologHRDLHandle, values(i), HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1, HRDLRange.HRDL_2500_MV))

                        End If

                        Console.WriteLine()

                    Next i

                End If

            Else

                Console.WriteLine("Data collection aborted.")

            End If

            Call HRDLStop(picologHRDLHandle)

            Console.WriteLine()
            Console.WriteLine("Press any key to exit.")

            While keyPress = False

                System.Threading.Thread.Sleep(100)

                If Console.KeyAvailable Then                ' Check if the user has hit a key to indicate they want to stop

                    keyPress = True

                End If

            End While

        Else

            Console.WriteLine("Unable to open unit please confirm PicoLog High Resolution Data Logger is connected to a USB port.")

        End If

        Call HRDLCloseUnit(picologHRDLHandle)

        Console.WriteLine("Closing application...")

        System.Threading.Thread.Sleep(1000)


    End Sub

    Function adc2mV(ByVal handle As Short, ByVal value As Integer, ByVal channel As HRDLInputs, ByVal range As HRDLRange) As Double

        Dim max As Integer
        Dim min As Integer

        Dim millivoltValue As Double

        Dim maxVoltage As Integer = 0

        maxVoltage = 2500 / Math.Pow(2, range)

        Call HRDLGetMinMaxAdcCounts(handle, min, max, channel)

        ' Calculate value depending on number of bits
        millivoltValue = CDbl((value / max) * maxVoltage)

        Return millivoltValue

    End Function

End Module

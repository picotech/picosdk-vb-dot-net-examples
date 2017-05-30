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
Imports System.Math

Module PicoHRDLVBCon

    Sub Main()

        Dim status As Short
        Dim picologHRDLHandle As Short
        Dim info As String

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
            '  voltage range 2500mV
            '  single ended

            Call HRDLSetAnalogInChannel(picologHRDLHandle, HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1, 1, HRDLRange.HRDL_2500_MV, 1)

            ' Set interval between conversion starts to 121 ms
            Call HRDLSetInterval(picologHRDLHandle, 121, HRDLConversionTime.HRDL_60MS)

            ' Start the data collection

            Dim numberOfSamples As Integer = 100

            Console.WriteLine("Starting data collection..." & vbNewLine)

            Call HRDLRun(picologHRDLHandle, numberOfSamples, BlockMethod.HRDL_BM_BLOCK)

            Console.WriteLine("Waiting for data (press any key to cancel)..." & vbNewLine)

            Do While ready <> 1 And Console.KeyAvailable <> True

                ready = HRDLReady(picologHRDLHandle)
                Thread.Sleep(100)

            Loop

            If ready Then


                Dim times(numberOfSamples - 1) As Integer
                Dim values(numberOfSamples - 1) As Integer

                Dim numberOfSamplesCollected As Integer = 0

                ' Get a block of 100 readings...
                ' We can call this routine repeatedly to get more blocks with the same settings
                numberOfSamplesCollected = HRDLGetTimesAndValues(picologHRDLHandle, times(0), values(0), 0, numberOfSamples)

                If (numberOfSamplesCollected > 0) Then

                    ' Output data to console window

                    Console.WriteLine("Collected {0} samples" & vbNewLine, numberOfSamplesCollected)

                    Console.WriteLine("Times (ms)" & vbTab & "Ch 1")
                    Console.WriteLine("----------" & vbTab & "----")


                    For i = 0 To numberOfSamplesCollected - 1

                        Console.WriteLine("{0}" & vbTab & vbTab & "{1}", times(i), adc2mV(picologHRDLHandle, values(i), HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1, HRDLRange.HRDL_2500_MV))

                    Next i

                End If

            Else

                Console.WriteLine("Data collection aborted.")

            End If


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

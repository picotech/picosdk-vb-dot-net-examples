'========================================================================================
'	
'   Filename: USBTC08VBCon.vb
'
'	Description:
'	This file demonstrates how to use VB .NET with the USB TC-08 Thermocouple Data Logger.
'   It shows the use of API call functions to collect temperature data.
'
'   Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Module USBTC08VBCon

    Sub Main()

        Dim status As Short
        Dim usbtc08Handle As Short
        Dim info As String
        Dim minimumIntervalMS As Integer
        Dim tempBuffer(8) As Single
        Dim count As Integer
        Dim overflowFlag As Short
        Dim keypress As Boolean

        usbtc08Handle = 0
        keypress = False

        If (usbtc08Handle < 1) Then

            Console.WriteLine("USB TC-08 VB.NET Console application" & vbNewLine)

            Console.WriteLine("Opening connection to USB TC-08..." & vbNewLine)

            usbtc08Handle = usb_tc08_open_unit()

            If (usbtc08Handle > 0) Then

                Console.WriteLine("USB TC-08 connected." & vbNewLine)

                info = "              "

                'Output info
                Call usb_tc08_get_unit_info2(usbtc08Handle, info, 80, 0)
                Console.WriteLine("Driver Version " & info)

                Call usb_tc08_get_unit_info2(usbtc08Handle, info, 80, 1)
                Console.WriteLine("Kernel Driver Version " & info)

                Call usb_tc08_get_unit_info2(usbtc08Handle, info, 80, 4)
                Console.WriteLine("Serial Number " & info)

                Call usb_tc08_get_unit_info2(usbtc08Handle, info, 80, 5)
                Console.WriteLine("Cal Date " & info & vbNewLine)

                ' Set noise rejection to 50 or 60Hz
                Call usb_tc08_set_mains(usbtc08Handle, USBTC08MainsFrequency.USBTC08_REJECT_50HZ)

                ' Set Channels - Cold Junction, Ch 1 and Ch 2
                status = usb_tc08_set_channel(usbtc08Handle, USBTC08Channels.USBTC08_CHANNEL_CJC, "K")
                status = usb_tc08_set_channel(usbtc08Handle, USBTC08Channels.USBTC08_CHANNEL_1, "K")
                status = usb_tc08_set_channel(usbtc08Handle, USBTC08Channels.USBTC08_CHANNEL_2, "K")

                ' Minimum Sampling Interval
                minimumIntervalMS = usb_tc08_get_minimum_interval_ms(usbtc08Handle)

                'Obtain 10 sets of temperature readings with 1 second between requests

                Console.WriteLine("Collecting 10 sets of readings..." & vbNewLine)

                Console.WriteLine("CJC" & vbTab & vbTab & "Ch1" & vbTab & vbTab & "Ch2")

                For count = 0 To 10

                    status = usb_tc08_get_single(usbtc08Handle, tempBuffer(0), overflowFlag, USBTC08TempUnits.USBTC08_UNITS_CENTIGRADE)

                    Console.Write("{0:F}", tempBuffer(0).ToString() & vbTab)
                    Console.Write("{0:F}", tempBuffer(1).ToString() & vbTab)
                    Console.Write("{0:F}", tempBuffer(2).ToString() & vbTab)
                    Console.WriteLine()

                    System.Threading.Thread.Sleep(1000)

                Next

                Console.WriteLine()
                Console.WriteLine("Data collection complete, please press any key to exit.")

                While keypress = False

                    System.Threading.Thread.Sleep(100)

                    If Console.KeyAvailable Then                ' Check if the user has hit a key to indicate they want to stop
                        keypress = True
                    End If

                End While

                Call usb_tc08_close_unit(usbtc08Handle)

                Console.WriteLine("Closing application...")

                System.Threading.Thread.Sleep(1000)

            Else

                Console.WriteLine("Unable to open USB TC-08")

            End If

        End If

    End Sub

End Module

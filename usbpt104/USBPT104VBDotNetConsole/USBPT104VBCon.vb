'========================================================================================
'	
'   Filename: USBPT104VBCon.vb
'
'   Description:
'	This file demonstrates how to use VB .NET with the USB PT-104 data logger.
'   It shows how to call the API functions to:
'
'       Open a connection to a data logger, print device information and capture some
'       data.
'
'   Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
'
'========================================================================================

Module USBPt104VBCon

    Dim handle As Integer
    Dim status As UInteger

    ' ******************************************************************************************************************************************************************
    ' getDeviceInfo - Reads and displays the scopes device information.
    '
    ' Parameters - handle    - The device handle
    ' *******************************************************************************************************************************************************************
    Sub getDeviceInfo(ByVal handle As Short)

        Dim infoText(6) As String
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

        For i = 0 To 6

            str = "            "
            Call UsbPt104GetUnitInfo(handle, str, CShort(str.Length), requiredSize, i)
            infoText(i) += str
            Console.WriteLine(infoText(i) & vbTab)

        Next i

    End Sub

    Sub Main()

        Console.WriteLine("USB PT-104 VB .NET Example")
        Console.WriteLine("==========================")
        Console.WriteLine()

        ' Enumerate Units

        Dim details As String
        details = "                             "

        Dim length As UInteger = 0

        status = UsbPt104Enumerate(details, CUInt(details.Length), CommunicationType.CT_ALL)

        If status <> PICO_OK Then

            MsgBox("No devices found.", vbOKOnly, "Error Message")
            Exit Sub

        Else

            Console.WriteLine("Devices found: {0}", details)
            Console.WriteLine()

        End If

        ' Open Unit

        status = UsbPt104OpenUnit(handle, vbNullString)

        If status <> PICO_OK Then

            MsgBox("Unit not opened", vbOKOnly, "Error Message")
            Exit Sub

        Else

            Console.WriteLine("Opened USB PT-104 device successfully:")
            Console.WriteLine()

        End If

        ' Display Device Information

        Call getDeviceInfo(handle)
        Console.WriteLine()


        ' Setup Channels

        ' Set Channel 1 for a 4-wire PT-100 measurement
        status = UsbPt104SetChannel(handle, UsbPt104Channels.USBPT104_CHANNEL_1, UsbPt104DataType.USBPT104_PT100, CShort(4))

        ' Turn off other channels
        status = UsbPt104SetChannel(handle, UsbPt104Channels.USBPT104_CHANNEL_2, UsbPt104DataType.USBPT104_OFF, CShort(4))
        status = UsbPt104SetChannel(handle, UsbPt104Channels.USBPT104_CHANNEL_3, UsbPt104DataType.USBPT104_OFF, CShort(4))
        status = UsbPt104SetChannel(handle, UsbPt104Channels.USBPT104_CHANNEL_4, UsbPt104DataType.USBPT104_OFF, CShort(4))


        ' Collect some data 

        Dim numberOfReadings As UInteger = 10
        Dim i As UInteger
        Dim value As Integer = 0

        Console.WriteLine("Collecting {0} readings...", numberOfReadings)
        Console.WriteLine()

        Console.WriteLine("Raw" & vbTab & "Scaled")
        Console.WriteLine("---" & vbTab & "------")


        For i = 1 To numberOfReadings

            status = UsbPt104GetValue(handle, UsbPt104Channels.USBPT104_CHANNEL_1, value, 0)

            If status = PICO_OK Then

                ' Show raw value and also scaled for temperature
                Console.WriteLine("{0}" & vbTab & "{1}", value, CSng(value / 1000))

            Else

                Console.WriteLine("Reading {0}, Status: {1}", i, status)

            End If

            Sleep(1000)

        Next

        Sleep(2000)

        ' Close Unit

        Call UsbPt104CloseUnit(handle)

    End Sub

End Module
